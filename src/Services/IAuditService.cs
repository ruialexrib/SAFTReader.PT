using Programatica.Saft.Models;

using SAFT_Reader.Models;

namespace SAFT_Reader.Services
{
    /// <summary>
    /// Defines the contract for an auditing service.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Performs an audit operation and returns an audit report.
        /// </summary>
        /// <returns>An AuditReport containing the results of the audit.</returns>
        AuditReport Audit();

        /// <summary>
        /// Merges multiple audit files into a consolidated audit file.
        /// </summary>
        /// <returns>The consolidated AuditFile containing merged data.</returns>
        AuditFile MergeAudits();

        /// <summary>
        /// Opens and deserializes an audit file from the specified path.
        /// </summary>
        /// <param name="path">The file path of the audit file to open and deserialize.</param>
        /// <returns>The deserialized AuditFile object.</returns>
        AuditFile OpenFile(string path);
    }

}