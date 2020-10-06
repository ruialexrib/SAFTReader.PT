using System;
using System.Collections.Generic;

namespace SAFT_Reader.Models
{
    public class AuditReport
    {
        public DateTime AuditDate { get; set; }
        public List<AuditError> AuditErrorList { get; set; }
    }

    public class AuditError
    {
        public string ErrorDescription { get; set; }
        public string AuditElement { get; set; }
        public AuditErrorType AuditErrorType { get; set; }
    }

    public enum AuditErrorType
    {
        Error = 0,
        Warning = 1
    }
}
