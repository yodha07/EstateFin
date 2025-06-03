using EstateFin.Models;

namespace EstateFin.Repositories
{
    public interface ILeaseTenantRepo
    {

        List<LeaseAgreement> GetLeasesByTenantId(int id);
        LeaseAgreement GetLeaseById(int id);
        void AcceptLeaseAgreement(int id);
        void RejectLeaseAgreement(int id);
        void PaySecurityDeposit(int id);
        List<Rent> GetRentByLeaseId(int id);
        Rent GetRentById(int id);
        void PayRent(int id);
    }
}
