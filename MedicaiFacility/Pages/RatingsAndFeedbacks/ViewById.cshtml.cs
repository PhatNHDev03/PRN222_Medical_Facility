using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.RatingsAndFeedbacks
{
    [BindProperties]
    public class ViewByIdModel : PageModel
    {
        private readonly IRatingsAndFeedbackService _ratingsAndFeedbackService;
        public List<RatingsAndFeedback> Feedbacks { get; set; }
 
        public int ExId { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public ViewByIdModel(IRatingsAndFeedbackService ratingsAndFeedbackService)
        {
            _ratingsAndFeedbackService = ratingsAndFeedbackService;
        }
        public void OnGet(int id, int pg = 1)
        {
 
			Feedbacks = _ratingsAndFeedbackService.GetAllByExpertId(id);
      

        }

    }
}
