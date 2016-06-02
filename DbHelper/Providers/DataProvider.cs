using System;
using System.Data;
using System.Data.SqlClient;

namespace DbHelper.Providers
{
    public delegate T SqlCommandDelegate<out T>(SqlCommand command);
    /// <summary>
    ///     Подключение к Базе данных
    /// </summary>
    internal class DataProvider : IDataProvider
    {
        /// <summary>
        /// Информационные сообщения выводимые БД
        /// </summary>
        public String InfoMessage { get; private set; }

        public String ConnectionString { get; }

        public DataProvider(String connectionString)
        {
            if (String.IsNullOrEmpty(connectionString) || String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(ConnectionString));

            ConnectionString = connectionString;
        }

        /// <summary>
        /// Создать комманду
        /// </summary>
        /// <param name="name">Название комманды</param>
        /// <param name="delegate"></param>
        /// <param name="type">тип комманды</param>
        /// <returns></returns>
        public T ExecuteCommand<T>(String name, SqlCommandDelegate<T> @delegate, CommandType type = CommandType.StoredProcedure)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    connection.InfoMessage += (sbyteender, message) => { InfoMessage += message + Environment.NewLine; };
                    using (var command = new SqlCommand(name, connection))
                    {
                        command.CommandType = type;
                        return @delegate.Invoke(command);
                    }
                }
            }
            catch (SqlException e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}
