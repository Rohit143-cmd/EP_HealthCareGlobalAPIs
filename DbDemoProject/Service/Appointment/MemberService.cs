using DbDemoProject.Data;
using DbDemoProject.Model.Data.Model;
using DbDemoProject.Model.ResponseDTO;
using Microsoft.EntityFrameworkCore;

namespace DbDemoProject.Service.Appointment
{
    public class MemberService : IMemberService
    {
        private readonly DbContextHelper _dbContextHelper;
        public MemberService(DbContextHelper dbContextHelper) 
        {
            _dbContextHelper = dbContextHelper;   
        }

        public int DeleteMember(Guid? id)
        {
            var result = _dbContextHelper.Member.Where(a=>a.MemberUUID==id).FirstOrDefault();
            if (result != null)
            {
                result.IsActive=false;
                result.UpdatedDate=DateTime.Now;
                //_dbContextHelper.Member.Update(result);
                return _dbContextHelper.SaveChanges();
            }
            return -1;
        }

        public MemberModel GetMemberById(Guid? id)
        {
            var result = _dbContextHelper.Member.Where(a => a.MemberUUID == id).FirstOrDefault();
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public async Task<List<MemberModel>> GetMembers()
        {
            var result = await _dbContextHelper.Member.Where(a=>a.IsActive).ToListAsync();
            return result;
        }

        public async Task<int> SaveMember(MemberModel memberModel)
        {
            try
            {
                if (memberModel != null)
                {
                    memberModel.IsActive = true;
                    memberModel.UpdatedDate = DateTime.Now;
                    memberModel.CreatedDate = DateTime.Now;
                    memberModel.MemberUUID = Guid.NewGuid();
                    var id = await GenerateMemberID();
                    memberModel.MemberID = id.ToString();
                    _dbContextHelper.Member.Add(memberModel);
                    return await _dbContextHelper.SaveChangesAsync();
                }
                return 0;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        private async Task<string> GenerateMemberID()
        {
            int nextNumber = await _dbContextHelper.Member
                .MaxAsync(m => (int?)m.MemberNumber) ?? 0;

            return $"MBR{nextNumber + 1:D6}";
        }

        public async Task<int> UpdapteMember(MemberModel memberModel)
        {
            if (memberModel != null)
            {
                var result = await _dbContextHelper.Member.Where(a => a.MemberUUID == memberModel.MemberUUID).FirstOrDefaultAsync();
                result.FirstName = memberModel.FirstName;
                result.LastName = memberModel.LastName;
                result.Email = memberModel.Email;
                result.PhoneNumber = memberModel.PhoneNumber;
                result.Address = memberModel.Address;
                result.DateOfBirth = memberModel.DateOfBirth;
                result.Gender = memberModel.Gender;
                result.UpdatedDate = DateTime.Now;
                //_dbContextHelper.Member.Update(result);
                return _dbContextHelper.SaveChanges();
            }
            return 0;
        }
    }
}
