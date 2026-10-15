using DbDemoProject.Data;
using DbDemoProject.Model;

namespace DbDemoProject.Repository.Account
{
    public class NavigationMenuRepository : INavigationMenuRepository
    {
        private readonly DbContextHelper _dbContextHelper;
        public NavigationMenuRepository(DbContextHelper dbContextHelper)
        {
            _dbContextHelper = dbContextHelper;
        }
        public IQueryable<NavigationMenu> GetActiveMenu()
        {
            return _dbContextHelper.NavigationMenu.Where(x => x.IsActive);
        }
    }
}
