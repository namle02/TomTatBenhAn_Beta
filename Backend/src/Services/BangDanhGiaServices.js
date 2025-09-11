const BangDanhGiaModel = require("../Repos/Model/BangDanhGiaModel");
const PhacDoModel = require("../Repos/Model/PhacDoModel");

class BangDanhGiaServices {
    async checkExistsByNameAndProtocol(tenBangKiem, phacDoId) {
        if (!tenBangKiem || !phacDoId) return null;
        return await BangDanhGiaModel.findOne({ tenBangKiem: { $regex: `^${tenBangKiem.trim()}$`, $options: 'i' }, phacDoId });
    }

    async createBangDanhGia(bangDanhGia) {
        console.log("Thêm bảng đánh giá");
        const { tenBangKiem, phacDoId } = bangDanhGia || {};
        if (!tenBangKiem || !phacDoId) {
            throw new Error('Thiếu tenBangKiem hoặc phacDoId');
        }

        const protocol = await PhacDoModel.findById(phacDoId);
        if (!protocol) {
            throw new Error('Không tìm thấy phác đồ liên kết');
        }

        const exists = await this.checkExistsByNameAndProtocol(tenBangKiem, phacDoId);
        if (exists) {
            return { success: false, message: 'Bảng đánh giá đã tồn tại cho phác đồ này', existingId: exists._id, data: exists };
        }

        const created = await BangDanhGiaModel.create(bangDanhGia);
        return { success: true, message: 'Tạo bảng đánh giá thành công', data: created };
    }

    async updateBangDanhGia(id, updateData) {
        if (!id) throw new Error('Thiếu id');
        const updated = await BangDanhGiaModel.findByIdAndUpdate(id, updateData, { new: true, runValidators: true });
        if (!updated) throw new Error('Không tìm thấy bảng đánh giá để cập nhật');
        return { success: true, message: 'Cập nhật bảng đánh giá thành công', data: updated };
    }

    async deleteBangDanhGia(id) {
        console.log("Xóa bảng đánh giá", id);
        if (!id) throw new Error('Thiếu id');
        const deleted = await BangDanhGiaModel.findByIdAndDelete(id);
        if (!deleted) throw new Error('Không tìm thấy bảng đánh giá để xóa');
        return { success: true, message: 'Xóa bảng đánh giá thành công' };
    }

    async getBangDanhGiaById(id) {
        const item = await BangDanhGiaModel.findById(id).populate({ path: 'phacDoId', select: 'protocol.name protocol.code' });
        if (!item) throw new Error('Không tìm thấy bảng đánh giá');
        const mapped = {
            _id: item._id,
            bangKiemId: item._id, // Sử dụng _id làm bangKiemId
            phacDoId: item.phacDoId?._id || item.phacDoId,
            tenBangKiem: item.tenBangKiem,
            danhGiaVaChanDoan: item.danhGiaVaChanDoan,
            dieuTri: item.dieuTri,
            chamSoc: item.chamSoc,
            raVien: item.raVien,
            createdAt: item.createdAt,
            updatedAt: item.updatedAt,
            tenPhacDo: item.phacDoId?.protocol?.name || ''
        };
        return { success: true, message: 'Lấy chi tiết bảng đánh giá thành công', data: mapped };
    }

    async getAllBangDanhGia({ phacDoId, search } = {}) {
        console.log("Lấy tất cả bảng đánh giá");
        const query = {};
        if (phacDoId) query.phacDoId = phacDoId;
        if (search) query.tenBangKiem = { $regex: search, $options: 'i' };

        const list = await BangDanhGiaModel.find(query)
            .sort({ createdAt: -1 })
            .populate({ path: 'phacDoId', select: 'protocol.name protocol.code' });
        const mapped = list.map(item => ({
            _id: item._id,
            bangKiemId: item._id, // Sử dụng _id làm bangKiemId
            phacDoId: item.phacDoId?._id || item.phacDoId,
            tenBangKiem: item.tenBangKiem,
            tenPhacDo: item.phacDoId?.protocol?.name || '',
            danhGiaVaChanDoan: item.danhGiaVaChanDoan,
            dieuTri: item.dieuTri,
            chamSoc: item.chamSoc,
            raVien: item.raVien,
            createdAt: item.createdAt,
            updatedAt: item.updatedAt,
        }));
        return { success: true, message: 'Lấy danh sách bảng đánh giá thành công', data: mapped };
    }
}

module.exports = new BangDanhGiaServices();
