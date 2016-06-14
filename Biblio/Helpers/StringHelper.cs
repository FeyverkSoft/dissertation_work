using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Рекурсивный метод удаления ришних пробелов
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static String RemoveWhitespace(this String @string)
        {
            var res = @string.Replace("  ", " ");
            return res.Length != @string.Length ? RemoveWhitespace(res) : res;
        }

        /// <summary>
        /// Разрезать строку с учетом символов
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String[] TextSplit(this String text)
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
