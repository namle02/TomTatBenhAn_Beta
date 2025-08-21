SELECT 
    Field_1 AS LyDoVaoVien,
    ISNULL(N'Quá trình bệnh lý: ' + CAST(Field_25 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Khám thai tại: ' + CAST(Field_3 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Bắt đầu chuyển dạ: ' + CAST(Field_23 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Dấu hiệu lúc đầu: ' + CAST(Field_32 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Biến chuyển: ' + CAST(Field_24 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Vào lúc đẻ: ' + CAST(Field_27 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Ngày mổ đẻ: ' + CAST(Field_75 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Cách thức đẻ: ' + CAST(Field_137 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Ngôi thai: ' + CAST(Field_136 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Kiểm soát tử cung: ' + CAST(Field_138 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Toàn trạng: ' + CAST(Field_11 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Tuần  hoàn: ' + CAST(Field_18 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Hô hấp: ' + CAST(Field_19 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Tiêu hóa: ' + CAST(Field_20 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Tiết niệu: ' + CAST(Field_21 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Các bộ phận khác: ' + CAST(Field_22 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Mạch: ' + CAST(Field_12 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Nhiệt độ: ' + CAST(Field_13 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Huyết áp: ' + CAST(Field_14 AS NVARCHAR(MAX)) + N'/' + CAST(Field_15 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Nhịp thở: ' + CAST(Field_16 AS NVARCHAR(MAX)), '') + N' / ' +
    ISNULL(N'Cân nặng: ' + CAST(Field_17 AS NVARCHAR(MAX)), '') AS QuaTrinhBenhLy,
    ISNULL(N'Bản thân: ' + CAST(Field_4 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
    ISNULL(N'Gia đình: ' + CAST(Field_5 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
    N'Tiền sử phụ khoa:' + CHAR(13) + CHAR(10) +
    N'Bắt đầu thấy kinh năm: ' + ISNULL(CAST(Field_33 AS NVARCHAR(MAX)), '') + 
    N', Tuổi: ' + ISNULL(CAST(Field_34 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
    N'Lấy chồng năm: ' + ISNULL(CAST(Field_35 AS NVARCHAR(MAX)), '') + 
    N', Tuổi: ' + ISNULL(CAST(Field_36 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
    ISNULL(N'Tính chất kinh nguyệt: ' + CAST(Field_37 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
    ISNULL(N'Chu kỳ (ngày): ' + CAST(Field_38 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
    ISNULL(N'Lượng kinh: ' + CAST(Field_39 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
    ISNULL(N'Những bệnh phụ khoa đã điều trị: ' + CAST(Field_40 AS NVARCHAR(MAX)), '')
    AS TienSuBenh,
    Field_31 AS HuongDieuTri
FROM 
    dbo.BenhAnTongQuat_SanKhoa
WHERE 
    BenhAnTongQuat_Id = @ID;