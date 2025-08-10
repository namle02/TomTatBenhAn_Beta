const User = require('../Repos/Model/UserModel');


class UserServices{
    async CreateUser(user){
        console.log(user);
    }
   
}

module.exports = new UserServices();