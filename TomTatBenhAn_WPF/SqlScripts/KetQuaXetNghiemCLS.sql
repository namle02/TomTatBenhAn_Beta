SELECT 
  ndv.TenNhomDichVu, 
  clsyc.NoiDungChiTiet, 
  pb.TenPhongBan, 
  dv.TenDichVu,
  clskqct.KetQua,
  clskqct.MucBinhThuong,
  clskqct.MucBinhThuongMin,
  clskqct.MucBinhThuongMax,
  dv.DonViTinh,
  clskqct.BatThuong,
  clskq.ThoiGianThucHien,
  clskq.KetLuan,
  clskq.MoTa_Text
FROM 
  dbo.BenhAn ba
  INNER JOIN dbo.CLSYeuCau clsyc ON ba.BenhAn_Id = clsyc.BenhAn_Id
  LEFT JOIN ehosdict.DM_NhomDichVu ndv ON clsyc.NhomDichVu_Id = ndv.NhomDichVu_Id 
  INNER JOIN dbo.CLSKetQua clskq ON clsyc.CLSYeuCau_Id = clskq.CLSYeuCau_Id 
  LEFT JOIN ehosdict.DM_PhongBan pb ON clsyc.NoiYeuCau_Id = pb.PhongBan_Id 
  LEFT JOIN dbo.CLSKetQuaChiTiet clskqct ON clskq.CLSKetQua_Id = clskqct.CLSKetQua_Id 
  LEFT JOIN ehosdict.DM_DichVu dv ON clskqct.DichVu_Id = dv.DichVu_Id
WHERE 
  ba.SoBenhAn = N'@SoBenhAn_Params'
  AND (clskqct.BatThuong = 1 OR clskq.PhanLoaiKetQua_Id IS NOT NULL OR (clskqct.MucBinhThuong IS NULL AND clskqct.MucBinhThuongMin IS NULL AND clskqct.MucBinhThuongMax IS NULL AND clskqct.KetQua IS NOT NULL))
ORDER BY 
  clskq.ThoiGianThucHien DESC