using DbHelper.Converter;
using DbHelper.Deserializers;

namespace DbHelper
{
    public class DbReaderSettings
    {
        /// <summary>
        /// Правила десериализации сущности
        /// </summary>
        public IDbReaderDeserializer DbReaderDeserializer { get; set; }
        /// <summary>
        /// Конвертер для входных значений в хранимую процедуру
        /// </summary>
        public IDbConverter DbConverter { get; set; }

        public DbReaderSettings(IDbReaderDeserializer dbReaderDeserializer = null, IDbConverter dbConverter = null)
        {
            DbReaderDeserializer = dbReaderDeserializer;
            DbConverter = dbConverter;
        }
    }
}
