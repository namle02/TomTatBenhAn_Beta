SELECT
	Field_12 AS LyDoVaoVien,
	N'Quá Trình bệnh lý: ' + CAST(Field_13 AS NVARCHAR(MAX)) +
	N'/ Toàn thân (ý thức, da niêm mạc, hệ thống hạch, tuyến giáp, vị trí, kích thước...): ' + CAST(Field_21 AS NVARCHAR(MAX)) +
	N'/ Mạch: ' + CAST(Field_22 AS NVARCHAR(MAX)) +
	N'/ Nhiệt độ: ' + CAST(Field_23 AS NVARCHAR(MAX)) +
	N'/ Huyết áp: ' + CAST(Field_24 AS NVARCHAR(MAX)) + N'/' + CAST(Field_25 AS NVARCHAR(MAX)) +
	N'/ Nhịp thở: ' + CAST(Field_26 AS NVARCHAR(MAX)) +
	N'/ Cân nặng: ' + CAST(Field_27 AS NVARCHAR(MAX)) +
	N'/ Chiều cao: ' + CAST(Field_28 AS NVARCHAR(MAX)) +
	N'/ IMB: ' + CAST(Field_29 AS NVARCHAR(MAX)) +
	N'/ Các bộ phận: ' +
	N'/ Tuần  hoàn: ' + CAST(Field_30 AS NVARCHAR(MAX)) +
	N'/ Hô hấp: ' + CAST(Field_31 AS NVARCHAR(MAX)) +
	N'/ Tiêu Hóa: ' + CAST(Field_32 AS NVARCHAR(MAX)) +
	N'/ Tiết niệu - sinh dục: ' + CAST(Field_33 AS NVARCHAR(MAX)) +
	N'/ Thần  kinh: ' + CAST(Field_34 AS NVARCHAR(MAX)) +
	N'/ Cơ xương khớp: ' + CAST(Field_35 AS NVARCHAR(MAX)) +
	N'/ Tai - mũi - họng: ' + CAST(Field_36 AS NVARCHAR(MAX)) +
	N'/ Răng - Hàm - Mặt: ' + CAST(Field_37 AS NVARCHAR(MAX)) +
	N'/ Mắt: ' + CAST(Field_38 AS NVARCHAR(MAX)) +
	N'/ Nội tiết, dinh dưỡng và các bệnh lý khác: ' + CAST(Field_39 AS NVARCHAR(MAX)) +
	N'/ Kết quả tóm tắt lâm sàng: ' + CAST(Field_40 AS NVARCHAR(MAX)) +
	N'/ Chuẩn đoán: ' +
	N'/ Bệnh chính: ' + CAST(Field_40 AS NVARCHAR(MAX)) +
	N'/ Phân biệt: ' + CAST(Field_41 AS NVARCHAR(MAX)) +
	N'/ Y học cổ truyền: ' +
	N'/ Vọng chẩn: ' + CAST(Field_42 AS NVARCHAR(MAX)) +
	N'/ Văn chẩn: ' + CAST(Field_43 AS NVARCHAR(MAX)) +
	N'/ Vấn chẩn: ' + CAST(Field_44 AS NVARCHAR(MAX)) +
	N'/ Thiết chẩn' +
	N'/ Xúc chẩn: ' + CAST(Field_45 AS NVARCHAR(MAX)) +
	N'/ Mạch tay trái: ' + CAST(Field_46 AS NVARCHAR(MAX)) +
	N'/ Mạch tay phải: ' + CAST(Field_47 AS NVARCHAR(MAX)) +
	N'/ Tóm Tắt Tứ Chẩn: ' + CAST(Field_48 AS NVARCHAR(MAX)) +
	N'/ Biện Chứng luận trị: ' + CAST(Field_49 AS NVARCHAR(MAX)) +
	N'/ Bệnh danh: ' + CAST(Field_50 AS NVARCHAR(MAX)) +
	N'/ Bát cương: ' + CAST(Field_51 AS NVARCHAR(MAX)) +
	N'/ Nguyên nhân: ' + CAST(Field_52 AS NVARCHAR(MAX)) +
	N'/ Tạng Phủ: ' + CAST(Field_53 AS NVARCHAR(MAX)) +
	N'/ Kinh mạch: ' + CAST(Field_54 AS NVARCHAR(MAX)) +
	N'/ Định vị bệnh (dinh, vệ, khí, huyết): ' + CAST(Field_55 AS NVARCHAR(MAX)) +
	N'/ Dự hậu tiên lượng: ' + CAST(Field_62 AS NVARCHAR(MAX)) AS QuatrinhBenhLy,
	N'Y Học hiện đại: ' + CAST(Field_66 AS NVARCHAR(MAX)) +
	CHAR(13)+CHAR(10) + N'Y học cổ truyền: ' + CAST(Field_67 AS NVARCHAR(MAX)) AS HuongDieuTri,
	N'Bản thân: ' + CAST(Field_56 AS NVARCHAR(MAX)) +
	CHAR(13)+CHAR(10) + N'Đặc điểm liên quan bệnh tật: ' + CAST(Field_60 AS NVARCHAR(MAX)) +
	CHAR(13)+CHAR(10) + N'Gia đình: ' + CAST(Field_20 AS NVARCHAR(MAX)) AS TienSuBenh
FROM
	dbo.BenhAnTongQuat_YHCT_NT_NEW
WHERE 
	BenhAnTongQuat_Id = @ID;