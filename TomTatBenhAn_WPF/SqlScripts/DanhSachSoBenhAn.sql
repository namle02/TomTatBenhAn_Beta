select ba.SoBenhAn SoBenhAn, ba.BenhAn_Id BenhAnId, batq.BenhAnTongQuat_Id BenhAnTongQuatId From DM_BenhNhan dmbn
left join BenhAn ba on dmbn.BenhNhan_Id = ba.BenhNhan_Id
left join BenhAnTongQuat batq on ba.BenhAn_Id = batq.BenhAn_Id
where dmbn.SoVaoVien = @MaYTe_Params