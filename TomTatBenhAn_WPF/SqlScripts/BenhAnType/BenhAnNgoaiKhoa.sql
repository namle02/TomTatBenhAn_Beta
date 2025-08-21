SELECT 
    Field_1 AS LyDoVaoVien, 
    N'Quá Trình bệnh lý: ' + COALESCE(CAST(Field_3 AS NVARCHAR(MAX)), N'') +
    N'/ Khám bệnh: ' + COALESCE(CAST(Field_11 AS NVARCHAR(MAX)), N'') +
    N'/ Bệnh ngoại khoa: ' + COALESCE(CAST(Field_9 AS NVARCHAR(MAX)), N'') +
    N'/ Tuần hoàn: ' + COALESCE(CAST(Field_18 AS NVARCHAR(MAX)), N'') +
    N'/ Hô hấp: ' + COALESCE(CAST(Field_19 AS NVARCHAR(MAX)), N'') +
    N'/ Tiêu hóa: ' + COALESCE(CAST(Field_20 AS NVARCHAR(MAX)), N'') +
    N'/ Thận-tiết niệu-sinh dục: ' + COALESCE(CAST(Field_21 AS NVARCHAR(MAX)), N'') +
    N'/ Thần kinh: ' + COALESCE(CAST(Field_22 AS NVARCHAR(MAX)), N'') +
    N'/ Cơ Xương khớp: ' + COALESCE(CAST(Field_23 AS NVARCHAR(MAX)), N'') +
    N'/ Tai mũi họng: ' + COALESCE(CAST(Field_24 AS NVARCHAR(MAX)), N'') +
    N'/ Răng hàm mặt: ' + COALESCE(CAST(Field_6 AS NVARCHAR(MAX)), N'') +
    N'/ Mắt: ' + COALESCE(CAST(Field_8 AS NVARCHAR(MAX)), N'') +
    N', Huyết áp: ' + COALESCE(CAST(Field_14 AS NVARCHAR(MAX)), N'') + '/' +
    COALESCE(CAST(Field_15 AS NVARCHAR(MAX)), N'') + N' mmHg ' +
    N'/ Các dấu hiệu bệnh lý khác: ' + COALESCE(CAST(Field_7 AS NVARCHAR(MAX)), N'') 
    AS QuaTrinhBenhLy, 
    Field_4 AS TienSuBenh, 
    Field_10 AS HuongDieuTri 
FROM 
    dbo.BenhAnTongQuat_NgoaiKhoa 
WHERE 
    BenhAnTongQuat_Id = @ID;
