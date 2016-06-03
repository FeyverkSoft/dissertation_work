using System.Collections.Generic;
using System.Data.SqlClient;

namespace DbHelper.Converter
{
    public interface IDbConverter
    {
        /// <summary>
        /// Получить массив параметров по словарю
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IEnumerable<SqlParameter> SerializeParams(Dictionary<string, object> obj);

        /// <summary>
        /// Получить массив параметров по объекту
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IEnumerable<SqlParameter> SerializeParams(object obj);

        /// <summary>
        /// Прочитать UOTPUT параметры из бд обратно в dbParams объект
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="obj"></param>
        void UpdateOutputParams<T>(SqlCommand command, T obj) where T : class;
    }
}