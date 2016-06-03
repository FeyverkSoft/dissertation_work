using System.Collections.Generic;
using System.Data.SqlClient;

namespace DbHelper.Converter
{
    public interface IDbConverter
    {
        /// <summary>
        /// �������� ������ ���������� �� �������
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IEnumerable<SqlParameter> SerializeParams(Dictionary<string, object> obj);

        /// <summary>
        /// �������� ������ ���������� �� �������
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IEnumerable<SqlParameter> SerializeParams(object obj);

        /// <summary>
        /// ��������� UOTPUT ��������� �� �� ������� � dbParams ������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="obj"></param>
        void UpdateOutputParams<T>(SqlCommand command, T obj) where T : class;
    }
}