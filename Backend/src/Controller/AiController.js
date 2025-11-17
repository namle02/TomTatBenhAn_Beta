const AiServices = require('../Services/AiServices');
const ApiResponse = require('../Utils/ApiResponse');

class AiController{
    async DanhGiaTuanThuPhacDo(req, res){
        try {
            const body = req.body || {};
            const PhacDo = body.PhacDo || body.phacdo || body.phacDo || body.protocol || null;
            const BangKiem = body.BangKiem || body.bangkiem || body.bangKiem || body.checklist || null;
            const PatientData = body.PatientData || body.patient || body.patientData || null;
            const Prompt = body.Prompt || body.prompt || null;
            if (!PhacDo || !BangKiem || !PatientData){
                return ApiResponse.badRequest("Thiếu dữ liệu PhacDo/BangKiem/PatientData").send(res);
            }

            const result = await AiServices.DanhGiaTuanThuPhacDo(PhacDo, BangKiem, PatientData, Prompt);
            return ApiResponse.success(result, "Đánh giá tuân thủ phác đồ thành công").send(res);
        } catch (error) {
            return ApiResponse.error(error.message, 400).send(res);
        }
    }
}

module.exports = new AiController();