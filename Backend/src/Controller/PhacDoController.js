const PhacDoServices = require('../Services/PhacDoServices');
const ApiResponse = require('../Utils/ApiResponse');

// Thêm phác đồ
const AddPhacDo = async (req, res) => {
    try {
        // Xử lý cả text thuần và JSON
        let phacDoText;
        let force = false;

        // Kiểm tra force từ query parameter trước (ưu tiên cao hơn)
        if (req.query.force === 'true' || req.query.force === true) {
            force = true;
        }

        if (typeof req.body === 'string') {
            // Request dạng text/plain
            phacDoText = req.body;
            console.log("Nhận phác đồ dạng text/plain, force:", force);
        } else if (req.body && req.body.Rawtext) {
            // Request dạng JSON
            phacDoText = req.body.Rawtext;
            // Force từ body JSON có thể ghi đè query parameter
            if (req.body.force === true || req.body.force === 'true') {
                force = true;
            }
            console.log("Nhận phác đồ dạng JSON, force:", force);
        } else {
            return ApiResponse.badRequest("Thiếu dữ liệu phác đồ (Rawtext hoặc text/plain)").send(res);
        }

        // Sử dụng method mới có hỗ trợ force
        const result = await PhacDoServices.AddPhacDoWithForce(phacDoText, force);

        // Trả về status code khác nhau tùy theo kết quả
        if (result.success) {
            return ApiResponse.success(result.data, result.message).send(res);
        } else {
            // Phác đồ đã tồn tại và không force
            return ApiResponse.error(result.message, 409).send(res);
        }
    } catch (ex) {
        console.error("Lỗi AddPhacDo:", ex);
        return ApiResponse.error(`Lỗi khi phân tích phác đồ: ${ex.message}`, 400).send(res);
    }
}

// Lấy tất cả phác đồ
const GetAllPhacDo = async (req, res) => {
    try {
        const protocols = await PhacDoServices.getAllPhacDo();
        return ApiResponse.success(protocols, "Lấy danh sách phác đồ thành công").send(res);
    } catch (error) {
        console.error("Lỗi GetAllPhacDo:", error);
        return ApiResponse.error(error.message, 400).send(res);
    }
};

// Lấy phác đồ theo ID
const GetPhacDoById = async (req, res) => {
    try {
        const { id } = req.params;
        const protocol = await PhacDoServices.getPhacDoById(id);
        res.status(200).json({
            success: true,
            message: "Lấy phác đồ thành công",
            data: protocol
        });
    } catch (error) {
        console.error("Lỗi GetPhacDoById:", error);
        res.status(404).json({
            success: false,
            message: error.message
        });
    }
};

// Tìm kiếm phác đồ theo tên
const SearchPhacDo = async (req, res) => {
    try {
        const { search } = req.query;
        if (!search) {
            return res.status(400).json({
                success: false,
                message: "Thiếu từ khóa tìm kiếm"
            });
        }
        const protocols = await PhacDoServices.searchPhacDoByName(search);
        res.status(200).json({
            success: true,
            message: "Tìm kiếm phác đồ thành công",
            data: protocols
        });
    } catch (error) {
        console.error("Lỗi SearchPhacDo:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// Xóa phác đồ theo ID
const DeletePhacDo = async (req, res) => {
    try {
        const { id } = req.params;
        const result = await PhacDoServices.deletePhacDoById(id);
        res.status(200).json(result);
    } catch (error) {
        console.error("Lỗi DeletePhacDo:", error);
        res.status(404).json({
            success: false,
            message: error.message
        });
    }
};

// Xóa tất cả phác đồ
const DeleteAllPhacDo = async (req, res) => {
    try {
        const result = await PhacDoServices.deleteAllPhacDo();
        res.status(200).json(result);
    } catch (error) {
        console.error("Lỗi DeleteAllPhacDo:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
}

// Kiểm tra phác đồ có tồn tại hay không
const CheckProtocolExists = async (req, res) => {
    try {
        const { name, code } = req.query;
        if (!name) {
            return res.status(400).json({
                success: false,
                message: "Thiếu tham số 'name' để kiểm tra"
            });
        }

        const existingProtocol = await PhacDoServices.checkProtocolExists(name, code);

        if (existingProtocol) {
            res.status(200).json({
                success: true,
                exists: true,
                message: "Phác đồ đã tồn tại trong hệ thống",
                data: existingProtocol
            });
        } else {
            res.status(200).json({
                success: true,
                exists: false,
                message: "Phác đồ chưa tồn tại trong hệ thống"
            });
        }
    } catch (error) {
        console.error("Lỗi CheckProtocolExists:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// Cập nhật phác đồ hiện có
const UpdatePhacDo = async (req, res) => {
    try {
        const { id } = req.params;
        const updateData = req.body;

        if (!updateData || Object.keys(updateData).length === 0) {
            return res.status(400).json({
                success: false,
                message: "Thiếu dữ liệu để cập nhật"
            });
        }

        const result = await PhacDoServices.updateExistingProtocol(id, updateData);
        res.status(200).json(result);
    } catch (error) {
        console.error("Lỗi UpdatePhacDo:", error);
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

module.exports = {
    AddPhacDo,
    GetAllPhacDo,
    GetPhacDoById,
    SearchPhacDo,
    DeletePhacDo,
    CheckProtocolExists,
    UpdatePhacDo,
    DeleteAllPhacDo
}