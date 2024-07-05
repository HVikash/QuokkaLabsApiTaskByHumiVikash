//using QuokkaLabsApi_By_HumiVikash.Migrations;

using QuokkaLabsApi_By_HumiVikash.Models;

namespace QuokkaLabsApi_By_HumiVikash.Repository.IRepository
{
    public interface IArticlesRepository : IQuokkaLabsBaseRepository<Articles> 
    {
        Articles GetById(int Id);
        List<Articles> GetAll();
    }
}
