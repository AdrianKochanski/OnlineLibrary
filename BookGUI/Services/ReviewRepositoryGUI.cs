﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookApiProject.Dtos;

namespace BookGUI.Services
{
    public class ReviewRepositoryGUI : IReviewRepositoryGUI
    {
        public BookDto GetBookOfAReview(int reviewId)
        {
            BookDto book = new BookDto();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"reviews/{reviewId}/book");
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

        public ReviewDto GetReviewById(int reviewId)
        {
            ReviewDto review = new ReviewDto();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"reviews/{reviewId}");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var readTask = response.Result.Content.ReadAsAsync<ReviewDto>();
                    readTask.Wait();
                    review = readTask.Result;
                }
            }
            return review;
        }

        public IEnumerable<ReviewDto> GetReviews()
        {
            IEnumerable<ReviewDto> reviews = new List<ReviewDto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync("reviews");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var taskList = response.Result.Content.ReadAsAsync<List<ReviewDto>>();
                    taskList.Wait();
                    reviews = taskList.Result;
                }
            }
            return reviews;
        }

        public IEnumerable<ReviewDto> GetReviewsOfABook(int bookId)
        {
            IEnumerable<ReviewDto> reviews = new List<ReviewDto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"reviews/books/{bookId}");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var readTask = response.Result.Content.ReadAsAsync<IList<ReviewDto>>();
                    readTask.Wait();
                    reviews = readTask.Result;
                }
            }
            return reviews;
        }
    }
}
