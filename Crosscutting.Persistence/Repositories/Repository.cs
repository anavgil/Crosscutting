using Microsoft.EntityFrameworkCore;

namespace Crosscutting.Persistence.Repositories;

public class Repository<T>(DbContext context) : IRepository<T> where T : class
{
}
