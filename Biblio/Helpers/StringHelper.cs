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
    }
}
