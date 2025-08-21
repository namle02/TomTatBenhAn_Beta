SELECT 
  ChanDoanVaoKhoa AS BenhChinhVaoVien, 
  icdCvv.MaICD As MaICDChinhVaoVien, 
  ba.ChanDoanRaVien As BenhChinhRaVien, 
  icdCrv.MaICD As MaICDChinhRaVien, 
  Replace(ba.ChanDoanPhuRaVien, '- ', '/') as BenhKemTheoRaVien, 
  dbo.Get_MaICD_Phu_ByBenhAn_Id(
    (ba.BenhAn_Id), 
    'M'
  ) As MaICDKemTheoRaVien 
FROM 
  BenhAn ba 
  Left join DM_ICD icdCvv on ba.ICD_VaoKhoa = icdCvv.ICD_Id 
  Left join DM_ICD icdCrv on ba.ICD_BenhChinh = icdCrv.ICD_Id 
WHERE 
  SoBenhAn = N'@SoBenhAn_Params'
