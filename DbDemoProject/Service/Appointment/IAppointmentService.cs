using DbDemoProject.Model.ResponseDTO;

namespace DbDemoProject.Service.Appointment
{
    public interface IAppointmentService
    {
        public Task<int> BookAppointment(AppointmentRequestModel appointmentRequest);
        public Task<List<AppointmentRequestModel>> GetAppointment();
        public Task<List<AppointmentRequestModel>> GetAppointmentByUser(Guid? userUUID);
    }
}
