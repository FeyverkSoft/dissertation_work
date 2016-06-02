using System;
using System.Runtime.Serialization;
using DbHelper.Attributes;

namespace Biblio.db.Entity
{
    public class Author
    {
        /// <summary>
        /// Фамилия автора
        /// </summary>        
        [DataMember]
        [SqlFieldOptional]
        public String AuthorFamily { get; set; }
        /// <summary>
        /// Инициалы автора
        /// </summary>
        [DataMember]
        [SqlFieldOptional]
        public String AuthorTrails { get; set; }
    }
}
