using System.Data;

namespace DbHelper.Deserializers
{
    public interface IDbReaderDeserializer
    {
        /// <summary>
        /// Десериализатор для записи из бд
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRecord"></param>
        /// <returns></returns>
        T Deserialize<T>(IDataRecord dataRecord) where T : new();
    }
}
