using System;
using System.Collections.Generic;
using System.Data;
using Biblio.db.Entity;
using DbHelper.Attributes;

namespace Biblio.db.DbParamsObj
{
    public class AddRecord
    {
        /// <summary>
        /// Место на книжной полке
        /// </summary>
        [DbParam(dbType: SqlDbType.NVarChar)]
        public String BookShelfSeat { get; set; }
        /// <summary>
        /// Название книги
        /// </summary>
        [DbParam(dbType: SqlDbType.NVarChar)]
        public String Title { get; set; }
        /// <summary>
        /// Лицевая часть книги
        /// </summary>
        [DbParam(dbType: SqlDbType.NVarChar)]
        public String Face { get; set; }
        /// <summary>
        /// Список авторов
        /// </summary>
        [DbParam(dbType: SqlDbType.Structured)]
        public List<Author> AuthorArray { get; set; }
    }
}
