using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTatBenhAn_WPF.Repos.Model;

namespace TomTatBenhAn_WPF.Repos.Mappers.Interface
{
    public interface IDataMapper
    {
        Task<HanhChinhModel> GetHanhChinhData(string SoBenhAn);
    }
}
