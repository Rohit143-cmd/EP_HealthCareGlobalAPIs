using DbDemoProject.Model;
using DbDemoProject.Repository.Account;

namespace DbDemoProject.Service.Account
{
    public class NavigationMenuService : INavigationMenuService
    {
        private readonly INavigationMenuRepository _navigationMenuRepository;
        public NavigationMenuService(INavigationMenuRepository navigationMenuRepository)
        {
            _navigationMenuRepository = navigationMenuRepository;
        }
        public List<NavigationResponseModel> GetActiveMenu()
        {
            var result = _navigationMenuRepository
                .GetActiveMenu()
                .ToList();

            var parentMenus = result
                .Where(m => m.IsParent == true)
                .ToList();

            var response = parentMenus.Select(parent => new NavigationResponseModel
            {
                NavigationMenu = parent,

                NavigationMenuChildModel = result
                    .Where(child => child.ParentUUID == parent.NavigationMenuUUID)
                    .ToList()
            }).ToList();

            return response;
        }
    }
}
