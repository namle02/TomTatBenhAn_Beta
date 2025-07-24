using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.Repos.Mappers.Interface;
using TomTatBenhAn_WPF.Repos.Model;
using TomTatBenhAn_WPF.Services.Interface;

namespace TomTatBenhAn_WPF.ViewModel.ControlViewModel
{
    public partial class ContentViewModel : ObservableObject, IRecipient<LoadData>
    {
        private readonly IDataMapper _dataMapper;
        private readonly IAiService _aiService;
        private readonly ILoadingService _loadingService; 

        public ContentViewModel( IDataMapper dataMapper,IAiService aiService,ILoadingService loadingService) 
        {
            _dataMapper = dataMapper;
            _aiService = aiService;
            _loadingService = loadingService;
            _loadingService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ILoadingService.IsLoading))
                    OnPropertyChanged(nameof(IsLoading));
            };

            WeakReferenceMessenger.Default.Register<LoadData>(this);
        }

        [ObservableProperty] private ThongTinBenhNhan patientInfo = new();
        [ObservableProperty] private ChuanDoanModel chuanDoanInfo = new();
        [ObservableProperty] private BenhAnTypeModel benhAnTypeInfo = new();
        [ObservableProperty] private BenhAnChiTietModel chiTietBenhAnInfo = new();
        [ObservableProperty] private HanhChinhModel hanhChinhInfo = new();
        [ObservableProperty] private List<KetQuaXetNghiemCLSModel> aiKQXNClsList = new();
        [ObservableProperty] private List<LoadData> soBenhAnList = new();
        [ObservableProperty] private List<DienBienModel> dienBienInfo = new();
        [ObservableProperty] private CheckBoxModel checkBoxInfo = new();

        [ObservableProperty] private string ai_TomTatQuaTrinhBenhLy;
        [ObservableProperty] private string ai_DauHieuLamSang;
        [ObservableProperty] private string aiResultQuaTrinhBenhLy = string.Empty;
        [ObservableProperty] private string aiResultKQXN = string.Empty;
        [ObservableProperty] private string aiResultDienBien = string.Empty;

        public Visibility IsLoading => _loadingService.IsLoading;


        public async void Receive(LoadData message)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(message.maYTe) && string.IsNullOrWhiteSpace(message.soBenhAn))
                {
                    //  nhập mã y tế, fetch danh sách
                    SoBenhAnList = await _dataMapper.GetSoBenhAnData(message.maYTe);

                    // Gửi danh sách cho Sidebar
                    WeakReferenceMessenger.Default.Send(new SoBenhAnListMessage(SoBenhAnList));

                    // Gửi lại mục được chọn 
                    var matched = SoBenhAnList.FirstOrDefault(x =>
                        x.soBenhAn?.Trim().Equals(message.soBenhAn?.Trim(), StringComparison.OrdinalIgnoreCase) == true
                    );

                    if (matched != null)
                        WeakReferenceMessenger.Default.Send(new LoadDataMessage(matched));

                    return; // Dừng ở đây, chờ user chọn
                }

                if (!string.IsNullOrWhiteSpace(message.soBenhAn))
                {
                    
                    _loadingService.Show();

                    //   xử lý
                    PatientInfo = await _dataMapper.GetThongTinBenhNhanData(message.soBenhAn);
                    ChuanDoanInfo = await _dataMapper.GetChuanDoanData(message.soBenhAn);
                    BenhAnTypeInfo = await _dataMapper.GetBenhAnTypeData(message.soBenhAn);
                    AiKQXNClsList = await _dataMapper.GetKetQuaXetNghiemModelData(message.soBenhAn);
                    HanhChinhInfo = await _dataMapper.GetHanhChinhData(message.soBenhAn);
                    DienBienInfo = await _dataMapper.GetDienBienData(message.soBenhAn);

                    if (!string.IsNullOrWhiteSpace(BenhAnTypeInfo.LoaiBenhAn) &&
                        !string.IsNullOrWhiteSpace(BenhAnTypeInfo.BenhAnTongQuatId))
                    {
                        ChiTietBenhAnInfo = await _dataMapper.GetBenhAnChiTietAsync(
                            BenhAnTypeInfo.LoaiBenhAn,
                            BenhAnTypeInfo.BenhAnTongQuatId
                        );

                        if (!string.IsNullOrWhiteSpace(ChiTietBenhAnInfo.QuaTrinhBenhLy))
                        {
                            var aiResultDict = await _aiService.TomTatBenhLyAsync(ChiTietBenhAnInfo.QuaTrinhBenhLy);
                            Ai_TomTatQuaTrinhBenhLy = aiResultDict.GetValueOrDefault("BN_TomTatQuaTrinhBenhLy", "⚠️ Không tóm tắt được quá trình bệnh lý");
                            Ai_DauHieuLamSang = aiResultDict.GetValueOrDefault("BN_DauHieuLamSang", "⚠️ Không tóm tắt được dấu hiệu lâm sàng");
                        }
                        else
                        {
                            Ai_TomTatQuaTrinhBenhLy = "⚠️ Không có dữ liệu quá trình bệnh lý để tóm tắt";
                            Ai_DauHieuLamSang = "⚠️ Không có dữ liệu dấu hiệu lâm sàng để tóm tắt";
                        }
                    }

                    AiResultKQXN = (!string.IsNullOrWhiteSpace(ChuanDoanInfo.benhChinhVaoVien) && AiKQXNClsList.Any())
                        ? await _aiService.TomTatKetQuaXetNghiemCSLAsync(ChuanDoanInfo.benhChinhVaoVien, AiKQXNClsList)
                        : "Không có dữ liệu xét nghiệm hoặc chẩn đoán chính.";

                    if (DienBienInfo?.Any() == true)
                    {
                        var allDienBien = string.Join("\n", DienBienInfo.Select(d => d.DienBien));
                        var allLoiDan = string.Join("\n", DienBienInfo.Select(d => d.LoiDanThayThuoc));

                        var aiDienBienDict = await _aiService.HuongDieuTriAsync(allDienBien, allLoiDan);
                        AiResultDienBien = string.Join("\n", aiDienBienDict.Select(kv => $"{kv.Key}: {kv.Value}"));
                    }
                    else
                    {
                        AiResultDienBien = "⚠️ Không có dữ liệu diễn biến bệnh để tóm tắt hướng điều trị.";
                    }

                    var model = await _dataMapper.UpdateCheckBoxesFromKetQuaDieuTri(HanhChinhInfo.KetQuaDieuTri);

                    if (model != null)
                    {
                        CheckBoxInfo = model;
                    }

                   
                    _loadingService.Hide();
                }
            }
            catch (Exception ex)
            {
                AiResultQuaTrinhBenhLy = $"❌ Lỗi: {ex.Message}";
                AiResultKQXN = $"❌ Lỗi tóm tắt XN: {ex.Message}";

                _loadingService.Hide(); // đảm bảo luôn tắt loading nếu lỗi
            }
        }
    }
}
