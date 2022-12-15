using Microsoft.AspNetCore.Mvc;
using Redeo.Data.Base;
using Redeo.Models;

namespace Redeo.Data.Services
{
    public class ProducersService : EntityBaseRepository<Producers>, IProducersService
    {
        public ProducersService(AppDbContext context) : base(context)
        {
        }

      
    }
}
