using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using DevReviews.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevReviews.API.Controllers
{
    [ApiController] //anotações
    [Route("api/[controller]")] //anotações
    public class ProductsController : ControllerBase
    {
        private readonly DevReviewsDbContext _dbContext; // cria uma variável readonly do objeto DevReviewsDbContext

        /*
        => Construtor que receve o objeto DevReviewsDbContexto como parâmetro
        => Essa injeção de dependência precisa ser configurada no startp.cs
        */
        private readonly IMapper _mapper;
        public ProductsController(DevReviewsDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext; // a variável recebe o parâmetro
        }

        // GET para api/products
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _dbContext.Products;

            //var productsViewModel = products.Select(p => new ProductViewModel(p.Id, p.Title, p.Price));
            var productsViewModel = _mapper.Map<List<ProductViewModel>>(products);
            return Ok(productsViewModel);
        }

        // GET para api/products/id
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // se não achar, retornar not found
            /*
            => expressão lambida p => p.Id == id
            */
            var product = _dbContext.Products.SingleOrDefault(p => p.Id == id);
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
        public IActionResult Post(AddProductInputModel model)
        {
            // se tiver erros de validação retornar bad request
            // string title, string description, decimal price
            var product = new Product(model.Title, model.Description, model.Price);

            _dbContext.Products.Add(product);

            return CreatedAtAction(nameof(GetById), new { id = 1 }, model);
        }

        // PUT para api/product/{id}
        [HttpPut("{id}")]
        public IActionResult put(int id, UpdateProductInputModel model)
        {
            // se tiver erros de validação retornar BadRequest()
            // se não existir produto com o id especificado, retornar NotFoud()
            if (model.Description.Length > 50)
            {
                return BadRequest();
            }

            var product = _dbContext.Products.SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.Update(model.Description, model.Price);

            return NoContent();
        }
    }
}