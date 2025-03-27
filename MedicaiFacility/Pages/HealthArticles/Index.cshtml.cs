using AutoMapper;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.HealthArticles
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        [Inject]
        IMapper Mapper { get; set; }

        private readonly IHealthArticleService _healthArticleService;
        public List<HealthArticleViewModel> HealthArticles = new List<HealthArticleViewModel>();
        public Pager Pager { get; set; }
        public IndexModel(IHealthArticleService db)
        {
            _healthArticleService = db;

        }
        public async Task OnGetAsync(int pg =1)
        {
            int pageSize = 5;
            var (list,total) = _healthArticleService.findAllWithPagination(pg,pageSize);
            Pager = new Pager(total,pg,pageSize);
            HealthArticles = list.Select(h => new HealthArticleViewModel
            {
                ArticleID = h.ArticleId,
                Title = h.Title,
                Content = h.Content,
                AuthorName = h.Author.FullName,
                CreatedAt = h.CreatedAt,
                IsActive = h.IsActive,  
            }).ToList();

        }

     


        public IActionResult OnPostDelete(int articleId) {
       
               var item = _healthArticleService.FindById(articleId);
            if (item != null) { 
               item.IsActive=false;
                _healthArticleService.Udpate(item); 
            }

			return RedirectToPage("/HealthArticles/Index");
		}

    }
}
