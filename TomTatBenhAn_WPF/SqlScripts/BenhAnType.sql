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
and batq.LoaiBenhAn_Id not like N'41'
ORDER BY batq.NgayTao ASC;
