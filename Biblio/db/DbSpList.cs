using System;

namespace Biblio.db
{
    public static class DbSpList
    {
        /// <summary>
        /// Получить список N документов из бд
        /// </summary>
        public static String GetRawDocList => "[dbo].[GetRawDocList]";
        /// <summary>
        /// Добавить или обновить индексную запись в бд
        /// </summary>
        public static String AddRecord => "[Record].[AddRecord]";
        /// <summary>
        /// Добавить или обновить индексную запись в бд
        /// </summary>
        public static String AddFullTextRecord => "[dbo].[AddFullTextRecord]";
    }
}
