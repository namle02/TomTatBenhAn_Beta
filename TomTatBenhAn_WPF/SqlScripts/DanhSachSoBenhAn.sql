SELECT 
    ba.SoBenhAn AS SoBenhAn, 
    ba.BenhAn_Id AS BenhAnId, 
    batq.BenhAnTongQuat_Id AS BenhAnTongQuatId 
FROM ehosdict.DM_BenhNhan dmbn
    INNER JOIN dbo.BenhAn ba ON dmbn.BenhNhan_Id = ba.BenhNhan_Id
    LEFT JOIN dbo.BenhAnTongQuat batq ON ba.BenhAn_Id = batq.BenhAn_Id
WHERE dmbn.SoVaoVien = @MaYTe_Params
ORDER BY ba.NgayVaoVien DESC