using System.Linq;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using DevReviews.API.Persistence;
using gerenciarAvaliacoesDeProdutos.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/productreviews")]
    public class ProductReviewsController : ControllerBase
    {
        private readonly DevReviewsDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductReviewsController(DevReviewsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        
        // GET api/products/1/productreviews/5
        [HttpGet("{id}")]
        public IActionResult GetById(int productId, int id){
            // Se não existir com o id especificado, retornar NotFound()
            var productReview = _dbContext.ProductReviews.SingleOrDefault(p => p.Id == id);

            if(productReview == null){
                return NotFound();
            }
            var productDetails = _mapper.Map<ProductReviewDetailsViewModel>(productReview);

            return Ok(productDetails);
        }

        // POST api/products/1/productsreviews
        [HttpPost]
        public IActionResult Post(int productId, AddProductReviewInputModel model){
            // se estiverr com os dadps invalidos, retornar BadRequest()
            var productReview = new ProductReview(model.Author, model.Rating, model.Comments, productId);

            _dbContext.ProductReviews.Add(productReview);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = productReview.Id, productId = productId }, model);
        }
    }
}