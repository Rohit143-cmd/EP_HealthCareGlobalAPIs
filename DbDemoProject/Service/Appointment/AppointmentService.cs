using DbDemoProject.Data;
using DbDemoProject.Model;
using DbDemoProject.Model.Data.Model;
using DbDemoProject.Model.ResponseDTO;
using Microsoft.EntityFrameworkCore;

namespace DbDemoProject.Service.Appointment
{
    public class AppointmentService : IAppointmentService
    {
        private readonly DbContextHelper _dbContextHelper;
        public AppointmentService(DbContextHelper dbContextHelper)
        {
            _dbContextHelper = dbContextHelper;
        }
        public async Task<int> BookAppointment(AppointmentRequestModel appointmentRequest)
        {
            bool IsMemberExists = false;
            MemberModel memberModel = new MemberModel();
            AppointmentModel appointment = new AppointmentModel();
            if (appointmentRequest != null)
            {
                IsMemberExists = appointmentRequest.MemberUUID != null ? true : false;
                if (IsMemberExists)
                {
                    memberModel.MemberUUID = (Guid)appointmentRequest.MemberUUID;
                }
                memberModel.FirstName = appointmentRequest.FirstName;
                memberModel.LastName = appointmentRequest.LastName;
                memberModel.DateOfBirth = appointmentRequest.DateOfBirth;
                memberModel.Address = appointmentRequest.Address;
                memberModel.CreatedDate = DateTime.Now;
                memberModel.PhoneNumber = appointmentRequest.PhoneNumber;
                memberModel.Email = appointmentRequest.Email;
                memberModel.Gender = appointmentRequest.Gender;
                memberModel.MemberID = await GenerateMemberID();
                appointment.MemberUUID = memberModel.MemberUUID;
                appointment.AppointmentUUID = Guid.NewGuid();
                appointment.AppointmentDate = (DateTime)appointmentRequest.AppointmentDate;
                appointment.CreatedOn = DateTime.Now;
                appointment.UpdatedOn = DateTime.Now;
                appointment.IsActive = true;
                appointment.Status = "Booked";
                appointment.DoctorUUID = appointmentRequest.AssignDoctorUUID;
                appointment.AppointmentTime = appointmentRequest.AppointmentTime;
                if (IsMemberExists)
                {
                    _dbContextHelper.Member.Update(memberModel);
                    _dbContextHelper.Appointment.Add(appointment);
                    return _dbContextHelper.SaveChanges();
                }
                _dbContextHelper.Member.Add(memberModel);
                _dbContextHelper.Appointment.Add(appointment);
                return _dbContextHelper.SaveChanges();
            }
            return 0;
        }

        public async Task<List<AppointmentRequestModel>> GetAppointment()
        {
            var appointments = await (
                from appointment in _dbContextHelper.Appointment
                join member in _dbContextHelper.Member
                    on appointment.MemberUUID equals member.MemberUUID
                    join doctor in _dbContextHelper.Users on appointment.DoctorUUID equals doctor.UserUUID
                    
                select new AppointmentRequestModel
                {
                    // Member details
                    MemberUUID = member.MemberUUID,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    PhoneNumber = member.PhoneNumber,
                    DateOfBirth = member.DateOfBirth,
                    Gender = member.Gender,
                    Address = member.Address,
                    MemberID = member.MemberID,
                    AppointmentUUID = appointment.AppointmentUUID,
                    AppointmentDate = appointment.AppointmentDate.Date,
                    AssignDoctorUUID = appointment.DoctorUUID,
                    DoctorName = doctor.FirstName + " " + doctor.LastName,
                    //PaymentMode = appointment.PaymentMode,
                    //TransactionUUID = appointment.TransactionUUID,
                    //PaymentStatus = appointment.PaymentStatus,
                    //CreatedBy = (DateTime)appointment.CreatedOn
                }
            ).ToListAsync();

            return appointments;
        }

        public async Task<List<AppointmentRequestModel>> GetAppointmentByUser(Guid? userUUID)
        {
            var appointments = await (
                from appointment in _dbContextHelper.Appointment
                join member in _dbContextHelper.Member
                    on appointment.MemberUUID equals member.MemberUUID
                join doctor in _dbContextHelper.Users on appointment.DoctorUUID equals doctor.UserUUID where doctor.UserUUID == userUUID

                select new AppointmentRequestModel
                {
                    // Member details
                    MemberUUID = member.MemberUUID,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    PhoneNumber = member.PhoneNumber,
                    DateOfBirth = member.DateOfBirth,
                    Gender = member.Gender,
                    Address = member.Address,
                    MemberID = member.MemberID,
                    AppointmentUUID = appointment.AppointmentUUID,
                    AppointmentDate = appointment.AppointmentDate.Date,
                    AssignDoctorUUID = appointment.DoctorUUID,
                    DoctorName = doctor.FirstName + " " + doctor.LastName,
                    //PaymentMode = appointment.PaymentMode,
                    //TransactionUUID = appointment.TransactionUUID,
                    //PaymentStatus = appointment.PaymentStatus,
                    //CreatedBy = (DateTime)appointment.CreatedOn
                }
            ).ToListAsync();

            return appointments;
        }

        private async Task<string> GenerateMemberID()
        {
            int nextNumber = await _dbContextHelper.Member
                .MaxAsync(m => (int?)m.MemberNumber) ?? 0;

            return $"MBR{nextNumber + 1:D6}";
        }
    }
}
