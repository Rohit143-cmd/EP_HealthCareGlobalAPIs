
using DbDemoProject.Data;
using DbDemoProject.Model.ResponseDTO;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace DbDemoProject.Service.Account
{
    public class DashboardItemService : IDashbordItems
    {
        private readonly DbContextHelper _dbContextHelper;

        public DashboardItemService(DbContextHelper dbContextHelper)
        {
            _dbContextHelper = dbContextHelper;
        }

        public async Task<List<DashboardItemResponseDto>> GetDashboardItems(
            Guid? roleUUID)
        {
            try
            {
                if (!roleUUID.HasValue)
                    return new List<DashboardItemResponseDto>();

                var items = await _dbContextHelper.DashboardItems
                    .AsNoTracking()
                    .Where(x => x.RoleUUID == roleUUID.Value)
                    .ToListAsync();

                var result = new List<DashboardItemResponseDto>();

                // Allow only known table names.
                var allowedTables = new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase)
            {
                "Users",
                "Product"
            };

                foreach (var item in items)
                {
                    int count = 0;
                    var tableName = item.nameofTableCount;

                    if (!string.IsNullOrWhiteSpace(tableName) &&
                        Regex.IsMatch(
                            tableName,
                            @"^[a-zA-Z_][a-zA-Z0-9_]*$") &&
                        allowedTables.Contains(tableName))
                    {
                        var connection =
                            _dbContextHelper.Database.GetDbConnection();

                        bool openedHere = false;

                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                await _dbContextHelper.Database
                                    .OpenConnectionAsync();

                                openedHere = true;
                            }

                            await using var command =
                                connection.CreateCommand();

                            command.CommandText =
                                $"SELECT COUNT(*) FROM dbo.[{tableName}] WHERE IsActive = 1";

                            command.CommandType = CommandType.Text;

                            var queryResult =
                                await command.ExecuteScalarAsync();

                            count = Convert.ToInt32(queryResult);
                        }
                        finally
                        {
                            if (openedHere && connection.State == ConnectionState.Open)
                            {
                                await _dbContextHelper.Database.CloseConnectionAsync();
                            }
                        }

                    }

                    result.Add(new DashboardItemResponseDto
                    {
                        id = item.id,
                        value = count,
                        title = item.title,
                        icon = item.icon,
                        gradient = item.gradient,
                        shadowColor = item.shadowColor,
                        viewAllURL = item.viewAllURL,
                        buttonLabel = item.buttonLabel
                    });
                }

                return result;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
