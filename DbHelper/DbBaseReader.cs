using System;
using System.Collections.Generic;
using System.Linq;
using DbHelper.Converter;
using DbHelper.Deserializers;
using DbHelper.Providers;

namespace DbHelper
{
    public class DbBaseReader : IDbBaseReader
    {
        private readonly String _connectionString;

        #region Дефолтные поставщики
        /// <summary>
        /// Дефолтный десериализатор
        /// </summary>
        private readonly IDbReaderDeserializer _defaultDeserializer;
        /// <summary>
        /// Конвертер для параметров БД
        /// </summary>
        private readonly IDbConverter _defaultDbConverter;
        #endregion

        #region Ползовательские поставщики
        /// <summary>
        /// Пользовательские десериализаторы
        /// </summary>
        private readonly IDictionary<Type, IDbReaderDeserializer> _userDeserializers;

        /// <summary>
        /// Пользовательские конвертеры
        /// </summary>
        private readonly IDictionary<Type, IDbConverter> _userDbConverter;
        #endregion

        #region Ctors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString">Строка подключения к источнику данных</param>
        /// <param name="deserializers">Набор пользовательский десериализаторов/сериализаторов</param>
        /// <param name="userDbConverters">Пользовательские конвертеры объектов с данными о входных параметрах по уполчанию для всех методов экземпляра класса указанного типа, может быть переопределено в конкретном вызове</param>
        public DbBaseReader(String connectionString, IDictionary<Type, IDbReaderDeserializer> deserializers = null, IDictionary<Type, IDbConverter> userDbConverters = null)
        {
            _defaultDeserializer = new DataRecordDeserializer();
            _defaultDbConverter = new DbConverter();

            _connectionString = connectionString;
            _userDbConverter = userDbConverters;
            _userDeserializers = deserializers;

        }
        #endregion


        #region Helpers func

        private IDataProvider GetProvider()
        {
            return new DataProvider(_connectionString);
        }

        /// <summary>
        /// Выбрать конвертер входных параметров с учётом приоритетов
        /// </summary>
        /// <param name="dbReaderSettings"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private IDbConverter GetDbConverter(DbReaderSettings dbReaderSettings = null, Type type = null)
        {
            if (dbReaderSettings?.DbConverter != null)
                return dbReaderSettings.DbConverter;

            if (type == null || _userDeserializers == null || _userDeserializers.Count == 0)
                return _defaultDbConverter;

            return _userDbConverter.ContainsKey(type) ? _userDbConverter[type] : _defaultDbConverter;
        }

        /// <summary>
        /// Выбрать десериализатор с учётом приоритетов
        /// </summary>
        /// <param name="dbReaderSettings"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private IDbReaderDeserializer GetDeserializer(DbReaderSettings dbReaderSettings, Type type = null)
        {
            if (dbReaderSettings?.DbReaderDeserializer != null)
                return dbReaderSettings.DbReaderDeserializer;

            if (type == null || _userDeserializers == null || _userDeserializers.Count == 0)
                return _defaultDeserializer;

            return _userDeserializers.ContainsKey(type) ? _userDeserializers[type] : _defaultDeserializer;
        }
        #endregion


        /// <summary>
        /// получить список всех возможных объектов
        /// </summary>
        /// <param name="procName">Название хранимки</param>
        /// <param name="params">Список параметров хранимой процедуры</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        public List<T> GetObjectList<T>(String procName, Object @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new()
        {
            return GetProvider().ExecuteCommand(procName, command =>
            {
                var res = new List<T>();
                if (@params != null)
                    command.Parameters.AddRange(GetDbConverter(dbReaderSettings, @params?.GetType()).SerializeParams(@params).ToArray());
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        res.Add(GetDeserializer(dbReaderSettings, typeof(T)).Deserialize<T>(dr));
                    }
                }
                GetDbConverter(dbReaderSettings, @params?.GetType()).UpdateOutputParams(command, @params);
                return res;
            });
        }


        /// <summary>
        /// получить список всех возможных объектов
        /// </summary>
        /// <param name="procName">Название хранимки</param>
        /// <param name="params">Список параметров хранимой процедуры в виде словаря</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        [Obsolete]
        public List<T> GetObjectList<T>(String procName, Dictionary<String, Object> @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new()
        {
            return GetProvider().ExecuteCommand(procName, command =>
            {
                var res = new List<T>();
                if (@params != null)
                    command.Parameters.AddRange(GetDbConverter(dbReaderSettings, @params?.GetType()).SerializeParams(@params).ToArray());
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        res.Add(GetDeserializer(dbReaderSettings, typeof(T)).Deserialize<T>(dr));
                    }
                }
                GetDbConverter(dbReaderSettings, @params?.GetType()).UpdateOutputParams(command, @params);
                return res;
            });
        }

        /// <summary>
        /// получить объект
        /// </summary>
        /// <param name="procName">Название хранимки</param>
        /// <param name="params">Список параметров хранимой процедуры в виде словаря</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        [Obsolete]
        public T GetObject<T>(String procName, Dictionary<String, Object> @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new()
        {
            return GetProvider().ExecuteCommand(procName, command =>
            {
                if (@params != null)
                    command.Parameters.AddRange(GetDbConverter(dbReaderSettings, @params?.GetType()).SerializeParams(@params).ToArray());
                using (var dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        GetDbConverter(dbReaderSettings, @params?.GetType()).UpdateOutputParams(command, @params);
                        return GetDeserializer(dbReaderSettings, typeof(T)).Deserialize<T>(dr);
                    }
                    throw new Exception("Запрос не вернул результата");
                }
            });
        }

        /// <summary>
        /// получить объект
        /// </summary>
        /// <param name="procName">Название хранимки</param>
        /// <param name="params">Список параметров хранимой процедуры</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        public T GetObject<T>(String procName, Object @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new()
        {
            return GetProvider().ExecuteCommand(procName, command =>
            {
                if (@params != null)
                    command.Parameters.AddRange(GetDbConverter(dbReaderSettings, @params?.GetType()).SerializeParams(@params).ToArray());
                using (var dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        GetDbConverter(dbReaderSettings, @params?.GetType()).UpdateOutputParams(command, @params);
                        return GetDeserializer(dbReaderSettings, typeof(T)).Deserialize<T>(dr);
                    }
                    throw new Exception("Запрос не вернул результата");
                }
            });
        }

        /// <summary>
        /// Получиьт скалярное значение
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        public object ExecuteScalar(String procName, Object @params = null)
        {
            return GetProvider().ExecuteCommand(procName, command =>
            {
                if (@params != null)
                    command.Parameters.AddRange(GetDbConverter().SerializeParams(@params).ToArray());
                var res = command.ExecuteScalar();
                GetDbConverter().UpdateOutputParams(command, @params);
                return res;
            });
        }

        /// <summary>
        /// Выполнить указанную комманду
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="params"></param>
        public void Execute(String procName, Object @params = null)
        {
            GetProvider().ExecuteCommand(procName, command =>
            {
                if (@params != null)
                    command.Parameters.AddRange(GetDbConverter().SerializeParams(@params).ToArray());
                var res = command.ExecuteNonQuery();
                GetDbConverter().UpdateOutputParams(command, @params);
                return res;
            });
        }
    }
}
