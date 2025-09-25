# Demo API Bảng kiểm mới

## 1. Tạo bảng kiểm mới với file Word

### Request
```http
POST /api/bang-danh-gia
Content-Type: multipart/form-data

wordFile: [file Word binary data]
data: {
  "phacDoId": "64a1b2c3d4e5f6789012345",
  "tenBangKiem": "BẢNG KIỂM ĐIỀU TRỊ TIM MẠCH",
  "danhGiaVaChanDoan": {
    "stt": "1",
    "tenHangMucKiemTra": "Đánh giá và chẩn đoán",
    "danhSachNoiDung": [
      {
        "tenNoiDungKiemTra": "Khám lâm sàng",
        "danhSachTieuChi": [
          {
            "stt": "1.1",
            "yeuCauDatDuoc": "Đo huyết áp đúng cách",
            "dat": false,
            "khongDat": false,
            "lyDoKhongDat": "",
            "khongApDung": false,
            "isImportant": true
          }
        ]
      }
    ]
  }
}
```

### Response
```json
{
  "success": true,
  "message": "Tạo bảng đánh giá thành công",
  "data": {
    "_id": "64a1b2c3d4e5f6789012346",
    "bangKiemId": "64a1b2c3d4e5f6789012346",
    "phacDoId": "64a1b2c3d4e5f6789012345",
    "tenBangKiem": "BẢNG KIỂM ĐIỀU TRỊ TIM MẠCH",
    "tenPhacDo": "Phác đồ điều trị tim mạch",
    "originalFileName": "BangKiem_TimMach.docx",
    "originalFilePath": "./uploads/bang-kiem/bang-kiem-1703123456789-123456789.docx",
    "fileSize": 245760,
    "uploadedAt": "2023-12-21T10:30:00.000Z",
    "createdAt": "2023-12-21T10:30:00.000Z",
    "updatedAt": "2023-12-21T10:30:00.000Z"
  }
}
```

## 2. Lấy danh sách bảng kiểm

### Request
```http
GET /api/bang-danh-gia?phacDoId=64a1b2c3d4e5f6789012345&search=tim mạch
```

### Response
```json
{
  "success": true,
  "message": "Lấy danh sách bảng đánh giá thành công",
  "data": [
    {
      "_id": "64a1b2c3d4e5f6789012346",
      "bangKiemId": "64a1b2c3d4e5f6789012346",
      "phacDoId": "64a1b2c3d4e5f6789012345",
      "tenBangKiem": "BẢNG KIỂM ĐIỀU TRỊ TIM MẠCH",
      "tenPhacDo": "Phác đồ điều trị tim mạch",
      "originalFileName": "BangKiem_TimMach.docx",
      "originalFilePath": "./uploads/bang-kiem/bang-kiem-1703123456789-123456789.docx",
      "fileSize": 245760,
      "uploadedAt": "2023-12-21T10:30:00.000Z",
      "createdAt": "2023-12-21T10:30:00.000Z",
      "updatedAt": "2023-12-21T10:30:00.000Z"
    }
  ]
}
```

## 3. Download file Word gốc

### Request
```http
GET /api/bang-danh-gia/64a1b2c3d4e5f6789012346/download
```

### Response
```
Content-Type: application/vnd.openxmlformats-officedocument.wordprocessingml.document
Content-Disposition: attachment; filename="BangKiem_TimMach.docx"

[binary file data]
```

## 4. Xóa bảng kiểm

### Request
```http
DELETE /api/bang-danh-gia/64a1b2c3d4e5f6789012346
```

### Response
```json
{
  "success": true,
  "message": "Xóa bảng đánh giá và file Word thành công",
  "data": null
}
```

## Demo Client (WPF)

### 1. Upload file Word
```csharp
// Trong BangKiemVM.cs
[RelayCommand]
private async Task SaveBangKiem()
{
    // Validate dữ liệu
    if (string.IsNullOrWhiteSpace(BangKiemMoi.TenBangKiem) || 
        string.IsNullOrWhiteSpace(SelectedPhacDo) ||
        string.IsNullOrWhiteSpace(BangKiemPath))
    {
        MessageBox.Show("Vui lòng điền đầy đủ thông tin và chọn file Word.");
        return;
    }

    // Extract table từ file Word
    var requestDto = _phacDoReportServices.ExtractTableBangDanhGiaFromWord(
        BangKiemPath, SelectedPhacDo, BangKiemMoi.TenBangKiem);

    // Upload file và tạo bảng kiểm
    var result = await _bangKiemServices.CreateWithFileAsync(requestDto, BangKiemPath);
    
    if (result.Success)
    {
        DanhSachBangKiem.Insert(0, result.Data);
        MessageBox.Show("Import bảng kiểm từ Word thành công.");
        CloseModalAddBangKiemr();
    }
    else
    {
        MessageBox.Show(result.GetErrorMessage());
    }
}
```

### 2. Download và xuất file Word
```csharp
// Trong KiemTraPhacDoVM.cs
[RelayCommand]
private async Task XuatBangKiem(string bangKiemId)
{
    // Tìm bảng kiểm đã đánh giá
    var bangKiemDaDanhGia = DanhSachBangKiemDaDanhGia.FirstOrDefault(x => x.BangKiemId == bangKiemId);
    if (bangKiemDaDanhGia == null) return;

    // Download file Word gốc
    var tempFilePath = Path.Combine(Path.GetTempPath(), $"temp_{bangKiemId}.docx");
    var downloadResult = await _bangKiemServices.DownloadOriginalFileAsync(bangKiemId, tempFilePath);

    if (downloadResult.Success)
    {
        // Tạo file output với dữ liệu đã đánh giá
        var outputFileName = $"BangKiem_{bangKiemDaDanhGia.TenBangKiem}_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
        var outputPath = await CreateOutputFileWithData(tempFilePath, bangKiemDaDanhGia, outputFileName);
        
        if (!string.IsNullOrWhiteSpace(outputPath))
        {
            MessageBox.Show($"Xuất bảng kiểm thành công!\nFile đã lưu tại: {outputPath}");
        }
    }
}
```

## Cấu trúc thư mục Server

```
Backend/
├── uploads/
│   └── bang-kiem/
│       ├── bang-kiem-1703123456789-123456789.docx
│       ├── bang-kiem-1703123456790-123456790.docx
│       └── ...
├── src/
│   ├── Controller/
│   │   └── BangDanhGiaController.js
│   ├── Services/
│   │   └── BangDanhGiaServices.js
│   ├── Routes/
│   │   └── BangDanhGiaRoutes.js
│   └── Repos/Model/
│       └── BangDanhGiaModel.js
└── package.json
```

## Lưu ý bảo mật

1. **File Upload Security**:
   - Validate file type và size
   - Scan virus cho file upload
   - Giới hạn quyền truy cập thư mục uploads

2. **API Security**:
   - Rate limiting cho upload endpoint
   - Authentication và authorization
   - Logging các hoạt động upload/download

3. **Data Protection**:
   - Backup định kỳ thư mục uploads
   - Mã hóa file nhạy cảm
   - Xóa file khi hết hạn lưu trữ
