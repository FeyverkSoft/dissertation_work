using System;
using System.Collections.Generic;

namespace DbHelper
{
    public interface IDbBaseReader
    {
        /// <summary>
        /// получить список всех возможных объектов
        /// </summary>
        /// <param name="procName">Название хранимки</param>
        /// <param name="params">Список параметров хранимой процедуры</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        List<T> GetObjectList<T>(String procName, Object @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new();

        /// <summary>
        /// получить список всех возможных объектов
        /// </summary>
        /// <param name="procName">Название хранимки</param>
        /// <param name="params">Список параметров хранимой процедуры в виде словаря</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        List<T> GetObjectList<T>(String procName, Dictionary<String, Object> @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new();

        /// <summary>
        /// получить объект
        /// </summary>
        /// <param name="procName">Название хранимки</param>
        /// <param name="params">Список параметров хранимой процедуры в виде словаря</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        T GetObject<T>(String procName, Dictionary<String, Object> @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new();

        /// <summary>
        /// получить объект
        /// </summary>
        /// <param name="procName">Название хранимки</param>
        /// <param name="params">Список параметров хранимой процедуры</param>
        /// <param name="dbReaderSettings"></param>
        /// <returns></returns>
        T GetObject<T>(String procName, Object @params = null, DbReaderSettings dbReaderSettings = null) where T : class, new();

        /// <summary>
        /// Получиьт скалярное значение
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        object ExecuteScalar(String procName, Object @params = null);

        /// <summary>
        /// Выполнить указанную комманду
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="params"></param>
        void Execute(String procName, Object @params = null);
    }
}