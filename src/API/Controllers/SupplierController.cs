using API.ViewModels;
using AutoMapper;
using Hard.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class SupplierController : MainController
    {
        private readonly ISupplierRepository _supplierRepository;
        
        private readonly IMapper _mapper;

        public SupplierController(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupplierViewModel>> Get()
        {
            var supplierList = await _supplierRepository.RecoverAll();

            var supplierViewModelList = _mapper.Map<IEnumerable<SupplierViewModel>>(supplierList);
            
            return supplierViewModelList;
        }
    }
}
