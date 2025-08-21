SELECT 
  LyDoVaoVien, 
  N'Quá Trình bệnh lý: ' + ISNULL(QuaTrinhBenhLy, '') + N'Toàn thân: ' + ISNULL(KhamXetToanThan, '') + N'Các bộ phận: ' + ISNULL(KhamXetCacBoPhan, '') + N'Mạch: ' + CAST(
    ISNULL(Mach, 0) AS NVARCHAR(MAX)
  ) + N'Nhiệt độ: ' + CAST(
    ISNULL(NhietDo, 0) AS NVARCHAR(MAX)
  ) + N'Huyết áp: ' + CAST(
    ISNULL(HuyetApCao, 0) AS NVARCHAR(MAX)
  ) + N'/' + CAST(
    ISNULL(HuyetApThap, 0) AS NVARCHAR(MAX)
  ) + N'Nhịp thở: ' + CAST(
    ISNULL(NhipTho, 0) AS NVARCHAR(MAX)
  ) + N'Cân nặng: ' + CAST(
    ISNULL(CanNang, 0) AS NVARCHAR(MAX)
  ) + N'Chiều cao: ' + CAST(
    ISNULL(ChieuCao, 0) AS NVARCHAR(MAX)
  ) + N'SpO2: ' + CAST(
    ISNULL(SPO2, 0) AS NVARCHAR(MAX)
  ) + N'Tóm tắt cậNlâm sàng: ' + ISNULL(KhamXetKQLS, '') + N'Chẩn đoán vào viện: ' + ISNULL(ChanDoanVao, '') AS QuaTrinhBenhLy, 
  N'Bản thân: ' + ISNULL(TienSuBanThan, '') + N'Gia Đình: ' + ISNULL(TienSuGiaDinh, '') AS TienSuBenh, 
  NULL AS HuongDieuTri 
FROM 
  KhamBenh_VaoVieN
WHERE 
  TiepNhan_Id = @TiepNhan_Id;
