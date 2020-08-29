using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShrt.Helpers
{
    public class RandomHelper
    {
        private static readonly Random rnd = new Random();

        private const string alphaNumericalChars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        static public string NextAlphanumeric(int len)
        {
            return new string(Enumerable.Repeat(alphaNumericalChars, len).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}
