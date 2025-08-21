const mongoose = require('mongoose');

// Schema cho loại bệnh án
const LoaiBenhAnSchema = new mongoose.Schema({
    loaiBenhAn_Id: { type: String, default: '' },
    loaiBenhAn: { type: String, default: '' },
    benhAnTongQuat_Id: { type: String, default: '' },
    tiepNhan_id: { type: String, default: '' }
}, { _id: false });

// Schema cho thông tin hành chính
const ThongTinHanhChinhSchema = new mongoose.Schema({
    soBenhAn: { type: String, default: '' },
    soVaoVien: { type: String, default: '' },
    cccd: { type: String, default: '' },
    tenBN: { type: String, default: '' },
    ngaySinh: { type: String, default: '' },
    tuoi: { type: Number, default: null },
    gioiTinh: { type: String, default: '' },
    diaChi: { type: String, default: '' },
    soBHYT: { type: String, default: '' },
    ngayVaoVien: { type: Date, default: null },
    ngayRaVien: { type: Date, default: null },
    danToc: { type: String, default: '' },
    maYTe: { type: String, default: '' },
    thoiGianVaoVien: { type: String, default: '' },
    thoiGianRaVien: { type: String, default: '' },
    ketQuaDieuTri: { type: String, default: '' }
}, { _id: false });

// Schema cho thông tin khám bệnh
const ThongTinKhamBenhSchema = new mongoose.Schema({
    lyDoVaoVien: { type: String, default: '' },
    quaTrinhBenhLy: { type: String, default: '' },
    tienSuBenh: { type: String, default: '' },
    huongDieuTri: { type: String, default: '' },
    huongDieuTri_PTTT: { type: String, default: '' },
}, { _id: false });

// Schema cho ID bệnh án
const BenhAnIdSchema = new mongoose.Schema({
    benhAnId: { type: String, default: '' },
    soBenhAn: { type: String, default: '' }
}, { _id: false });

// Schema cho chẩn đoán ICD
const ChanDoanICDSchema = new mongoose.Schema({
    benhChinhVaoVien: { type: String, default: '' },
    maICDChinhVaoVien: { type: String, default: '' },
    benhPhuVaoVien: { type: String, default: '' },
    maICDPhuVaoVien: { type: String, default: '' },
    benhChinhRaVien: { type: String, default: '' },
    maICDChinhRaVien: { type: String, default: '' },
    benhKemTheoRaVien: { type: String, default: '' },
    maICDKemTheoRaVien: { type: String, default: '' }
}, { _id: false });

// Schema cho tình trạng người bệnh ra viện
const TinhTrangNguoiBenhRaVienSchema = new mongoose.Schema({
    dienBien: { type: String, default: '' },
    loiDanThayThuoc: { type: String, default: '' },
    ppdt: { type: String, default: '' }
}, { _id: false });

// Schema cho kết quả xét nghiệm
const KetQuaXetNghiemSchema = new mongoose.Schema({
    tenDichVu: { type: String, default: '' },
    ketQua: { type: String, default: '' },
    mucBinhThuong: { type: String, default: '' },
    ketLuan: { type: String, default: '' }
}, { _id: false });

// Schema cho data tóm tắt
const DataTomTatSchema = new mongoose.Schema({
    tomTatQuaTrinhBenhLy: { type: String, default: '' },
    tomTatDauHieuLamSang: { type: String, default: '' },
    tomTatKetQuaXN: { type: String, default: '' },
    tomTatTinhTrangNguoiBenhRaVien: { type: String, default: '' },
    tomTatHuongDieuTriTiepTheo: { type: String, default: '' }
}, { _id: false });

// Schema chính cho bệnh nhân
const BenhNhanSchema = new mongoose.Schema({
    reportNumber: { type: String, default: '' },
    doctorName: { type: String, default: '' },
    loaiBenhAn: { type: LoaiBenhAnSchema, default: null },
    thongTinKhamBenh: [{ type: ThongTinKhamBenhSchema }],
    danhSachBenhAn: [{ type: BenhAnIdSchema }],
    thongTinHanhChinh: [{ type: ThongTinHanhChinhSchema }],
    chanDoanIcd: [{ type: ChanDoanICDSchema }],
    tinhTrangNguoiBenhRaVien: [{ type: TinhTrangNguoiBenhRaVienSchema }],
    ketQuaXetNghien: [{ type: KetQuaXetNghiemSchema }],
    thongTinTomTat: [{ type: DataTomTatSchema }],
    createdAt: { type: Date, default: Date.now },
    updatedAt: { type: Date, default: Date.now }
});

// Index cho tìm kiếm nhanh theo số bệnh án
BenhNhanSchema.index({ 'thongTinHanhChinh.soBenhAn': 1 });

// Middleware để cập nhật updatedAt
BenhNhanSchema.pre('save', function(next) {
    this.updatedAt = new Date();
    next();
});

module.exports = mongoose.model('BenhNhan', BenhNhanSchema);
