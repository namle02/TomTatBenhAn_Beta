const mongoose = require('mongoose');
require('dotenv').config();

const dbConnect = () => mongoose.connect(process.env.MONGODB_URI || 'mongodb://localhost:27017/TomTatBenhAn', {
    useNewUrlParser: true,
    useUnifiedTopology: true
}).then(() => {
    console.log('Connected to MongoDB');
}).catch((err) => {
    console.log(err);
});

module.exports = dbConnect;