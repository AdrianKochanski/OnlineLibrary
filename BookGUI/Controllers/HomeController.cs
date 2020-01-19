using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookGUI.Services;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
    }
}