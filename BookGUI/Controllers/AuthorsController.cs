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
    public class AuthorsController : Controller
    {
        private IAuthorRepositoryGUI _authorRespository;
        private ICountryRepositoryGUI _countryRepository;
        private ICategoryRepositoryGUI _categoryRepository;

        public AuthorsController(
            IAuthorRepositoryGUI authorRespository,
            ICountryRepositoryGUI countryRepository,
            ICategoryRepositoryGUI categoryRepository)
        {
            _authorRespository = authorRespository;
            _countryRepository = countryRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var authors = _authorRespository.GetAuthors();
            if (authors.Count() <= 0)
            {
                ViewBag.Message = "There was a problem retrieving authors from " +
                    "the database or no author exists";
            }
            return View(authors);
        }

        public IActionResult GetAuthorById(int authorId)
        {
            var author = _authorRespository.GetAuthorById(authorId);
            if(author == null)
            {
                ModelState.AddModelError("", $"There is no author with id:{authorId}");
                ViewBag.Message = $"There was a problem retrieving author with id {authorId} " +
                    $"from the database or no author with this id exists";
                author = new AuthorDto();
            }

            var country = _countryRepository.GetCountryOfAnAuthor(authorId);
            if (country == null)
            {
                ModelState.AddModelError("", "Error getting a country");
                ViewBag.Message += $"There was a problem retrieving country of an author with Id={authorId} " +
                    $"from the database or no country exists for this author";
                country = new CountryDto();
            }

            var books = _authorRespository.GetBooksByAuthor(authorId);
            IDictionary<BookDto, IEnumerable<CategoryDto>> bookCategories = new Dictionary<BookDto, IEnumerable<CategoryDto>>();
            if(books.Count() <= 0)
            {
                ViewBag.BookMessage =  $"No books for {author.FirstName} {author.LastName} exists.";
            }
            foreach (var book in books)
            {
                var categories = _categoryRepository.GetAllCategoriesOfABook(book.Id);
                bookCategories.Add(book,categories);
            }

            var ACBCVM = new AuthorCountryBookCategoriesViewModel
            {
                Author = author,
                Country = country,
                BookCategories = bookCategories
            };

            return View(ACBCVM);
        }
    }
}