SELECT 
  TOP 1 Lower(lba.TenLoaiBenhAn) LoaiBenhAn, 
  batq.BenhAnTongQuat_Id 
FROM 
  dbo.BenhAnTongQuat batq 
  LEFT JOIN ehosdict.DM_LoaiBenhAn lba ON batq.LoaiBenhAn_Id = lba.LoaiBenhAn_Id 
WHERE 
  batq.BenhAn_Id = (
    SELECT 
      BenhAn_Id 
    FROM 
      dbo.BenhAn 
    WHERE 
      SoBenhAn = N'@SoBenhAn_Params'
  ) 
ORDER BY 
  batq.NgayTao ASC