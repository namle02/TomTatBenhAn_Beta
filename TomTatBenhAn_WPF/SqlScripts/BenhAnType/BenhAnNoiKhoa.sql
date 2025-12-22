SELECT 
  Field_1 AS LyDoVaoVien, 
  -- Tối ưu string concatenation bằng CONCAT (tự động handle NULL)
  CONCAT(
    N'Quá Trình bệnh lý: ', Field_3,
    N' Khám bệnh: ', Field_11,
    N'Tuần  hoàn: ', Field_18,
    N'Hô Hấp: ', Field_19,
    N'Tiêu hóa: ', Field_20,
    N'Thận-tiết niệu-sinh dục: ', Field_21,
    N'Thần  kinh: ', Field_22,
    N'Cơ Xương khớp: ', Field_23,
    N', Huyết áp: ', Field_14, '/', Field_15, N' mmHG, Các dấu hiệu bệnh lý khác: ', Field_24
  ) AS QuaTrinhBenhLy, 
  Field_4 AS TienSuBenh, 
  Field_9 AS HuongDieuTri 
FROM 
  dbo.BenhAnTongQuat_NoiKhoa 
WHERE 
  BenhAnTongQuat_Id = @ID;
