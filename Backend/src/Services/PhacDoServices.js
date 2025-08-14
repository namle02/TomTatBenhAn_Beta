require('dotenv').config();
const PhacDoModel = require("../Repos/Model/PhacDoModel");


class PhacDoServices {
    async AddPhacDo(phacdo) {
        const apiKey = process.env.GEMINI_API_KEY;
        if (!apiKey) {
            throw new Error("Thiếu biến môi trường GEMINI_API_KEY");
        }

        const prompt = `Bạn là AI NLP y khoa kiêm backend. Hãy phân tích văn bản phác đồ điều trị (tiếng Việt) và CHỈ TRẢ VỀ MỘT JSON HỢP LỆ, KHÔNG THÊM BẤT KỲ VĂN BẢN NÀO KHÁC (không markdown, không code fence, không giải thích).\n\nYÊU CẦU:\n1) Cấu trúc đầu ra:\n{\n  \"protocol\": {\n    \"name\": string,\n    \"code\": string|null,\n    \"source\": string|null,\n    \"sections\": Section[]\n  }\n}\nSection = {\n  \"id\": string,\n  \"title\": string,\n  \"content\": string[],\n  \"children\": Section[]\n}\n\n2) Quy tắc bóc tách:\n- Giữ nguyên phân cấp và thứ tự mục như bản gốc; không được bỏ sót nội dung.\n- Không trộn khóa: chỉ dùng \"children\" (không dùng \"subItems\"/\"nodes\"/\"items\").\n- Chuẩn hóa khoảng trắng; giữ nguyên dấu tiếng Việt; KHÔNG tự ý sửa nội dung y khoa.\n- Nếu một mục chỉ có các mục con, đặt \"content\": [].\n- Nếu có phần \"Tài liệu tham khảo\", bỏ khỏi sections và đưa vào \"protocol.source\" (gộp thành một chuỗi, phân tách bằng \" | \").\n\n3) Đầu ra phải là JSON hợp lệ duy nhất (parse được ngay bằng JSON.parse).\n\nDỮ LIỆU VÀO:\n${phacdo}`;

        const url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        const res = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "X-goog-api-key": apiKey
            },
            body: JSON.stringify({
                contents: [
                    {
                        parts: [
                            { text: prompt }
                        ]
                    }
                ]
            })
        });

        if (!res.ok) {
            const errorText = await res.text().catch(() => "");
            throw new Error(`Gemini API lỗi ${res.status}: ${errorText}`);
        }

        const data = await res.json();
        const result = data.candidates[0].content.parts[0].text;
        const jsonResult = result.replace(/```json/g, '').replace(/```/g, '');
        const parsedResult = JSON.parse(jsonResult);

        // Kiểm tra phác đồ đã tồn tại chưa
        try {
            const existingProtocol = await this.checkProtocolExists(
                parsedResult.protocol.name,
                parsedResult.protocol.code
            );

            if (existingProtocol) {
                return {
                    success: false,
                    message: "Phác đồ đã tồn tại trong hệ thống",
                    data: parsedResult,
                    existingId: existingProtocol._id,
                    existingProtocol: existingProtocol
                };
            }

            // Lưu vào MongoDB nếu chưa tồn tại
            const savedPhacDo = await PhacDoModel.create(parsedResult);
            console.log("Đã lưu phác đồ vào MongoDB:", savedPhacDo._id);
            return {
                success: true,
                message: "Phân tích và lưu phác đồ thành công",
                data: parsedResult,
                mongoId: savedPhacDo._id
            };
        } catch (mongoError) {
            console.error("Lỗi lưu MongoDB:", mongoError);
            // Vẫn trả về kết quả phân tích nếu lưu DB thất bại
            return {
                success: true,
                message: "Phân tích thành công, nhưng có lỗi khi lưu vào database",
                data: parsedResult,
                error: mongoError.message
            };
        }
    }

    // Thêm phác đồ với tùy chọn ghi đè
    async AddPhacDoWithForce(phacdo, forceOverwrite = false) {
        const apiKey = process.env.GEMINI_API_KEY;
        if (!apiKey) {
            throw new Error("Thiếu biến môi trường GEMINI_API_KEY");
        }

        const prompt = `Bạn là AI NLP y khoa kiêm backend. Hãy phân tích văn bản phác đồ điều trị (tiếng Việt) và CHỈ TRẢ VỀ MỘT JSON HỢP LỆ, KHÔNG THÊM BẤT KỲ VĂN BẢN NÀO KHÁC (không markdown, không code fence, không giải thích).\n\nYÊU CẦU:\n1) Cấu trúc đầu ra:\n{\n  \"protocol\": {\n    \"name\": string,\n    \"code\": string|null,\n    \"source\": string|null,\n    \"sections\": Section[]\n  }\n}\nSection = {\n  \"id\": string,\n  \"title\": string,\n  \"content\": string[],\n  \"children\": Section[]\n}\n\n2) Quy tắc bóc tách:\n- Giữ nguyên phân cấp và thứ tự mục như bản gốc; không được bỏ sót nội dung.\n- Không trộn khóa: chỉ dùng \"children\" (không dùng \"subItems\"/\"nodes\"/\"items\").\n- Chuẩn hóa khoảng trắng; giữ nguyên dấu tiếng Việt; KHÔNG tự ý sửa nội dung y khoa.\n- Nếu một mục chỉ có các mục con, đặt \"content\": [].\n- Nếu có phần \"Tài liệu tham khảo\", bỏ khỏi sections và đưa vào \"protocol.source\" (gộp thành một chuỗi, phân tách bằng \" | \").\n\n3) Đầu ra phải là JSON hợp lệ duy nhất (parse được ngay bằng JSON.parse).\n\nDỮ LIỆU VÀO:\n${phacdo}`;
    
        const url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        const res = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "X-goog-api-key": apiKey
            },
            body: JSON.stringify({
                contents: [
                    {
                        parts: [
                            { text: prompt }
                        ]
                    }
                ]
            })
        });

        if (!res.ok) {
            const errorText = await res.text().catch(() => "");
            throw new Error(`Gemini API lỗi ${res.status}: ${errorText}`);
        }

        const data = await res.json();
        const result = data.candidates[0].content.parts[0].text;
        const jsonResult = result.replace(/```json/g, '').replace(/```/g, '');
        const parsedResult = JSON.parse(jsonResult);

        // Kiểm tra phác đồ đã tồn tại chưa
        try {
            const existingProtocol = await this.checkProtocolExists(
                parsedResult.protocol.name,
                parsedResult.protocol.code
            );

            if (existingProtocol && !forceOverwrite) {
                return {
                    success: false,
                    message: "Phác đồ đã tồn tại trong hệ thống. Sử dụng force=true để ghi đè.",
                    data: parsedResult,
                    existingId: existingProtocol._id,
                    existingProtocol: existingProtocol
                };
            } else if (existingProtocol && forceOverwrite) {
                // Cập nhật phác đồ hiện có
                const updateResult = await this.updateExistingProtocol(existingProtocol._id, parsedResult);
                return {
                    success: true,
                    message: "Phân tích và cập nhật phác đồ thành công",
                    data: updateResult.data,
                    mongoId: existingProtocol._id,
                    action: "updated"
                };
            }

            // Lưu vào MongoDB nếu chưa tồn tại
            const savedPhacDo = await PhacDoModel.create(parsedResult);
            console.log("Đã lưu phác đồ vào MongoDB:", savedPhacDo._id);
            return {
                success: true,
                message: "Phân tích và lưu phác đồ thành công",
                data: parsedResult,
                mongoId: savedPhacDo._id,
                action: "created"
            };
        } catch (mongoError) {
            console.error("Lỗi lưu MongoDB:", mongoError);
            return {
                success: true,
                message: "Phân tích thành công, nhưng có lỗi khi lưu vào database",
                data: parsedResult,
                error: mongoError.message
            };
        }
    }

    // Kiểm tra phác đồ đã tồn tại dựa trên tên và mã code
    async checkProtocolExists(name, code) {
        try {
            // Tìm kiếm chính xác theo tên (không phân biệt hoa thường)
            const query = { "protocol.name": { $regex: `^${name.trim()}$`, $options: "i" } };

            // Nếu có mã code, thêm điều kiện tìm kiếm theo code
            if (code && code.trim()) {
                query["protocol.code"] = { $regex: `^${code.trim()}$`, $options: "i" };
            }

            const existingProtocol = await PhacDoModel.findOne(query);
            return existingProtocol;
        } catch (error) {
            console.error("Lỗi khi kiểm tra phác đồ tồn tại:", error);
            return null;
        }
    }

    // Kiểm tra phác đồ có tồn tại hay không (chỉ trả về true/false)
    async isProtocolExists(name, code) {
        const existing = await this.checkProtocolExists(name, code);
        return !!existing;
    }

    // Lấy phác đồ theo tên và code chính xác
    async getProtocolByNameAndCode(name, code) {
        try {
            const query = { "protocol.name": { $regex: `^${name.trim()}$`, $options: "i" } };
            if (code && code.trim()) {
                query["protocol.code"] = { $regex: `^${code.trim()}$`, $options: "i" };
            }
            return await PhacDoModel.findOne(query);
        } catch (error) {
            throw new Error(`Lỗi khi tìm phác đồ: ${error.message}`);
        }
    }

    // Cập nhật phác đồ hiện có thay vì tạo mới
    async updateExistingProtocol(id, newData) {
        try {
            const updatedProtocol = await PhacDoModel.findByIdAndUpdate(
                id,
                newData,
                { new: true, runValidators: true }
            );
            if (!updatedProtocol) {
                throw new Error("Không tìm thấy phác đồ để cập nhật");
            }
            return {
                success: true,
                message: "Cập nhật phác đồ thành công",
                data: updatedProtocol
            };
        } catch (error) {
            throw new Error(`Lỗi khi cập nhật phác đồ: ${error.message}`);
        }
    }

    // Lấy tất cả phác đồ
    async getAllPhacDo() {
        try {
            const protocols = await PhacDoModel.find().sort({ createdAt: -1 });
            return protocols;
        } catch (error) {
            throw new Error(`Lỗi khi lấy danh sách phác đồ: ${error.message}`);
        }
    }

    // Lấy phác đồ theo ID
    async getPhacDoById(id) {
        try {
            const protocol = await PhacDoModel.findById(id);
            if (!protocol) {
                throw new Error("Không tìm thấy phác đồ");
            }
            return protocol;
        } catch (error) {
            throw new Error(`Lỗi khi lấy phác đồ: ${error.message}`);
        }
    }

    // Tìm kiếm phác đồ theo tên
    async searchPhacDoByName(searchTerm) {
        try {
            const protocols = await PhacDoModel.find({
                "protocol.name": { $regex: searchTerm, $options: "i" }
            }).sort({ createdAt: -1 });
            return protocols;
        } catch (error) {
            throw new Error(`Lỗi khi tìm kiếm phác đồ: ${error.message}`);
        }
    }

    // Xóa phác đồ
    async deletePhacDoById(id) {
        try {
            const result = await PhacDoModel.findByIdAndDelete(id);
            if (!result) {
                throw new Error("Không tìm thấy phác đồ để xóa");
            }
            return { success: true, message: "Xóa phác đồ thành công" };
        } catch (error) {
            throw new Error(`Lỗi khi xóa phác đồ: ${error.message}`);
        }
    }
}
module.exports = new PhacDoServices();