using System;

namespace DbHelper.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SqlFieldOptionalAttribute : Attribute { }
}
