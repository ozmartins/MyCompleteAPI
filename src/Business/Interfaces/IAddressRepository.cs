using Hard.Business.Models;
using System;
using System.Threading.Tasks;

namespace Hard.Business.Interfaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address> RecoverBySupplier(Guid supplierId);
    }
}
