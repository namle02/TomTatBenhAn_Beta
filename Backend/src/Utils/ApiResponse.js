/**
 * Lớp ApiResponse chung cho tất cả API endpoints
 * Đảm bảo format response nhất quán giữa frontend và backend
 */
class ApiResponse {
    constructor(success = false, message = '', data = null, statusCode = 200) {
        this.success = success;
        this.message = message;
        this.data = data;
        this.statusCode = statusCode;
        this.timestamp = new Date().toISOString();
    }

    /**
     * Tạo response thành công
     * @param {any} data - Dữ liệu trả về
     * @param {string} message - Thông báo thành công
     * @param {number} statusCode - Mã trạng thái HTTP
     * @returns {ApiResponse}
     */
    static success(data = null, message = 'Thành công', statusCode = 200) {
        return new ApiResponse(true, message, data, statusCode);
    }

    /**
     * Tạo response lỗi
     * @param {string} message - Thông báo lỗi
     * @param {number} statusCode - Mã trạng thái HTTP
     * @param {any} data - Dữ liệu lỗi (nếu có)
     * @returns {ApiResponse}
     */
    static error(message = 'Có lỗi xảy ra', statusCode = 500, data = null) {
        return new ApiResponse(false, message, data, statusCode);
    }

    /**
     * Tạo response không tìm thấy
     * @param {string} message - Thông báo không tìm thấy
     * @param {any} data - Dữ liệu (nếu có)
     * @returns {ApiResponse}
     */
    static notFound(message = 'Không tìm thấy dữ liệu', data = null) {
        return new ApiResponse(false, message, data, 404);
    }

    /**
     * Tạo response dữ liệu không hợp lệ
     * @param {string} message - Thông báo lỗi validation
     * @param {any} data - Dữ liệu lỗi validation
     * @returns {ApiResponse}
     */
    static badRequest(message = 'Dữ liệu không hợp lệ', data = null) {
        return new ApiResponse(false, message, data, 400);
    }

    /**
     * Tạo response không có quyền truy cập
     * @param {string} message - Thông báo lỗi quyền
     * @returns {ApiResponse}
     */
    static unauthorized(message = 'Không có quyền truy cập') {
        return new ApiResponse(false, message, null, 401);
    }

    /**
     * Tạo response bị cấm
     * @param {string} message - Thông báo lỗi cấm
     * @returns {ApiResponse}
     */
    static forbidden(message = 'Bị cấm truy cập') {
        return new ApiResponse(false, message, null, 403);
    }

    /**
     * Chuyển đổi thành JSON để gửi response
     * @returns {Object}
     */
    toJSON() {
        return {
            success: this.success,
            message: this.message,
            data: this.data,
            statusCode: this.statusCode,
            timestamp: this.timestamp
        };
    }

    /**
     * Gửi response trực tiếp qua Express res object
     * @param {Object} res - Express response object
     */
    send(res) {
        return res.status(this.statusCode).json(this.toJSON());
    }
}

module.exports = ApiResponse;
