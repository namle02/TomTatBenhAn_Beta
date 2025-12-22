SELECT 
  TOP 1 ntkb.DienBien, 
  bact.LoiDanThayThuoc,
  bact.PPDT 
FROM 
  dbo.BenhAn ba
  INNER JOIN dbo.NoiTru_KhamBenh ntkb ON ba.BenhAn_Id = ntkb.BenhAn_Id
  LEFT JOIN dbo.BenhAnChiTiet bact ON ba.BenhAn_Id = bact.BenhAn_Id 
WHERE 
  ba.SoBenhAn = N'@SoBenhAn_Params'
  AND ntkb.DienBien IS NOT NULL 
ORDER BY 
  ntkb.ThoiGianKham DESC