using System.Data;

namespace DbHelper.Providers
{
    public interface IDataProvider
    {
        string ConnectionString { get; }
        /// <summary>
        /// Информационные сообщения выводимые БД
        /// </summary>
        string InfoMessage { get; }
        /// <summary>
        /// Создать комманду
        /// </summary>
        /// <param name="name">Название комманды</param>
        /// <param name="delegate"></param>
        /// <param name="type">тип комманды</param>
        /// <returns></returns>
        T ExecuteCommand<T>(string name, SqlCommandDelegate<T> @delegate, CommandType type = CommandType.StoredProcedure);
    }
}