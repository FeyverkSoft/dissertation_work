using System;

// <summary>
// Набор аттрибутов помошников десериализатора
// </summary>
namespace DbHelper.Attributes
{
    /// <summary>
    /// Показывает что данное поле является десериализуемым из XML
    /// Базовый объект
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XmlParamAttribute : Attribute
    {
        /// <summary>
        /// Название поля в поставщике данных
        /// </summary>
        public String Name { get; private set; }
        /// <summary>
        /// Название узла
        /// </summary>
        public String XmlRoot { get; private set; }

        /// <summary>
        /// Инициализирует экземпляр объекта указанными значениями
        /// </summary>
        /// <param name="name"> Название поля в поставщике данных</param>
        /// <param name="xmlRoot"></param>
        public XmlParamAttribute(String name = null, String xmlRoot = null)
        {
            Name = name;
            XmlRoot = xmlRoot;
        }
    }
    /// <summary>
    /// Показывает что данное поле является объектом десериализуемым из XML
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XmlParamsAttribute : XmlParamAttribute
    {
        /// <summary>
        /// Инициализирует экземпляр объекта указанными значениями
        /// </summary>
        /// <param name="name"> Название поля в поставщике данных</param>
        /// <param name="xmlRoot"></param>
        public XmlParamsAttribute(String name = null, String xmlRoot = null) : base(name, xmlRoot) { }
    }
    /// <summary>
    /// Показывает что данное поле является словарем десериализуемым из XML
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XmlParamsDictAttribute : XmlParamAttribute
    {
        /// <summary>
        /// Инициализирует экземпляр объекта указанными значениями
        /// </summary>
        /// <param name="name"> Название поля в поставщике данных</param>
        public XmlParamsDictAttribute(String name = null) : base(name) { }
    }
    /// <summary>
    /// Показывает что данное поле является списком групп пользователей  десериализуемым из XML
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XmlUserGroupsAttribute : XmlParamAttribute
    {
        /// <summary>
        /// Инициализирует экземпляр объекта указанными значениями
        /// </summary>
        /// <param name="name"> Название поля в поставщике данных</param>
        public XmlUserGroupsAttribute(String name = null) : base(name) { }
    }

}
