const express = require('express');
const router = express.Router();
const BangDanhGiaController = require('../Controller/BangDanhGiaController');

// CRUD & Check exists
router.get('/', (req, res) => BangDanhGiaController.getAll(req, res));
router.get('/check', (req, res) => BangDanhGiaController.checkExists(req, res));
router.get('/:id', (req, res) => BangDanhGiaController.getById(req, res));
router.post('/', (req, res) => BangDanhGiaController.create(req, res));
router.put('/:id', (req, res) => BangDanhGiaController.update(req, res));
router.delete('/:id', (req, res) => BangDanhGiaController.remove(req, res));

module.exports = router;
