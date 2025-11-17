require('dotenv').config();
const fs = require("fs");

class AiServices{
    async DanhGiaTuanThuPhacDo(PhacDo, BangKiem, PatientData, Prompt){
        const apiKey = process.env.GEMINI_API_KEY;
        if (!apiKey) {
            throw new Error("Thiếu biến môi trường GEMINI_API_KEY");
        }

        const promptPath = __dirname + "/../PromtDanhGiaPhacDo.txt";
        const fallbackPath = __dirname + "/../../TestPromt.txt";
        const template = fs.existsSync(promptPath)
            ? fs.readFileSync(promptPath, "utf8")
            : fs.readFileSync(fallbackPath, "utf8");

        const basePrompt = template
            .replace("{{PhacDo}}", JSON.stringify(PhacDo))
            .replace("{{BangKiem}}", JSON.stringify(BangKiem))
            .replace("{{PatientData}}", JSON.stringify(PatientData));
        const prompt = Prompt
            ? `${Prompt.trim()}\n\n${basePrompt}`
            : basePrompt;

        const url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
        const res = await fetch(url + `?key=${apiKey}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                contents: [
                    {
                        parts: [{ text: prompt }]
                    }
                ]
            })
        });

        const data = await res.json();
        const text = data?.candidates?.[0]?.content?.parts?.[0]?.text || "";

        // Kết quả AI có thể trả về theo dạng ```json ...```, bóc lấy JSON
        const jsonString = extractJson(text);
        let parsed;
        try { parsed = JSON.parse(jsonString); } catch { throw new Error("AI trả về JSON không hợp lệ"); }

        return parsed; // giả định đúng schema BangKiem model theo prompt
    }
}

function extractJson(s){
    if (!s) return "{}";
    const fence = /```json[\s\S]*?```/i;
    const m = s.match(fence);
    const raw = m ? m[0].replace(/```json|```/gi, "").trim() : s.trim();
    return raw;
}

module.exports = new AiServices();