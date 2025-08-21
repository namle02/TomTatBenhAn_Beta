const BangDanhGiaModel = require("../Repos/Model/BangDanhGiaModel");
const PhacDoModel = require("../Repos/Model/PhacDoModel");

class BangDanhGiaServices {
    // Tạo bảng đánh giá mới
    async createBangDanhGia(data) {
        try {
            const bangDanhGia = new BangDanhGiaModel(data);
            const savedBangDanhGia = await bangDanhGia.save();
            return {
                success: true,
                message: "Tạo bảng đánh giá thành công",
                data: savedBangDanhGia
            };
        } catch (error) {
            throw new Error(`Lỗi khi tạo bảng đánh giá: ${error.message}`);
        }
    }

    // Tạo bảng đánh giá từ template phác đồ
    async createBangDanhGiaFromPhacDo(phacDoId, thongTinDanhGia = {}) {
        try {
            // Lấy thông tin phác đồ
            const phacDo = await PhacDoModel.findById(phacDoId);
            if (!phacDo) {
                throw new Error("Không tìm thấy phác đồ");
            }

            // Tạo template bảng đánh giá từ phác đồ
            const templateData = this.generateTemplateFromPhacDo(phacDo);

            const bangDanhGiaData = {
                tenBangDanhGia: `Đánh giá tuân thủ - ${phacDo.protocol.name}`,
                phacDoId: phacDoId,
                nguoiDanhGia: thongTinDanhGia.nguoiDanhGia,
                thongTinBenhNhan: thongTinDanhGia.thongTinBenhNhan,
                "Mục tiêu kiểm tra": templateData,
                trangThai: 'draft'
            };

            return await this.createBangDanhGia(bangDanhGiaData);
        } catch (error) {
            throw new Error(`Lỗi khi tạo bảng đánh giá từ phác đồ: ${error.message}`);
        }
    }

    // Tạo template từ phác đồ (logic tự động mapping)
    generateTemplateFromPhacDo(phacDo) {
        const sections = phacDo.protocol.sections || [];
        
        // Khởi tạo template trống
        const template = {
            "I. Đánh giá và chẩn đoán": {
                "Nội dung kiểm tra": {
                    "Tiền sử": [],
                    "Bệnh sử": [],
                    "Khám bệnh": [],
                    "Cận lâm sàng": [],
                    "Chẩn đoán": []
                }
            },
            "II. Điều trị": {
                "Nội dung kiểm tra": {
                    "Phẫu thuật": [],
                    "Thuốc, dịch": [],
                    "Theo dõi": []
                }
            },
            "III. Chăm sóc": {
                "Nội dung kiểm tra": {
                    "Phân cấp chăm sóc": [],
                    "Thực hiện chăm sóc": []
                }
            },
            "IV. Ra viện": {
                "Nội dung kiểm tra": {
                    "Tiêu chuẩn xuất viện": [],
                    "Hướng dẫn ra viện": []
                }
            }
        };

        let counter = 1;

        // Mapping sections của phác đồ vào template
        sections.forEach(section => {
            const mappedItems = this.mapSectionToTemplate(section, counter);
            
            // Phân loại theo title của section
            const title = section.title.toLowerCase();
            
            if (title.includes('chẩn đoán')) {
                if (title.includes('giai đoạn') || title.includes('phân biệt') || title.includes('biến chứng')) {
                    template["I. Đánh giá và chẩn đoán"]["Nội dung kiểm tra"]["Chẩn đoán"].push(...mappedItems);
                } else {
                    template["I. Đánh giá và chẩn đoán"]["Nội dung kiểm tra"]["Chẩn đoán"].push(...mappedItems);
                }
            } else if (title.includes('điều trị')) {
                if (title.includes('phẫu thuật') || title.includes('thủ thuật')) {
                    template["II. Điều trị"]["Nội dung kiểm tra"]["Phẫu thuật"].push(...mappedItems);
                } else if (title.includes('thuốc') || title.includes('dịch')) {
                    template["II. Điều trị"]["Nội dung kiểm tra"]["Thuốc, dịch"].push(...mappedItems);
                } else {
                    template["II. Điều trị"]["Nội dung kiểm tra"]["Theo dõi"].push(...mappedItems);
                }
            } else if (title.includes('theo dõi') || title.includes('monitoring')) {
                template["II. Điều trị"]["Nội dung kiểm tra"]["Theo dõi"].push(...mappedItems);
            } else if (title.includes('chăm sóc')) {
                if (title.includes('phân cấp')) {
                    template["III. Chăm sóc"]["Nội dung kiểm tra"]["Phân cấp chăm sóc"].push(...mappedItems);
                } else {
                    template["III. Chăm sóc"]["Nội dung kiểm tra"]["Thực hiện chăm sóc"].push(...mappedItems);
                }
            } else if (title.includes('ra viện') || title.includes('xuất viện') || title.includes('tiêu chuẩn')) {
                if (title.includes('tiêu chuẩn')) {
                    template["IV. Ra viện"]["Nội dung kiểm tra"]["Tiêu chuẩn xuất viện"].push(...mappedItems);
                } else {
                    template["IV. Ra viện"]["Nội dung kiểm tra"]["Hướng dẫn ra viện"].push(...mappedItems);
                }
            } else {
                // Default mapping
                template["I. Đánh giá và chẩn đoán"]["Nội dung kiểm tra"]["Khám bệnh"].push(...mappedItems);
            }
            
            counter += mappedItems.length;
        });

        return template;
    }

    // Map một section của phác đồ thành tiêu chí đánh giá
    mapSectionToTemplate(section, startCounter) {
        const items = [];
        let counter = startCounter;

        // Tạo tiêu chí từ title của section
        if (section.title) {
            items.push({
                "Thứ tự tiêu chí": counter.toString(),
                "Tiêu chuẩn/Yêu cầu đạt được": `Thực hiện đúng: ${section.title}`,
                "Kết quả đánh giá": {
                    "Đạt/có": false,
                    "Không đạt/ Không có": {
                        "Checked": false,
                        "Mô tả nội dung không đạt": ""
                    },
                    "Không áp dụng": false
                }
            });
            counter++;
        }

        // Tạo tiêu chí từ content
        if (section.content && section.content.length > 0) {
            section.content.forEach(content => {
                if (content.trim()) {
                    items.push({
                        "Thứ tự tiêu chí": counter.toString(),
                        "Tiêu chuẩn/Yêu cầu đạt được": content.trim(),
                        "Kết quả đánh giá": {
                            "Đạt/có": false,
                            "Không đạt/ Không có": {
                                "Checked": false,
                                "Mô tả nội dung không đạt": ""
                            },
                            "Không áp dụng": false
                        }
                    });
                    counter++;
                }
            });
        }

        // Đệ quy cho children
        if (section.children && section.children.length > 0) {
            section.children.forEach(child => {
                const childItems = this.mapSectionToTemplate(child, counter);
                items.push(...childItems);
                counter += childItems.length;
            });
        }

        return items;
    }

    // Lấy tất cả bảng đánh giá
    async getAllBangDanhGia() {
        try {
            const bangDanhGiaList = await BangDanhGiaModel.find()
                .populate('phacDoId', 'protocol.name protocol.code')
                .sort({ createdAt: -1 });
            return bangDanhGiaList;
        } catch (error) {
            throw new Error(`Lỗi khi lấy danh sách bảng đánh giá: ${error.message}`);
        }
    }

    // Lấy bảng đánh giá theo ID
    async getBangDanhGiaById(id) {
        try {
            const bangDanhGia = await BangDanhGiaModel.findById(id)
                .populate('phacDoId');
            if (!bangDanhGia) {
                throw new Error("Không tìm thấy bảng đánh giá");
            }
            return bangDanhGia;
        } catch (error) {
            throw new Error(`Lỗi khi lấy bảng đánh giá: ${error.message}`);
        }
    }

    // Cập nhật bảng đánh giá
    async updateBangDanhGia(id, updateData) {
        try {
            const updatedBangDanhGia = await BangDanhGiaModel.findByIdAndUpdate(
                id,
                updateData,
                { new: true, runValidators: true }
            );
            if (!updatedBangDanhGia) {
                throw new Error("Không tìm thấy bảng đánh giá để cập nhật");
            }
            return {
                success: true,
                message: "Cập nhật bảng đánh giá thành công",
                data: updatedBangDanhGia
            };
        } catch (error) {
            throw new Error(`Lỗi khi cập nhật bảng đánh giá: ${error.message}`);
        }
    }

    // Xóa bảng đánh giá
    async deleteBangDanhGia(id) {
        try {
            const result = await BangDanhGiaModel.findByIdAndDelete(id);
            if (!result) {
                throw new Error("Không tìm thấy bảng đánh giá để xóa");
            }
            return {
                success: true,
                message: "Xóa bảng đánh giá thành công"
            };
        } catch (error) {
            throw new Error(`Lỗi khi xóa bảng đánh giá: ${error.message}`);
        }
    }

    // Tìm kiếm bảng đánh giá
    async searchBangDanhGia(searchTerm) {
        try {
            const bangDanhGiaList = await BangDanhGiaModel.find({
                $or: [
                    { tenBangDanhGia: { $regex: searchTerm, $options: "i" } },
                    { "thongTinBenhNhan.hoTen": { $regex: searchTerm, $options: "i" } },
                    { "thongTinBenhNhan.maBenhNhan": { $regex: searchTerm, $options: "i" } }
                ]
            }).populate('phacDoId', 'protocol.name protocol.code')
              .sort({ createdAt: -1 });
            return bangDanhGiaList;
        } catch (error) {
            throw new Error(`Lỗi khi tìm kiếm bảng đánh giá: ${error.message}`);
        }
    }

    // Lấy bảng đánh giá theo phác đồ
    async getBangDanhGiaByPhacDo(phacDoId) {
        try {
            const bangDanhGiaList = await BangDanhGiaModel.find({ phacDoId })
                .populate('phacDoId')
                .sort({ createdAt: -1 });
            return bangDanhGiaList;
        } catch (error) {
            throw new Error(`Lỗi khi lấy bảng đánh giá theo phác đồ: ${error.message}`);
        }
    }

    // Tính toán thống kê
    async getThongKe() {
        try {
            const [
                tongSoBangDanhGia,
                bangDanhGiaDraft,
                bangDanhGiaCompleted,
                bangDanhGiaApproved
            ] = await Promise.all([
                BangDanhGiaModel.countDocuments(),
                BangDanhGiaModel.countDocuments({ trangThai: 'draft' }),
                BangDanhGiaModel.countDocuments({ trangThai: 'completed' }),
                BangDanhGiaModel.countDocuments({ trangThai: 'approved' })
            ]);

            return {
                tongSoBangDanhGia,
                bangDanhGiaDraft,
                bangDanhGiaCompleted,
                bangDanhGiaApproved,
                bangDanhGiaReviewed: await BangDanhGiaModel.countDocuments({ trangThai: 'reviewed' })
            };
        } catch (error) {
            throw new Error(`Lỗi khi tính thống kê: ${error.message}`);
        }
    }
}

module.exports = new BangDanhGiaServices();
