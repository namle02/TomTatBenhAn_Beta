const express = require('express');
const router = express.Router();
const PhacDoController = require('../Controller/PhacDoController')

// Lấy tất cả phác đồ
router.get('/', PhacDoController.GetAllPhacDo);

// Tìm kiếm phác đồ theo tên
router.get('/search', PhacDoController.SearchPhacDo);

// Kiểm tra phác đồ có tồn tại hay không
router.get('/check', PhacDoController.CheckProtocolExists);

// Lấy phác đồ theo ID
router.get('/:id', PhacDoController.GetPhacDoById);

// Thêm phác đồ mới (phân tích từ text)
router.post('/add', PhacDoController.AddPhacDo);

// Cập nhật phác đồ hiện có
router.put('/:id', PhacDoController.UpdatePhacDo);

// Xóa phác đồ theo ID
router.delete('/:id', PhacDoController.DeletePhacDo);

module.exports = router;