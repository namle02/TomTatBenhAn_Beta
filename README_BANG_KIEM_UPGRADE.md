# Hướng dẫn nâng cấp chức năng Bảng kiểm

## Tổng quan
Đã nâng cấp chức năng bảng kiểm để hỗ trợ upload và lưu trữ file Word gốc, đồng thời cho phép xuất dữ liệu đã đánh giá ra file Word.

## Thay đổi trên Server (Node.js + Express + MongoDB)

### 1. Cài đặt dependencies mới
```bash
cd Backend
npm install multer@^1.4.5-lts.1
```

### 2. Cấu trúc API mới
Chỉ giữ lại 3 endpoint chính:

- **GET** `/api/bang-danh-gia` - Lấy danh sách tất cả bảng kiểm (có kèm metadata file)
- **POST** `/api/bang-danh-gia` - Tạo mới bảng kiểm với file Word gốc (multipart/form-data)
- **DELETE** `/api/bang-danh-gia/:id` - Xóa bảng kiểm và file Word gốc
- **GET** `/api/bang-danh-gia/:id/download` - Download file Word gốc

### 3. Schema MongoDB mới
```javascript
const BangDanhGiaSchema = new mongoose.Schema({
    phacDoId: { type: mongoose.Schema.Types.ObjectId, ref: 'PhacDoModel', required: true },
    tenBangKiem: { type: String, required: true, trim: true },
    danhGiaVaChanDoan: { type: HangMucKiemTraSchema, default: () => ({ stt: "1", tenHangMucKiemTra: "Đánh giá và chẩn đoán" }) },
    dieuTri: { type: HangMucKiemTraSchema, default: () => ({ stt: "2", tenHangMucKiemTra: "Điều trị" }) },
    chamSoc: { type: HangMucKiemTraSchema, default: () => ({ stt: "3", tenHangMucKiemTra: "Chăm sóc" }) },
    raVien: { type: HangMucKiemTraSchema, default: () => ({ stt: "4", tenHangMucKiemTra: "Ra viện" }) },
    // Metadata file Word gốc
    originalFileName: { type: String, trim: true },
    originalFilePath: { type: String, trim: true },
    fileSize: { type: Number },
    uploadedAt: { type: Date, default: Date.now }
}, { timestamps: true });
```

### 4. Cấu hình Multer
- Lưu file vào thư mục `./uploads/bang-kiem/`
- Giới hạn kích thước file: 10MB
- Chỉ chấp nhận file `.docx` và `.doc`

## Thay đổi trên Client (WPF)

### 1. Model mới
```csharp
public partial class BangKiemResponseDTO : ObservableObject
{
    // ... các property cũ ...
    
    // Metadata file Word gốc
    [ObservableProperty] private string originalFileName = string.Empty;
    [ObservableProperty] private string originalFilePath = string.Empty;
    [ObservableProperty] private long fileSize;
    [ObservableProperty] private DateTime uploadedAt;
}
```

### 2. Services mới
- `CreateWithFileAsync()` - Tạo bảng kiểm với file Word gốc
- `DownloadOriginalFileAsync()` - Download file Word gốc từ server

### 3. Chức năng Drag & Drop cải tiến
- Hỗ trợ cả `.docx` và `.doc`
- Validation kích thước file (giới hạn 10MB)
- Thông báo rõ ràng khi chọn file

### 4. Chức năng xuất dữ liệu mới
- Download file Word gốc từ server
- Copy table từ file gốc
- Đổ dữ liệu bảng kiểm đã đánh giá vào các cột:
  - Cột 6: Đạt (✓)
  - Cột 7: Không đạt (✓)
  - Cột 8: Không áp dụng (✓)
  - Cột 9: Lý do không đạt (text)
- Lưu file output cho người dùng

## Hướng dẫn sử dụng

### 1. Thêm bảng kiểm mới
1. Mở trang "Bảng kiểm"
2. Click "Thêm bảng kiểm"
3. Kéo thả file Word vào vùng drop hoặc click "Chọn file"
4. Nhập tên bảng kiểm và chọn phác đồ liên kết
5. Click "Lưu"

### 2. Xuất dữ liệu bảng kiểm
1. Mở trang "Kiểm tra phác đồ"
2. Nhập số bệnh án và kiểm tra
3. Chọn phác đồ và đánh giá tuân thủ
4. Click "Xuất bảng kiểm" trên bảng kiểm đã đánh giá
5. Chọn nơi lưu file output

## Lưu ý quan trọng

### Server
- Tạo thư mục `uploads/bang-kiem/` để lưu file
- Cấu hình CORS để cho phép upload file từ client
- Backup định kỳ thư mục uploads

### Client
- Cần có DocumentFormat.OpenXml package để xử lý file Word
- File tạm được lưu trong thư mục temp của hệ thống
- Tự động dọn dẹp file tạm sau khi xuất

### Bảo mật
- Validate file type và size trên cả client và server
- Giới hạn quyền truy cập thư mục uploads
- Xóa file khi xóa bảng kiểm

## Troubleshooting

### Lỗi upload file
- Kiểm tra kích thước file (< 10MB)
- Kiểm tra định dạng file (.docx, .doc)
- Kiểm tra quyền ghi thư mục uploads

### Lỗi xuất file
- Kiểm tra kết nối server
- Kiểm tra file Word gốc có tồn tại không
- Kiểm tra quyền ghi thư mục output

### Lỗi xử lý Word
- Cài đặt DocumentFormat.OpenXml package
- Kiểm tra file Word có bị lỗi không
- Kiểm tra cấu trúc bảng trong file Word
