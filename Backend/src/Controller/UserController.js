const UserService = require('../Services/UserService');

const createUser = async (req, res) => {
    try {
        const userService = new UserService();
        await userService.CreateUser(req.body);
    }
    catch {

    }

}