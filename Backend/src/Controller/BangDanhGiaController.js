const BangDanhGiaServices = require('../Services/BangDanhGiaServices');
const ApiResponse = require('../Utils/ApiResponse');

class BangDanhGiaController {
    async checkExists(req, res) {
        try {
            const { tenBangKiem, phacDoId } = req.query;
            if (!tenBangKiem || !phacDoId) {
                return ApiResponse.badRequest('Thiếu tham số tenBangKiem hoặc phacDoId').send(res);
            }
            const exists = await BangDanhGiaServices.checkExistsByNameAndProtocol(tenBangKiem, phacDoId);
            if (exists) {
                return ApiResponse.success({ exists: true, data: exists }, 'Bảng đánh giá đã tồn tại').send(res);
            }
            return ApiResponse.success({ exists: false }, 'Bảng đánh giá chưa tồn tại').send(res);
        } catch (error) {
            return ApiResponse.error(error.message, 400).send(res);
        }
    }

    async create(req, res) {
        try {
            const result = await BangDanhGiaServices.createBangDanhGia(req.body);
            if (result.success) {
                return ApiResponse.success(result.data, result.message).send(res);
            }
            return ApiResponse.error(result.message, 409).send(res);
        } catch (error) {
            return ApiResponse.error(error.message, 400).send(res);
        }
    }

    async update(req, res) {
        try {
            const { id } = req.params;
            const result = await BangDanhGiaServices.updateBangDanhGia(id, req.body);
            return ApiResponse.success(result.data, result.message).send(res);
        } catch (error) {
            return ApiResponse.error(error.message, 400).send(res);
        }
    }

    async remove(req, res) {
        try {
            const { id } = req.params;
            const result = await BangDanhGiaServices.deleteBangDanhGia(id);
            return ApiResponse.success(null, result.message).send(res);
        } catch (error) {
            return ApiResponse.error(error.message, 400).send(res);
        }
    }

    async getById(req, res) {
        try {
            const { id } = req.params;
            const result = await BangDanhGiaServices.getBangDanhGiaById(id);
            return ApiResponse.success(result.data, result.message).send(res);
        } catch (error) {
            return ApiResponse.notFound(error.message).send(res);
        }
    }

    async getAll(req, res) {
        try {
            const { phacDoId, search } = req.query;
            const result = await BangDanhGiaServices.getAllBangDanhGia({ phacDoId, search });
            return ApiResponse.success(result.data, result.message).send(res);
        } catch (error) {
            return ApiResponse.error(error.message, 400).send(res);
        }
    }
}

module.exports = new BangDanhGiaController();
