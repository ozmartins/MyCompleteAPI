using Hard.Business.Models;
using System;
using System.Threading.Tasks;

namespace Hard.Business.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<Supplier> RecoverWithAddress(Guid supplierId);

        Task<Supplier> RecoverWithAddressAndProducts(Guid supplierId);
    }
}
