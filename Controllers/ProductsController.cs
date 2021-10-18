using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using GerenciarAvaliacoesDeProdutos.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevReviews.API.Controllers
{
    [ApiController] //anotações
    [Route("api/[controller]")] //anotações
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        /*
        => Construtor que receve o objeto DevReviewsDbContexto como parâmetro
        => Essa injeção de dependência precisa ser configurada no startp.cs
        */
        private readonly IMapper _mapper;
        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        // GET para api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _repository.GetAllAsync();

            //var productsViewModel = products.Select(p => new ProductViewModel(p.Id, p.Title, p.Price));
            var productsViewModel = _mapper.Map<List<ProductViewModel>>(products);
            return Ok(productsViewModel);
        }

        // GET para api/products/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // se não achar, retornar not found
            /*
            => expressão lambida p => p.Id == id
            */
            var product = await _repository.GetDetailsByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // ProductReview para ProductReviewViewModel
            var productDetails = _mapper.Map<ProductDetailsViewModel>(product);

            return Ok(productDetails);
        }

        // POST para api/products
        [HttpPost]
        public async Task<IActionResult> Post(AddProductInputModel model)
        {
            // se tiver erros de validação retornar bad request
            // string title, string description, decimal price
            var product = new Product(model.Title, model.Description, model.Price);

            await _repository.AddAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = 1 }, model);
        }

        // PUT para api/product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> put(int id, UpdateProductInputModel model)
        {
            // se tiver erros de validação retornar BadRequest()
            // se não existir produto com o id especificado, retornar NotFoud()
            if (model.Description.Length > 50)
            {
                return BadRequest();
            }

            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Update(model.Description, model.Price);
            await _repository.UpdateAsync(product);

            return NoContent();
        }
    }
}