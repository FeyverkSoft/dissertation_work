using System;
using System.Collections.Generic;

namespace DbHelper
{
    public interface IDbBaseReader
    {
        /// <summary>
        /// �������� ������ ���� ��������� ��������
        /// </summary>
        /// <param name="procName">�������� ��������</param>
        /// <param name="params">������ ���������� �������� ���������</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        List<T> GetObjectList<T>(String procName, Object @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new();

        /// <summary>
        /// �������� ������ ���� ��������� ��������
        /// </summary>
        /// <param name="procName">�������� ��������</param>
        /// <param name="params">������ ���������� �������� ��������� � ���� �������</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        List<T> GetObjectList<T>(String procName, Dictionary<String, Object> @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new();

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <param name="procName">�������� ��������</param>
        /// <param name="params">������ ���������� �������� ��������� � ���� �������</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        T GetObject<T>(String procName, Dictionary<String, Object> @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new();

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <param name="procName">�������� ��������</param>
        /// <param name="params">������ ���������� �������� ���������</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        T GetObject<T>(String procName, Object @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new();

        /// <summary>
        /// �������� ��������� ��������
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        object ExecuteScalar(String procName, Object @params = null);

        /// <summary>
        /// ��������� ��������� ��������
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="params"></param>
        void Execute(String procName, Object @params = null);
    }
}