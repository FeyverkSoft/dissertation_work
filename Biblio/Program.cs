using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            Random r = new Random();
            var impl = new Impl(connectionString);
            IParseProvider fp = new HtmlProvider();

            var u = impl.GetRawDocList(10100000);
            Console.ReadLine();



            foreach (var raw in u)
            {
                var face = fp.Parse(raw.WebFace);
                if (raw.Title != null)
                    impl.AddRecord(new AddRecord
                    {
                        BookShelfSeat = raw.BookShelfSeat,
                        Title = raw.Title,
                        Face = face,
                        AuthorArray = new List<Author>()
                    });
                //foreach (var word in TextHandl(raw.Title))
                //{
                //    impl.SaveRecord(word, raw.RecordId);
                //}
            }
            Console.ReadLine();
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
