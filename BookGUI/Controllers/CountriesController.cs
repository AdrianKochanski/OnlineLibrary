using BookGUI.Services;
using BookApiProject.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookGUI.ViewModels;
using BookApiProject.Models;
using System.Net.Http;

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

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(countryAuthors);
        }

        [HttpGet]
        public IActionResult CreateCountry()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCountry(Country country)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var responseTask = client.PostAsJsonAsync("countries", country);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var newCountryTask = result.Content.ReadAsAsync<Country>();
                    newCountryTask.Wait();
                    var newCountry = newCountryTask.Result;
                    TempData["SuccessMessage"] = $"Country {newCountry.Name} was successfully created.";
                    return RedirectToAction("GetCountryById", new { countryId = newCountry.Id });
                }
                if((int)result.StatusCode == 422)
                {
                    ModelState.AddModelError("", "Country Already Exists!");
                } else {
                    ModelState.AddModelError("", "Some kind of error. Country not created!");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult UpdateCountry(int countryId)
        {
            var countryToUpdate = _countryRepository.GetCountryById(countryId);
            if(countryToUpdate == null)
            {
                ModelState.AddModelError("", "Error getting country");
                countryToUpdate = new CountryDto();
            }
            return View(countryToUpdate);
        }

        [HttpPost]
        public IActionResult UpdateCountry(CountryDto countryToUpdate)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var responseTask = client.PutAsJsonAsync($"countries/{countryToUpdate.Id}", countryToUpdate);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"Country {countryToUpdate.Name} was successfully updated.";
                    return RedirectToAction("GetCountryById", new { countryId = countryToUpdate.Id });
                }
                if ((int)result.StatusCode == 422)
                {
                    ModelState.AddModelError("", "Country Already Exists!");
                }
                else
                {
                    ModelState.AddModelError("", "Some kind of error. Country not created!");
                }
            }
            var countryDto = _countryRepository.GetCountryById(countryToUpdate.Id);
            return View(countryDto);
        }
    }
}
