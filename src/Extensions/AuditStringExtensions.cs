using System;

namespace SAFT_Reader.Extensions
{
    /// <summary>
    /// Extension methods for converting and processing string values commonly used in audit data.
    /// </summary>
    public static class AuditStringExtensions
    {
        /// <summary>
        /// Converts a string to a float value, optionally rounding it to two decimal places.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="round">Specify whether to round the result (default is true).</param>
        /// <returns>The float value converted from the string.</returns>
        public static float ToFloat(this string s, bool round = true)
        {
            var r = float.Parse(s.ToDecimal());
            if (round)
            {
                return r.Round();
            }
            else
            {
                return r;
            }
        }

        /// <summary>
        /// Rounds a float value to two decimal places.
        /// </summary>
        /// <param name="f">The float value to round.</param>
        /// <returns>The rounded float value.</returns>
        public static float Round(this float f)
        {
            return (float)Math.Round(f, 2);
        }

        /// <summary>
        /// Converts a string by replacing the decimal point with the system-specific decimal separator.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted string with the appropriate decimal separator.</returns>
        public static string ToDecimal(this string s)
        {
            return s.Replace(".", Globals.NumberDecimalSeparator.ToString());
        }

        /// <summary>
        /// Converts a string to an integer value.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The integer value converted from the string.</returns>
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }

        /// <summary>
        /// Converts a string representing an account grouping category code to its description.
        /// </summary>
        /// <param name="s">The account grouping category code.</param>
        /// <returns>The corresponding account grouping category description.</returns>
        public static string ToAccountGroupCatDesc(this string s)
        {
            switch (s)
            {
                case "GR":
                    return "Razão - CG";

                case "GA":
                    return "Agregadora - CG";

                case "GM":
                    return "Movimento - CG";

                case "AR":
                    return "Razão - CA";

                case "AA":
                    return "Agregadora - CA";

                case "AM":
                    return "Movimento - CA";

                default:
                    return "";
            }
        }
    }
}