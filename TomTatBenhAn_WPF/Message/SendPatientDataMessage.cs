using TomTatBenhAn_WPF.Repos._Model;

namespace TomTatBenhAn_WPF.Message
{
    public class SendPatientDataMessage
    {
        public PatientAllData? patient { get; set; }
        public string CalledBy { get; set; }

        public SendPatientDataMessage(PatientAllData patient, string calledBy = "Search")
        {
            this.patient = patient;
            CalledBy = calledBy;
        }
    }
}
