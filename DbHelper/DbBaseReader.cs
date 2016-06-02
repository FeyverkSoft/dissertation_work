using System;
using System.Collections.Generic;
using System.Linq;
using DbHelper.Deserializers;
using DbHelper.Providers;

namespace DbHelper
{
    public class DbBaseReader : IDbBaseReader
    {
        private readonly String _con;

        /// <summary>
        /// Пользовательские десериализаторы
        /// </summary>
        private readonly IDictionary<Type, IDbReaderDeserializer> _userDeserializers;
        /// <summary>
        /// Дефолтный десериализатор
        /// </summary>
        private readonly IDbReaderDeserializer _defaultDeserializer;

        private IDataProvider GetProvider()
        {
            return new DataProvider(_con);
        }


        public DbBaseReader(String connectionString, IDictionary<Type, IDbReaderDeserializer> deserializers = null)
        {
            _con = connectionString;
            _userDeserializers = deserializers ;//?? new Dictionary<Type, IDbReaderDeserializer>();
            _defaultDeserializer = new DataRecordDeserializer();
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
                    command.Parameters.AddRange(DbConverter.SerializeParams(@params).ToArray());
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        res.Add(GetDeserializer(dbReaderSettings, typeof(T)).Deserialize<T>(dr));
                    }
                }
                DbConverter.UpdateOutputParams(command, @params);
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
                    command.Parameters.AddRange(DbConverter.SerializeParams(@params).ToArray());
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        res.Add(GetDeserializer(dbReaderSettings, typeof(T)).Deserialize<T>(dr));
                    }
                }
                DbConverter.UpdateOutputParams(command, @params);
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
                    command.Parameters.AddRange(DbConverter.SerializeParams(@params).ToArray());
                using (var dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        DbConverter.UpdateOutputParams(command, @params);
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
                    command.Parameters.AddRange(DbConverter.SerializeParams(@params).ToArray());
                using (var dr = command.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        DbConverter.UpdateOutputParams(command, @params);
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
                    command.Parameters.AddRange(DbConverter.SerializeParams(@params).ToArray());
                var res = command.ExecuteScalar();
                DbConverter.UpdateOutputParams(command, @params);
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
                    command.Parameters.AddRange(DbConverter.SerializeParams(@params).ToArray());
                var res = command.ExecuteNonQuery();
                DbConverter.UpdateOutputParams(command, @params);
                return res;
            });
        }
    }
}
