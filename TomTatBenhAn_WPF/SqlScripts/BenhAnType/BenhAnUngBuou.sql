SELECT 
  Field_1 AS LyDoVaoVien, 
  N'Quá Trình bệnh lý: ' + CAST(
    Field_3 AS NVARCHAR(MAX)
  ) + N'/ Khám bệnh: ' + CAST(
    Field_6 AS NVARCHAR(MAX)
  ) + N'/ Bộ phận tổn thương: ' + CAST(
    Field_15 AS NVARCHAR(MAX)
  ) + N'/ Tuần  hoàn: ' + CAST(
    Field_20 AS NVARCHAR(MAX)
  ) + N'/ Hô hấp: ' + CAST(
    Field_21 AS NVARCHAR(MAX)
  ) + N'/ Tiêu hóa: ' + CAST(
    Field_22 AS NVARCHAR(MAX)
  ) + N'/ Thận-tiết niệu-sinh dục: ' + CAST(
    Field_19 AS NVARCHAR(MAX)
  ) + N'/ Thần  kinh: ' + CAST(
    Field_18 AS NVARCHAR(MAX)
  ) + N'/ Cơ Xương khớp: ' + CAST(
    Field_14 AS NVARCHAR(MAX)
  ) + N'/ Các dấu hiệu bệnh lý khác: ' + CAST(
    Field_33 AS NVARCHAR(MAX)
  ) AS QuaTrinhBenhLy, 
  Field_4 AS TienSuBenh, 
  Field_31 AS HuongDieuTri 
FROM 
  dbo.BenhAnTongQuat_UngBuou 
WHERE 
  BenhAnTongQuat_Id = @ID;