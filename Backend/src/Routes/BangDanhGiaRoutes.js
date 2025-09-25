const express = require('express');
const router = express.Router();
const BangDanhGiaController = require('../Controller/BangDanhGiaController');
const multer = require('multer');

// Cấu hình multer để upload file Word
const storage = multer.diskStorage({
    destination: function (req, file, cb) {
        // Tạo thư mục uploads nếu chưa có (dùng đường dẫn tuyệt đối để tránh phụ thuộc CWD)
        const path = require('path');
        const fs = require('fs');
        const uploadDir = path.resolve(process.cwd(), 'uploads', 'bang-kiem');
        if (!fs.existsSync(uploadDir)) {
            fs.mkdirSync(uploadDir, { recursive: true });
        }
        cb(null, uploadDir);
    },
    filename: function (req, file, cb) {
        // Tạo tên file unique với timestamp
        const uniqueSuffix = Date.now() + '-' + Math.round(Math.random() * 1E9);
        const fileExt = require('path').extname(file.originalname);
        cb(null, `bang-kiem-${uniqueSuffix}${fileExt}`);
    }
});

const upload = multer({ 
    storage: storage,
    limits: {
        fileSize: 10 * 1024 * 1024 // Giới hạn 10MB
    },
    fileFilter: function (req, file, cb) {
        // Chỉ chấp nhận file Word
        const allowedTypes = ['.docx', '.doc'];
        const fileExt = require('path').extname(file.originalname).toLowerCase();
        if (allowedTypes.includes(fileExt)) {
            cb(null, true);
        } else {
            cb(new Error('Chỉ chấp nhận file Word (.docx, .doc)'), false);
        }
    }
});

// Chỉ giữ lại 3 endpoint chính theo yêu cầu
router.get('/', (req, res) => BangDanhGiaController.getAll(req, res));
router.post('/', upload.single('wordFile'), (req, res) => BangDanhGiaController.create(req, res));
router.delete('/:id', (req, res) => BangDanhGiaController.remove(req, res));

// Endpoint để download file Word gốc
router.get('/:id/download', (req, res) => BangDanhGiaController.downloadOriginalFile(req, res));

module.exports = router;
