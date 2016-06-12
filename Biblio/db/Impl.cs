using System;
using System.Collections.Generic;
using System.Linq;
using Biblio.db.DbParamsObj;
using Biblio.db.Entity;
using DbHelper;

namespace Biblio.db
{
    public class Impl
    {
        private readonly IDbBaseReader _dbBaseReader;

        public Impl(String connectionString)
        {
            _dbBaseReader = new DbBaseReader(connectionString);
        }

        ///// <summary>
        ///// Сохранить индексную запись
        ///// </summary>
        ///// <param name="word"></param>
        ///// <param name="recordId"></param>
        public void AddFulltextRecord(String word, Int64 recordId)
        {
            if (word == null || recordId < 0)
                return;
            _dbBaseReader.Execute(DbSpList.AddFullTextRecord, new
            {
                @Word = word,
                @RecordId = recordId
            });
        }

        /// <summary>
        /// Получить список сырых, не обработанных данных из бд
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Raw> GetRawDocList(Int64 count)
        {
            return _dbBaseReader.GetObjectList<Raw>(DbSpList.GetRawDocList, new
            {
                @Count = count
            }).ToList();
        }

        /// <summary>
        /// Сохранить запись о записи
        /// </summary>
        /// <param name="record"></param>
        public void AddRecord(AddRecord record)
        {
            if (record == null)
                return;
            _dbBaseReader.Execute(DbSpList.AddRecord, record);
        }
    }
}
