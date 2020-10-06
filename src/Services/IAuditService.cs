using Programatica.Saft.Models;
using SAFT_Reader.Models;

namespace SAFT_Reader.Services
{
    public interface IAuditService
    {
        AuditReport Audit(AuditFile audit);
    }
}
