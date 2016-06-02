using System;

namespace DbHelper.Attributes
{
    /// <summary>
    /// Помечается необязательный параметр запроса
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class OptionalParameterAttribute : Attribute
    {
    }
}
