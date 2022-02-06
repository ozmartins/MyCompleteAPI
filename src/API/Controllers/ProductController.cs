using API.ViewModels;
using AutoMapper;
using Hard.Business.Interfaces;
using Hard.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : MainController
    {
        IProductRepository _productRepository;
        IProductService _productService;
        IMapper _mapper;

        public ProductController(IProductRepository productRepository, IProductService productService, IMapper mapper, INotifier notifier) : base(notifier)
        {
            _productRepository = productRepository;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> Post(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);            

            productViewModel.Image = Guid.NewGuid().ToString();

            UpdateFile(productViewModel.Image, productViewModel.ImageUpload);
           
            await _productService.Create(_mapper.Map<Product>(productViewModel));

            return CustomResponse(productViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> Put(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return BadRequest("id != model.Id");

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _productService.Update(_mapper.Map<Product>(productViewModel));

            return CustomResponse(productViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> Delete(Guid id)
        {
            var product = _productRepository.Recover(id);

            if (product == null) return NotFound();

            await _productService.Delete(id);

            return CustomResponse(product);
        }

        [HttpGet]
        public async Task<IEnumerable<ProductViewModel>> Get()
        {
            return _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.RecoverAll());
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> Get(Guid id)
        {
            return _mapper.Map<ProductViewModel>(await _productRepository.Recover(id));
        }

        private bool UpdateFile(string file, string fileName)
        {
            var imageDataByteArray = Convert.FromBase64String(file);

            if (string.IsNullOrEmpty(file)) NotifyError("Product image is required.");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            if (System.IO.File.Exists(filePath)) NotifyError("File already exists.");

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }
    }
}
