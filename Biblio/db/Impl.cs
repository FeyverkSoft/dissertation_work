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
        /// <summary>
        /// Сохранить слово в списке всех слов
        /// </summary>
        /// <param name="word"></param>
        public void SaveWord(String word)
        {
            if (word == null)
                return;
            _dbBaseReader.Execute(DbSpList.AddWord, new
            {
                @word = word,
            });
        }

        ///// <summary>
        ///// Сохранить индексную запись
        ///// </summary>
        ///// <param name="word"></param>
        ///// <param name="recordId"></param>
        public void SaveRecord(String word, Int64 recordId)
        {
            if (word == null || recordId < 0)
                return;
            _dbBaseReader.Execute(DbSpList.AddRecord, new
            {
                @Word = word,
                @RecordId = recordId
            });
        }

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
