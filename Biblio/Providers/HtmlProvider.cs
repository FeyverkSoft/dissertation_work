using System;

namespace Biblio.Providers
{
    public class HtmlProvider : IParseProvider
    {
        public String Parse(String input)
        {
            var result = String.Empty;
            var count = 0;
            var tempStr = String.Empty;
            if (input == null)
                return input;
            foreach (var ch in input)
            {
                tempStr += ch;
                if (ch == '<')
                {
                    count++;
                }
                else if (ch == '>')
                {
                    count++;
                    if (count < 3)
                    {
                        result += tempStr;
                    }
                    count = 0;
                    tempStr = String.Empty;
                }
                else if (count == 0)
                    result += ch;
                else
                    count++;
            }
            result = result.Replace("&nbsp;", "");
            return RemoveWhitespace(result.Trim());
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
    }
}
