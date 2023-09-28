using System.Linq;

namespace Anura.Extensions
{
    public static class StringExtensionMethods
    {
        public static bool IsEmptyOrWhiteSpace(this string value)
        {
            return value.All(char.IsWhiteSpace);
        }
    }
}