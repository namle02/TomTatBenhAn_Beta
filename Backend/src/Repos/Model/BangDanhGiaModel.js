const mongoose = require('mongoose');

const TieuChiKiemTraSchema = new mongoose.Schema(
    {
        stt: { type: String, trim: true },
        yeuCauDatDuoc: { type: String, required: true, trim: true },
        dat: { type: Boolean, default: false },
        khongDat: { type: Boolean, default: false },
        lyDoKhongDat: { type: String, default: '' },
        khongApDung: { type: Boolean, default: false },
        isImportant: { type: Boolean, default: false }
    },
    { _id: false }
);

const NoiDungKiemTraSchema = new mongoose.Schema(
    {
        tenNoiDungKiemTra: { type: String, required: true, trim: true },
        danhSachTieuChi: { type: [TieuChiKiemTraSchema], default: [] }
    },
    { _id: false }
);

const HangMucKiemTraSchema = new mongoose.Schema(
    {
        stt: { type: String, trim: true },
        tenHangMucKiemTra: { type: String, required: true, trim: true },
        danhSachNoiDung: { type: [NoiDungKiemTraSchema], default: [] }
    },
    { _id: false }
);

const BangDanhGiaSchema = new mongoose.Schema(
    {
        phacDoId: { type: mongoose.Schema.Types.ObjectId, ref: 'PhacDoModel', required: true },
        tenBangKiem: { type: String, required: true, trim: true },
        danhGiaVaChanDoan: { type: HangMucKiemTraSchema, default: () => ({ stt: "1", tenHangMucKiemTra: "Đánh giá và chẩn đoán" }) },
        dieuTri: { type: HangMucKiemTraSchema, default: () => ({ stt: "2", tenHangMucKiemTra: "Điều trị" }) },
        chamSoc: { type: HangMucKiemTraSchema, default: () => ({ stt: "3", tenHangMucKiemTra: "Chăm sóc" }) },
        raVien: { type: HangMucKiemTraSchema, default: () => ({ stt: "4", tenHangMucKiemTra: "Ra viện" }) },
        // Thêm trường lưu trữ file Word gốc
        originalFileName: { type: String, trim: true }, // Tên file gốc
        originalFilePath: { type: String, trim: true }, // Đường dẫn file trên server
        fileSize: { type: Number }, // Kích thước file (bytes)
        uploadedAt: { type: Date, default: Date.now } // Thời gian upload
    },
    { timestamps: true }
);

// Chỉ mục kiểm tra trùng theo tên bảng kiểm trong cùng phác đồ
BangDanhGiaSchema.index({ tenBangKiem: 1, phacDoId: 1 }, { unique: true });

module.exports = mongoose.model('BangDanhGiaModel', BangDanhGiaSchema);


