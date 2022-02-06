using API.ViewModels;
using AutoMapper;
using Hard.Business.Interfaces;
using Hard.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : MainController
    {
        private readonly ISupplierRepository _supplierRepository;

        private readonly IAddressRepository _addressRepository;

        private readonly IMapper _mapper;

        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierRepository supplierRepository, IAddressRepository addressRepository, IMapper mapper, ISupplierService supplierService, INotifier notifier) : base(notifier)
        {
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
            _supplierService = supplierService;
        }        

        [HttpPost]
        public async Task<ActionResult<SupplierViewModel>> Post(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);           

            await _supplierService.Create(_mapper.Map<Supplier>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Put(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id) return BadRequest("id != model.Id");

            if (!ModelState.IsValid) return CustomResponse(ModelState);                        

            await _supplierService.Update(_mapper.Map<Supplier>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Delete(Guid id)
        {
            var supplier = await _supplierRepository.RecoverWithAddress(id);

            if (supplier == null) return NotFound();

            await _supplierService.Delete(id);            

            return CustomResponse(_mapper.Map<SupplierViewModel>(supplier));
        }

        [HttpGet]
        public async Task<IEnumerable<SupplierViewModel>> Get()
        {
            var supplierList = await _supplierRepository.RecoverAll();          

            return _mapper.Map<IEnumerable<SupplierViewModel>>(supplierList);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Get(Guid id)
        {
            var supplier = await _supplierRepository.RecoverWithAddressAndProducts(id);

            if (supplier == null) return NotFound();            

            return _mapper.Map<SupplierViewModel>(supplier);
        }

        [HttpGet("address/{id:guid}")]
        public async Task<ActionResult<AddressViewModel>> GetAddress(Guid id)
        {
            var address = await _addressRepository.Recover(id);

            if (address == null) return NotFound();

            return _mapper.Map<AddressViewModel>(address);
        }

        [HttpPut("address/{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> PutAddress(Guid id, AddressViewModel addressViewModel)
        {
            if (id != addressViewModel.Id) return BadRequest("id != model.Id");

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _supplierService.UpdateAddress(_mapper.Map<Address>(addressViewModel));

            return CustomResponse(addressViewModel);
        }
    }
}
