using EstateFin.Models;

namespace EstateFin.ILeaseRepo
{
    public interface ILeaseRepo
    {
       
            List<LeaseAgreement> GetAll();
            LeaseAgreement GetById(int id);
            void Add(LeaseAgreement lease);
            void Update(LeaseAgreement lease);
            void Delete(int id);
            void Save();
        

    }
}
