const BangDanhGiaModel = require("../Repos/Model/BangDanhGiaModel");
const PhacDoModel = require("../Repos/Model/PhacDoModel");
const fs = require('fs');
const path = require('path');

class BangDanhGiaServices {
    /**
     * Tạo mới bảng kiểm với file Word gốc
     */
    async createBangDanhGia(bangDanhGia) {
        console.log("Thêm bảng đánh giá với file Word");
        const { data, originalFileName, originalFilePath, fileSize } = bangDanhGia || {};
      
        if (!data.tenBangKiem || !data.phacDoId) {
            throw new Error('Thiếu tenBangKiem hoặc phacDoId');
        }

        if (!originalFilePath) {
            throw new Error('Thiếu file Word gốc');
        }

        const protocol = await PhacDoModel.findById(data.phacDoId);
        if (!protocol) {
            throw new Error('Không tìm thấy phác đồ liên kết');
        }

        // Kiểm tra trùng tên trong cùng phác đồ
        const exists = await BangDanhGiaModel.findOne({ 
            tenBangKiem: { $regex: `^${data.tenBangKiem.trim()}$`, $options: 'i' }, 
            phacDoId: data.phacDoId 
        });
        
        if (exists) {
            // Xóa file đã upload nếu trùng tên
            if (fs.existsSync(originalFilePath)) {
                fs.unlinkSync(originalFilePath);
            }
            return { 
                success: false, 
                message: 'Bảng đánh giá đã tồn tại cho phác đồ này', 
                existingId: exists._id, 
                data: exists 
            };
        }

        // Gộp metadata file vào document lưu DB
        const payloadToCreate = {
            ...data,
            originalFileName,
            originalFilePath,
            fileSize,
            uploadedAt: new Date()
        };

        const created = await BangDanhGiaModel.create(payloadToCreate);
        return { success: true, message: 'Tạo bảng đánh giá thành công', data: created };
    }

    /**
     * Xóa bảng kiểm và file Word gốc
     */
    async deleteBangDanhGia(id) {
        console.log("Xóa bảng đánh giá và file Word", id);
        if (!id) throw new Error('Thiếu id');
        
        const bangDanhGia = await BangDanhGiaModel.findById(id);
        if (!bangDanhGia) {
            throw new Error('Không tìm thấy bảng đánh giá để xóa');
        }

        // Xóa file Word gốc nếu tồn tại
        if (bangDanhGia.originalFilePath && fs.existsSync(bangDanhGia.originalFilePath)) {
            try {
                fs.unlinkSync(bangDanhGia.originalFilePath);
                console.log(`Đã xóa file: ${bangDanhGia.originalFilePath}`);
            } catch (error) {
                console.error(`Lỗi khi xóa file: ${error.message}`);
            }
        }

        // Xóa record trong database
        await BangDanhGiaModel.findByIdAndDelete(id);
        return { success: true, message: 'Xóa bảng đánh giá và file Word thành công' };
    }

    /**
     * Lấy thông tin bảng kiểm theo ID (chỉ dùng cho download file)
     */
    async getBangDanhGiaById(id) {
        const item = await BangDanhGiaModel.findById(id).populate({ 
            path: 'phacDoId', 
            select: 'protocol.name protocol.code' 
        });
        
        if (!item) throw new Error('Không tìm thấy bảng đánh giá');
        
        const mapped = {
            _id: item._id,
            bangKiemId: item._id,
            phacDoId: item.phacDoId?._id || item.phacDoId,
            tenBangKiem: item.tenBangKiem,
            tenPhacDo: item.phacDoId?.protocol?.name || '',
            originalFileName: item.originalFileName,
            originalFilePath: item.originalFilePath,
            fileSize: item.fileSize,
            uploadedAt: item.uploadedAt,
            createdAt: item.createdAt,
            updatedAt: item.updatedAt
        };
        
        return { success: true, message: 'Lấy thông tin bảng đánh giá thành công', data: mapped };
    }

    /**
     * Lấy danh sách tất cả bảng kiểm với metadata file
     */
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
            bangKiemId: item._id,
            phacDoId: item.phacDoId?._id || item.phacDoId,
            tenBangKiem: item.tenBangKiem,
            tenPhacDo: item.phacDoId?.protocol?.name || '',
            danhGiaVaChanDoan: item.danhGiaVaChanDoan,
            dieuTri: item.dieuTri,
            chamSoc: item.chamSoc,
            raVien: item.raVien,
            // Metadata file Word gốc
            originalFileName: item.originalFileName,
            originalFilePath: item.originalFilePath,
            fileSize: item.fileSize,
            uploadedAt: item.uploadedAt,
            createdAt: item.createdAt,
            updatedAt: item.updatedAt,
        }));
        
        return { success: true, message: 'Lấy danh sách bảng đánh giá thành công', data: mapped };
    }
}

module.exports = new BangDanhGiaServices();
