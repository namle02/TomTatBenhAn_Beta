SELECT 
  dmbn.TenBenhNhan TenBN, 
  CONVERT(VARCHAR, dmbn.NgaySinh, 103) NgaySinh, 
  (
    YEAR(
      GETDATE()
    ) - dmbn.NamSinh
  ) Tuoi, 
  dmbn.GioiTinh GioiTinh, 
  dmbn.DiaChi DiaChi, 
  SUBSTRING(xncp.SoBHYT, 0, 16) SoBHYT, 
  ba.NgayVaoVien,
  ba.NgayRaVien,
  td.Dictionary_Name DanToc 
FROM 
  dbo.BenhAn ba 
  LEFT JOIN dbo.XacNhanChiPhi xncp ON ba.BenhAn_Id = xncp.BenhAn_Id 
  LEFT JOIN ehosdict.DM_BenhNhan dmbn ON ba.BenhNhan_Id = dmbn.BenhNhan_Id 
  LEFT JOIN ehosdict.Lst_Dictionary td ON dmbn.DanToc_Id = td.Dictionary_Id 
WHERE 
  ba.SoBenhAn = N'@SoBenhAn_Params'