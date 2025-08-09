using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTatBenhAn_WPF.Repos.Model
{
    public class QueryStorage
    {
        private static QueryStorage instance;
        public static QueryStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QueryStorage();
                }
                return instance;
            }
        }
        private QueryStorage() { }

        public Dictionary<string, object> Storage = new Dictionary<string, object>
            {
                {"bệnh án truyền nhiễm", @"SELECT 
                                            Field_1 AS LyDoVaoVien, 
                                            N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                            N'/ Khám bệnh: ' + CAST(Field_11 AS NVARCHAR(MAX)) + 
                                            N'/ Tuần hoàn: ' + CAST(Field_18 AS NVARCHAR(MAX)) + 
                                            N'/ Hô Hấp: ' + CAST(Field_19 AS NVARCHAR(MAX)) + 
                                            N'/ Tiêu hóa: ' + CAST(Field_20 AS NVARCHAR(MAX)) + 
                                            N'/ Thận-tiết niệu-sinh dục: ' + CAST(Field_21 AS NVARCHAR(MAX)) + 
                                            N'/ Thần kinh: ' + CAST(Field_22 AS NVARCHAR(MAX)) + 
                                            N'/ Cơ Xương khớp: ' + CAST(Field_23 AS NVARCHAR(MAX)) + 
                                            N'/ Các dấu hiệu bệnh lý khác: ' + CAST(Field_24 AS NVARCHAR(MAX)) AS QuaTrinhBenhLy, 
                                            Field_4 AS TienSuBenh, 
                                            Field_31 AS HuongDieuTri
                                        FROM 
                                            dbo.BenhAnTongQuat_TruyenNhiem
                                        WHERE 
                                            BenhAnTongQuat_Id = @ID;"},
                {"bệnh án nội khoa", @"SELECT 
                                            Field_1 AS LyDoVaoVien,     
                                            N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                            N'/ Khám bệnh: ' + CAST(Field_11 AS NVARCHAR(MAX)) + 
                                            N'/ Tuần hoàn: ' + CAST(Field_18 AS NVARCHAR(MAX)) + 
                                            N'/ Hô Hấp: ' + CAST(Field_19 AS NVARCHAR(MAX)) + 
                                            N'/ Tiêu hóa: ' + CAST(Field_20 AS NVARCHAR(MAX)) + 
                                            N'/ Thận-tiết niệu-sinh dục: ' + CAST(Field_21 AS NVARCHAR(MAX)) + 
                                            N'/ Thần kinh: ' + CAST(Field_22 AS NVARCHAR(MAX)) + 
                                            N'/ Cơ Xương khớp: ' + CAST(Field_23 AS NVARCHAR(MAX)) + 
                                            N'/ Các dấu hiệu bệnh lý khác: ' + CAST(Field_24 AS NVARCHAR(MAX)) AS QuaTrinhBenhLy, 
                                            Field_4 AS TienSuBenh, 
                                            Field_9 AS HuongDieuTri
                                        FROM 
                                            dbo.BenhAnTongQuat_NoiKhoa
                                        WHERE 
                                            BenhAnTongQuat_Id = @ID;"},
                {"bệnh án mắt", @"SELECT 
                                    Field_1 AS LyDoVaoVien, 
                                    N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                    N'/ Khám bệnh: ' + CAST(Field_45 AS NVARCHAR(MAX)) + 
                                    N'/ Nội tiết: ' + CAST(Field_52 AS NVARCHAR(MAX)) + 
                                    N'/ Tuần hoàn: ' + CAST(Field_54 AS NVARCHAR(MAX)) + 
                                    N'/ Hô hấp: ' + CAST(Field_55 AS NVARCHAR(MAX)) + 
	                                N'/ Tiêu hóa: ' + CAST(Field_56 AS NVARCHAR(MAX)) +
                                    N'/ Thận-tiết niệu-sinh dục: ' + CAST(Field_58 AS NVARCHAR(MAX)) + 
                                    N'/ Thần kinh: ' + CAST(Field_53 AS NVARCHAR(MAX)) + 
                                    N'/ Cơ Xương khớp: ' + CAST(Field_57 AS NVARCHAR(MAX)) + 
                                    N'/ Các dấu hiệu bệnh lý khác: ' + CAST(Field_59 AS NVARCHAR(MAX)) AS QuaTrinhBenhLy, 
                                    Field_4 AS TienSuBenh, 
                                    Field_66 AS HuongDieuTri
                                FROM 
                                    dbo.BenhAnTongQuat_Mat
                                WHERE 
                                    BenhAnTongQuat_Id = @ID;"},
                {"bệnh án ngoại khoa", @"SELECT 
                                            Field_1 AS LyDoVaoVien, 
                                            N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                            N'/ Khám bệnh: ' + CAST(Field_11 AS NVARCHAR(MAX)) + 
	                                        N'/ Bệnh ngoại khoa: ' + CAST(Field_9 AS NVARCHAR(MAX)) +
                                            N'/ Tuần hoàn: ' + CAST(Field_18 AS NVARCHAR(MAX)) + 
                                            N'/ Hô hấp: ' + CAST(Field_19 AS NVARCHAR(MAX)) + 
                                            N'/ Tiêu hóa: ' + CAST(Field_20 AS NVARCHAR(MAX)) + 
	                                        N'/ Thận-tiết niệu-sinh dục: ' + CAST(Field_21 AS NVARCHAR(MAX)) +
                                            N'/ Thần kinh: ' + CAST(Field_22 AS NVARCHAR(MAX)) + 
                                            N'/ Cơ Xương khớp: ' + CAST(Field_23 AS NVARCHAR(MAX)) + 
                                            N'/ Tai mũi họng: ' + CAST(Field_24 AS NVARCHAR(MAX)) + 
                                            N'/ Răng hàm mặt: ' + CAST(Field_6 AS NVARCHAR(MAX)) + 
                                            N'/ Mắt: ' + CAST(Field_8 AS NVARCHAR(MAX)) + 
                                            N'/ Các dấu hiệu bệnh lý khác: ' + CAST(Field_7 AS NVARCHAR(MAX)) AS QuaTrinhBenhLy, 
                                            Field_4 AS TienSuBenh, 
                                            Field_10 AS HuongDieuTri
                                        FROM 
                                            dbo.BenhAnTongQuat_NgoaiKhoa
                                        WHERE 
                                            BenhAnTongQuat_Id = @ID;"},
                {"bệnh án nhi khoa", @"SELECT 
                                            Field_1 AS LyDoVaoVien, 
                                            N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                            N'/ Khám bệnh: ' + CAST(Field_6 AS NVARCHAR(MAX)) + 
                                            N'/ Tuần hoàn: ' + CAST(Field_19 AS NVARCHAR(MAX)) + 
                                            N'/ Hô hấp: ' + CAST(Field_20 AS NVARCHAR(MAX)) + 
                                            N'/ Tiêu hóa: ' + CAST(Field_21 AS NVARCHAR(MAX)) + 
	                                        N'/ Thận-tiết niệu-sinh dục: ' + CAST(Field_22 AS NVARCHAR(MAX)) +
                                            N'/ Thần kinh: ' + CAST(Field_23 AS NVARCHAR(MAX)) + 
                                            N'/ Cơ Xương khớp: ' + CAST(Field_24 AS NVARCHAR(MAX)) +  
                                            N'/ Các dấu hiệu bệnh lý khác: ' + CAST(Field_32 AS NVARCHAR(MAX)) AS QuaTrinhBenhLy, 
                                            Field_4 AS TienSuBenh, 
                                            Field_31 AS HuongDieuTri
                                        FROM 
                                            dbo.BenhAnTongQuat_NhiKhoa
                                        WHERE 
                                            BenhAnTongQuat_Id = @ID;"},
                {"bệnh án phụ khoa", @"SELECT 
                                            Field_1 AS LyDoVaoVien, 
                                            N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                            N'/ Khám bệnh: ' + CAST(Field_11 AS NVARCHAR(MAX)) + 
	                                        N'/ Hạch: ' + CAST(Field_9 AS NVARCHAR(MAX)) +
	                                        N'/ Vú: ' + CAST(Field_48 AS NVARCHAR(MAX)) +
                                            N'/ Tuần hoàn: ' + CAST(Field_18 AS NVARCHAR(MAX)) + 
                                            N'/ Hô hấp: ' + CAST(Field_19 AS NVARCHAR(MAX)) + 
                                            N'/ Tiêu hóa: ' + CAST(Field_20 AS NVARCHAR(MAX)) + 
	                                        N'/ Thận-tiết niệu-sinh dục: ' + CAST(Field_24 AS NVARCHAR(MAX)) +
                                            N'/ Thần kinh: ' + CAST(Field_22 AS NVARCHAR(MAX)) + 
                                            N'/ Cơ Xương khớp: ' + CAST(Field_23 AS NVARCHAR(MAX)) +  
                                            N'/ Các dấu hiệu bệnh lý khác: ' + CAST(Field_6 AS NVARCHAR(MAX)) AS QuaTrinhBenhLy, 
                                            CAST(Field_4 AS NVARCHAR(MAX)) + 
	                                        N'/ Tiền sử phụ khoa: ' + CAST(Field_43 AS NVARCHAR(MAX)) AS TienSuBenh, 
                                            Field_31 AS HuongDieuTri
                                        FROM 
                                            dbo.BenhAnTongQuat_PhuKhoa
                                        WHERE 
                                            BenhAnTongQuat_Id = @ID;"},
                {"bệnh án răng-hàm-mặt", @"SELECT 
                                            Field_1 AS LyDoVaoVien, 
                                            N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                            N'/ Khám bệnh: ' + CAST(Field_6 AS NVARCHAR(MAX)) + 
	                                        N'/ Bệnh chuyên khoa: ' + CAST(Field_13 AS NVARCHAR(MAX)) +
                                            N'/ Tuần hoàn: ' + CAST(Field_20 AS NVARCHAR(MAX)) + 
                                            N'/ Hô hấp: ' + CAST(Field_21 AS NVARCHAR(MAX)) + 
                                            N'/ Tiêu hóa: ' + CAST(Field_22 AS NVARCHAR(MAX)) + 
	                                        N'/ Da và mô dưới da: ' + CAST(Field_23 AS NVARCHAR(MAX)) + 
	                                        N'/ Thận-tiết niệu-sinh dục: ' + CAST(Field_32 AS NVARCHAR(MAX)) +
                                            N'/ Thần kinh: ' + CAST(Field_19 AS NVARCHAR(MAX)) + 
                                            N'/ Cơ Xương khớp: ' + CAST(Field_24 AS NVARCHAR(MAX)) +  
                                            N'/ Các dấu hiệu bệnh lý khác: ' + CAST(Field_33 AS NVARCHAR(MAX)) AS QuaTrinhBenhLy, 
                                            Field_4  AS TienSuBenh, 
                                            Field_31 AS HuongDieuTri
                                        FROM 
                                            dbo.BenhAnTongQuat_RHM
                                        WHERE 
                                            BenhAnTongQuat_Id = @ID;"},
                {"bệnh án tai-mũi-họng", @"SELECT 
                                                Field_1 AS LyDoVaoVien, 
                                                N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                                N'/ Khám bệnh: ' + CAST(Field_6 AS NVARCHAR(MAX)) + 
	                                            N'/ Bệnh chuyên khoa: ' + CAST(Field_13 AS NVARCHAR(MAX)) +
                                                N'/ Tuần hoàn: ' + CAST(Field_20 AS NVARCHAR(MAX)) + 
                                                N'/ Hô hấp: ' + CAST(Field_21 AS NVARCHAR(MAX)) + 
                                                N'/ Tiêu hóa: ' + CAST(Field_22 AS NVARCHAR(MAX)) + 
	                                            N'/ Da và mô dưới da: ' + CAST(Field_23 AS NVARCHAR(MAX)) + 
	                                            N'/ Thận-tiết niệu-sinh dục: ' + CAST(Field_32 AS NVARCHAR(MAX)) +
                                                N'/ Thần kinh: ' + CAST(Field_19 AS NVARCHAR(MAX)) + 
                                                N'/ Cơ Xương khớp: ' + CAST(Field_24 AS NVARCHAR(MAX)) +  
                                                N'/ Các dấu hiệu bệnh lý khác: ' + CAST(Field_33 AS NVARCHAR(MAX)) AS QuaTrinhBenhLy, 
                                                Field_4  AS TienSuBenh, 
                                                Field_31 AS HuongDieuTri
                                            FROM 
                                                dbo.BenhAnTongQuat_TMH
                                            WHERE 
                                                BenhAnTongQuat_Id = @ID;"},
                {"bệnh án tâm bệnh", @"SELECT 
                                        Field_1 AS LyDoVaoVien, 
                                        N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                        N'/ Khám bệnh: ' + CAST(Field_11 AS NVARCHAR(MAX)) + 
                                        N'/ Tuần hoàn: ' + CAST(Field_18 AS NVARCHAR(MAX)) + 
                                        N'/ Hô hấp: ' + CAST(Field_19 AS NVARCHAR(MAX)) + 
                                        N'/ Tiêu hóa: ' + CAST(Field_20 AS NVARCHAR(MAX)) + 
	                                    N'/ Thận-tiết niệu-sinh dục: ' + CAST(Field_21 AS NVARCHAR(MAX)) +
                                        N'/ Thần kinh: ' + CAST(Field_22 AS NVARCHAR(MAX)) + 
                                        N'/ Cơ Xương khớp: ' + CAST(Field_23 AS NVARCHAR(MAX)) +  
                                        N'/ Các dấu hiệu bệnh lý khác: ' + CAST(Field_34 AS NVARCHAR(MAX)) AS QuaTrinhBenhLy, 
                                        Field_4  AS TienSuBenh, 
                                        Field_9 AS HuongDieuTri
                                    FROM 
                                        dbo.BenhAnTongQuat_TamBenh
                                    WHERE 
                                        BenhAnTongQuat_Id = @ID;"},
                {"bệnh án ung bướu", @"SELECT 
                                            Field_1 AS LyDoVaoVien, 
                                            N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                            N'/ Khám bệnh: ' + CAST(Field_6 AS NVARCHAR(MAX)) + 
	                                        N'/ Bộ phận tổn thương: ' + CAST(Field_15 AS NVARCHAR(MAX)) +
                                            N'/ Tuần hoàn: ' + CAST(Field_20 AS NVARCHAR(MAX)) + 
                                            N'/ Hô hấp: ' + CAST(Field_21 AS NVARCHAR(MAX)) + 
                                            N'/ Tiêu hóa: ' + CAST(Field_22 AS NVARCHAR(MAX)) + 
	                                        N'/ Thận-tiết niệu-sinh dục: ' + CAST(Field_19 AS NVARCHAR(MAX)) +
                                            N'/ Thần kinh: ' + CAST(Field_18 AS NVARCHAR(MAX)) + 
                                            N'/ Cơ Xương khớp: ' + CAST(Field_14 AS NVARCHAR(MAX)) +  
                                            N'/ Các dấu hiệu bệnh lý khác: ' + CAST(Field_33 AS NVARCHAR(MAX)) AS QuaTrinhBenhLy, 
                                            Field_4  AS TienSuBenh, 
                                            Field_31 AS HuongDieuTri
                                        FROM 
                                            dbo.BenhAnTongQuat_UngBuou
                                        WHERE 
                                            BenhAnTongQuat_Id = @ID;"},
                {"bệnh án bỏng", @"SELECT 
                                        Field_1 AS LyDoVaoVien, 
                                        N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                        N'/ Khám bệnh: ' + CAST(Field_6 AS NVARCHAR(MAX)) + 
	                                    N'/ Bệnh chuyên khoa ' + CAST(Field_13 AS NVARCHAR(MAX)) +
                                        N'/ Tuần hoàn: ' + CAST(Field_20 AS NVARCHAR(MAX)) + 
                                        N'/ Hô hấp: ' + CAST(Field_21 AS NVARCHAR(MAX)) + 
                                        N'/ Tiêu hóa: ' + CAST(Field_22 AS NVARCHAR(MAX)) + 
	                                    N'/ tiết niệu: ' + CAST(Field_24 AS NVARCHAR(MAX)) +
	                                    N'/ Sinh dục: ' + CAST(Field_32 AS NVARCHAR(MAX)) +
                                        N'/ Thần kinh: ' + CAST(Field_19 AS NVARCHAR(MAX)) + 
                                        N'/ Cơ Xương khớp: ' + CAST(Field_23 AS NVARCHAR(MAX)) +  
                                        N'/ Các dấu hiệu bệnh lý khác: ' + CAST(Field_33 AS NVARCHAR(MAX)) AS QuaTrinhBenhLy, 
                                        Field_4  AS TienSuBenh, 
                                        Field_31 AS HuongDieuTri
                                    FROM 
                                        dbo.BenhAnTongQuat_BONG
                                    WHERE 
                                        BenhAnTongQuat_Id = @ID;"},
                {"bệnh án phcn nội trú", @"SELECT 
                                            Field_7 AS LyDoVaoVien, 
                                            N'Quá Trình bệnh lý: ' + CAST(Field_8 AS NVARCHAR(MAX)) +
	                                        N'/ Khám bệnh: ' +
                                            N'/ Thể trạng chung: ' + CAST(Field_12 AS NVARCHAR(MAX)) +
	                                        N'/ Tình trạng đau: ' + CAST(Field_21 AS NVARCHAR(MAX)) +
	                                        N'/ Tri giác: ' + CAST(Field_22 AS NVARCHAR(MAX)) + 
	                                        N'/ Thăng Bằng: ' + CAST(Field_23 AS NVARCHAR(MAX)) + 
	                                        N'/ Vận động: ' + CAST(Field_24 AS NVARCHAR(MAX)) +
	                                        N'/ Điều hợp: ' + CAST(Field_25 AS NVARCHAR(MAX)) + 
	                                        N'/ Cảm giác: ' + CAST(Field_26 AS NVARCHAR(MAX)) + 
	                                        N'/ Hội chứng tiểu não: ' + CAST(Field_27 AS NVARCHAR(MAX)) + 
	                                        N'/ Phản xạ gân xương: ' + CAST(Field_28 AS NVARCHAR(MAX)) +
	                                        N'/ Hội chứng ngoại tháp: ' + CAST(Field_29 AS NVARCHAR(MAX)) + 
	                                        N'/ Phản xạ da: ' + CAST(Field_30 AS NVARCHAR(MAX)) + 
	                                        N'/ Các hội chứng tâm thần: ' + CAST(Field_31 AS NVARCHAR(MAX)) + 
	                                        N'/ trương lực cơ: ' + CAST(Field_32 AS NVARCHAR(MAX)) + 
	                                        N'/ Thần kinh khác: ' + CAST(Field_33 AS NVARCHAR(MAX)) + 
	                                        N'/ Thần kinh sọ não: ' + CAST(Field_34 AS NVARCHAR(MAX)) + 
	                                        N'/ Hình thể: ' + CAST(Field_35 AS NVARCHAR(MAX)) + 
	                                        N'/ Cơ: ' + CAST(Field_36 AS NVARCHAR(MAX)) + 
	                                        N'/ Tầm vận động khớp: ' + CAST(Field_37 AS NVARCHAR(MAX)) + 
	                                        N'/ Tình trạng bệnh lý cột sống: ' + CAST(Field_38 AS NVARCHAR(MAX)) + 
	                                        N'/ Rối loạn chức năng cột sống: ' + CAST(Field_39 AS NVARCHAR(MAX)) +
	                                        N'/ Nhịp tim: ' + CAST(Field_40 AS NVARCHAR(MAX)) + 
	                                        N'/ Lồng ngực: ' + CAST(Field_41 AS NVARCHAR(MAX)) + 
	                                        N'/ Rối loạn chức năng tim mạch: ' + CAST(Field_42 AS NVARCHAR(MAX)) + 
	                                        N'/ Tình trạng bệnh lý hô hấp: ' + CAST(Field_43 AS NVARCHAR(MAX)) + 
	                                        N'/ Tình trạng bệnh lý tiêu hóa: ' + CAST(Field_44 AS NVARCHAR(MAX)) + 
	                                        N'/ Tình trạng bệnh lý nội tiết: ' + CAST(Field_45 AS NVARCHAR(MAX)) + 
	                                        N'/ Tiết niệu, sinh dục: ' + CAST(Field_46 AS NVARCHAR(MAX)) + 
	                                        N'/ Các cơ quan khác: ' + CAST(Field_47 AS NVARCHAR(MAX)) + 
	                                        N'/ Da và mô dưới da: ' + CAST(Field_48 AS NVARCHAR(MAX)) 
	                                        AS QuaTrinhBenhLy, 
                                            N'Tiền sử dị ứng: ' +  CAST(Field_9 AS NVARCHAR(MAX)) +
	                                        N'/ Tiền sử bản thân: ' + CAST(Field_10 AS NVARCHAR(MAX)) AS TienSuBenh, 
                                            Field_56 AS HuongDieuTri
                                        FROM 
                                            dbo.BenhAnTongQuat_PHCN
                                        WHERE 
                                            BenhAnTongQuat_Id = @ID;"},
            {"Bệnh án tâm bệnh", @"SELECT 
                                            Field_1 AS LyDoVaoVien,
                                            N'Quá Trình bệnh lý: ' + CAST(Field_3 AS NVARCHAR(MAX)) +
                                            N'/ Toàn thân:'+CAST(Field_11 AS NVARCHAR(MAX))+
                                            N'/ Tuần hoàn:'+CAST(Field_18 AS NVARCHAR(MAX))+
                                            N'/ Hô Hấp:'+CAST(Field_19 AS NVARCHAR(MAX))+
                                            N'/ Tiêu Hóa:'+CAST(Field_20 AS NVARCHAR(MAX))+
                                            N'/ Thận - Tiết niệu - sinh dục:'+CAST(Field_21 AS NVARCHAR(MAX))+
                                            N'/ Cơ - Xương _ khớp:'+CAST(Field_6 AS NVARCHAR(MAX))+
                                            N'/ Tai - Mũi - Họng:'+CAST(Field_7 AS NVARCHAR(MAX))+
                                            N'/ Răng - Hàm - Mặt:'+CAST(Field_8 AS NVARCHAR(MAX))+
                                            N'/ Mắt :'+CAST(Field_23 AS NVARCHAR(MAX))+
                                            N'/ Nội tiết dinh dưỡng và các bệnh lí khác:'+CAST(Field_24 AS NVARCHAR(MAX))+
                                            N'/ Dây thần kinh sọ não:'+CAST(Field_30 AS NVARCHAR(MAX))+
                                            N'/ Đáy mắt:'+CAST(Field_31 AS NVARCHAR(MAX))+
                                            N'/ Vận Động:'+CAST(Field_32 AS NVARCHAR(MAX))+
                                            N'/ Trường lực cơ:'+CAST(Field_33 AS NVARCHAR(MAX))+
                                            N'/ Cảm giác(nông sâu):'+CAST(Field_34 AS NVARCHAR(MAX))+
                                            N'/ Phản xạ:'+CAST(Field_35 AS NVARCHAR(MAX))+
                                            N'/ Biểu hiện chung:'+CAST(Field_36 AS NVARCHAR(MAX))+
                                            N'/ Ý thức hướng lực:'+
                                            N'/ Không gian:'+CAST(Field_37 AS NVARCHAR(MAX))+
                                            N'/ Thiời gian:'+CAST(Field_38 AS NVARCHAR(MAX))+
                                            N'/ Bản thân:'+CAST(Field_39 AS NVARCHAR(MAX))+
                                            N'/ Tình cảm, cảm xúc:'+CAST(Field_40 AS NVARCHAR(MAX))+
                                            N'/ Tri giác(khả năng nhận thức thực tại khách quan,các rối loạn ):'+CAST(Field_41 AS NVARCHAR(MAX))+
                                            N'/ Tư duy - Hình thức:'+CAST(Field_42 AS NVARCHAR(MAX))+
                                            N'/ Nội dung:'+CAST(Field_43 AS NVARCHAR(MAX))+
                                            N'/ Hàng vi tác phong:'+
                                            N'/Hoạt động có ý chí:'+CAST(Field_44 AS NVARCHAR(MAX))+
                                            N'/ Hoạt động bản năng'+CAST(Field_45 AS NVARCHAR(MAX))+
                                            N'/Trí nhớ:'+
                                            N'/ Nhớ máy móc:'+CAST(Field_46 AS NVARCHAR(MAX))+
                                            N'/Nhớ thông hiểu:'+CAST(Field_47 AS NVARCHAR(MAX))+
                                            N'/ Trí năng:'+
                                            N'/ khả năng phân tích:'+CAST(Field_48 AS NVARCHAR(MAX))+
                                            N'/ khả năng tổng hợp:'+CAST(Field_49 AS NVARCHAR(MAX))+
                                            N'/ Chú ý:'+CAST(Field_50 AS NVARCHAR(MAX))+
                                            N'/ Các nhận xét cận lâm sàng cần làm:'+CAST(Field_26 AS NVARCHAR(MAX))
                                            AS QuaTrinhBenhLy,
                                            N'/Bản thân(sự phát triển về tâm thần và thể chất từ nhỏ đến lớn, những bệnh đã mắt phải,tình hình học tập , khả năng lao động công tác, đặc điểm tính cách từ nhỏ đến lớn:):' +CAST(Field_4 AS NVARCHAR(MAX)) +
                                            N'/Gia Đình(ông bà , mẹ , anh, chị em ruột, con cái ,họ hàng có ai mắc bệnh gì không : bệnh thần kinh, tâm thần)?:' +CAST(Field_5 AS NVARCHAR(MAX))
                                            AS TienSuBenh,
                                            Field_9 as HuongDieuTri
                                        FROM BenhAnTongQuat_TamBenh
                                        WHERE BenhAnTongQuat_Id = @ID;"},
             {
            "bệnh án sơ sinh",@"SELECT 
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
                                        N'/ Các xét nghiệm lâm sàng cần làm:' + ISNULL(CAST(ss.Field_26 AS NVARCHAR(MAX)), '')+
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
                                        WHERE ss.BenhAnTongQuat_Id = @ID;"
        },
             {
            "bệnh án y học cổ truyền ngoại trú mới",
                                @"SELECT
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
                                        N'/ Tuần hoàn: ' + CAST(Field_30 AS NVARCHAR(MAX)) +
                                        N'/ Hô hấp: ' + CAST(Field_31 AS NVARCHAR(MAX)) +
                                        N'/ Tiêu Hóa: ' + CAST(Field_32 AS NVARCHAR(MAX)) +
                                        N'/ Tiết niệu - sinh dục: ' + CAST(Field_33 AS NVARCHAR(MAX)) +
                                        N'/ Thần kinh: ' + CAST(Field_34 AS NVARCHAR(MAX)) +
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
                                        N'/ Dự hậu tiên lượng: ' + CAST(Field_62 AS NVARCHAR(MAX))
                                    AS QuatrinhBenhLy,
                                     N'Y Học hiện đại: ' + CAST(Field_66 AS NVARCHAR(MAX)) +
                                     CHAR(13)+CHAR(10) + N'Y học cổ truyền: ' + CAST(Field_67 AS NVARCHAR(MAX)) AS HuongDieuTri,
                                        N'Bản thân: ' + CAST(Field_56 AS NVARCHAR(MAX)) +
                                        CHAR(13)+CHAR(10) + N'Đặc điểm liên quan bệnh tật: ' + CAST(Field_60 AS NVARCHAR(MAX)) +
                                        CHAR(13)+CHAR(10) + N'Gia đình: ' + CAST(Field_20 AS NVARCHAR(MAX)) AS TienSuBenh
                                        FROM dbo.BenhAnTongQuat_YHCT_NT_NEW
                                         WHERE BenhAnTongQuat_Id = @ID;"
        },
            {
             "bệnh án sản khoa",
                                        @"SELECT 
                                            Field_1 AS LyDoVaoVien,
                                            ISNULL(N'Quá trình bệnh lý: ' + CAST(Field_25 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Khám thai tại: ' + CAST(Field_3 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Bắt đầu chuyển dạ: ' + CAST(Field_23 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Dấu hiệu lúc đầu: ' + CAST(Field_32 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Biến chuyển: ' + CAST(Field_24 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Vào lúc đẻ: ' + CAST(Field_27 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Ngày mổ đẻ: ' + CAST(Field_75 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Cách thức đẻ: ' + CAST(Field_137 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Ngôi thai: ' + CAST(Field_136 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Kiểm soát tử cung: ' + CAST(Field_138 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Toàn trạng: ' + CAST(Field_11 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Tuần hoàn: ' + CAST(Field_18 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Hô hấp: ' + CAST(Field_19 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Tiêu hóa: ' + CAST(Field_20 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Tiết niệu: ' + CAST(Field_21 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Các bộ phận khác: ' + CAST(Field_22 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Mạch: ' + CAST(Field_12 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Nhiệt độ: ' + CAST(Field_13 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Huyết áp: ' + CAST(Field_14 AS NVARCHAR(MAX)) + N'/' + CAST(Field_15 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Nhịp thở: ' + CAST(Field_16 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Cân nặng: ' + CAST(Field_17 AS NVARCHAR(MAX)), '') + N' / ' +
                                            ISNULL(N'Bản thân: ' + CAST(Field_4 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
                                            ISNULL(N'Gia đình: ' + CAST(Field_5 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
                                            N'Tiền sử phụ khoa:' + CHAR(13) + CHAR(10) +
                                            N'Bắt đầu thấy kinh năm: ' + ISNULL(CAST(Field_33 AS NVARCHAR(MAX)), '') + 
                                            N', Tuổi: ' + ISNULL(CAST(Field_34 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
                                            N'Lấy chồng năm: ' + ISNULL(CAST(Field_35 AS NVARCHAR(MAX)), '') + 
                                            N', Tuổi: ' + ISNULL(CAST(Field_36 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
                                            ISNULL(N'Tính chất kinh nguyệt: ' + CAST(Field_37 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
                                            ISNULL(N'Chu kỳ (ngày): ' + CAST(Field_38 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
                                            ISNULL(N'Lượng kinh: ' + CAST(Field_39 AS NVARCHAR(MAX)), '') + CHAR(13) + CHAR(10) +
                                            ISNULL(N'Những bệnh phụ khoa đã điều trị: ' + CAST(Field_40 AS NVARCHAR(MAX)), '')
                                            AS TienSuBenh,

                                            Field_31 AS HuongDieuTri
                                        FROM dbo.BenhAnTongQuat_SanKhoa
                                        WHERE BenhAnTongQuat_Id = @ID;"
        },
            {"Khám bệnh vào viện", @"SELECT 
                                    LyDoVaoVien,
                                                        N'Quá Trình bệnh lý: ' + ISNULL(QuaTrinhBenhLy, '') +
                                                        N'Toàn thân: ' + ISNULL(KhamXetToanThan, '') +
                                                        N'Các bộ phận: ' + ISNULL(KhamXetCacBoPhan, '') +
                                                        N'Mạch: ' + CAST(ISNULL(Mach, 0) AS NVARCHAR(MAX)) +
                                                        N'Nhiệt độ: ' + CAST(ISNULL(NhietDo, 0) AS NVARCHAR(MAX)) +
                                                        N'Huyết áp: ' + CAST(ISNULL(HuyetApCao, 0) AS NVARCHAR(MAX)) + N'/' + CAST(ISNULL(HuyetApThap, 0) AS NVARCHAR(MAX)) +
                                                        N'Nhịp thở: ' + CAST(ISNULL(NhipTho, 0) AS NVARCHAR(MAX)) +
                                                        N'Cân nặng: ' + CAST(ISNULL(CanNang, 0) AS NVARCHAR(MAX)) +
                                                        N'Chiều cao: ' + CAST(ISNULL(ChieuCao, 0) AS NVARCHAR(MAX)) +
                                                        N'SpO2: ' + CAST(ISNULL(SPO2, 0) AS NVARCHAR(MAX)) +
                                                        N'Tóm tắt cận lâm sàng: ' + ISNULL(KhamXetKQLS, '') +
                                                        N'Chuẩn đoán vào viện: ' + ISNULL(ChanDoanVao, '')
                                                    AS QuaTrinhBenhLy,
                                                        N'Bản thân: ' + ISNULL(TienSuBanThan, '') +
                                                        N'Gia Đình: ' + ISNULL(TienSuGiaDinh, '')
                                                    AS TienSuBenh,
                                                        NULL AS HuongDieuTri
                                                    FROM KhamBenh_VaoVien
                                
                                   WHERE TiepNhan_Id = @TiepNhan_Id;"},
                {"bệnh án y học cổ truyền nội trú mới", @"Select
                                                Field_25 as LyDoVaoVien,
                                                N'Quá Trình bệnh lý: ' + CAST(Field_26 AS NVARCHAR(MAX)) +
                                                N'/Toàn thân(ý thức, da niêm mạc,hệ thống hạch,tuyến giáp,vị trí, kích thước...): ' + CAST(Field_35 AS NVARCHAR(MAX)) +
                                                N'/Mạch: ' + CAST(Field_36 AS NVARCHAR(MAX)) +
                                                N'/Nhiệt độ: ' + CAST(Field_43 AS NVARCHAR(MAX)) +
                                                N'/Huyết áp: ' + CAST(Field_38 AS NVARCHAR(MAX)) +N'/'+ CAST(Field_39 AS NVARCHAR(MAX)) +
                                                N'/Nhịp thở: ' + CAST(Field_41 AS NVARCHAR(MAX)) +
                                                N'/Cân nặng: ' + CAST(Field_42 AS NVARCHAR(MAX)) +
                                                N'/Chiều cao: ' + CAST(Field_37 AS NVARCHAR(MAX)) +
                                                N'/IMB: ' + CAST(Field_40 AS NVARCHAR(MAX)) +
                                                N'/Các bộ phận: ' +
                                                N'/Tuần hoàn:'+CAST(Field_44 AS NVARCHAR(MAX)) +
                                                N'/Hô hấp:'+CAST(Field_45 AS NVARCHAR(MAX)) +
                                                N'/Tiêu Hóa:'+CAST(Field_46 AS NVARCHAR(MAX)) +
                                                N'/Tiết niệu - sinh dục:'+CAST(Field_47 AS NVARCHAR(MAX)) +
                                                N'/Thần kinh:'+CAST(Field_48 AS NVARCHAR(MAX)) +
                                                N'/Cơ xương khớp:'+CAST(Field_49 AS NVARCHAR(MAX)) +
                                                N'/Tai -mũi - họng:'+CAST(Field_50 AS NVARCHAR(MAX)) +
                                                N'/Răng - Hàm - Mặt:'+CAST(Field_51 AS NVARCHAR(MAX)) +
                                                N'/Mắt:'+CAST(Field_52 AS NVARCHAR(MAX)) +
                                                N'/Nội tiết, diinh dưỡng và các bệnh lý khác:'+CAST(Field_53 AS NVARCHAR(MAX)) +
                                                N'/cận lâm sàng:'+CAST(Field_54 AS NVARCHAR(MAX))+
                                                N'/Tóm tắt bệnh án'+CAST(Field_55 AS NVARCHAR(MAX))+
                                                N'/Chuẩn đoán:'+
                                                N'/Bệnh chính:'+CAST(Field_40 AS NVARCHAR(MAX))+
                                                N'/Phân biệt:'+CAST(Field_56 AS NVARCHAR(MAX))
                                                as QuatrinhBenhLy,
                                                 N'Y Học hiện đại: ' + CAST(Field_388 AS NVARCHAR(MAX)) +
                                                CHAR(13)+CHAR(10) + N'Y học cổ truyền: ' + CAST(Field_389 AS NVARCHAR(MAX)) AS HuongDieuTri,
                                                N' Bản thân:'+CAST(Field_32 AS NVARCHAR(MAX))+
                                                CHAR(13)+CHAR(10) +N' Đặc điểm liên quan bệnh tật:'+CAST(Field_33 AS NVARCHAR(MAX))+
                                                CHAR(13)+CHAR(10) +N' Gia đình:'+CAST(Field_34 AS NVARCHAR(MAX))
                                                as  TienSuBenh
                                                from BenhAnTongQuat_YHCT_NoiTru_New
                                                where BenhAnTongQuat_Id = @ID;"},



            };
    }
}
