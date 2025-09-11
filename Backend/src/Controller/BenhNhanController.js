const BenhNhanServices = require('../Services/BenhNhanServices');
const ApiResponse = require('../Utils/ApiResponse');

class BenhNhanController {
    
    // Lưu thông tin bệnh nhân
    async saveBenhNhan(req, res) {
        try {
            const benhNhanData = req.body;
            
            // Validate dữ liệu cơ bản
            if (!benhNhanData || Object.keys(benhNhanData).length === 0) {
                return ApiResponse.badRequest('Dữ liệu bệnh nhân không được để trống').send(res);
            }

            const result = await BenhNhanServices.saveBenhNhan(benhNhanData);
            
            if (result.success) {
                return ApiResponse.success(result.data, result.message).send(res);
            } else {
                return ApiResponse.error(result.message, 400).send(res);
            }
        } catch (error) {
            console.error('Lỗi trong saveBenhNhan:', error);
            return ApiResponse.error(`Lỗi khi lưu bệnh nhân: ${error.message}`, 500).send(res);
        }
    }

    // Tìm kiếm bệnh nhân theo số bệnh án
    async getBenhNhanBySoBenhAn(req, res) {
        try {
            console.log('đã nhận yêu cầu lấy bệnh nhân theo số bệnh án');
            const { soBenhAn } = req.params;
            
            if (!soBenhAn) {
                return ApiResponse.badRequest('Số bệnh án không được để trống').send(res);
            }

            const result = await BenhNhanServices.findBenhNhanBySoBenhAn(soBenhAn);
            
            if (result.success) {
                return ApiResponse.success(result.data, result.message).send(res);
            } else {
                return ApiResponse.notFound(result.message).send(res);
            }
        } catch (error) {
            console.error('Lỗi trong getBenhNhanBySoBenhAn:', error);
            return ApiResponse.error(`Lỗi khi tìm kiếm bệnh nhân: ${error.message}`, 500).send(res);
        }
    }

    // Lấy danh sách tất cả bệnh nhân
    async getAllBenhNhan(req, res) {
        try {
            const page = parseInt(req.query.page) || 1;
            const limit = parseInt(req.query.limit) || 10;
            
            const result = await BenhNhanServices.getAllBenhNhan(page, limit);
            
            if (result.success) {
                return ApiResponse.success(result.data, result.message).send(res);
            } else {
                return ApiResponse.error(result.message, 400).send(res);
            }
        } catch (error) {
            console.error('Lỗi trong getAllBenhNhan:', error);
            return ApiResponse.error(`Lỗi khi lấy danh sách bệnh nhân: ${error.message}`, 500).send(res);
        }
    }

    // Xóa bệnh nhân
    async deleteBenhNhan(req, res) {
        try {
            const { id } = req.params;
            
            if (!id) {
                return ApiResponse.badRequest('ID bệnh nhân không được để trống').send(res);
            }

            const result = await BenhNhanServices.deleteBenhNhan(id);
            
            if (result.success) {
                return ApiResponse.success(result.data, result.message).send(res);
            } else {
                return ApiResponse.notFound(result.message).send(res);
            }
        } catch (error) {
            console.error('Lỗi trong deleteBenhNhan:', error);
            return ApiResponse.error(`Lỗi khi xóa bệnh nhân: ${error.message}`, 500).send(res);
        }
    }

    // Tìm kiếm bệnh nhân theo tên
    async getBenhNhanByName(req, res) {
        try {
            const { tenBN } = req.query;
            
            if (!tenBN) {
                return ApiResponse.badRequest('Tên bệnh nhân không được để trống').send(res);
            }

            const result = await BenhNhanServices.findBenhNhanByName(tenBN);
            
            if (result.success) {
                return ApiResponse.success(result.data, result.message).send(res);
            } else {
                return ApiResponse.error(result.message, 400).send(res);
            }
        } catch (error) {
            console.error('Lỗi trong getBenhNhanByName:', error);
            return ApiResponse.error(`Lỗi khi tìm kiếm bệnh nhân theo tên: ${error.message}`, 500).send(res);
        }
    }
}

module.exports = new BenhNhanController();
