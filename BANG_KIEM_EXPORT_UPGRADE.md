# Nâng cấp chức năng xuất bảng kiểm ra file Word

## Tổng quan
Đã hoàn thiện chức năng xuất bảng kiểm ra file Word với việc tự động điền các trường thông tin bệnh nhân, thống kê và kết quả đánh giá.

## Các tính năng đã thêm

### 1. Tính toán thống kê tiêu chí
- **Tổng tiêu chí**: Đếm tổng số tiêu chí trong bảng kiểm
- **Tiêu chí có dấu \***: Đếm số tiêu chí quan trọng (isImportant = true)
- **Tiêu chí thường**: Đếm số tiêu chí không có dấu sao
- **Tiêu chí đạt**: Đếm số tiêu chí đã đạt yêu cầu
- **Tiêu chí được đánh giá**: Đếm số tiêu chí đã được đánh giá (đạt/không đạt/không áp dụng)

### 2. Cập nhật thông tin bệnh nhân
- **Họ tên người bệnh**: Lấy từ `patientData.ThongTinHanhChinh.TenBN`
- **Số HSBA**: Lấy từ `patientData.ThongTinHanhChinh.SoBenhAn`
- **Ngày vào viện**: Lấy từ `patientData.ThongTinHanhChinh.NgayVaoVien`
- **Ngày ra viện**: Lấy từ `patientData.ThongTinHanhChinh.NgayRaVien`
- **BS điều trị**: Tạm thời để trống (cần thêm thuộc tính vào model)
- **Ngày đánh giá**: Tự động lấy ngày hiện tại

### 3. Cập nhật thống kê và kết quả
- **Tổng tiêu chí**: Hiển thị tổng số tiêu chí
- **Số tiêu chí có dấu \***: Hiển thị số tiêu chí quan trọng
- **Số tiêu chí thường áp dụng**: Hiển thị số tiêu chí thường
- **Kết quả tiêu chí có dấu \***: Hiển thị dạng "số đạt/tổng được đánh giá"
- **Kết quả tiêu chí thường**: Hiển thị dạng "số đạt/tổng được đánh giá"

### 4. Cập nhật kết luận
- **Tiêu chuẩn đạt**: Tất cả tiêu chí có dấu \* phải đạt và tiêu chí thường đạt > 90%
- **Tiêu chuẩn không đạt**: Nếu có tiêu chí có dấu \* không đạt hoặc tiêu chí thường đạt ≤ 90%
- **Đánh dấu kết luận**: Tự động đánh dấu ✓ vào kết luận đúng

## Các file đã được cập nhật

### 1. `PhacDoReportServices.cs`
- Thêm class `BangKiemStatistics` để lưu trữ thống kê
- Thêm phương thức `CalculateBangKiemStatistics()` để tính toán thống kê
- Thêm phương thức `UpdatePatientInfoAndStatisticsInWord()` để cập nhật thông tin
- **Cập nhật phương thức `UpdatePatientFields()`** để xử lý đúng cấu trúc paragraph:
  - `UpdatePatientNameAndHSBA()`: Cập nhật họ tên và số HSBA cùng một dòng
  - `UpdateDatesAndDoctor()`: Cập nhật ngày vào/ra viện, BS điều trị và ngày đánh giá cùng một dòng
- **Cập nhật phương thức `UpdateStatisticsFields()`** để xử lý đúng cấu trúc paragraph:
  - `UpdateTotalCriteriaAndBreakdown()`: Cập nhật tổng tiêu chí và phân loại cùng một dòng
  - `UpdateEvaluationResults()`: Cập nhật kết quả đánh giá cùng một dòng
- **Cập nhật phương thức `UpdateConclusionFields()`** để xử lý đúng cấu trúc paragraph:
  - `UpdateConclusion()`: Cập nhật kết luận đạt/không đạt
- Thêm các phương thức helper để xử lý text trong paragraph:
  - `UpdateTextBetweenMarkers()`: Cập nhật text giữa hai marker
  - `UpdateTextAfterLastMarker()`: Cập nhật text sau marker cuối cùng
  - `UpdateDatePattern()`: Cập nhật pattern ngày tháng
- Cập nhật `CreateOutputFileWithDataAsync()` để nhận thêm tham số `patientData`

### 2. `IPhacDoReportServices.cs`
- Cập nhật interface để thêm tham số `patientData` vào phương thức `CreateOutputFileWithDataAsync()`

### 3. `KiemTraPhacDoVM.cs`
- Cập nhật lời gọi `CreateOutputFileWithDataAsync()` để truyền `PatientData`

## Cách sử dụng

Khi người dùng nhấn nút "Xuất bảng kiểm" trong giao diện:

1. Hệ thống sẽ tải file Word gốc từ server
2. Copy file gốc sang thư mục output
3. Mở file Word bằng OpenXML
4. Cập nhật dữ liệu bảng (các tiêu chí đã đánh giá)
5. Cập nhật thông tin bệnh nhân và thống kê
6. Cập nhật kết luận đạt/không đạt
7. Lưu file và thông báo thành công

## Cấu trúc Paragraph thực tế

Dựa trên phân tích file Word gốc, các trường thông tin được tổ chức như sau:

### Paragraph 1: Thông tin bệnh nhân
```
"Họ tên người bệnh:                                                    Số HSBA:"
```

### Paragraph 2: Ngày tháng và BS điều trị
```
"Ngày vào viện:  / /202; Ngày ra   / /202;             BS điều trị:                                      ; Ngày đánh giá:"
```

### Paragraph 3: Thống kê tiêu chí
```
"- Tổng tiêu chí 24 trong đó: Số tiêu chí dấu* là 8; Số tiêu chí thường áp dụng16"
```

### Paragraph 4: Kết quả đánh giá
```
"Kết quả: 1) Số lượng tiêu chí có dấu * đạt/tiêu chí * được đánh giá:          ; 2) số tiêu chí thường đạt/tiêu chí được đánh giá:"
```

### Paragraph 5: Kết luận
```
"- Kết luận (khoanh tròn số tương ứng): 1) Đạt   ;  2)  Không đạt"
```

## Lưu ý

- **BS điều trị**: Hiện tại để trống vì không có thuộc tính `BacSiDieuTri` trong model `NoiTruKhamBenh`. Có thể cần thêm thuộc tính này hoặc lấy từ nguồn khác.
- **Định dạng ngày tháng**: Sử dụng định dạng "dd/MM/yyyy"
- **Tiêu chuẩn đạt**: Tiêu chí có dấu \* phải đạt 100% và tiêu chí thường phải đạt > 90%
- **Xử lý paragraph**: Các trường thông tin không nằm riêng biệt mà được ghép lại với nhau trong cùng một paragraph

## Kết quả

File Word xuất ra sẽ có đầy đủ thông tin:
- Thông tin bệnh nhân được điền tự động
- Thống kê tiêu chí được tính toán chính xác
- Kết quả đánh giá được hiển thị rõ ràng
- Kết luận đạt/không đạt được đánh dấu tự động
