SELECT 

  ba.ChanDoanVaoKhoa AS ChanDoanVaoKhoaBenhChinh,
  icdVK_Chinh.MaICD AS ICDVaoKhoa_Chinh,
  icdVK_Phu.MaICD AS ICDVaoKhoa_Phu,
  icdVK_Phu.TenICD AS ICDVaoKhoa_TenPhu,
  ba.ChanDoanRaVien AS ChanDoanRaVienBenhChinh,
  ba.ChanDoanPhuRaVien AS ChanDoanRaVienBenhPhu,
  icdRV_Chinh.MaICD AS ICDRaVien_Chinh,
  icdRV_Phu.MaICD AS ICDRaVien_Phu

FROM 
  dbo.BenhAn ba
  LEFT JOIN ehosdict.DM_ICD icdVK_Chinh ON ba.ICD_VaoKhoa = icdVK_Chinh.ICD_Id
  LEFT JOIN ehosdict.DM_ICD icdVK_Phu ON ba.ICD_BenhPhu = icdVK_Phu.ICD_Id
  LEFT JOIN ehosdict.DM_ICD icdRV_Chinh ON ba.ICD_BenhChinh = icdRV_Chinh.ICD_Id
  LEFT JOIN ehosdict.DM_ICD icdRV_Phu ON ba.ICD_BenhPhu = icdRV_Phu.ICD_Id

WHERE
  ba.SoBenhAn = N'@SoBenhAn_Params'