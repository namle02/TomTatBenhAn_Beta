const mongoose = require('mongoose');

// Schema cho kết quả đánh giá từng tiêu chí
const KetQuaDanhGiaSchema = new mongoose.Schema({
    "Đạt/có": {
        type: Boolean,
        default: false
    },
    "Không đạt/ Không có": {
        Checked: {
            type: Boolean,
            default: false
        },
        "Mô tả nội dung không đạt": {
            type: String,
            default: "Mô tả",
            trim: true
        }
    },
    "Không áp dụng": {
        type: Boolean,
        default: false
    }
}, { _id: false });

// Schema cho từng tiêu chí kiểm tra
const TieuChiKiemTraSchema = new mongoose.Schema({
    "Thứ tự tiêu chí": {
        type: String,
        required: true,
        trim: true
    },
    "Tiêu chuẩn/Yêu cầu đạt được": {
        type: String,
        required: true,
        trim: true
    },
    "Kết quả đánh giá": {
        type: KetQuaDanhGiaSchema,
        default: () => ({})
    }
}, { _id: false });

// Schema cho nội dung kiểm tra các mục con
const NoiDungKiemTraSchema = new mongoose.Schema({
    "Tiền sử": {
        type: [TieuChiKiemTraSchema],
        default: []
    },
    "Bệnh sử": {
        type: [TieuChiKiemTraSchema],
        default: []
    },
    "Khám bệnh": {
        type: [TieuChiKiemTraSchema],
        default: []
    },
    "Cận lâm sàng": {
        type: [TieuChiKiemTraSchema],
        default: []
    },
    "Chẩn đoán": {
        type: [TieuChiKiemTraSchema],
        default: []
    }
}, { _id: false });

// Schema cho nội dung kiểm tra điều trị
const NoiDungKiemTraDieuTriSchema = new mongoose.Schema({
    "Phẫu thuật": {
        type: [TieuChiKiemTraSchema],
        default: []
    },
    "Thuốc, dịch": {
        type: [TieuChiKiemTraSchema],
        default: []
    },
    "Theo dõi": {
        type: [TieuChiKiemTraSchema],
        default: []
    }
}, { _id: false });

// Schema cho nội dung kiểm tra chăm sóc
const NoiDungKiemTraChamSocSchema = new mongoose.Schema({
    "Phân cấp chăm sóc": {
        type: [TieuChiKiemTraSchema],
        default: []
    },
    "Thực hiện chăm sóc": {
        type: [TieuChiKiemTraSchema],
        default: []
    }
}, { _id: false });

// Schema cho nội dung kiểm tra ra viện
const NoiDungKiemTraRaVienSchema = new mongoose.Schema({
    "Tiêu chuẩn xuất viện": {
        type: [TieuChiKiemTraSchema],
        default: []
    },
    "Hướng dẫn ra viện": {
        type: [TieuChiKiemTraSchema],
        default: []
    }
}, { _id: false });

// Schema cho từng mục tiêu kiểm tra chính
const MucTieuKiemTraSchema = new mongoose.Schema({
    "I. Đánh giá và chẩn đoán": {
        "Nội dung kiểm tra": {
            type: NoiDungKiemTraSchema,
            default: () => ({})
        }
    },
    "II. Điều trị": {
        "Nội dung kiểm tra": {
            type: NoiDungKiemTraDieuTriSchema,
            default: () => ({})
        }
    },
    "III. Chăm sóc": {
        "Nội dung kiểm tra": {
            type: NoiDungKiemTraChamSocSchema,
            default: () => ({})
        }
    },
    "IV. Ra viện": {
        "Nội dung kiểm tra": {
            type: NoiDungKiemTraRaVienSchema,
            default: () => ({})
        }
    }
}, { _id: false });

// Schema chính cho bảng đánh giá
const BangDanhGiaSchema = new mongoose.Schema({
    // Thông tin cơ bản
    tenBangDanhGia: {
        type: String,
        required: true,
        trim: true
    },
    maBangDanhGia: {
        type: String,
        trim: true,
        sparse: true // Cho phép null nhưng unique nếu có giá trị
    },
    
    // Liên kết với phác đồ
    phacDoId: {
        type: mongoose.Schema.Types.ObjectId,
        ref: 'PhacDoModel',
        required: false // Có thể có bảng đánh giá độc lập
    },
    
    // Thông tin người đánh giá
    nguoiDanhGia: {
        type: String,
        trim: true
    },
    ngayDanhGia: {
        type: Date,
        default: Date.now
    },
    
    // Thông tin bệnh nhân/hồ sơ được đánh giá
    thongTinBenhNhan: {
        hoTen: {
            type: String,
            trim: true
        },
        maBenhNhan: {
            type: String,
            trim: true
        },
        ngayNhapVien: {
            type: Date
        },
        ngayXuatVien: {
            type: Date
        }
    },
    
    // Nội dung đánh giá chính
    "Mục tiêu kiểm tra": {
        type: MucTieuKiemTraSchema,
        required: true,
        default: () => ({})
    },
    
    // Kết quả tổng hợp
    ketQuaTongHop: {
        tongSoTieuChi: {
            type: Number,
            default: 0
        },
        soTieuChiDat: {
            type: Number,
            default: 0
        },
        soTieuChiKhongDat: {
            type: Number,
            default: 0
        },
        soTieuChiKhongApDung: {
            type: Number,
            default: 0
        },
        tiLeDat: {
            type: Number,
            default: 0,
            min: 0,
            max: 100
        }
    },
    
    // Ghi chú và nhận xét
    ghiChu: {
        type: String,
        trim: true
    },
    nhanXet: {
        type: String,
        trim: true
    },
    
    // Trạng thái đánh giá
    trangThai: {
        type: String,
        enum: ['draft', 'completed', 'reviewed', 'approved'],
        default: 'draft'
    }
}, {
    timestamps: true,
    collection: 'bangdanhgia'
});

// Index để tối ưu tìm kiếm
BangDanhGiaSchema.index({ 'phacDoId': 1 });
BangDanhGiaSchema.index({ 'ngayDanhGia': -1 });
BangDanhGiaSchema.index({ 'thongTinBenhNhan.maBenhNhan': 1 });
BangDanhGiaSchema.index({ 'trangThai': 1 });
BangDanhGiaSchema.index({ 'maBangDanhGia': 1 }, { unique: true, sparse: true });

// Virtual để tính toán kết quả tự động
BangDanhGiaSchema.virtual('ketQuaAuto').get(function() {
    let tongSo = 0;
    let dat = 0;
    let khongDat = 0;
    let khongApDung = 0;
    
    // Hàm đệ quy để đếm tiêu chí
    const demTieuChi = (obj) => {
        if (Array.isArray(obj)) {
            obj.forEach(item => {
                if (item['Kết quả đánh giá']) {
                    tongSo++;
                    if (item['Kết quả đánh giá']['Đạt/có']) dat++;
                    if (item['Kết quả đánh giá']['Không đạt/ Không có']?.Checked) khongDat++;
                    if (item['Kết quả đánh giá']['Không áp dụng']) khongApDung++;
                }
            });
        } else if (typeof obj === 'object' && obj !== null) {
            Object.values(obj).forEach(demTieuChi);
        }
    };
    
    demTieuChi(this['Mục tiêu kiểm tra']);
    
    return {
        tongSoTieuChi: tongSo,
        soTieuChiDat: dat,
        soTieuChiKhongDat: khongDat,
        soTieuChiKhongApDung: khongApDung,
        tiLeDat: tongSo > 0 ? Math.round((dat / tongSo) * 100) : 0
    };
});

// Middleware để tự động cập nhật kết quả tổng hợp trước khi save
BangDanhGiaSchema.pre('save', function(next) {
    const ketQua = this.ketQuaAuto;
    this.ketQuaTongHop = ketQua;
    next();
});

module.exports = mongoose.model('BangDanhGiaModel', BangDanhGiaSchema);
