using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookGUI.Services;
using BookGUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BookApiProject.Dtos;

namespace BookGUI.Controllers
{
    public class HomeController : Controller
    {
        private IBookRepositoryGUI _bookRepository;
        private IAuthorRepositoryGUI _authorRepository;
        private ICountryRepositoryGUI _countryRepository;
        private ICategoryRepositoryGUI _categoryRepository;
        private IReviewerRepositoryGUI _reviewerRepository;
        private IReviewRepositoryGUI _reviewRepository;

        public HomeController(
            IBookRepositoryGUI bookRepository,
            IAuthorRepositoryGUI authorRepository, 
            ICountryRepositoryGUI countryRepository, 
            ICategoryRepositoryGUI categoryRepository, 
            IReviewerRepositoryGUI reviewerRepository, 
            IReviewRepositoryGUI reviewRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _countryRepository = countryRepository;
            _categoryRepository = categoryRepository;
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }

        public IActionResult Index()
        {
            var books = _bookRepository.GetBooks();
            if(books.Count() <= 0)
            {
                ViewBag.Message = "Ther was a problem retrieving books from " +
                    "the database or no book   exists";
            }

            var bACRViewModel = new List<BACRViewModel>();

            foreach(var book in books)
            {
                var authors = _authorRepository.GetAuthorsOfABook(book.Id).ToList();
                if(authors.Count() <= 0)
                {
                    ModelState.AddModelError("", "Error getting authors");
                }

                var categories = _categoryRepository.GetAllCategoriesOfABook(book.Id).ToList();
                if (categories.Count() <= 0)
                {
                    ModelState.AddModelError("", "Error getting categories");
                }

                var rating = _bookRepository.GetBookRating(book.Id);

                bACRViewModel.Add(new BACRViewModel {
                    Authors = authors,
                    Book = book,
                    Categories = categories,
                    Rating = rating
                });
            }

            return View(bACRViewModel);
        }

        public IActionResult GetBookById(int bookId)
        {
            var ComplBViewModel = new CompleteBookViewModel
            {
                AuthorCountry = new Dictionary<AuthorDto, CountryDto>(),
                ReviewReviewer = new Dictionary<ReviewDto, ReviewerDto>()
            };

            var book = _bookRepository.GetBookById(bookId);
            if(book == null)
            {
                ModelState.AddModelError("", "Some kind of error during getting a book");
                book = new BookDto();
            }

            var categories = _categoryRepository.GetAllCategoriesOfABook(bookId);
            if (categories.Count() <= 0)
            {
                ModelState.AddModelError("", "Some kind of error during getting categories");
            }

            var rating = _bookRepository.GetBookRating(bookId);
            ComplBViewModel.Book = book;
            ComplBViewModel.Categories = categories;
            ComplBViewModel.Rating = rating;

            var authors = _authorRepository.GetAuthorsOfABook(bookId);
            if (authors.Count() <= 0)
            {
                ModelState.AddModelError("", "Some kind of error during getting categories");
            }
            foreach(var author in authors)
            {
                var country = _countryRepository.GetCountryOfAnAuthor(author.Id);
                ComplBViewModel.AuthorCountry.Add(author, country);
            }

            var reviews = _reviewRepository.GetReviewsOfABook(bookId);
            if(reviews.Count() <= 0)
            {
                ViewBag.ReviewsMessage = "There are no reviews yet";
            }
            foreach (var review in reviews)
            {
                var reviewer = _reviewerRepository.GetReviewerOfAReview(review.Id);
                ComplBViewModel.ReviewReviewer.Add(review, reviewer);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.BookMessage = "Teher was an error retrieving a complete book record";
            }

            return View(ComplBViewModel);
        }
    }
}