using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookApiProject.Dtos;

namespace BookGUI.Services
{
    public class AuthorRepositoryGUI : IAuthorRepositoryGUI
    {
        public AuthorDto GetAuthorById(int authorId)
        {
            AuthorDto author = new AuthorDto();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"authors/{authorId}");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var readTask = response.Result.Content.ReadAsAsync<AuthorDto>();
                    readTask.Wait();
                    author = readTask.Result;
                }
            }
            return author;
        }

        public IEnumerable<AuthorDto> GetAuthors()
        {
            IEnumerable<AuthorDto> authors = new List<AuthorDto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync("authors");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var readTask = response.Result.Content.ReadAsAsync<IList<AuthorDto>>();
                    readTask.Wait();
                    authors = readTask.Result;
                }
            }
            return authors;
        }

        public IEnumerable<AuthorDto> GetAuthorsOfABook(int bookId)
        {
            IEnumerable<AuthorDto> authors = new List<AuthorDto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"authors/books/{bookId}");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var readTask = response.Result.Content.ReadAsAsync<IList<AuthorDto>>();
                    readTask.Wait();
                    authors = readTask.Result;
                }
            }
            return authors;
        }

        public IEnumerable<BookDto> GetBooksByAuthor(int authorId)
        {
            IEnumerable<BookDto> books = new List<BookDto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"authors/{authorId}/books");
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
