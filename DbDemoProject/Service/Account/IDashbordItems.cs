using DbDemoProject.Model.ResponseDTO;

namespace DbDemoProject.Service.Account
{
    public interface IDashbordItems
    {
        public Task<List<DashboardItemResponseDto>> GetDashboardItems(Guid? roleUUID);
    }
}
