const BenhNhanServices = require('../Services/BenhNhanServices');

class BenhNhanController {
    
    // Lưu thông tin bệnh nhân
    async saveBenhNhan(req, res) {
        try {
            const benhNhanData = req.body;
            
            // Validate dữ liệu cơ bản
            if (!benhNhanData || Object.keys(benhNhanData).length === 0) {
                return res.status(400).json({
                    success: false,
                    message: 'Dữ liệu bệnh nhân không được để trống',
                    data: null
                });
            }

            const result = await BenhNhanServices.saveBenhNhan(benhNhanData);
            
            return res.status(200).json(result);
        } catch (error) {
            console.error('Lỗi trong saveBenhNhan:', error);
            return res.status(500).json({
                success: false,
                message: error.message,
                data: null
            });
        }
    }

    // Tìm kiếm bệnh nhân theo số bệnh án
    async getBenhNhanBySoBenhAn(req, res) {
        try {
            const { soBenhAn } = req.params;
            
            if (!soBenhAn) {
                return res.status(400).json({
                    success: false,
                    message: 'Số bệnh án không được để trống',
                    data: null
                });
            }

            const result = await BenhNhanServices.findBenhNhanBySoBenhAn(soBenhAn);
            
            if (!result.success) {
                return res.status(404).json(result);
            }
            
            return res.status(200).json(result);
        } catch (error) {
            console.error('Lỗi trong getBenhNhanBySoBenhAn:', error);
            return res.status(500).json({
                success: false,
                message: error.message,
                data: null
            });
        }
    }

    // Lấy danh sách tất cả bệnh nhân
    async getAllBenhNhan(req, res) {
        try {
            const page = parseInt(req.query.page) || 1;
            const limit = parseInt(req.query.limit) || 10;
            
            const result = await BenhNhanServices.getAllBenhNhan(page, limit);
            
            return res.status(200).json(result);
        } catch (error) {
            console.error('Lỗi trong getAllBenhNhan:', error);
            return res.status(500).json({
                success: false,
                message: error.message,
                data: null
            });
        }
    }

    // Xóa bệnh nhân
    async deleteBenhNhan(req, res) {
        try {
            const { id } = req.params;
            
            if (!id) {
                return res.status(400).json({
                    success: false,
                    message: 'ID bệnh nhân không được để trống',
                    data: null
                });
            }

            const result = await BenhNhanServices.deleteBenhNhan(id);
            
            if (!result.success) {
                return res.status(404).json(result);
            }
            
            return res.status(200).json(result);
        } catch (error) {
            console.error('Lỗi trong deleteBenhNhan:', error);
            return res.status(500).json({
                success: false,
                message: error.message,
                data: null
            });
        }
    }

    // Tìm kiếm bệnh nhân theo tên
    async getBenhNhanByName(req, res) {
        try {
            const { tenBN } = req.query;
            
            if (!tenBN) {
                return res.status(400).json({
                    success: false,
                    message: 'Tên bệnh nhân không được để trống',
                    data: null
                });
            }

            const result = await BenhNhanServices.findBenhNhanByName(tenBN);
            
            return res.status(200).json(result);
        } catch (error) {
            console.error('Lỗi trong getBenhNhanByName:', error);
            return res.status(500).json({
                success: false,
                message: error.message,
                data: null
            });
        }
    }
}

module.exports = new BenhNhanController();
