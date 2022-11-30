using Redeo.Data.Base;
using Redeo.Models;

namespace Redeo.Data.Services
{
    public class CategoryService : EntityBaseRepository<Category>, ICategoryService
    {
        public CategoryService(AppDbContext context) : base(context) { }
    }
}
