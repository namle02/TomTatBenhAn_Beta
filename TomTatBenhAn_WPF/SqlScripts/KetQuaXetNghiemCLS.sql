SELECT 
                              ndv.TenNhomDichVu, 
                              clsyc.NoiDungChiTiet, 
                              pb.TenPhongBan, 
                              dv.TenDichVu,
                              clskqct.KetQua,
                              clskqct.MucBinhThuong,
                              clskqct.MucBinhThuongMin,
                              clskqct.MucBinhThuongMax,
                              clskqct.BatThuong,
                              clskq.ThoiGianThucHien,
                              clskq.KetLuan,
                              clskq.MoTa_Text
                            FROM 
                              dbo.CLSYeuCau clsyc 
                              LEFT JOIN ehosdict.DM_NhomDichVu ndv ON clsyc.NhomDichVu_Id = ndv.NhomDichVu_Id 
                              LEFT JOIN dbo.CLSKetQua clskq ON clsyc.CLSYeuCau_Id = clskq.CLSYeuCau_Id 
                              LEFT JOIN ehosdict.DM_PhongBan pb ON clsyc.NoiYeuCau_Id = pb.PhongBan_Id 
                              LEFT JOIN dbo.CLSKetQuaChiTiet clskqct ON clskq.CLSKetQua_Id = clskqct.CLSKetQua_Id 
                              LEFT JOIN ehosdict.DM_DichVu dv ON clskqct.DichVu_Id = dv.DichVu_Id
                            WHERE 
                              clsyc.BenhAn_Id = (
                                SELECT 
                                  benhan_id 
                                FROM 
                                  benhan 
                                WHERE 
                                  SoBenhAn = N'@SoBenhAn_Params'
                              ) 
                              AND clskq.CLSKetQua_Id IS NOT NULL 
                              AND (clskqct.BatThuong = 1 OR clskq.PhanLoaiKetQua_Id IS NOT NULL OR (clskqct.MucBinhThuong is null and clskqct.MucBinhThuongMin is null and clskqct.MucBinhThuongMax is null and clskqct.KetQua is not null))
                            ORDER BY 
                              clskq.ThoiGianThucHien desc