using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookApiProject.Dtos;

namespace BookGUI.Services
{
    public class BookRepositoryGUI : IBookRepositoryGUI
    {
        public BookDto GetBookById(int bookId)
        {
            BookDto book = new BookDto();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"books/{bookId}");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var readTask = response.Result.Content.ReadAsAsync<BookDto>();
                    readTask.Wait();
                    book = readTask.Result;
                }
            }
            return book;
        }

        public BookDto GetBookByIsbn(string bookIsbn)
        {
            BookDto book = new BookDto();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"books/ISBN/{bookIsbn}");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var readTask = response.Result.Content.ReadAsAsync<BookDto>();
                    readTask.Wait();
                    book = readTask.Result;
                }
            }
            return book;
        }

        public decimal GetBookRating(int bookId)
        {
            decimal rating = 0.0m;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"books/{bookId}/rating");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var readTask = response.Result.Content.ReadAsAsync<decimal>();
                    readTask.Wait();
                    rating = readTask.Result;
                }
            }
            return rating;
        }

        public IEnumerable<BookDto> GetBooks()
        {
            IEnumerable<BookDto> books = new List<BookDto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync("books");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var readTask = response.Result.Content.ReadAsAsync<IList<BookDto>>();
                    readTask.Wait();
                    books = readTask.Result;
                }
            }
            return books;
        }
    }
}
