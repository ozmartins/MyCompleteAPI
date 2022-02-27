using API.Extensions;
using API.ViewModels;
using AutoMapper;
using Hard.Business.Interfaces;
using Hard.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        
        [ClaimsAuthorize("supplier", "create")]
        [HttpPost]
        public async Task<ActionResult<SupplierViewModel>> Post(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);           

            await _supplierService.Create(_mapper.Map<Supplier>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }

        [ClaimsAuthorize("supplier", "update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Put(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id) return BadRequest("id != model.Id");

            if (!ModelState.IsValid) return CustomResponse(ModelState);                        

            await _supplierService.Update(_mapper.Map<Supplier>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }

        [ClaimsAuthorize("supplier", "delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Delete(Guid id)
        {
            var supplier = await _supplierRepository.RecoverWithAddress(id);

            if (supplier == null) return NotFound();

            await _supplierService.Delete(id);            

            return CustomResponse(_mapper.Map<SupplierViewModel>(supplier));
        }

        [ClaimsAuthorize("supplier", "recover")]
        [HttpGet]
        public async Task<IEnumerable<SupplierViewModel>> Get()
        {
            var supplierList = await _supplierRepository.RecoverAll();          

            return _mapper.Map<IEnumerable<SupplierViewModel>>(supplierList);
        }

        [ClaimsAuthorize("supplier", "recover")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Get(Guid id)
        {
            var supplier = await _supplierRepository.RecoverWithAddressAndProducts(id);

            if (supplier == null) return NotFound();            

            return _mapper.Map<SupplierViewModel>(supplier);
        }

        [ClaimsAuthorize("supplier", "recover")]
        [HttpGet("address/{id:guid}")]
        public async Task<ActionResult<AddressViewModel>> GetAddress(Guid id)
        {
            var address = await _addressRepository.Recover(id);

            if (address == null) return NotFound();

            return _mapper.Map<AddressViewModel>(address);
        }

        [ClaimsAuthorize("supplier", "update")]
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
