const mongoose = require('mongoose');

const userSchema = new mongoose.Schema({
    TenNhanVien: {
        type: String,
        required: true
    },
    MaNhanVien:{
        type: String,
        required: true
    },
    PhongBan:{
        type: String,
        required: true
    },
    SoLuotSuDung:{
        type: Number,
        required: true
    }
})

module.exports = mongoose.model('User', userSchema);