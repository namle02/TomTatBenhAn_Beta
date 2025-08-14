Select 
  Field_25 as LyDoVaoVien, 
  N'Quá Trình bệnh lý: ' + CAST(
    Field_26 AS NVARCHAR(MAX)
  ) + N'/Toàn thân(ý thức, da niêm mạc,hệ thống hạch,tuyến giáp,vị trí, kích thước...): ' + CAST(
    Field_35 AS NVARCHAR(MAX)
  ) + N'/Mạch: ' + CAST(
    Field_36 AS NVARCHAR(MAX)
  ) + N'/Nhiệt độ: ' + CAST(
    Field_43 AS NVARCHAR(MAX)
  ) + N'/Huyết áp: ' + CAST(
    Field_38 AS NVARCHAR(MAX)
  ) + N'/' + CAST(
    Field_39 AS NVARCHAR(MAX)
  ) + N'/Nhịp thở: ' + CAST(
    Field_41 AS NVARCHAR(MAX)
  ) + N'/Cân nặng: ' + CAST(
    Field_42 AS NVARCHAR(MAX)
  ) + N'/Chiều cao: ' + CAST(
    Field_37 AS NVARCHAR(MAX)
  ) + N'/IMB: ' + CAST(
    Field_40 AS NVARCHAR(MAX)
  ) + N'/Các bộ phận: ' + N'/Tuần  hoàn:' + CAST(
    Field_44 AS NVARCHAR(MAX)
  ) + N'/Hô hấp:' + CAST(
    Field_45 AS NVARCHAR(MAX)
  ) + N'/Tiêu Hóa:' + CAST(
    Field_46 AS NVARCHAR(MAX)
  ) + N'/Tiết niệu - sinh dục:' + CAST(
    Field_47 AS NVARCHAR(MAX)
  ) + N'/Thần  kinh:' + CAST(
    Field_48 AS NVARCHAR(MAX)
  ) + N'/Cơ xương khớp:' + CAST(
    Field_49 AS NVARCHAR(MAX)
  ) + N'/Tai -mũi - họng:' + CAST(
    Field_50 AS NVARCHAR(MAX)
  ) + N'/Răng - Hàm - Mặt:' + CAST(
    Field_51 AS NVARCHAR(MAX)
  ) + N'/Mắt:' + CAST(
    Field_52 AS NVARCHAR(MAX)
  ) + N'/Nội tiết, diinh dưỡng và các bệnh lý khác:' + CAST(
    Field_53 AS NVARCHAR(MAX)
  ) + N'/cận lâm sàng:' + CAST(
    Field_54 AS NVARCHAR(MAX)
  )+ N'/Tóm tắt bệnh án' + CAST(
    Field_55 AS NVARCHAR(MAX)
  )+ N'/Chuẩn đoán:' + N'/Bệnh chính:' + CAST(
    Field_40 AS NVARCHAR(MAX)
  )+ N'/Phân biệt:' + CAST(
    Field_56 AS NVARCHAR(MAX)
  ) as QuatrinhBenhLy, 
  N'Y Học hiện đại: ' + CAST(
    Field_388 AS NVARCHAR(MAX)
  ) + CHAR(13)+ CHAR(10) + N'Y học cổ truyền: ' + CAST(
    Field_389 AS NVARCHAR(MAX)
  ) AS HuongDieuTri, 
  N' Bản thân:' + CAST(
    Field_32 AS NVARCHAR(MAX)
  )+ CHAR(13)+ CHAR(10) + N' Đặc điểm liên quan bệnh tật:' + CAST(
    Field_33 AS NVARCHAR(MAX)
  )+ CHAR(13)+ CHAR(10) + N' Gia đình:' + CAST(
    Field_34 AS NVARCHAR(MAX)
  ) as TienSuBenh 
from 
  BenhAnTongQuat_YHCT_NoiTru_New 
where 
  BenhAnTongQuat_Id = @ID;
