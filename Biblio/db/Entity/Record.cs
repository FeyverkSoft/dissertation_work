using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DbHelper.Attributes;

namespace Biblio.db.Entity
{
    public class Record
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        [DataMember]
        public Int64 RecordId { get; set; }
        /// <summary>
        /// Название книги/публикации
        /// </summary>
        [DataMember]
        [SqlFieldOptional]
        public String Title { get; set; }
        /// <summary>
        /// Веб-обложка публикации
        /// </summary>
        [DataMember]
        [SqlFieldOptional]
        public String Face { get; set; }
        /// <summary>
        /// Год публикации
        /// </summary>
        [DataMember]
        Int32 YearOfPublishing { get; set; }
        /// <summary>
        /// Номер книги
        /// </summary>
        [DataMember]
        Int32 NumOfBooks { get; set; }
        /// <summary>
        /// Расположение на книжной полке
        /// </summary>
        [DataMember]
        [SqlFieldOptional]
        public String BookShelfSeat { get; set; }
        /// <summary>
        /// Список авторов
        /// </summary>
        public List<Author> AuthorArray { get; set; }
    }
}
