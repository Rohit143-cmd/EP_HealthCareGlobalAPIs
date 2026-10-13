using DbDemoProject.Model;

namespace DbDemoProject.Service.Account
{
    public interface IRoleService
    {
        public IQueryable<UserRoles> GetActiveRole();
        public int CreateRole(UserRoles userRoles);
        public int UpdateRole(UserRoles userRoles);
        public UserRoles GetRoleByID(Guid? roleid);
        public int DeleteRoleByID(Guid? roleid);
    }
}
