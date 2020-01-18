using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookApiProject.Dtos;

namespace BookGUI.Services
{
    public class ReviewerRepositoryGUI : IReviewerRepositoryGUI
    {
        public ReviewerDto GetReviewerById(int reviewerId)
        {
            ReviewerDto reviewer = new ReviewerDto();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"reviewers/{reviewerId}");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var readTask = response.Result.Content.ReadAsAsync<ReviewerDto>();
                    readTask.Wait();
                    reviewer = readTask.Result;
                }
            }
            return reviewer;
        }

        public ReviewerDto GetReviewerOfAReview(int reviewId)
        {
            ReviewerDto reviewer = new ReviewerDto();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"reviewers/{reviewId}/reviewer");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var readTask = response.Result.Content.ReadAsAsync<ReviewerDto>();
                    readTask.Wait();
                    reviewer = readTask.Result;
                }
            }
            return reviewer;
        }

        public IEnumerable<ReviewerDto> GetReviewers()
        {
            IEnumerable<ReviewerDto> reviewers = new List<ReviewerDto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync("reviewers");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var taskList = response.Result.Content.ReadAsAsync<List<ReviewerDto>>();
                    taskList.Wait();
                    reviewers = taskList.Result;
                }
            }
            return reviewers;
        }

        public IEnumerable<ReviewDto> GetReviewsByAReviewer(int reviewerId)
        {
            IEnumerable<ReviewDto> reviews = new List<ReviewDto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60039/api/");
                var response = client.GetAsync($"reviewers/{reviewerId}/reviews");
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
    }
}
