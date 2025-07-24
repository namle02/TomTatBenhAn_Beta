select SoBenhAn from BenhAn ba
left join DM_BenhNhan bn on bn.BenhNhan_Id = ba.BenhNhan_Id
where bn.SoVaoVien = N'@SoVaoVien_Params'