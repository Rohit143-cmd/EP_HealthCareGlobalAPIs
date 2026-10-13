using DbDemoProject.Data;
using DbDemoProject.Model;

namespace DbDemoProject.Repository.Account
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbContextHelper _dbContextHelper;
        public RoleRepository(DbContextHelper dbContextHelper) 
        {
            _dbContextHelper = dbContextHelper;
        }
        public int CreateRole(UserRoles userRoles)
        {
            _dbContextHelper.Roles.Add(userRoles);
            var result =  _dbContextHelper.SaveChanges();
            return result;
        }

        public int DeleteRoleByID(Guid? roleid)
        {
            var role = _dbContextHelper.Roles.Where(a=>a.RoleUUID==roleid).FirstOrDefault();
            if (role != null)
            {
                role.IsActive = false;
                _dbContextHelper.Update(role);
                var result = _dbContextHelper.SaveChanges();
                return result;
            }
            return 0;
        }

        public IQueryable<UserRoles> GetActiveRole()
        {
            var roles = _dbContextHelper.Roles.Where(a => a.IsActive == true);
            if (roles != null)
            {
                return roles;
            }
            return null;
        }

        public UserRoles GetRoleByID(Guid? roleid)
        {
            var role = _dbContextHelper.Roles.Where(a => a.RoleUUID == roleid).FirstOrDefault();
            if (role != null)
            {
                return role;
            }
            return null;
        }

        public int UpdateRole(UserRoles userRoles)
        {
            if(userRoles != null)
            {
                _dbContextHelper.Update(userRoles);
                var result = _dbContextHelper.SaveChanges();
                return result;
            }
            return 0;
        }
    }
}
