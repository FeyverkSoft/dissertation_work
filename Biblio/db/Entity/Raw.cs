using System;
using System.Runtime.Serialization;
using DbHelper.Attributes;

namespace Biblio.db.Entity
{
    [Serializable]
    public class Raw: DbEntity
    {
        /// <summary>
        /// Номер записи в базе
        /// </summary>
        [DataMember]
        public Int64 RecordId { get; set; }
        /// <summary>
        /// Заголовок книги
        /// </summary>
        [DataMember]
        [SqlFieldOptional]
        public String Title { get; set; }
        /// <summary>
        /// Издательство
        /// </summary>
        [DataMember]
        [SqlFieldOptional]
        public String Publishers { get; set; }
        /// <summary>
        /// Год публикации, может отсутствовать
        /// </summary>
        [DataMember]
        [SqlFieldOptional]
        public Int64? YearOfPublishing { get; set; }
        /// <summary>
        /// Авторы документа
        /// </summary>
        [DataMember]
        [SqlFieldOptional]
        public String Authors { get; set; }
        /// <summary>
        /// Номер публикации в серии
        /// </summary>
        [DataMember]
        [SqlFieldOptional]
        public Int64? NumOfBooks { get; set; }
        /// <summary>
        /// Веб-морада публикации, она же аннотация
        /// </summary>
        [DataMember]
        [SqlFieldOptional]
        public String WebFace { get; set; }
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
        /// <summary>
        /// Номер книги на книжней полке
        /// </summary>
        [DataMember]
        [SqlFieldOptional]
        public String BookShelfSeat { get; set; }
        /// <summary>
        /// Возвращает строку, представляющую текущий объект.
        /// </summary>
        /// <returns>
        /// Строка, представляющая текущий объект.
        /// </returns>
        public override string ToString()
        {
            return $"RecordId:{RecordId};\tTitle:{Title};";
        }
    }
}
