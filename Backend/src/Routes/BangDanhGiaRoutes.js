const express = require('express');
const router = express.Router();
const BangDanhGiaController = require('../Controller/BangDanhGiaController');

// Lấy tất cả bảng đánh giá
router.get('/', BangDanhGiaController.GetAllBangDanhGia);

// Tìm kiếm bảng đánh giá
router.get('/search', BangDanhGiaController.SearchBangDanhGia);

// Lấy thống kê bảng đánh giá
router.get('/thongke', BangDanhGiaController.GetThongKeBangDanhGia);

// Lấy bảng đánh giá theo phác đồ
router.get('/phacdo/:phacDoId', BangDanhGiaController.GetBangDanhGiaByPhacDo);

// Lấy bảng đánh giá theo ID
router.get('/:id', BangDanhGiaController.GetBangDanhGiaById);

// Tạo bảng đánh giá mới
router.post('/', BangDanhGiaController.CreateBangDanhGia);

// Tạo bảng đánh giá từ template phác đồ
router.post('/from-phacdo/:phacDoId', BangDanhGiaController.CreateBangDanhGiaFromPhacDo);

// Cập nhật bảng đánh giá
router.put('/:id', BangDanhGiaController.UpdateBangDanhGia);

// Cập nhật trạng thái bảng đánh giá
router.patch('/:id/trangthai', BangDanhGiaController.UpdateTrangThaiBangDanhGia);

// Xóa bảng đánh giá
router.delete('/:id', BangDanhGiaController.DeleteBangDanhGia);

module.exports = router;
