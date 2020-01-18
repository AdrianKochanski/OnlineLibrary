using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookGUI.Services;
using Microsoft.AspNetCore.Mvc;
using BookApiProject.Dtos;
using BookGUI.ViewModels;

namespace BookGUI.Controllers
{
    public class ReviewersController : Controller
    {
        private IReviewerRepositoryGUI _reviewerRepository;
        private IReviewRepositoryGUI _reviewRepository;

        public ReviewersController(
            IReviewerRepositoryGUI reviewerRepository,
            IReviewRepositoryGUI reviewRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }

        public IActionResult Index()
        {
            var reviewers = _reviewerRepository.GetReviewers();
            if(reviewers.Count() <= 0)
            {
                ViewBag.Message = "Ther was a problem retrieving reviewers from " +
                    "the database or no reviever exists";
            }
            return View(reviewers);
        }

        public IActionResult GetReviewerById(int reviewerId)
        {
            var reviewer = _reviewerRepository.GetReviewerById(reviewerId);
            if (reviewer == null)
            {
                ModelState.AddModelError("", "Error getting a reviewer");
                ViewBag.Reviewer = $"There was a problem retrieving reviewer with Id={reviewerId} " +
                    $"from the database or no reviewer with this id exists";
                reviewer = new ReviewerDto();
            }

            var reviews = _reviewerRepository.GetReviewsByAReviewer(reviewerId);
            if (reviews.Count() <= 0)
            {
                ViewBag.ReviewsMessage = $"Reviewer {reviewer.FirstName} {reviewer.LastName} " +
                    $"has no any reviews at this time, please come back later";
            }

            IDictionary<ReviewDto, BookDto> reviewAndBook = new Dictionary<ReviewDto, BookDto>();
            foreach(var review in reviews)
            {
                var book = _reviewRepository.GetBookOfAReview(review.Id);
                reviewAndBook.Add(review, book);
            }

            var reviewerReviewsBooksViewModel = new ReviewerReviewsBooksViewModel()
            {
                Reviewer = reviewer,
                ReviewBook = reviewAndBook
            };

            return View(reviewerReviewsBooksViewModel);
        }
    }
}