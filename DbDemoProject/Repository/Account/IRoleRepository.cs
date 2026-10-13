using DbDemoProject.Model;

namespace DbDemoProject.Repository.Account
{
    public interface IRoleRepository
    {
        public IQueryable<UserRoles> GetActiveRole();
        public int CreateRole(UserRoles userRoles);
        public int UpdateRole(UserRoles userRoles);
        public UserRoles GetRoleByID(Guid? roleid);
        public int DeleteRoleByID(Guid? roleid);
    }
}
