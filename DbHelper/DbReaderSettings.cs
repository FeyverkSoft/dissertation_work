using DbHelper.Deserializers;

namespace DbHelper
{
    public class DbReaderSettings
    {
        /// <summary>
        /// Правила десериализации сущности
        /// </summary>
        public IDbReaderDeserializer DbReaderDeserializer { get; set; }
    }
}
