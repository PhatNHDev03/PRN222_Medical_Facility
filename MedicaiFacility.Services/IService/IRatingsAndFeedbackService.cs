using MedicaiFacility.BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicaiFacility.Service.IService
{
    public interface IRatingsAndFeedbackService
    {
        List<RatingsAndFeedback> GetAll();
        RatingsAndFeedback FindById(int id);
        void Save(RatingsAndFeedback healthArticle);
        void Udpate(RatingsAndFeedback healthArticle);
        void deleteById(int id);

        // Đổi từ phương thức đồng bộ sang bất đồng bộ
        Task<(List<RatingsAndFeedback>, int totalItem)> FindAllWithPaginationAsync(int pg, int pageSize);
    }
}
