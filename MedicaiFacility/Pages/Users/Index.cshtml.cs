using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace MedicaiFacility.RazorPage.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public List<User> Users { get; set; }
        public Pager Pager { get; set; }

        public void OnGet(int pg = 1)
        {
            int pageSize = 5; 
            var allUsers = _userService.GetAllUsers().ToList(); 
            var totalItems = allUsers.Count;

            Pager = new Pager(totalItems, pg, pageSize);
            Users = allUsers.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}