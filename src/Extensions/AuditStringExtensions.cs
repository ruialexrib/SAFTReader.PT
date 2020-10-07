using System;

namespace SAFT_Reader.Extensions
{
    public static class AuditStringExtensions
    {
        public static float ToAuditFloat(this string s, bool round = true)
        {
            var r = float.Parse(s.ToAuditDecimalString());
            if (round)
            {
                return r.ToAuditRound();
            }
            else
            {
                return r;
            }
        }

        public static float ToAuditRound(this float f)
        {
            return (float)Math.Round(f, 2);
        }

        public static string ToAuditDecimalString(this string s)
        {
            return s.Replace(".", ",");
        }

        public static int ToAuditInt(this string s)
        {
            return int.Parse(s);
        }

        public static string ToAuditGroupingCategoryDesc(this string s)
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
