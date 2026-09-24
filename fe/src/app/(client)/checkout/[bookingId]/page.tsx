'use client';

import { useEffect, useState, use } from 'react';
import { Card, Button, Spin, Typography, message, Result, Statistic } from 'antd';
import { CheckCircleOutlined, ClockCircleOutlined } from '@ant-design/icons';
import { clientService } from '@/services/client.service';
import { BookingResponseDto } from '@/types/client.type';
import { useBookingStore } from '@/stores/booking.store';
import dayjs from 'dayjs';
import duration from 'dayjs/plugin/duration';
import { getErrorMessage } from '@/utils/error.util';

dayjs.extend(duration);

const { Title, Text } = Typography;
const { Countdown } = Statistic;

export default function CheckoutPage({ params }: { params: Promise<{ bookingId: string }> }) {
  const resolvedParams = use(params);
  const bookingId = parseInt(resolvedParams.bookingId);

  const [loading, setLoading] = useState(true);
  const [booking, setBooking] = useState<BookingResponseDto | null>(null);
  const [isExpired, setIsExpired] = useState(false);
  const [isPaid, setIsPaid] = useState(false);
  
  const { clearBooking } = useBookingStore();

  useEffect(() => {
    if (bookingId) {
      fetchBooking();
    }
  }, [bookingId]);

  const fetchBooking = async () => {
    try {
      const data = await clientService.getBookingById(bookingId);
      setBooking(data);
      
      if (data.status === 'Canceled' || dayjs().isAfter(dayjs(data.holdExpiration))) {
        setIsExpired(true);
        clearBooking();
      } else if (data.status === 'Confirmed') {
        setIsPaid(true);
        clearBooking();
      }
    } catch (error) {
      message.error('Không tìm thấy thông tin đặt vé!');
    } finally {
      setLoading(false);
    }
  };

  const handlePayment = async () => {
    setLoading(true);
    try {
      const res = await clientService.createPaymentUrl(bookingId);
      window.location.href = res.url;
    } catch (error: any) {
      const errorMsg = getErrorMessage(error, 'Không thể tạo phiên thanh toán!');
      message.error(errorMsg);
      setLoading(false);
    }
  };

  const onExpire = () => {
    setIsExpired(true);
    clearBooking();
  };

  if (loading) return <div className="flex justify-center py-20"><Spin size="large" /></div>;
  if (!booking) return <div className="text-center py-20"><Text type="danger">Lỗi hệ thống</Text></div>;

  if (isPaid) {
    return (
      <div className="max-w-2xl mx-auto py-16 px-4">
        <Card className="shadow-lg rounded-xl text-center">
          <Result
            status="success"
            title="Thanh toán thành công!"
            subTitle={`Mã giao dịch: ${booking.id}. Vé của bạn đã được gửi qua email.`}
            extra={[
              <Button type="primary" key="home" href="/">
                Về Trang Chủ
              </Button>,
              <Button key="tickets" href="/profile">
                Xem Vé Của Tôi
              </Button>,
            ]}
          />
        </Card>
      </div>
    );
  }

  if (isExpired) {
    return (
      <div className="max-w-2xl mx-auto py-16 px-4">
        <Card className="shadow-lg rounded-xl text-center">
          <Result
            status="warning"
            title="Đã hết thời gian giữ vé"
            subTitle="Phiên giao dịch đã hết hạn. Vui lòng thực hiện đặt vé lại từ đầu."
            extra={
              <Button type="primary" href="/">
                Quay lại
              </Button>
            }
          />
        </Card>
      </div>
    );
  }

  return (
    <div className="max-w-2xl mx-auto py-10 px-4">
      <Title level={2} className="text-center mb-8">Thanh Toán</Title>
      
      <Card className="shadow-lg rounded-xl mb-6 border-t-4 border-red-500">
        <div className="text-center mb-6 p-4 bg-gray-50 rounded-lg">
          <div className="text-gray-500 mb-2">Thời gian giữ vé còn lại</div>
          <Countdown 
            value={dayjs(booking.holdExpiration).valueOf()} 
            onFinish={onExpire}
            valueStyle={{ color: '#cf1322', fontSize: '2rem', fontWeight: 'bold' }}
            prefix={<ClockCircleOutlined className="mr-2" />}
          />
        </div>

        <div className="space-y-4">
          <div className="flex justify-between pb-4 border-b">
            <Text className="text-gray-500">Mã đơn hàng</Text>
            <Text strong>#{booking.id}</Text>
          </div>
          
          <div className="flex justify-between pb-4 border-b">
            <Text className="text-gray-500">Trạng thái</Text>
            <Text className="text-yellow-600 font-semibold uppercase">{booking.status}</Text>
          </div>

          <div className="flex flex-col pb-4 border-b">
            <Text className="text-gray-500 mb-2">Thông tin vé</Text>
            <Text strong className="text-lg">{booking.movieTitle}</Text>
            <Text className="text-sm text-gray-600">{booking.cinemaName} - Phòng {booking.roomName}</Text>
            <Text className="text-sm text-gray-600">
              Suất: {dayjs(booking.showtimeStart).format('HH:mm - DD/MM/YYYY')}
            </Text>
            <div className="mt-2 flex flex-wrap gap-2">
              {booking.tickets?.map(t => (
                <div key={t.id} className="px-2 py-1 bg-gray-200 rounded font-mono text-sm font-bold">
                  {t.seatCode}
                </div>
              ))}
            </div>
          </div>

          <div className="flex justify-between items-center pt-4">
            <Text className="text-lg">Tổng thanh toán</Text>
            <Text className="text-3xl font-bold text-red-500">
              {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(booking.totalAmount)}
            </Text>
          </div>
        </div>
      </Card>

      <Button 
        type="primary" 
        danger 
        block 
        size="large" 
        className="h-14 text-lg font-bold shadow-md rounded-xl"
        onClick={handlePayment}
      >
        XÁC NHẬN THANH TOÁN
      </Button>
      
      <p className="text-center text-gray-400 text-xs mt-4">
        Bằng việc bấm xác nhận, bạn đồng ý với các Điều khoản & Thể lệ của CinemaMS.
      </p>
    </div>
  );
}
