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
            while (result.Length != (result = result.Replace("  ", " ")).Length) ;
            return result;
        }
    }
}
