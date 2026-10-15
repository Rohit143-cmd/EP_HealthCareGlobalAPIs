using DbDemoProject.Model;

namespace DbDemoProject.Service.Account
{
    public interface INavigationMenuService
    {
        public List<NavigationResponseModel> GetActiveMenu();
    }
}
