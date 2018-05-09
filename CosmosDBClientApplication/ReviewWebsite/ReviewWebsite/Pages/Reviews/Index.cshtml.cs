using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReviewWebsite.Core.Interfaces;
using ReviewWebsite.Core.Model;

namespace ReviewWebsite.Pages.Reviews
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<ReviewDocument> _reviewRepository;

        public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
        [BindProperty]
        public ReviewViewModel ReviewToAdd { get; set; }

        public class ReviewViewModel
        {
            public string Id { get; set; }
            [Required]
            public string Content { get; set; }

            public string Feedback { get; set; }

            public override string ToString()
            {
                return $"{Id} - {Content}";
            }
        }

        private const string UnknownFeedback = "Not Evaluated";
        private const string GoodFeedback = "Good";
        private const string BadFeedback = "Bad";
        private const string NeutralFeedback = "Neutral";

        public IndexModel(IRepository<ReviewDocument> reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var reviews = await _reviewRepository.ListAsync();
            foreach (var review in reviews)
            {
                var feedback = UnknownFeedback;
                if (!string.IsNullOrEmpty(review.Satisfaction) && double.TryParse(review.Satisfaction, out double score))
                {
                    if (score > 0.75)
                    {
                        feedback = GoodFeedback;
                    }
                    else if (score < 0.25)
                    {
                        feedback = BadFeedback;
                    }
                    else
                    {
                        feedback = NeutralFeedback;
                    }
                }
                Reviews.Add(new ReviewViewModel
                {
                    Id = review.id,
                    Content = review.Content,
                    Feedback = feedback
                });
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                return await OnGetAsync();
            }
            var entity = new ReviewDocument()
            {
                Content = ReviewToAdd.Content
            };
            await _reviewRepository.AddAsync(entity);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await _reviewRepository.DeleteAsync(id.ToString());
            return RedirectToPage();
        }

    }
}