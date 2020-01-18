using BookApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookGUI.Services
{
    public interface IReviewerRepositoryGUI
    {
        IEnumerable<ReviewerDto> GetReviewers();
        ReviewerDto GetReviewerById(int reviewerId);
        IEnumerable<ReviewDto> GetReviewsByAReviewer(int reviewerId);
        ReviewerDto GetReviewerOfAReview(int reviewId);
    }
}
