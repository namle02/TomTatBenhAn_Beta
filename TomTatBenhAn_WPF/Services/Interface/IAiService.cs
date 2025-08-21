using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTatBenhAn_WPF.Repos._Model;


namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface IAiService
    {
        Task TomTatBenhAn(PatientAllData patient);
    }
}
