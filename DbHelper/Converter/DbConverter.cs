using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using DbHelper.Attributes;
using DbHelper.Domain;

namespace DbHelper.Converter
{
    public class DbConverter : IDbConverter
    {

        private readonly Dictionary<Type, SqlDbType> _typeToSqlDbTypeMap = new Dictionary<Type, SqlDbType>
        {
            {typeof (Boolean), SqlDbType.Bit},
            {typeof (Boolean?), SqlDbType.Bit},
            {typeof (Byte[]), SqlDbType.Binary},
            {typeof (Byte), SqlDbType.TinyInt},
            {typeof (Byte?), SqlDbType.TinyInt},
            {typeof (Char), SqlDbType.NChar},
            {typeof (Char?), SqlDbType.NChar},
            {typeof (Char[]), SqlDbType.NVarChar},
            {typeof (String), SqlDbType.NVarChar},
            {typeof (Int16), SqlDbType.SmallInt},
            {typeof (Int16?), SqlDbType.SmallInt},
            {typeof (UInt16), SqlDbType.SmallInt},
            {typeof (UInt16?), SqlDbType.SmallInt},
            {typeof (Int32), SqlDbType.Int},
            {typeof (Int32?), SqlDbType.Int},
            {typeof (UInt32), SqlDbType.Int},
            {typeof (UInt32?), SqlDbType.Int},
            {typeof (Int64), SqlDbType.BigInt},
            {typeof (Int64?), SqlDbType.BigInt},
            {typeof (UInt64), SqlDbType.BigInt},
            {typeof (UInt64?), SqlDbType.BigInt},
            {typeof (Double?), SqlDbType.Float},
            {typeof (Decimal), SqlDbType.Decimal},
            {typeof (Decimal?), SqlDbType.Decimal},
            {typeof (Double), SqlDbType.Float},
            {typeof (DateTime), SqlDbType.DateTime},
            {typeof (DateTime?), SqlDbType.DateTime},
            {typeof (DateTimeOffset), SqlDbType.DateTimeOffset},
            {typeof (DateTimeOffset?), SqlDbType.DateTimeOffset},
            {typeof (Guid), SqlDbType.UniqueIdentifier},
            {typeof (TimeSpan), SqlDbType.Time},
            {typeof (object), SqlDbType.Variant},
            {typeof (float), SqlDbType.Real},
            {typeof (float?), SqlDbType.Real},
            {typeof (sbyte), SqlDbType.TinyInt},
            {typeof (sbyte?), SqlDbType.TinyInt},
        };

        private SqlDbType GetSqlDbType(Type type)
        {
            return _typeToSqlDbTypeMap.ContainsKey(type) ? _typeToSqlDbTypeMap[type] : SqlDbType.Variant;
        }

        /// <summary>
        /// Получить список свойст класса с параметрами
        /// </summary>
        /// <param name="type">Тип класса</param>
        /// <returns></returns>
        private IEnumerable<DbPropCacheInfo> GetTypeProperty(Type type)
        {
            var cache = MemoryCache.Default;
            var members = cache[$"DbHelper_{type.FullName}"] as List<DbPropCacheInfo>;
            if (members != null)
                return members;

            var temp = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                .Select(prop => new DbPropCacheInfo
                {
                    PropertyInfo = prop,
                    DbParamAttribute = prop.GetCustomAttribute(typeof(DbParamAttribute)) as DbParamAttribute
                }).ToList();

            cache[$"DbHelper_{type.FullName}"] = temp;
            return temp;
        }

        /// <summary>
        /// Получить массив параметров по словарю
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Obsolete]
        public IEnumerable<SqlParameter> SerializeParams(Dictionary<String, Object> obj)
        {
            if (obj == null)
                yield break;
            Debug.WriteLine("in SerializeParams was transferred Dictionary<>, it is an outdated method. Use objects");
            foreach (var kv in obj)
                yield return new SqlParameter
                {
                    ParameterName = kv.Key,
                    Value = kv.Value,
                    Direction = ParameterDirection.Input
                };
        }


        /// <summary>
        /// Получить массив параметров по объекту
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<SqlParameter> SerializeParams(object obj)
        {
            if (obj == null)
                yield break;

            foreach (var propertyInfo in GetTypeProperty(obj.GetType()))
            {
                var value = propertyInfo.PropertyInfo.GetValue(obj, null);
                var type = propertyInfo.PropertyInfo.PropertyType;
                var dbType = propertyInfo.DbParamAttribute?.DbType;

                if (dbType == null || dbType == SqlDbType.Variant)
                    dbType = GetSqlDbType(type);

                if (dbType == SqlDbType.Structured && value != null)
                {
                    if (propertyInfo.DbParamAttribute?.DbDirection != ParameterDirection.Input)
                        throw new Exception($"Parameter direction of «{propertyInfo.Name}» can be 'Input' only");
                    value = GetDataTable(value);
                }

                yield return new SqlParameter
                {
                    ParameterName = propertyInfo.DbParamAttribute?.DbParamName ?? propertyInfo.Name,
                    Value = value,
                    SqlDbType = (SqlDbType)dbType,
                    Direction = propertyInfo.DbParamAttribute?.DbDirection ?? ParameterDirection.Input
                };
            }
        }

        private DataTable GetDataTable(object value)
        {
            if (value == null)
                return null;
            if (!(value is IEnumerable))
                throw new Exception($"The param type «{value.GetType()}» should be derived from IEnumerable");
            var list = ((IEnumerable)value).Cast<object>().ToList();
            if (!list.Any())
            {
                Debug.WriteLine($"{value.GetType().FullName}: The type of Structured insufficient entries. The Structured types must contain at least one entry", "Warning");
                return null;
            }
            var dataTable = new DataTable();
            var type = list.First().GetType();
            var members = GetTypeProperty(type).ToList();
            foreach (var name in members.Select(propertyInfo => !String.IsNullOrEmpty(propertyInfo.DbParamAttribute?.DbParamName) ? propertyInfo.DbParamAttribute.DbParamName : propertyInfo.Name))
            {
                dataTable.Columns.Add(name);
            }
            foreach (var values in list.Select(obj => members.Select(propertyInfo => propertyInfo?.PropertyInfo?.GetValue(obj, null)).ToArray()))
                dataTable.Rows.Add(values);
            return dataTable;
        }

        /// <summary>
        /// Прочитать UOTPUT параметры из бд обратно в dbParams объект
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="obj"></param>
        public void UpdateOutputParams<T>(SqlCommand command, T obj) where T : class
        {
            if (obj == null)
                return;
            foreach (var propertyInfo in GetTypeProperty(obj.GetType()).Where(x => x.DbParamAttribute?.DbDirection != null && x.DbParamAttribute?.DbDirection != ParameterDirection.Input))
            {
                var dbParamName = propertyInfo.DbParamAttribute.DbParamName;
                if (String.IsNullOrEmpty(dbParamName))
                    dbParamName = propertyInfo.Name;
                SetPropertyValue(obj, command.Parameters["@" + dbParamName].Value, propertyInfo.PropertyInfo);
            }
        }

        /// <summary>
        /// Ставим значение проперти
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="propertyInfo"></param>
        private void SetPropertyValue(Object obj, Object value, PropertyInfo propertyInfo)
        {
            var safeValue = GetSafeValue(propertyInfo.PropertyType, value);
            propertyInfo.SetValue(obj, safeValue, null);
        }

        /// <summary>
        /// Получает значение выходных параметров, фишка в том что тримим строчки и парсим Enum
        /// </summary>
        /// <param name="memberType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private object GetSafeValue(Type memberType, Object value)
        {
            object safeValue = null;
            if (!(Convert.IsDBNull(value)))
            {
                var type = Nullable.GetUnderlyingType(memberType) ?? memberType;
                if (type.IsEnum)
                    safeValue = Enum.Parse(type, Convert.ToString(value), true);
                else if (type == typeof(string))
                    safeValue = value?.ToString().Trim();
                else
                    safeValue = Convert.ChangeType(value, type);
            }
            return safeValue;
        }
    }
}
