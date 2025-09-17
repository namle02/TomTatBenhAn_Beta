SELECT DienBenh
	,YLenhChamSoc
	,NgayThucHien
FROM NoiTru_ChamSoc
WHERE BenhAn_Id = (
		SELECT benhan_id
		FROM benhan
		WHERE sobenhan = N'@SoBenhAn_Params'
		)
ORDER BY NgayThucHien