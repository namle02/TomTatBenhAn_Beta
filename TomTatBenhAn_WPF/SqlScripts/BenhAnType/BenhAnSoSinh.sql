SELECT 
N'Quá Trình bệnh lý: ' + ISNULL(CAST(ss.Field_3 AS NVARCHAR(MAX)), '') +
N'/ Ối vỡ lúc: ' + ISNULL(CAST(ss.Field_34 AS NVARCHAR(MAX)), '') + 
N'/ Màu sắc: ' + ISNULL(CAST(ss.Field_4 AS NVARCHAR(MAX)), '') + 
N'/ Cách đẻ thường: ' + ISNULL(CAST(ss.Field_35 AS NVARCHAR(MAX)), '') + 
N' (Can thiệp: ' + ISNULL(CAST(ss.Field_36 AS NVARCHAR(MAX)), '') + 
N', Thời gian: ' + ISNULL(CAST(ss.Field_34 AS NVARCHAR(MAX)), '') + N')' +
N'/ Lý do can thiệp: ' + ISNULL(CAST(ss.Field_38 AS NVARCHAR(MAX)), '') +
N'/ Nhóm máu mẹ: ' + ISNULL(CAST(ss.Field_76 AS NVARCHAR(MAX)), '') +
N'/ Tiền sử para: ' +
    N'Sinh đủ tháng:' + ISNULL(CAST(ss.Field_77 AS NVARCHAR(MAX)), '') +
    N' Sớm để non:' + ISNULL(CAST(ss.Field_78 AS NVARCHAR(MAX)), '') +
    N' Sớm nạo hút:' + ISNULL(CAST(ss.Field_79 AS NVARCHAR(MAX)), '') +
    N' Sống:' + ISNULL(CAST(ss.Field_80 AS NVARCHAR(MAX)), '') +
N'/ Tình trạng sơ sinh lúc ra đời:' +
    N' Khóc ' + ISNULL(CAST(ss.Field_39 AS NVARCHAR(MAX)), '') +
    N' Ngạt ' + ISNULL(CAST(ss.Field_40 AS NVARCHAR(MAX)), '') +
    N' Khác ' + ISNULL(CAST(ss.Field_41 AS NVARCHAR(MAX)), '') +
N'/ Người đỡ đẻ phẫu thuật:' + ISNULL(CAST(ss.Field_75 AS NVARCHAR(MAX)), '') +
N'/ Apgar ' +
    N'1 phút:' + ISNULL(CAST(ss.Field_42 AS NVARCHAR(MAX)), '') +
    N' 5 phút:' + ISNULL(CAST(ss.Field_43 AS NVARCHAR(MAX)), '') +
    N' 10 phút:' + ISNULL(CAST(ss.Field_44 AS NVARCHAR(MAX)), '') +
N' Cân nặng:' + ISNULL(CAST(ss.Field_45 AS NVARCHAR(MAX)), '') +
N'/ Tình trạng dinh dưỡng sau sinh:' + ISNULL(CAST(ss.Field_46 AS NVARCHAR(MAX)), '')+
N'/ Phương pháp hồi sinh ngay sau đẻ:'+
N'/ Hút dịch:' + ISNULL(CAST(ss.Field_47 AS NVARCHAR(MAX)), '')+
N'/ Xoa bóp tim:' + ISNULL(CAST(ss.Field_48 AS NVARCHAR(MAX)), '')+
N'/ Thở O2:' + ISNULL(CAST(ss.Field_49 AS NVARCHAR(MAX)), '')+
N'/ Đặt nội khí quản:' + ISNULL(CAST(ss.Field_50 AS NVARCHAR(MAX)), '')+
N'/ Bóp bóng O2:' + ISNULL(CAST(ss.Field_51 AS NVARCHAR(MAX)), '')+
N'/ Khác:' + ISNULL(CAST(ss.Field_52 AS NVARCHAR(MAX)), '')+
N'/ Toàn thân:' +
N'/ Cụ thể dị tật:' + ISNULL(CAST(ss.Field_6 AS NVARCHAR(MAX)), '')+
N'/ Tình hình sơ sinh khi vào khoa:' + ISNULL(CAST(ss.Field_13 AS NVARCHAR(MAX)), '')+
N'/ Tình hình toàn thân:' + ISNULL(CAST(ss.Field_17 AS NVARCHAR(MAX)), '')+
N'/ Chiều dài:' + ISNULL(CAST(ss.Field_7 AS NVARCHAR(MAX)), '')+
N'/ Vòng đầu:' + ISNULL(CAST(ss.Field_11 AS NVARCHAR(MAX)), '')+
N'/ Nhiệt độ:' + ISNULL(CAST(ss.Field_8 AS NVARCHAR(MAX)), '')+
N'/ Nhịp thở:' + ISNULL(CAST(ss.Field_19 AS NVARCHAR(MAX)), '')+
N'/ Hô hấp:' + ISNULL(CAST(ss.Field_73 AS NVARCHAR(MAX)), '')+
N'/ Dấu hiệu gắng sức:' + ISNULL(CAST(ss.Field_74 AS NVARCHAR(MAX)), '')+
N'/ Nghe phổi:' + ISNULL(CAST(ss.Field_21 AS NVARCHAR(MAX)), '')+
N'/ Chỉ số silverman:' + ISNULL(CAST(ss.Field_22 AS NVARCHAR(MAX)), '')+
N'/ Tim mạch (Nhịp tim):' + ISNULL(CAST(ss.Field_24 AS NVARCHAR(MAX)), '')+
N'/ Bụng:' + ISNULL(CAST(ss.Field_32 AS NVARCHAR(MAX)), '')+
N'/ Cơ quan sinh dục ngoài:' + ISNULL(CAST(ss.Field_33 AS NVARCHAR(MAX)), '')+
N'/ Xương khớp:' + ISNULL(CAST(ss.Field_25 AS NVARCHAR(MAX)), '')+
N'/ Phản xạ:' + ISNULL(CAST(ss.Field_62 AS NVARCHAR(MAX)), '')+
N'/ Chương lực cơ:' + ISNULL(CAST(ss.Field_63 AS NVARCHAR(MAX)), '')+
N'/ Các xét nghiệm lâm sàng cần  làm:' + ISNULL(CAST(ss.Field_26 AS NVARCHAR(MAX)), '')+
N'/ Tóm tắt bệnh án:' + ISNULL(CAST(ss.Field_27 AS NVARCHAR(MAX)), '')+
N'/ Chỉ định theo dõi:' + ISNULL(CAST(ss.Field_28 AS NVARCHAR(MAX)), '')
AS QuaTrinhBenhLy,
Field_1 AS LyDoVaoVien,
CAST(NULL AS NVARCHAR(MAX)) AS TienSuBenh,
bac.PPDT AS HuongDieuTri
FROM dbo.BenhAnTongQuat_SoSinh AS ss
JOIN dbo.BenhAnTongQuat AS batq ON ss.BenhAnTongQuat_Id = batq.BenhAnTongQuat_Id
JOIN dbo.BenhAn AS ba ON batq.BenhAn_Id = ba.BenhAn_Id
JOIN dbo.BenhAnChiTiet AS bac ON ba.BenhAn_Id = bac.BenhAn_Id
WHERE ss.BenhAnTongQuat_Id = @ID;