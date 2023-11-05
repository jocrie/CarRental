using System.Text.RegularExpressions;
using System.Globalization;

namespace CarRental.Common.Extensions;

public static class UiInputExtensions
{
    public static bool IsLettersOnly(this string inputString)
    {
        // Letter, space or minus to allow for double names
        Regex regex = new Regex("^[a-zA-Z -]+$");
        return regex.IsMatch(inputString);
    }

    public static bool IsNumber(this string inputString)
    {
        /*No leading 0s*/ 
        Regex regex = new Regex(@"^[1-9]\d*$");
        return regex.IsMatch(inputString);
    }

    public static string Capitalize(this string inputString)
    {
        char[] separators = { ' ', '-' };
        string pattern = "(?<=[" + string.Join("", separators) + "]|^)\\w+";
        return Regex.Replace(inputString, pattern, match => char.ToUpper(match.Value[0]) + match.Value.Substring(1).ToLower());
    }

}
