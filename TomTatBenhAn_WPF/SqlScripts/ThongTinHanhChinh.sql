SELECT 
    ba.SoBenhAn AS SoBenhAn,
	nv.TenNhanVien AS BacSiDieuTri,
    dmbn.SoVaoVien AS SoVaoVien,
    dmbn.TenBenhNhan AS TenBN,
    CONVERT(VARCHAR(10), dmbn.NgaySinh, 103) AS NgaySinh,
    (YEAR(GETDATE()) - dmbn.NamSinh) AS Tuoi,
    dmbn.GioiTinh,
    dmbn.DiaChi,
    SUBSTRING(xncp.SoBHYT, 1, 15) AS SoBHYT,   
	dmbn_cccd.CMND As Cccd,
    ba.NgayVaoVien,
    ba.NgayRaVien,
    tdDanToc.Dictionary_Name AS DanToc,
    dmbn.sovaovien AS MaYTe,

    -- Chuỗi thời gian vào/ra viện đã format
    CAST(DATEPART(HOUR, ba.ThoiGianVaoVien) AS NVARCHAR(10)) + N' giờ ' +
    CAST(DATEPART(MINUTE, ba.ThoiGianVaoVien) AS NVARCHAR(10)) + N', ngày ' +
    CAST(DAY(ba.ThoiGianVaoVien) AS NVARCHAR(10)) + N' tháng ' +
    CAST(MONTH(ba.ThoiGianVaoVien) AS NVARCHAR(10)) + N' năm ' +
    CAST(YEAR(ba.ThoiGianVaoVien) AS NVARCHAR(10)) AS ThoiGianVaoVien,

    CAST(DATEPART(HOUR, ba.ThoiGianRaVien) AS NVARCHAR(10)) + N' giờ ' +
    CAST(DATEPART(MINUTE, ba.ThoiGianRaVien) AS NVARCHAR(10)) + N', ngày ' +
    CAST(DAY(ba.ThoiGianRaVien) AS NVARCHAR(10)) + N' tháng ' +
    CAST(MONTH(ba.ThoiGianRaVien) AS NVARCHAR(10)) + N' năm ' +
    CAST(YEAR(ba.ThoiGianRaVien) AS NVARCHAR(10)) AS ThoiGianRaVien,

    tdKQDT.Dictionary_Name AS KetQuaDieuTri
FROM dbo.BenhAn ba
LEFT JOIN dbo.XacNhanChiPhi xncp
    ON ba.BenhAn_Id = xncp.BenhAn_Id
LEFT JOIN ehosdict.DM_BenhNhan dmbn
    ON ba.BenhNhan_Id = dmbn.BenhNhan_Id
LEFT JOIN ehosdict.Lst_Dictionary tdDanToc
    ON dmbn.DanToc_Id = tdDanToc.Dictionary_Id
LEFT JOIN ehosdict.Lst_Dictionary tdKQDT
    ON ba.KetQuaDieuTri_Id = tdKQDT.Dictionary_Id
LEFT JOIN vw_DM_BenhNhan dmbn_cccd on ba.BenhNhan_Id = dmbn_cccd.BenhNhan_Id
LEFT JOIN vw_NhanVien nv ON ba.BacSiDieuTri_Id = nv.NhanVien_Id
WHERE ba.SoBenhAn = N'@SoBenhAn_Params';
