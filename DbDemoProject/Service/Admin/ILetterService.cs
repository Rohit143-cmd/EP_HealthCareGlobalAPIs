using DbDemoProject.Model.Data.Model;
using DbDemoProject.Model.ResponseDTO;

namespace DbDemoProject.Service.Admin
{
    public interface ILetterService
    {
        public int SaveLetter(LetterRequestResponse letterModel);
        public List<LetterModel> GetLetter();
        public Task<byte[]> GenerateAppointmentLetter(Guid? appointmentUUID);
    }
}
