using System;
using System.Collections.Generic;
using System.Linq;
using Biblio.db;
using Biblio.db.DbParamsObj;
using Biblio.db.Entity;
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

            var autors = RemoveWhitespace(authors).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);//авторы разделены запятухой
            foreach (var a in autors.Select(x => x.Trim()).Where(x => x.Length > 1))
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
        /// <summary>
        /// Рекурсивный метод удаления ришних пробелов
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        static String RemoveWhitespace(String @string)
        {
            var res = @string.Replace("  ", " ");
            if (res.Length != @string.Length)
                return RemoveWhitespace(res);
            return res;
        }
        static String[] TextHandl(String text)
        {
            var splitters = new[]
            {
                ' ', '.', ',', ';', ':', '?', '!', '(', ')', '"', '}',
                '{', '>', '<', '=', '\\', '/', '|', '\r', '\n', '\t',
                '$', '%', '*', '@', '№', '[', ']'
            };
            return text?.ToLower().Split(splitters, StringSplitOptions.RemoveEmptyEntries).Where(x => !String.IsNullOrEmpty(x) && x.Length > 1 && Char.IsLetter(x[0])).ToArray();
        }
    }
}
