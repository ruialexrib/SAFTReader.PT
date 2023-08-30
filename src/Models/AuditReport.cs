using System;
using System.Collections.Generic;

namespace SAFT_Reader.Models
{
    /// <summary>
    /// Represents an audit report containing information about audit errors.
    /// </summary>
    public class AuditReport
    {
        public DateTime AuditDate { get; set; }
        public List<AuditError> AuditErrorList { get; set; }
    }

    /// <summary>
    /// Represents an error encountered during an audit.
    /// </summary>
    public class AuditError
    {
        public string ErrorDescription { get; set; }
        public string AuditElement { get; set; }
        public AuditErrorType AuditErrorType { get; set; }
    }

    /// <summary>
    /// Represents an error encountered during an audit.
    /// </summary>
    public enum AuditErrorType
    {
        Error = 0,
        Warning = 1
    }
}