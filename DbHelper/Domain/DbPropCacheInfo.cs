using System;
using System.Reflection;
using DbHelper.Attributes;

namespace DbHelper.Domain
{
    internal class DbPropCacheInfo
    {
        /// <summary>
        /// информация о члене
        /// </summary>
        public PropertyInfo @PropertyInfo { get; set; }
        /// <summary>
        /// аттрибут
        /// </summary>
        public DbParamAttribute @DbParamAttribute { get; set; }
        /// <summary>
        /// Название свойства
        /// </summary>
        public String Name => @PropertyInfo?.Name;
    }
}
