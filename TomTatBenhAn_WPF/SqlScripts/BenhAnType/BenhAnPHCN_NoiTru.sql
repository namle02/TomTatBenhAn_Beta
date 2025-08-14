SELECT 
  Field_7 AS LyDoVaoVien, 
  N'Quá Trình bệnh lý: ' + CAST(
    Field_8 AS NVARCHAR(MAX)
  ) + N'/ Khám bệnh: ' + N'/ Thể trạng chung: ' + CAST(
    Field_12 AS NVARCHAR(MAX)
  ) + N'/ Tình trạng đau: ' + CAST(
    Field_21 AS NVARCHAR(MAX)
  ) + N'/ Tri giác: ' + CAST(
    Field_22 AS NVARCHAR(MAX)
  ) + N'/ Thăng Bằng: ' + CAST(
    Field_23 AS NVARCHAR(MAX)
  ) + N'/ VậNđộng: ' + CAST(
    Field_24 AS NVARCHAR(MAX)
  ) + N'/ Điều hợp: ' + CAST(
    Field_25 AS NVARCHAR(MAX)
  ) + N'/ Cảm giác: ' + CAST(
    Field_26 AS NVARCHAR(MAX)
  ) + N'/ Hội chứng tiểu não: ' + CAST(
    Field_27 AS NVARCHAR(MAX)
  ) + N'/ PhảNxạ gâNxương: ' + CAST(
    Field_28 AS NVARCHAR(MAX)
  ) + N'/ Hội chứng ngoại tháp: ' + CAST(
    Field_29 AS NVARCHAR(MAX)
  ) + N'/ PhảNxạ da: ' + CAST(
    Field_30 AS NVARCHAR(MAX)
  ) + N'/ Các hội chứng tâm thần : ' + CAST(
    Field_31 AS NVARCHAR(MAX)
  ) + N'/ trương lực cơ: ' + CAST(
    Field_32 AS NVARCHAR(MAX)
  ) + N'/ Thần kinh khác: ' + CAST(
    Field_33 AS NVARCHAR(MAX)
  ) + N'/ Thần kinh sọ não: ' + CAST(
    Field_34 AS NVARCHAR(MAX)
  ) + N'/ Hình thể: ' + CAST(
    Field_35 AS NVARCHAR(MAX)
  ) + N'/ Cơ: ' + CAST(
    Field_36 AS NVARCHAR(MAX)
  ) + N'/ Tầm vậNđộng khớp: ' + CAST(
    Field_37 AS NVARCHAR(MAX)
  ) + N'/ Tình trạng bệnh lý cột sống: ' + CAST(
    Field_38 AS NVARCHAR(MAX)
  ) + N'/ Rối loạNchức năng cột sống: ' + CAST(
    Field_39 AS NVARCHAR(MAX)
  ) + N'/ Nhịp tim: ' + CAST(
    Field_40 AS NVARCHAR(MAX)
  ) + N'/ Lồng ngực: ' + CAST(
    Field_41 AS NVARCHAR(MAX)
  ) + N'/ Rối loạNchức năng tim mạch: ' + CAST(
    Field_42 AS NVARCHAR(MAX)
  ) + N'/ Tình trạng bệnh lý hô hấp: ' + CAST(
    Field_43 AS NVARCHAR(MAX)
  ) + N'/ Tình trạng bệnh lý tiêu hóa: ' + CAST(
    Field_44 AS NVARCHAR(MAX)
  ) + N'/ Tình trạng bệnh lý nội tiết: ' + CAST(
    Field_45 AS NVARCHAR(MAX)
  ) + N'/ Tiết niệu, sinh dục: ' + CAST(
    Field_46 AS NVARCHAR(MAX)
  ) + N'/ Các cơ quaNkhác: ' + CAST(
    Field_47 AS NVARCHAR(MAX)
  ) + N'/ Da và mô dưới da: ' + CAST(
    Field_48 AS NVARCHAR(MAX)
  ) AS QuaTrinhBenhLy, 
  N'Tiền sử dị ứng: ' + CAST(
    Field_9 AS NVARCHAR(MAX)
  ) + N'/ Tiền sử bảNthân: ' + CAST(
    Field_10 AS NVARCHAR(MAX)
  ) AS TienSuBenh, 
  Field_56 AS HuongDieuTri 
FROM 
  dbo.BenhAnTongQuat_PHCN
WHERE 
  BenhAnTongQuat_Id = @ID;
