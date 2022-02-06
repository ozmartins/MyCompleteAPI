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
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;

        private readonly IMapper _mapper;

        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierRepository supplierRepository, IMapper mapper, ISupplierService supplierService)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _supplierService = supplierService;
        }

        [HttpPost]
        public async Task<ActionResult<SupplierViewModel>> Post(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            await _supplierService.Create(supplier);

            return Ok(supplierViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Put(Guid id, SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (id != supplierViewModel.Id) return BadRequest();

            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            await _supplierService.Update(supplier);

            return Ok(supplierViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Delete(Guid id)
        {
            var supplier = await _supplierRepository.RecoverWithAddress(id);

            if (supplier == null) return NotFound();

            await _supplierService.Delete(id);

            var supplierViewModel = _mapper.Map<SupplierViewModel>(supplier);

            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<SupplierViewModel>> Get()
        {
            var supplierList = await _supplierRepository.RecoverAll();

            var supplierViewModelList = _mapper.Map<IEnumerable<SupplierViewModel>>(supplierList);

            return supplierViewModelList;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Get(Guid id)
        {
            var supplier = await _supplierRepository.RecoverWithAddressAndProducts(id);

            if (supplier == null) return NotFound();

            var supplierViewModel = _mapper.Map<SupplierViewModel>(supplier);

            return supplierViewModel;
        }
    }
}
