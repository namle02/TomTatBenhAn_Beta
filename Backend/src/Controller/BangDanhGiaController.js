const BangDanhGiaServices = require('../Services/BangDanhGiaServices');
const ApiResponse = require('../Utils/ApiResponse');
const fs = require('fs');
const path = require('path');

class BangDanhGiaController {
    /**
     * GET /api/bang-danh-gia - Lấy danh sách tất cả bảng kiểm
     */
    async getAll(req, res) {
        try {
            const { phacDoId, search } = req.query;
            const result = await BangDanhGiaServices.getAllBangDanhGia({ phacDoId, search });
            return ApiResponse.success(result.data, result.message).send(res);
        } catch (error) {
            return ApiResponse.error(error.message, 400).send(res);
        }
    }

    /**
     * POST /api/bang-danh-gia - Tạo mới bảng kiểm với file Word gốc
     */
    async create(req, res) {
        try {
            // Kiểm tra có file upload không
            if (!req.file) {
                return ApiResponse.badRequest('Vui lòng upload file Word bảng kiểm').send(res);
            }

            // Validate file type
            const allowedTypes = ['.docx', '.doc'];
            const fileExt = path.extname(req.file.originalname).toLowerCase();
            if (!allowedTypes.includes(fileExt)) {
                // Xóa file đã upload nếu không đúng định dạng
                fs.unlinkSync(req.file.path);
                return ApiResponse.badRequest('Chỉ chấp nhận file Word (.docx, .doc)').send(res);
            }

            // Parse JSON từ field multipart 'data' (client gửi JSON trong trường này)
            // Nếu client gửi các field phẳng (không bọc 'data') thì fallback sang req.body
            let parsedData = {};
            if (typeof req.body?.data === 'string') {
                try {
                    parsedData = JSON.parse(req.body.data);
                } catch (e) {
                    // Sai định dạng JSON
                    fs.unlinkSync(req.file.path);
                    return ApiResponse.badRequest('Trường data không phải JSON hợp lệ').send(res);
                }
            } else {
                parsedData = req.body || {};
            }

            // Chuẩn bị dữ liệu để tạo bảng kiểm
            const bangDanhGiaData = {
                data: parsedData,
                originalFileName: req.file.originalname,
                originalFilePath: req.file.path,
                fileSize: req.file.size,
                uploadedAt: new Date()
            };


            const result = await BangDanhGiaServices.createBangDanhGia(bangDanhGiaData);
            if (result.success) {
                return ApiResponse.success(result.data, result.message).send(res);
            }
            return ApiResponse.error(result.message, 409).send(res);
        } catch (error) {
            // Xóa file nếu có lỗi xảy ra
            if (req.file && fs.existsSync(req.file.path)) {
                fs.unlinkSync(req.file.path);
            }
            return ApiResponse.error(error.message, 400).send(res);
        }
    }

    /**
     * DELETE /api/bang-danh-gia/:id - Xóa bảng kiểm và file Word gốc
     */
    async remove(req, res) {
        try {
            const { id } = req.params;
            const result = await BangDanhGiaServices.deleteBangDanhGia(id);
            return ApiResponse.success(null, result.message).send(res);
        } catch (error) {
            return ApiResponse.error(error.message, 400).send(res);
        }
    }

    /**
     * GET /api/bang-danh-gia/:id/download - Download file Word gốc
     */
    async downloadOriginalFile(req, res) {
        try {
            const { id } = req.params;
            const result = await BangDanhGiaServices.getBangDanhGiaById(id);


            if (!result.data || !result.data.originalFilePath) {
                return ApiResponse.notFound('Không tìm thấy file gốc').send(res);
            }

            const filePath = result.data.originalFilePath;
            const fileName = result.data.originalFileName || `bang-kiem-${id}.docx`;

            // Kiểm tra file có tồn tại không
            if (!fs.existsSync(filePath)) {
                return ApiResponse.notFound('File không tồn tại trên server').send(res);
            }

            // Set headers cho download
            res.setHeader('Content-Disposition', `attachment; filename="${encodeURIComponent(fileName)}"`);
            res.setHeader('Content-Type', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document');

            // Stream file về client
            const fileStream = fs.createReadStream(filePath);
            fileStream.pipe(res);

        } catch (error) {
            return ApiResponse.error(error.message, 400).send(res);
        }
    }
}

module.exports = new BangDanhGiaController();
