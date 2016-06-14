using System;
using System.Collections.Generic;
using System.Linq;
using Biblio.db;
using Biblio.db.DbParamsObj;
using Biblio.db.Entity;
using Biblio.Helpers;
using Biblio.Providers;

namespace Biblio
{
    class Program
    {
        static String connectionString = @"Data Source=PETER-LAPTOP\SQLEXPRESS;Initial Catalog=magister;Integrated Security=True;";

        static void Main(string[] args)
        {
            var impl = new Impl(connectionString);
            IParseProvider fp = new HtmlProvider();

            var u = impl.GetRawDocList(10100000);

            foreach (var raw in u)
            {
                var face = fp.Parse(raw.WebFace);
                if (raw.Title != null)
                    impl.AddRecord(new AddRecord
                    {
                        Face = face,
                        Title = raw.Title,
                        NumOfBooks = raw.NumOfBooks,
                        BookShelfSeat = raw.BookShelfSeat,
                        YearOfPublishing = raw.YearOfPublishing,
                        AuthorArray = ParseAuthor(raw.Authors)
                    });
                var splFace = face.TextSplit();
                foreach (var s in splFace)
                {
                    impl.AddFulltextRecord(new AddFullTextRecord { });
                }
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Распарсить список авторов
        /// </summary>
        /// <param name="authors"></param>
        /// <returns></returns>
        static List<Author> ParseAuthor(String authors)
        {
            var res = new List<Author>();
            if (String.IsNullOrEmpty(authors))
                return null;

            var autors = authors.RemoveWhitespace().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);//авторы разделены запятухой
            foreach (var a in autors.Select(x => x.Trim().RemoveWhitespace()).Where(x => x.Length > 1))
            {
                var wh = a.IndexOf(' ');
                if (wh > 0)
                    res.Add(new Author
                    {
                        AuthorTrails = a.Substring(a.IndexOf(' ')),
                        AuthorFamily = a.Substring(0, a.IndexOf(' '))
                    });
                else
                    res.Add(new Author { AuthorFamily = a });
            }
            return res;
        }
    }
}
