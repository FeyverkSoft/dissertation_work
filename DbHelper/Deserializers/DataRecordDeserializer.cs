using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using DbHelper.Attributes;
using DbHelper.Domain;

namespace DbHelper.Deserializers
{
    /// <summary>
    ///     Публичные поля класса наследника заполняются 
    ///     на основе IDataReader
    /// </summary>
    [XmlType(IncludeInSchema = false)]
    internal class DataRecordDeserializer : IDbReaderDeserializer
    {

        #region Helpers
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<MemberCacheInfo> GetMemberCacheInfo(Type type)
        {
            var cacheName = $"DataRecordDeserializer_MemberCacheInfo_{type.FullName}";
            var cache = MemoryCache.Default;
            var members = (IEnumerable<MemberCacheInfo>) cache[cacheName];
            if (members != null)
                return members;

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

            var propsInf = fields.Where(memberInfo => Attribute.GetCustomAttribute(memberInfo, typeof(SqlFieldIgnoreAttribute)) == null)
                .Select(field => new MemberCacheInfo
                {
                    MemberInfo = field,
                    DataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(field, typeof(DataMemberAttribute))
                })
                .ToList();
            propsInf.AddRange(props.Where(memberInfo => Attribute.GetCustomAttribute(memberInfo, typeof(SqlFieldIgnoreAttribute)) == null)
                .Select(prop => new MemberCacheInfo
                {
                    MemberInfo = prop,
                    DataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(prop, typeof(DataMemberAttribute))
                }));

            cache[cacheName] = propsInf;
            return propsInf;
        }

        /// <summary>
        /// Это поле?
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Boolean HasField(IDataRecord dataReader, String fieldName)
        {
            for (var i = 0; i < dataReader.FieldCount; i++)
                if (dataReader.GetName(i) == fieldName)
                    return true;
            return false;
        }
        #endregion

        Object Deserialize(Type recordType, IDataRecord dataRecord, Int32 deep)
        {
            var result = recordType.GetConstructors().FirstOrDefault(x => x.GetParameters().Length == 0)?.Invoke(null);

            if (result == null)
                throw new Exception("Ошибка при десериализации поставщика данных");

            foreach (MemberCacheInfo memberCacheInfo in GetMemberCacheInfo(recordType))
            {
                FieldInfo fieldInfo = null;
                PropertyInfo propertyInfo = null;

                if (memberCacheInfo.MemberInfo.MemberType == MemberTypes.Field)
                    fieldInfo = memberCacheInfo.MemberInfo as FieldInfo;
                else
                    propertyInfo = memberCacheInfo.MemberInfo as PropertyInfo;

                Type memberType = fieldInfo?.FieldType ?? propertyInfo?.PropertyType;

                //нафиг, более быстрый вариант ниже
                //memberType = Nullable.GetUnderlyingType(memberType) ?? memberType;
                memberType = memberType.IsConstructedGenericType ? memberType.GenericTypeArguments[0] : memberType;
                //Nullable.GetUnderlyingType(memberType) ?? memberType;

                //var dataMember = memberInfo.GetCustomAttribute<DataMemberAttribute>();
                var dataName = !String.IsNullOrEmpty(memberCacheInfo.DataMemberAttribute?.Name)
                    ? memberCacheInfo.DataMemberAttribute.Name
                    : memberCacheInfo.MemberInfo.Name;

                // Берем значение из DataReader
                if (HasField(dataRecord, dataName))
                {
                    if (Convert.IsDBNull(dataRecord[dataName]))
                        continue;

                    object value;

                    if (memberType != null && memberType.IsEnum)
                        value = Enum.Parse(memberType, Convert.ToString(dataRecord[dataName]), true);
                    else
                        value = Convert.ChangeType(dataRecord[dataName], memberType);

                    switch (value?.GetType().Name)
                    {
                        case nameof(DateTime):
                            value = DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
                            break;
                        case nameof(String):
                            value = ((String)value)?.Trim();
                            break;
                    }

                    fieldInfo?.SetValue(result, value);
                    propertyInfo?.SetValue(result, value);
                }
                //если это не вложенный тип в пределах одного ридера
                else if (Attribute.GetCustomAttribute(memberCacheInfo.MemberInfo, typeof(SqlChildFieldAttribute)) != null)
                {
                    if (deep > 32)
                        throw new Exception("Возможно Рекурсивное зацикливание при десериализации поставщика данных");
                    fieldInfo?.SetValue(result, Deserialize(fieldInfo.FieldType, dataRecord, deep + 1));
                    propertyInfo?.SetValue(result, Deserialize(propertyInfo.PropertyType, dataRecord, deep + 1));
                }//надо будет причисать этот код :D
                else
                {
                    var attr = Attribute.GetCustomAttribute(memberCacheInfo.MemberInfo, typeof(XmlParamAttribute));
                    if (attr != null)
                    {
                        dataName = ((XmlParamAttribute)attr).Name ?? dataName;
                        switch (attr.GetType().Name)
                        {
                            case nameof(XmlParamsAttribute):
                                fieldInfo?.SetValue(result, XmlDeserialize(fieldInfo.FieldType, Convert.ToString(dataRecord[dataName]), ((XmlParamAttribute)attr).XmlRoot));
                                propertyInfo?.SetValue(result, XmlDeserialize(propertyInfo.PropertyType, Convert.ToString(dataRecord[dataName]), ((XmlParamAttribute)attr).XmlRoot));
                                break;
                        }
                    }
                    else
                    {
                        if (Attribute.GetCustomAttribute(memberCacheInfo.MemberInfo, typeof(SqlFieldOptionalAttribute)) == null)
                            throw new Exception(
                                $"Поставщик данных не содержит поле «{dataName}», необходимое для создании экземпляра класса «{recordType}»");
                    }
                }
            }
            return result;
        }

        public T Deserialize<T>(IDataRecord dataRecord) where T : new()
        {
            Int32 deep = 0;
            return (T)Deserialize(typeof(T), dataRecord, deep);
        }

        private Object XmlDeserialize(Type t, String xmlDoc, String xmlRoot)
        {

            if (String.IsNullOrEmpty(xmlDoc))
            {
                return t.GetConstructors().FirstOrDefault(x => x.GetParameters().Length == 0)?.Invoke(null);
            }
            //https://msdn.microsoft.com/ru-ru/library/system.xml.serialization.xmlserializer(v=vs.110).aspx
            //If you use any of the other constructors, multiple versions of the same assembly are generated and never unloaded, which results in a memory leak and poor performance. 
            //The easiest solution is to use one of the previously mentioned two constructors. Otherwise, you must cache the assemblies in a Hashtable, as shown in the following example.
            var key = $"__XmlDeserialize_{t.FullName}.{xmlRoot}";

            var se = MemoryCache.Default[key];

            XmlSerializer serializer;
            if (se != null)
            {
                serializer = (XmlSerializer)se;
            }
            else
            {
                serializer = String.IsNullOrEmpty(xmlRoot) ? new XmlSerializer(t) : new XmlSerializer(t, new XmlRootAttribute(xmlRoot));
                MemoryCache.Default[key] = serializer;
            }
            using (var memoryStream = new MemoryStream(StringToUtf8ByteArray(xmlDoc)))
            {
                return serializer.Deserialize(memoryStream);
            }
        }
        // Метод конвертирует строку в UTF8 Byte массив
        private Byte[] StringToUtf8ByteArray(string xmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(xmlString);
            return byteArray;
        }
    }
}