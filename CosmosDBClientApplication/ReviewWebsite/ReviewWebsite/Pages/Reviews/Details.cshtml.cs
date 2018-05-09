using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReviewWebsite.Core.Interfaces;
using ReviewWebsite.Core.Model;
using static ReviewWebsite.Pages.Reviews.IndexModel;

namespace ReviewWebsite.Pages.Reviews
{
    public class DetailsModel : PageModel
    {
        private readonly IRepository<ReviewDocument> _reviewRepository;

        public ReviewViewModel Review { get; set; }

        public DetailsModel(IRepository<ReviewDocument> customerRepository)
        {
            _reviewRepository = customerRepository;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var reviewDocument = await _reviewRepository.GetByIdAsync(id);

            Review = new ReviewViewModel
            {
                Id = reviewDocument.id,
                Content = reviewDocument.Content,
                Feedback = reviewDocument.Satisfaction
            };

            return Page();
        }
    }
}