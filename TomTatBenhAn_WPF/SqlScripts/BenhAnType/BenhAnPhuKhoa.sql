SELECT 
  Field_1 AS LyDoVaoVien, 
  N'Quá Trình bệnh lý: ' + CAST(
    Field_3 AS NVARCHAR(MAX)
  ) + N'/ Khám bệnh: ' + CAST(
    Field_11 AS NVARCHAR(MAX)
  ) + N'/ Hạch: ' + CAST(
    Field_9 AS NVARCHAR(MAX)
  ) + N'/ Vú: ' + CAST(
    Field_48 AS NVARCHAR(MAX)
  ) + N'/ Tuần hoàn: ' + CAST(
    Field_18 AS NVARCHAR(MAX)
  ) + N'/ Hô hấp: ' + CAST(
    Field_19 AS NVARCHAR(MAX)
  ) + N'/ Tiêu hóa: ' + CAST(
    Field_20 AS NVARCHAR(MAX)
  ) + N'/ Thận-tiết niệu-sinh dục: ' + CAST(
    Field_24 AS NVARCHAR(MAX)
  ) + N'/ Thần kinh: ' + CAST(
    Field_22 AS NVARCHAR(MAX)
  ) + N'/ Cơ Xương khớp: ' + CAST(
    Field_23 AS NVARCHAR(MAX)
  ) + N'/ Các dấu hiệu bệnh lý khác: ' + CAST(
    Field_6 AS NVARCHAR(MAX)
  ) AS QuaTrinhBenhLy, 
  CAST(
    Field_4 AS NVARCHAR(MAX)
  ) + N'/ Tiền sử phụ khoa: ' + CAST(
    Field_43 AS NVARCHAR(MAX)
  ) AS TienSuBenh, 
  Field_31 AS HuongDieuTri 
FROM 
  dbo.BenhAnTongQuat_PhuKhoa 
WHERE 
  BenhAnTongQuat_Id = @ID;
