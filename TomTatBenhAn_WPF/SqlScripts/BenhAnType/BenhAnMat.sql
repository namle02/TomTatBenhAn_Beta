SELECT 
  Field_1 AS LyDoVaoVien, 
  N'Quá Trình bệnh lý: ' + CAST(
    Field_3 AS NVARCHAR(MAX)
  ) + N'/ Khám bệnh: ' + CAST(
    Field_45 AS NVARCHAR(MAX)
  ) + N'/ Nội tiết: ' + CAST(
    Field_52 AS NVARCHAR(MAX)
  ) + N'/ Tuần hoàn: ' + CAST(
    Field_54 AS NVARCHAR(MAX)
  ) + N'/ Hô hấp: ' + CAST(
    Field_55 AS NVARCHAR(MAX)
  ) + N'/ Tiêu hóa: ' + CAST(
    Field_56 AS NVARCHAR(MAX)
  ) + N'/ Thận-tiết niệu-sinh dục: ' + CAST(
    Field_58 AS NVARCHAR(MAX)
  ) + N'/ Thần kinh: ' + CAST(
    Field_53 AS NVARCHAR(MAX)
  ) + N'/ Cơ Xương khớp: ' + CAST(
    Field_57 AS NVARCHAR(MAX)
  )  + N', Huyết áp: ' + CAST(
	Field_48 AS NVARCHAR(MAX)
  ) + '/' + CAST(
	Field_49 as NVARCHAR(MAX)
  ) + N' mmHg '  + N'/ Các dấu hiệu bệnh lý khác: ' + CAST(
    Field_59 AS NVARCHAR(MAX)
  ) AS QuaTrinhBenhLy, 
  Field_4 AS TienSuBenh, 
  Field_66 AS HuongDieuTri 
FROM 
  dbo.BenhAnTongQuat_Mat 
WHERE 
  BenhAnTongQuat_Id = @ID;
