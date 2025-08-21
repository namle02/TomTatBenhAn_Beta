SELECT 
  Field_1 AS LyDoVaoVien, 
  N'Quá Trình bệnh lý: ' + CAST(
    Field_3 AS NVARCHAR(MAX)
  ) + N'/ Khám bệnh: ' + CAST(
    Field_6 AS NVARCHAR(MAX)
  ) + N'/ Bệnh chuyêNkhoa ' + CAST(
    Field_13 AS NVARCHAR(MAX)
  ) + N'/ Tuần hoàn: ' + CAST(
    Field_20 AS NVARCHAR(MAX)
  ) + N'/ Hô hấp: ' + CAST(
    Field_21 AS NVARCHAR(MAX)
  ) + N'/ Tiêu hóa: ' + CAST(
    Field_22 AS NVARCHAR(MAX)
  ) + N'/ tiết niệu: ' + CAST(
    Field_24 AS NVARCHAR(MAX)
  ) + N'/ Sinh dục: ' + CAST(
    Field_32 AS NVARCHAR(MAX)
  ) + N'/ Thần kinh: ' + CAST(
    Field_19 AS NVARCHAR(MAX)
  ) + N'/ Cơ Xương khớp: ' + CAST(
    Field_23 AS NVARCHAR(MAX)
  ) + N', Huyết áp: ' + CAST(
	Field_9 AS NVARCHAR(MAX)
  ) + '/' + CAST(
	Field_10 as NVARCHAR(MAX)
  ) + N' mmHg ' +
   N'/ Các dấu hiệu bệnh lý khác: ' + CAST(
    Field_33 AS NVARCHAR(MAX)
  ) AS QuaTrinhBenhLy, 
  Field_4 AS TienSuBenh, 
  Field_31 AS HuongDieuTri 
FROM 
  dbo.BenhAnTongQuat_BONG 
WHERE 
  BenhAnTongQuat_Id = @ID;
