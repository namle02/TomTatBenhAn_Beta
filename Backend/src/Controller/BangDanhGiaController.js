const BangDanhGiaServices = require('../Services/BangDanhGiaServices');

// Tạo bảng đánh giá mới
const CreateBangDanhGia = async (req, res) => {
    try {
        const result = await BangDanhGiaServices.createBangDanhGia(req.body);
        res.status(201).json(result);
    } catch (error) {
        console.error("Lỗi CreateBangDanhGia:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// Tạo bảng đánh giá từ template phác đồ
const CreateBangDanhGiaFromPhacDo = async (req, res) => {
    try {
        const { phacDoId } = req.params;
        const thongTinDanhGia = req.body;
        
        const result = await BangDanhGiaServices.createBangDanhGiaFromPhacDo(phacDoId, thongTinDanhGia);
        res.status(201).json(result);
    } catch (error) {
        console.error("Lỗi CreateBangDanhGiaFromPhacDo:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// Lấy tất cả bảng đánh giá
const GetAllBangDanhGia = async (req, res) => {
    try {
        const bangDanhGiaList = await BangDanhGiaServices.getAllBangDanhGia();
        res.status(200).json({
            success: true,
            message: "Lấy danh sách bảng đánh giá thành công",
            data: bangDanhGiaList
        });
    } catch (error) {
        console.error("Lỗi GetAllBangDanhGia:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// Lấy bảng đánh giá theo ID
const GetBangDanhGiaById = async (req, res) => {
    try {
        const { id } = req.params;
        const bangDanhGia = await BangDanhGiaServices.getBangDanhGiaById(id);
        res.status(200).json({
            success: true,
            message: "Lấy bảng đánh giá thành công",
            data: bangDanhGia
        });
    } catch (error) {
        console.error("Lỗi GetBangDanhGiaById:", error);
        res.status(404).json({
            success: false,
            message: error.message
        });
    }
};

// Cập nhật bảng đánh giá
const UpdateBangDanhGia = async (req, res) => {
    try {
        const { id } = req.params;
        const updateData = req.body;
        
        if (!updateData || Object.keys(updateData).length === 0) {
            return res.status(400).json({
                success: false,
                message: "Thiếu dữ liệu để cập nhật"
            });
        }
        
        const result = await BangDanhGiaServices.updateBangDanhGia(id, updateData);
        res.status(200).json(result);
    } catch (error) {
        console.error("Lỗi UpdateBangDanhGia:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// Xóa bảng đánh giá
const DeleteBangDanhGia = async (req, res) => {
    try {
        const { id } = req.params;
        const result = await BangDanhGiaServices.deleteBangDanhGia(id);
        res.status(200).json(result);
    } catch (error) {
        console.error("Lỗi DeleteBangDanhGia:", error);
        res.status(404).json({
            success: false,
            message: error.message
        });
    }
};

// Tìm kiếm bảng đánh giá
const SearchBangDanhGia = async (req, res) => {
    try {
        const { search } = req.query;
        if (!search) {
            return res.status(400).json({
                success: false,
                message: "Thiếu từ khóa tìm kiếm"
            });
        }
        
        const bangDanhGiaList = await BangDanhGiaServices.searchBangDanhGia(search);
        res.status(200).json({
            success: true,
            message: "Tìm kiếm bảng đánh giá thành công",
            data: bangDanhGiaList
        });
    } catch (error) {
        console.error("Lỗi SearchBangDanhGia:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// Lấy bảng đánh giá theo phác đồ
const GetBangDanhGiaByPhacDo = async (req, res) => {
    try {
        const { phacDoId } = req.params;
        const bangDanhGiaList = await BangDanhGiaServices.getBangDanhGiaByPhacDo(phacDoId);
        res.status(200).json({
            success: true,
            message: "Lấy bảng đánh giá theo phác đồ thành công",
            data: bangDanhGiaList
        });
    } catch (error) {
        console.error("Lỗi GetBangDanhGiaByPhacDo:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// Lấy thống kê bảng đánh giá
const GetThongKeBangDanhGia = async (req, res) => {
    try {
        const thongKe = await BangDanhGiaServices.getThongKe();
        res.status(200).json({
            success: true,
            message: "Lấy thống kê bảng đánh giá thành công",
            data: thongKe
        });
    } catch (error) {
        console.error("Lỗi GetThongKeBangDanhGia:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// Cập nhật trạng thái bảng đánh giá
const UpdateTrangThaiBangDanhGia = async (req, res) => {
    try {
        const { id } = req.params;
        const { trangThai } = req.body;
        
        if (!trangThai) {
            return res.status(400).json({
                success: false,
                message: "Thiếu trạng thái để cập nhật"
            });
        }
        
        const validStates = ['draft', 'completed', 'reviewed', 'approved'];
        if (!validStates.includes(trangThai)) {
            return res.status(400).json({
                success: false,
                message: "Trạng thái không hợp lệ"
            });
        }
        
        const result = await BangDanhGiaServices.updateBangDanhGia(id, { trangThai });
        res.status(200).json(result);
    } catch (error) {
        console.error("Lỗi UpdateTrangThaiBangDanhGia:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

module.exports = {
    CreateBangDanhGia,
    CreateBangDanhGiaFromPhacDo,
    GetAllBangDanhGia,
    GetBangDanhGiaById,
    UpdateBangDanhGia,
    DeleteBangDanhGia,
    SearchBangDanhGia,
    GetBangDanhGiaByPhacDo,
    GetThongKeBangDanhGia,
    UpdateTrangThaiBangDanhGia
};
