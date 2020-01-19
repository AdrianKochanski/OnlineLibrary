using BookGUI.Services;
using BookApiProject.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookGUI.ViewModels;

namespace BookGUI.Controllers
{
    public class CountriesController : Controller
    {
        private ICountryRepositoryGUI _countryRepository;

        public CountriesController(ICountryRepositoryGUI countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public IActionResult Index()
        {
            var countries = _countryRepository.GetCountries();

            if (countries.Count()<=0)
            {
                ViewBag.Message = "Ther was a problem retrieving countries from " +
                    "the database or no country exists";
            } 

            return View(countries);
        }

        public IActionResult GetCountryById(int countryId)
        {
            var country = _countryRepository.GetCountryById(countryId);
            if (country == null)
            {
                ModelState.AddModelError("", "Error getting a country");
                ViewBag.Message = $"There was a problem retrieving country with Id={countryId} " +
                    $"from the database or no country with that id exists";
                country = new CountryDto();
            }

            var authors = _countryRepository.GetAuthorsFromACountry(countryId);
            if (authors.Count() <= 0)
            {
                ViewBag.AuthorMessage = $"There was a problem retrieving authors from a country with Id={countryId} " +
                    $"from the database or any author in this country exists";
            }

            var countryAuthors = new CountryAuthorsViewModel
            {
                Authors =  authors,
                Country = country
            };

            return View(countryAuthors);
        }
    }
}
