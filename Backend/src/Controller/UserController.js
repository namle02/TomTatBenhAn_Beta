const UserService = require('../Services/UserServices');
const ApiResponse = require('../Utils/ApiResponse');

class UserController {
    
    // Tạo user mới
    async createUser(req, res) {
        try {
            const userData = req.body;
            
            // Validate dữ liệu cơ bản
            if (!userData || Object.keys(userData).length === 0) {
                return ApiResponse.badRequest('Dữ liệu user không được để trống').send(res);
            }

            const result = await UserService.CreateUser(userData);
            
            if (result && result.success) {
                return ApiResponse.success(result.data, result.message).send(res);
            } else {
                return ApiResponse.error(result?.message || 'Lỗi khi tạo user', 400).send(res);
            }
        } catch (error) {
            console.error('Lỗi trong createUser:', error);
            return ApiResponse.error(`Lỗi khi tạo user: ${error.message}`, 500).send(res);
        }
    }
}

module.exports = new UserController();