  SELECT 
  TOP 1 ntkb.DienBien, 
  bact.LoiDanThayThuoc,
  bact.PPDT 
FROM 
  dbo.NoiTru_KhamBenh ntkb 
  LEFT JOIN dbo.BenhAnChiTiet bact ON ntkb.BenhAn_Id = bact.BenhAn_Id 
WHERE 
  ntkb.BenhAn_Id = (
    SELECT 
      BenhAn_Id 
    FROM 
      dbo.BenhAn 
    WHERE 
      SoBenhAn = N'@SoBenhAn_Params'
  ) 
  AND DienBien IS NOT NULL 
ORDER BY 
  ThoiGianKham DESC