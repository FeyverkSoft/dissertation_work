using System;
using System.Data;

namespace DbHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class DbParamAttribute : Attribute
    {
        /// <summary>
        /// Название поля в поставщике данных
        /// </summary>
        public String DbParamName { get; private set; }
        /// <summary>
        /// Тип данныз в бд
        /// </summary>
        public SqlDbType DbType { get; private set; }
        /// <summary>
        /// Направление данных в бд
        /// </summary>
        public ParameterDirection DbDirection { get; private set; }

        /// <summary>
        /// Инициализирует экземпляр объекта указанными значениями
        /// </summary>
        /// <param name="dbParamName"> Название поля в поставщике данных</param>
        /// <param name="dbType"></param>
        /// <param name="dbDirection"></param>
        public DbParamAttribute(String dbParamName = null, SqlDbType dbType = SqlDbType.Variant, ParameterDirection dbDirection = ParameterDirection.Input)
        {
            DbParamName = dbParamName;
            DbType = dbType;
            DbDirection = dbDirection;
        }
    }
}
