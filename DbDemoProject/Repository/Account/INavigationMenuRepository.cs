using DbDemoProject.Model;

namespace DbDemoProject.Repository.Account
{
    public interface INavigationMenuRepository
    {
        public IQueryable<NavigationMenu> GetActiveMenu();
    }
}
