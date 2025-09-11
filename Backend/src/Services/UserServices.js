const User = require('../Repos/Model/UserModel');

class UserServices{
    async CreateUser(user){
        try {
            console.log('Creating user:', user);
            
            // Validate dữ liệu cơ bản
            if (!user.username || !user.email) {
                return {
                    success: false,
                    message: 'Username và email là bắt buộc',
                    data: null
                };
            }

            // Kiểm tra user đã tồn tại chưa
            const existingUser = await User.findOne({ 
                $or: [
                    { username: user.username },
                    { email: user.email }
                ]
            });

            if (existingUser) {
                return {
                    success: false,
                    message: 'Username hoặc email đã tồn tại',
                    data: null
                };
            }

            // Tạo user mới
            const newUser = new User(user);
            await newUser.save();

            return {
                success: true,
                message: 'Tạo user thành công',
                data: newUser
            };
        } catch (error) {
            console.error('Error in CreateUser:', error);
            return {
                success: false,
                message: `Lỗi khi tạo user: ${error.message}`,
                data: null
            };
        }
    }
   
}

module.exports = new UserServices();