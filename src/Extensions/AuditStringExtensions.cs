namespace SAFT_Reader.Extensions
{
    public static class AuditStringExtensions
    {
        public static float ToAuditFloat(this string s)
        {
            return float.Parse(s.ToAuditDecimalString());
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
