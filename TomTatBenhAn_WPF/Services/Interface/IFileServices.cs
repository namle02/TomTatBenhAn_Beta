using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Services.Interface
{
    public interface IFileServices
    {
        string GetQuery(string FileName);
        string Decrypt(string Base64Input, string key);
        string GetPromt(string FileName);
    }
}
