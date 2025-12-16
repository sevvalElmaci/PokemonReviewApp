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
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewers);

        }
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)] 

        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);

        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByAReviewer(int reviewerId)
        {

            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviews = _mapper.Map<List<ReviewDto>>(
                _reviewerRepository.GetReviewsByAReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateReviewer([FromBody] ReviewerDtoCreate reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            //var reviewers = _reviewerRepository.GetReviewers()
            //    .Where(c => c.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper())
            //    .FirstOrDefault();

            //if (reviewers != null)
            //{
            //    ModelState.AddModelError("", "Reviewer alreadry exists");
            //    return StatusCode(422, ModelState);
            //}

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_reviewerRepository.CreateReviewer(reviewerMap, userId))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");


        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if (UpdateReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != updatedReviewer.Id)
                return BadRequest("That ID duo are not matching");

            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound("Reviewer with that id not found");

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewerMap = _mapper.Map<Reviewer>(updatedReviewer);
            int userId = int.Parse(User.FindFirst("userId").Value);


            if (!_reviewerRepository.UpdateReviewer(reviewerMap, userId))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [Authorize(Policy = "Reviewer.Restore")]
        [HttpPost("restore/{reviewerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RestoreReviewer(int reviewerId)
        {
            var reviewer = _reviewerRepository.GetReviewerIncludingDeleted(reviewerId);

            if (reviewer == null)
                return NotFound("Reviewer not found.");

            if (!reviewer.IsDeleted)
                return BadRequest("Reviewer is already active.");

            //// Prevent duplicate active reviewer
            //var conflict = _reviewerRepository.GetReviewers()
            //    .FirstOrDefault(r =>
            //        r.FirstName.Trim().ToUpper() == reviewer.FirstName.Trim().ToUpper() &&
            //        r.LastName.Trim().ToUpper() == reviewer.LastName.Trim().ToUpper() &&
            //        r.Id != reviewer.Id);

            //if (conflict != null)
            //{
            //    ModelState.AddModelError("", "An active reviewer with the same name already exists.");
            //    return StatusCode(422, ModelState);
            //}

            if (!_reviewerRepository.RestoreReviewer(reviewer))
            {
                ModelState.AddModelError("", "Something went wrong while restoring reviewer.");
                return StatusCode(500, ModelState);
            }

            return Ok("Reviewer restored successfully.");
        }

        [Authorize(Policy = "Reviewer.ListDeleted")]
        [HttpGet("deleted")]
        [ProducesResponseType(200)]
        public IActionResult GetDeletedReviewers()
        {
            var deleted = _reviewerRepository
                .GetReviewersIncludingDeleted()
                .Where(r => r.IsDeleted)
                .ToList();

            var reviewersDto = _mapper.Map<List<ReviewerDto>>(deleted);

            return Ok(reviewersDto);
        }


        [Authorize(Policy = "Reviewer.Delete")]
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_reviewerRepository.SoftDeleteReviewer(reviewerId, userId))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    

}
}

