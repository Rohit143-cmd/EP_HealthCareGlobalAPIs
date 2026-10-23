using DbDemoProject.Model.Data.Model;
using DbDemoProject.Model.ResponseDTO;

namespace DbDemoProject.Service.Appointment
{
    public interface IMemberService
    {
        public Task<List<MemberModel>> GetMembers();
        public Task<int> SaveMember(MemberModel memberModel);
        public Task<int> UpdapteMember(MemberModel memberModel);
        public int DeleteMember(Guid? id);
        public MemberModel GetMemberById(Guid? id);
    }
}
