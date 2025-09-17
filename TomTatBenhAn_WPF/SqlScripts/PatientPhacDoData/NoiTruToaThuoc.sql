SELECT nttt.Duoc_Id
	,d.TenDuocDayDu
	,nttt.SoLuong
	,nttt.SoLanTrenNgay
	,nttt.NgayTao
FROM NoiTru_ToaThuoc nttt
LEFT JOIN NoiTru_KhamBenh ntkb ON nttt.KhamBenh_Id = ntkb.KhamBenh_Id
LEFT JOIN DM_Duoc d ON nttt.Duoc_Id = d.Duoc_Id
WHERE ntkb.benhan_id = (
		SELECT benhan_id
		FROM benhan
		WHERE sobenhan = N'@SoBenhAn_Params'
		)
