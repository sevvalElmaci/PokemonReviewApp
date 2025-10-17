using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(IPokemonRepository pokemonRepository,
            IReviewRepository reviewRepository, 
            IReviewerRepository reviewerRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper
                .Map<List<ReviewDto>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review = _mapper
                .Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet("pokemon/{pokeId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var reviews = _mapper.Map<List<ReviewDto>>(
                _reviewRepository.GetReviewsForAPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDtoCreate reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);
            //    .Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper())
            //    .FirstOrDefault();
            //if (reviews != null)
            //{
            //    ModelState.AddModelError("", "Review already exists");
            //    return StatusCode(422, ModelState);
            //}

            var reviewer = _reviewerRepository.GetReviewer(reviewerId);
            if (reviewer == null)
                return NotFound("Reviewer with that id not found");

            var review = _reviewRepository.GetReviews();
            if (review == null)
                return NotFound("Review with that id not found");

            var pokemon = _pokemonRepository.GetPokemon(pokeId);
            if (pokemon == null)
                return NotFound("Pokemon with that id not found");


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState); 
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (UpdateReview == null)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest("That ID duo are not matching");

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound("Review with that id not found");

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewMap = _mapper.Map<Review>(updatedReview);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting review");
                return StatusCode(500, ModelState);

                
            }

            return NoContent();
        }

    }
}
