using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        // ================================
        // GET ALL ACTIVE
        // ================================
        [Authorize(Policy = "Review.List")]
        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            return Ok(reviews);
        }

        // ================================
        // GET BY ID
        // ================================
        [Authorize(Policy = "Review.List")]
        [HttpGet("{id}")]
        public IActionResult GetReview(int id)
        {
            var review = _reviewRepository.GetReview(id);
            if (review == null) return NotFound();

            return Ok(_mapper.Map<ReviewDto>(review));
        }

        // ================================
        // GET BY POKEMON
        // ================================
        [Authorize(Policy = "Review.List")]
        [HttpGet("pokemon/{pokeId}")]
        public IActionResult GetByPokemon(int pokeId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(
                _reviewRepository.GetReviewsForAPokemon(pokeId)
            );
            return Ok(reviews);
        }

        // ================================
        // GET DELETED
        // ================================
        [Authorize(Policy = "Review.ListDeleted")]
        [HttpGet("deleted")]
        public IActionResult GetDeleted()
        {
            var reviews = _reviewRepository
                .GetReviewsIncludingDeleted()
                .Where(r => r.IsDeleted)
                .ToList();

            return Ok(_mapper.Map<List<ReviewDto>>(reviews));
        }

        // ================================
        // CREATE / RESTORE
        // ================================
        [Authorize(Policy = "Review.Add")]
        [HttpPost]
        public IActionResult Create([FromBody] ReviewDtoCreate dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = int.Parse(User.FindFirst("userId").Value);

            // ⭐ YENİ ENTITY → SADECE FK ALIYOR
            var entity = _mapper.Map<Review>(dto);
            entity.ReviewerId = dto.ReviewerId;
            entity.PokemonId = dto.PokemonId;

            // ⭐ Deleted olanı bulmak için FK ve Title karşılaştırıyoruz
            var existing = _reviewRepository
                .GetReviewsIncludingDeleted()
                .FirstOrDefault(r =>
                    r.ReviewerId == dto.ReviewerId &&
                    r.PokemonId == dto.PokemonId &&
                    r.Title.Trim().ToUpper() == dto.Title.Trim().ToUpper()
                );

            // ⭐ RESTORE
            if (existing != null && existing.IsDeleted)
            {
                existing.Title = entity.Title;
                existing.Text = entity.Text;
                existing.Rating = entity.Rating;
                existing.UpdatedUserId = userId;
                existing.UpdatedDateTime = DateTime.UtcNow;

                _reviewRepository.RestoreReview(existing);
                return Ok(_mapper.Map<ReviewDto>(existing));
            }

            // ⭐ Aktif varsa conflict
            if (existing != null)
                return Conflict("Review already exists.");

            // ⭐ Yeni Review
            _reviewRepository.CreateReview(entity, userId);

            return Ok(_mapper.Map<ReviewDto>(entity));
        }


        // ================================
        // UPDATE
        // ================================
        [Authorize(Policy = "Review.Update")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ReviewDto dto)
        {
            var review = _reviewRepository.GetReviewIncludingDeleted(id);
            if (review == null || review.IsDeleted) return NotFound();

            _mapper.Map(dto, review);

            int userId = int.Parse(User.FindFirst("userId").Value);
            _reviewRepository.UpdateReview(review, userId);

            return NoContent();
        }

        // ================================
        // SOFT DELETE
        // ================================
        [Authorize(Policy = "Review.Delete")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_reviewRepository.ReviewExists(id))
                return NotFound();

            _reviewRepository.SoftDeleteReview(id, userId);

            return NoContent();
        }

        // ================================
        // RESTORE
        // ================================
        [Authorize(Policy = "Review.Restore")]
        [HttpPost("restore/{id}")]
        public IActionResult Restore(int id)
        {
            var review = _reviewRepository.GetReviewIncludingDeleted(id);
            if (review == null || !review.IsDeleted)
                return NotFound();

            _reviewRepository.RestoreReview(review);

            return Ok(_mapper.Map<ReviewDto>(review));
        }
    }
}
