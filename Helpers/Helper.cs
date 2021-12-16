using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPnetAuto.Helpers
{
    public static class Helper
    {
        public static string GetUntilOrEmpty(this string text, string startAt = "[", string stopAt = "]")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(startAt);
                int charLocationStop = text.IndexOf(stopAt);

                if (charLocation > 0)
                {
                    return text.Substring(charLocation, charLocationStop - charLocation + 1);
                }
            }

            return String.Empty;
        }
    }
}
