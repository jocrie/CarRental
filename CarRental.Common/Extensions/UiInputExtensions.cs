using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
}
