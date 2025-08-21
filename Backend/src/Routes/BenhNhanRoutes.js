const express = require('express');
const router = express.Router();
const BenhNhanController = require('../Controller/BenhNhanController');

// Route lưu thông tin bệnh nhân
router.post('/save', BenhNhanController.saveBenhNhan);

// Route tìm kiếm bệnh nhân theo số bệnh án
router.get('/soBenhAn/:soBenhAn', BenhNhanController.getBenhNhanBySoBenhAn);

// Route lấy danh sách tất cả bệnh nhân (có phân trang)
router.get('/all', BenhNhanController.getAllBenhNhan);

// Route tìm kiếm bệnh nhân theo tên
router.get('/search', BenhNhanController.getBenhNhanByName);

// Route xóa bệnh nhân theo ID
router.delete('/:id', BenhNhanController.deleteBenhNhan);

module.exports = router;
