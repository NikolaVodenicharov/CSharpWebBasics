using System.Linq;

namespace SimpleMvc.Framework.Helpers
{
    public static class StringExtension
    {
        public static string Capitalize(this string input)
        {
            if (input == null || input.Length == 0)
            {
                return input;
            }

            var firstLetter = char.ToUpper(input.First());
            var rest = input.Substring(1);

            return $"{firstLetter}{rest}";
        }
    }
}
