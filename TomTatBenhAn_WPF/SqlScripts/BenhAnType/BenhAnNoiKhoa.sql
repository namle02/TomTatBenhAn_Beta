SELECT 
  Field_1 AS LyDoVaoVien, 
  N'Quá Trình bệnh lý: ' + CAST(
    Field_3 AS NVARCHAR(MAX)
  ) + N' Khám bệnh: ' + CAST(
    Field_11 AS NVARCHAR(MAX)
  ) + N'Tuần  hoàn: ' + CAST(
    Field_18 AS NVARCHAR(MAX)
  ) + N'Hô Hấp: ' + CAST(
    Field_19 AS NVARCHAR(MAX)
  ) + N'Tiêu hóa: ' + CAST(
    Field_20 AS NVARCHAR(MAX)
  ) + N'Thận-tiết niệu-sinh dục: ' + CAST(
    Field_21 AS NVARCHAR(MAX)
  ) + N'Thần  kinh: ' + CAST(
    Field_22 AS NVARCHAR(MAX)
  ) + N'Cơ Xương khớp: ' + CAST(
    Field_23 AS NVARCHAR(MAX)
  ) + N'Các dấu hiệu bệnh lý khác: ' + CAST(
    Field_24 AS NVARCHAR(MAX)
  ) AS QuaTrinhBenhLy, 
  Field_4 AS TienSuBenh, 
  Field_9 AS HuongDieuTri 
FROM 
  dbo.BenhAnTongQuat_NoiKhoa 
WHERE 
  BenhAnTongQuat_Id = @ID;
