using DbDemoProject.Model;
using DbDemoProject.Repository.Account;

namespace DbDemoProject.Service.Account
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public int CreateRole(UserRoles userRoles)
        {
            return _roleRepository.CreateRole(userRoles);
        }

        public int DeleteRoleByID(Guid? roleid)
        {
            return _roleRepository.DeleteRoleByID(roleid);
        }

        public IQueryable<UserRoles> GetActiveRole()
        {
            return _roleRepository.GetActiveRole();
        }

        public UserRoles GetRoleByID(Guid? roleid)
        {
            return _roleRepository.GetRoleByID(roleid);
        }

        public int UpdateRole(UserRoles userRoles)
        {
            return _roleRepository.UpdateRole(userRoles);
        }
    }
}
