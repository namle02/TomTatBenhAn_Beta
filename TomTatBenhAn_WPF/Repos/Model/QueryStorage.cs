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
                                            BenhAnTongQuat_Id = @ID;"}
            };
    }
}
