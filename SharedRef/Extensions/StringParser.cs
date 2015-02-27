using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SharedRef.Helper;

namespace SharedRef.Extensions.StringParser
{
    public static class StringParser
    {

        public static Dictionary<string, string> ParseNameValues(this string input, char separator)
        {
            return Regex.Matches(input, string.Format(
                @"(?<Name>(?:\\.|[^={0}]+)*)=(?<Value>\'(?:\\.|[^\'\\]+)*\'|(?:\\.|[^{0}\'\\]+)*)", separator))
                .Cast<Match>().ToDictionary(m => m.Groups["Name"].Value.Trim(), m => m.Groups["Value"].Value.Trim().Trim('\''));
        }

        public static List<string> ParseSemicolonSeparated(this string input)
        {
            return Regex.Matches(input, @"(?:(?<=')([^']*)(?='))|(?<=;|^)([^;]*)(?=;|$)")
                .Cast<Match>().Select(m => m.Value.Trim()).ToList();
        }

        public static List<string> ParseCommaSeparated(this string input)
        {
            return Regex.Matches(input, @"(?:(?<=')([^']*)(?='))|(?<=,|^)([^,]*)(?=,|$)")
                .Cast<Match>().Select(m => m.Value.Trim()).ToList();
        }

        public static List<string> ParseFunction(this string input)
        {
            var func = Regex.Match(input, @"(?<Name>.*)\((?<Values>.*)\)");

            if (func.Success == false) 
                return new List<string>();

            var output = func.Groups["Values"].Value.ParseCommaSeparated();

            output.Insert(0, func.Groups["Name"].Value);

            return output;
        }
    }
}
