const express = require('express');
const router = express.Router();
const AiController = require('../Controller/AiController');

router.post('/DanhGiaTuanThuPhacDo', AiController.DanhGiaTuanThuPhacDo);

module.exports = router;