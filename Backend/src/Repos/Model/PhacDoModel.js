const mongoose = require('mongoose');

const SectionSchema = new mongoose.Schema(
  {
    id: { type: String, required: true, trim: true },
    title: { type: String, required: true, trim: true },
    content: {
      type: [String],
      default: [],
    },
  },
  { _id: false }
);

// Đệ quy: children là mảng các Section
SectionSchema.add({ children: [SectionSchema] });

const ProtocolInnerSchema = new mongoose.Schema(
  {
    name: { type: String, required: true, trim: true },
    code: { type: String, default: null, trim: true },
    source: { type: String, default: null, trim: true },
    sections: { type: [SectionSchema], default: [] },
    raw: { type: String, default: null},
  },
  { _id: false }
);

const ProtocolSchema = new mongoose.Schema(
  {
    protocol: { type: ProtocolInnerSchema, required: true },
  },
  { timestamps: true }
);

// Index gợi ý: tìm nhanh theo tên + mã
ProtocolSchema.index({ 'protocol.name': 1, 'protocol.code': 1 }, { unique: false });

module.exports = mongoose.model('PhacDoModel', ProtocolSchema);
