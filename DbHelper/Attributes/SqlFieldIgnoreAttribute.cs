using System;

namespace DbHelper.Attributes
{
    /// <summary>
    /// Игнорировать это поле при десериализации объекта из поставщика данных
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SqlFieldIgnoreAttribute : Attribute { }
}
