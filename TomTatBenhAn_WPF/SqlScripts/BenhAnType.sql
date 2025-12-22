SELECT TOP 1
	lba.LoaiBenhAn_Id LoaiBenhAn_Id,
    lba.TenLoaiBenhAn AS LoaiBenhAn,
    batq.BenhAnTongQuat_Id,
    ba.TiepNhan_Id
FROM dbo.BenhAn ba
LEFT JOIN dbo.BenhAnTongQuat batq 
    ON ba.BenhAn_Id = batq.BenhAn_Id
LEFT JOIN ehosdict.DM_LoaiBenhAn lba 
    ON batq.LoaiBenhAn_Id = lba.LoaiBenhAn_Id
WHERE ba.SoBenhAn = N'@SoBenhAn_Params'
    AND (batq.LoaiBenhAn_Id IS NULL OR batq.LoaiBenhAn_Id <> N'41')
ORDER BY batq.NgayTao ASC;
