select SoBenhAn from BenhAn
where BenhNhan_Id = ( select BenhNhan_Id from DM_BenhNhan where SoVaoVien = @SoVaoVien_Params )