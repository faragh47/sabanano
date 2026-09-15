using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public class CodeGenerator
    {
        public static string CreateCodeWithDate<T>(T Id, DateTime DateTime)
        {
            var date = CalendarExtensions.GetPersianYear(DateTime).ToString("D2") +
                CalendarExtensions.GetPersianMonth(DateTime).ToString("D2") +
                CalendarExtensions.GetPersianDay(DateTime).ToString("D2");

            var output = date + "-" + Id.ToString();

            return output;
        }

        public static string GenerateCode(string code, int length = 6)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateNumberCode(int length = 6)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
