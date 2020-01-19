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
    public class ReviewsController : Controller
    {
        IReviewRepositoryGUI _reviewRepository;
        IReviewerRepositoryGUI _reviewerRepository;

        public ReviewsController(
            IReviewRepositoryGUI reviewRepository,
            IReviewerRepositoryGUI reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
        }

        public IActionResult Index()
        {
            var reviews = _reviewRepository.GetReviews();
            if (reviews.Count() <= 0)
            {
                ViewBag.Message = "Ther was a problem retrieving reviews from " +
                    "the database or no reviews exists";
            }
            return View(reviews);
        }

        public IActionResult GetReviewById(int reviewId)
        {
            var review = _reviewRepository.GetReviewById(reviewId);
            if(review == null)
            {
                ModelState.AddModelError("", "Error getting a review");
                ViewBag.Review = $"There was a problem retrieving review with Id={reviewId} " +
                    $"from the database or no review with this id exists";
                review = new ReviewDto();
            }

            var reviewer = _reviewerRepository.GetReviewerOfAReview(reviewId);
            if (reviewer == null)
            {
                ModelState.AddModelError("", "Error getting a reviewer");
                ViewBag.Reviewer = $"There was a problem retrieving reviewer of review Id={reviewId} " +
                    $"from the database or no reviewer exists";
                reviewer = new ReviewerDto();
            }

            var book = _reviewRepository.GetBookOfAReview(reviewId);
            if (book == null)
            {
                ModelState.AddModelError("", "Error getting a book");
                ViewBag.Book = $"There was a problem retrieving book of review Id={reviewId} " +
                    $"from the database or no book exists";
                book = new BookDto();
            }

            var reviewReviewerBookViewModel = new ReviewReviewerBookViewModel()
            {
                Review = review,
                Reviewer = reviewer,
                Book = book
            };

            return View(reviewReviewerBookViewModel);
        }
    }
}