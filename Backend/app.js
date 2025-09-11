const express = require('express');
const app = express();
const userRoutes = require('./src/Routes/UserRoutes');
const phacDoRoutes = require('./src/Routes/PhacDoRoutes');
const bangDanhGiaRoutes = require('./src/Routes/BangDanhGiaRoutes');
const benhNhanRoutes = require('./src/Routes/BenhNhanRoutes');
const dbConnect = require('./src/Repos/Database');
const cors = require('cors');
const bodyParser = require('body-parser');
const healthRoutes = require('./src/Routes/healthRoutes');

dbConnect();

app.use(cors({
    origin: 'http://localhost:3000',
    credentials: true
}));

// Xử lý raw text và JSON với limit lớn hơn
app.use(express.text({ limit: '50mb', type: 'text/plain' }));
app.use(express.json({ 
    limit: '50mb',
    strict: false  // Cho phép JSON không nghiêm ngặt
}));
app.use(bodyParser.json({ limit: '50mb' }));
app.use(bodyParser.text({ limit: '50mb' }));

app.use('/user', userRoutes);
app.use('/phacdo', phacDoRoutes);
app.use('/bangdanhgia', bangDanhGiaRoutes);
app.use('/benhnhan', benhNhanRoutes);
app.use('/health', healthRoutes);



app.listen(3000, () => {
    console.log('Server is running on port 3000');
})