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
    public class CategoriesController : Controller
    {
        private ICategoryRepositoryGUI _categoryRepository;

        public CategoriesController(ICategoryRepositoryGUI categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = _categoryRepository.GetCategories();

            if(categories.Count() <= 0)
            {
                ViewBag.Message = "There was a problem retrieving categories from " +
                    "the database or no category exists";
            }
            return View(categories);
        }

        public IActionResult GetCategoryById(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);
            if(category == null)
            {
                ModelState.AddModelError("", $"There is no category with id:{categoryId}");
                ViewBag.Message = $"There was a problem retrieving category with id {categoryId} " + 
                    $"from the database or no category exists";
                category = new CategoryDto();
            }

            var books = _categoryRepository.GetAllBooksForCategory(categoryId);
            if(books.Count() <= 0)
            {
                ViewBag.BookMessage = $"Category: {category.Name} has no books";
            }

            var bookCategoryViewModel = new CategoryBooksViewModel()
            {
                Category = category,
                Books = books
            };


            return View(bookCategoryViewModel);
        }
    }
}