SELECT 
  Field_1 AS LyDoVaoVien, 
  N'Quá Trình bệnh lý: ' + CAST(
    Field_3 AS NVARCHAR(MAX)
  ) + N'/ Khám bệnh: ' + CAST(
    Field_11 AS NVARCHAR(MAX)
  ) + N'/ Bệnh ngoại khoa: ' + CAST(
    Field_9 AS NVARCHAR(MAX)
  ) + N'/ Tuần hoàn: ' + CAST(
    Field_18 AS NVARCHAR(MAX)
  ) + N'/ Hô hấp: ' + CAST(
    Field_19 AS NVARCHAR(MAX)
  ) + N'/ Tiêu hóa: ' + CAST(
    Field_20 AS NVARCHAR(MAX)
  ) + N'/ Thận-tiết niệu-sinh dục: ' + CAST(
    Field_21 AS NVARCHAR(MAX)
  ) + N'/ Thần kinh: ' + CAST(
    Field_22 AS NVARCHAR(MAX)
  ) + N'/ Cơ Xương khớp: ' + CAST(
    Field_23 AS NVARCHAR(MAX)
  ) + N'/ Tai mũi họng: ' + CAST(
    Field_24 AS NVARCHAR(MAX)
  ) + N'/ Răng hàm mặt: ' + CAST(
    Field_6 AS NVARCHAR(MAX)
  ) + N'/ Mắt: ' + CAST(
    Field_8 AS NVARCHAR(MAX)
  ) + N'/ Các dấu hiệu bệnh lý khác: ' + CAST(
    Field_7 AS NVARCHAR(MAX)
  ) AS QuaTrinhBenhLy, 
  Field_4 AS TienSuBenh, 
  Field_10 AS HuongDieuTri 
FROM 
  dbo.BenhAnTongQuat_NgoaiKhoa 
WHERE 
  BenhAnTongQuat_Id = @ID;
