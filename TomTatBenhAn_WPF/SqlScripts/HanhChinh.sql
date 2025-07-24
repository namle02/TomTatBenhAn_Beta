SELECT 
                              ba.SoBenhAn SoBenhAn, 
                              dmbn.SoVaoVien SoVaoVien, 
                              CAST(DATEPART(HOUR,ba.ThoiGianVaoVien) AS NVARCHAR(MAX)) + N' giờ ' +
	                            CAST(DATEPART(MINUTE, ba.ThoiGianVaoVien) AS NVARCHAR(MAX)) + N', ngày ' +
	                            CAST(DAY(ba.ThoiGianVaoVien) AS NVARCHAR(MAX)) + N' tháng ' +
	                            CAST(MONTH(ba.ThoiGianVaoVien) AS NVARCHAR(MAX)) + N' năm ' +
	                            CAST(YEAR(ba.ThoiGianVaoVien) AS NVARCHAR(MAX)) AS ThoiGianVaoVien, 

                              CAST(DATEPART(HOUR,ba.ThoiGianRaVien) AS NVARCHAR(MAX)) + N' giờ ' +
	                            CAST(DATEPART(MINUTE, ba.ThoiGianRaVien) AS NVARCHAR(MAX)) + N', ngày ' +
	                            CAST(DAY(ba.ThoiGianRaVien) AS NVARCHAR(MAX)) + N' tháng ' +
	                            CAST(MONTH(ba.ThoiGianRaVien) AS NVARCHAR(MAX)) + N' năm ' +
	                            CAST(YEAR(ba.ThoiGianRaVien) AS NVARCHAR(MAX)) AS ThoiGianRaVien,
	                            td.Dictionary_Name KetQuaDieuTri
                            FROM 
                              dbo.BenhAn ba 
                              LEFT JOIN ehosdict.DM_BenhNhan dmbn ON ba.BenhNhan_Id = dmbn.BenhNhan_Id 
                              LEFT JOIN ehosdict.Lst_Dictionary td ON ba.KetQuaDieuTri_Id = td.Dictionary_Id
                            WHERE 
                              ba.SoBenhAn = N'@SoBenhAn_Params'