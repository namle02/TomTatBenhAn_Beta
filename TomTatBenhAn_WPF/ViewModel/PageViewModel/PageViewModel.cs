//using CommunityToolkit.Mvvm.Input;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using TomTatBenhAn_WPF.Core;
//using TomTatBenhAn_WPF.Repos.Mappers.Interface;
//using TomTatBenhAn_WPF.Repos.Model;

//namespace TomTatBenhAn_WPF.ViewModel.PageViewModel
//{
//    public class PageViewModel : BaseViewModel
//    {
//        private readonly IDataMapper _dataMapper;

//        public PageViewModel(IDataMapper dataMapper)
//        {
//            _dataMapper = dataMapper;
//            LoadBenhAnCommand = new RelayCommand(LoadBenhAn);
//        }

//        public string SoBenhAnInput { get; set; }  // what user types
//        public HanhChinhModel PatientInfo { get; set; } = new HanhChinhModel();

//        public ICommand LoadBenhAnCommand { get; }

//        //private void LoadBenhAn()
//        //{
//        //    var result = _dataMapper.GetBySoBenhAn(SoBenhAnInput);
//        //    if (result != null)
//        //    {
//        //        PatientInfo = result;
//        //        OnPropertyChanged(nameof(PatientInfo));
//        //    }
//        //}
//    }

//}
