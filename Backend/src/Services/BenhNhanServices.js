const BenhNhan = require('../Repos/Model/BenhNhanModel');

class BenhNhanServices {
    
    // Lưu thông tin bệnh nhân mới
    async saveBenhNhan(benhNhanData) {
        try {
            // Kiểm tra xem bệnh nhân đã tồn tại chưa (dựa trên số bệnh án)
            const soBenhAn = benhNhanData.thongTinHanhChinh?.[0]?.soBenhAn;
            
            if (!soBenhAn) {
                throw new Error('Số bệnh án không được để trống');
            }

            // Tìm bệnh nhân hiện có
            const existingBenhNhan = await BenhNhan.findOne({
                'thongTinHanhChinh.soBenhAn': soBenhAn
            });

            if (existingBenhNhan) {
                // Cập nhật thông tin bệnh nhân hiện có
                Object.assign(existingBenhNhan, benhNhanData);
                existingBenhNhan.updatedAt = new Date();
                await existingBenhNhan.save();
                return {
                    success: true,
                    message: 'Cập nhật thông tin bệnh nhân thành công',
                    data: existingBenhNhan
                };
            } else {
                // Tạo bệnh nhân mới
                const newBenhNhan = new BenhNhan(benhNhanData);
                await newBenhNhan.save();
                return {
                    success: true,
                    message: 'Lưu thông tin bệnh nhân thành công',
                    data: newBenhNhan
                };
            }
        } catch (error) {
            throw new Error(`Lỗi khi lưu bệnh nhân: ${error.message}`);
        }
    }

    // Tìm kiếm bệnh nhân theo số bệnh án
    async findBenhNhanBySoBenhAn(soBenhAn) {
        try {
           
            if (!soBenhAn) {
                throw new Error('Số bệnh án không được để trống');
            }

            const benhNhan = await BenhNhan.findOne({
                'thongTinHanhChinh.soBenhAn': soBenhAn
            }).exec();

            if (!benhNhan) {
                return {
                    success: false,
                    message: 'Không tìm thấy bệnh nhân với số bệnh án này',
                    data: null
                };
            }

            return {
                success: true,
                message: 'Tìm thấy thông tin bệnh nhân',
                data: benhNhan
            };
        } catch (error) {
            throw new Error(`Lỗi khi tìm kiếm bệnh nhân: ${error.message}`);
        }
    }

    // Lấy danh sách tất cả bệnh nhân (có phân trang)
    async getAllBenhNhan(page = 1, limit = 10) {
        try {
            const skip = (page - 1) * limit;
            
            const benhNhans = await BenhNhan.find()
                .sort({ createdAt: -1 })
                .skip(skip)
                .limit(limit)
                .exec();

            const total = await BenhNhan.countDocuments();

            return {
                success: true,
                message: 'Lấy danh sách bệnh nhân thành công',
                data: {
                    benhNhans,
                    pagination: {
                        currentPage: page,
                        totalPages: Math.ceil(total / limit),
                        totalItems: total,
                        itemsPerPage: limit
                    }
                }
            };
        } catch (error) {
            throw new Error(`Lỗi khi lấy danh sách bệnh nhân: ${error.message}`);
        }
    }

    // Xóa bệnh nhân theo ID
    async deleteBenhNhan(id) {
        try {
            const deletedBenhNhan = await BenhNhan.findByIdAndDelete(id);
            
            if (!deletedBenhNhan) {
                return {
                    success: false,
                    message: 'Không tìm thấy bệnh nhân để xóa',
                    data: null
                };
            }

            return {
                success: true,
                message: 'Xóa bệnh nhân thành công',
                data: deletedBenhNhan
            };
        } catch (error) {
            throw new Error(`Lỗi khi xóa bệnh nhân: ${error.message}`);
        }
    }

    // Tìm kiếm bệnh nhân theo tên (tìm kiếm mờ)
    async findBenhNhanByName(tenBN) {
        try {
            if (!tenBN) {
                throw new Error('Tên bệnh nhân không được để trống');
            }

            const benhNhans = await BenhNhan.find({
                'thongTinHanhChinh.tenBN': { $regex: tenBN, $options: 'i' }
            }).exec();

            return {
                success: true,
                message: `Tìm thấy ${benhNhans.length} bệnh nhân`,
                data: benhNhans
            };
        } catch (error) {
            throw new Error(`Lỗi khi tìm kiếm bệnh nhân theo tên: ${error.message}`);
        }
    }
}

module.exports = new BenhNhanServices();
