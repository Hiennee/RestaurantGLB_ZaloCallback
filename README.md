## Hướng dẫn cài đặt Callback Endpoint cho RestaurantGLB
**B1: Clone repository**

**B2: Config file ZaloPay.ini**
- Địa chỉ file: RestaurantGLB/Build/ZaloPay.ini
- Thêm field *callback_url = [URL]*
- *Gợi ý sử dụng ngrok để mapping global endpoint đến localhost để testing*

**B3: Chạy RestaurantGLB**
- Sau khi quét mã QR ZaloPay, đợi 5 giây - nhấn 'Kiểm tra thanh toán'
