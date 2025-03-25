using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.Service.IService;
using MedicaiFacility.Service;

namespace MedicaiFacility.RazorPage.Pages.MedicalExperts
{
    public class IndexModel : PageModel
    {
        private readonly IMedicalExpertService _medicalExpertService;

        public List<MedicalExpert> MedicalExperts { get; set; }
        public Pager Pager { get; set; }
        public IndexModel(IMedicalExpertService medicalExpertService)
        {
            _medicalExpertService = medicalExpertService ?? throw new ArgumentNullException(nameof(medicalExpertService));
        }
        public async Task OnGetAsync(int? pg)
        {
            int pageSize = 10; // Number of items per page
            int pageNumber = pg ?? 1; // Default to page 1 if pg is null

            try
            {
                // Fetch paginated MedicalExperts using the service
                var (medicalExperts, totalItems) = _medicalExpertService.FindAllWithPagination(pageNumber, pageSize);

                MedicalExperts = medicalExperts ?? new List<MedicalExpert>();

                // Set up pagination
                Pager = new Pager(totalItems, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                // Log the exception
                MedicalExperts = new List<MedicalExpert>();
                Pager = new Pager(0, 1, pageSize);
                ModelState.AddModelError(string.Empty, "An error occurred while loading medical experts. " + ex.Message);
            }
        }
    }
}