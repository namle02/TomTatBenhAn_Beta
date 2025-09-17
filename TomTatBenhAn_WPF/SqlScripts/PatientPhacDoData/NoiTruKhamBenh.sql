SELECT ThoiGianKham
	,DinhBenh
	,DienBien
	,LoiDan
	,CheDoAn
	,CheDoChamSoc
FROM NoiTru_KhamBenh
WHERE benhan_id = (
		SELECT benhan_id
		FROM benhan
		WHERE sobenhan = N'@SoBenhAn_Params'
		)
