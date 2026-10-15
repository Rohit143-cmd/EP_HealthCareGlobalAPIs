namespace DbDemoProject.Model
{
    public class NavigationResponseModel
    {
        public NavigationMenu? NavigationMenu { get; set; }

        public List<NavigationMenu> NavigationMenuChildModel { get; set; }
            = new List<NavigationMenu>();
    }
}
