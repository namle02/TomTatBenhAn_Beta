using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface IConfigServices
    {
        Task GetConfigFromSheet();
        string? Get(string key);
    }
}
