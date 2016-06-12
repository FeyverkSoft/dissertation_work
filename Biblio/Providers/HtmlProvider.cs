using System;
using Biblio.Helpers;

namespace Biblio.Providers
{
    public class HtmlProvider : IParseProvider
    {
        public String Parse(String input)
        {
            var result = String.Empty;
            var count = 0;
            var tempStr = String.Empty;
            if (String.IsNullOrEmpty(input))
                return input;
            foreach (var ch in input)
            {
                tempStr += ch;
                switch (ch)
                {
                    case '<':
                        count++;
                        break;
                    case '>':
                        count++;
                        if (count < 3)
                        {
                            result += tempStr;
                        }
                        count = 0;
                        tempStr = String.Empty;
                        break;
                    default:
                        if (count == 0)
                            result += ch;
                        else
                            count++;
                        break;
                }
            }
            result = result.Replace("&nbsp;", "");
            return result.Trim().RemoveWhitespace();
        }
    }
}
