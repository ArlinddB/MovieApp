using Redeo.Data.Base;
using Redeo.Models;

namespace Redeo.Data.Services
{
    public class ActorService : EntityBaseRepository<Actor>, IActorService
    {
        public ActorService(AppDbContext context) : base(context) {}
    }
}
