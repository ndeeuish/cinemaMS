'use client';

import { useEffect, useState } from 'react';
import { Card, Table, Tag, Typography, message, Space, Button } from 'antd';
import { clientService } from '@/services/client.service';
import { BookingResponseDto } from '@/types/client.type';
import { useAuthStore } from '@/stores/auth.store';
import { useRouter } from 'next/navigation';
import dayjs from 'dayjs';
import { getErrorMessage } from '@/utils/error.util';

const { Title } = Typography;

export default function ProfilePage() {
  const [bookings, setBookings] = useState<BookingResponseDto[]>([]);
  const [loading, setLoading] = useState(true);
  const { isAuthenticated, user } = useAuthStore();
  const router = useRouter();

  useEffect(() => {
    if (!isAuthenticated) {
      router.push('/login');
      return;
    }
    fetchMyBookings();
  }, [isAuthenticated, router]);

  const fetchMyBookings = async () => {
    try {
      setLoading(true);
      const data = await clientService.getMyBookings();
      // Sort by newest first
      data.sort((a, b) => b.id - a.id);
      setBookings(data);
    } catch (error) {
      message.error(getErrorMessage(error, 'Không thể tải lịch sử đặt vé'));
    } finally {
      setLoading(false);
    }
  };

  const columns = [
    {
      title: 'Mã vé',
      dataIndex: 'id',
      key: 'id',
      render: (id: number) => <span className="font-bold">#{id}</span>
    },
    {
      title: 'Phim',
      dataIndex: 'movieTitle',
      key: 'movieTitle',
      render: (text: string) => <span className="font-semibold text-gray-800">{text}</span>
    },
    {
      title: 'Rạp & Phòng',
      key: 'cinema',
      render: (_: any, record: BookingResponseDto) => (
        <span>{record.cinemaName} - Phòng {record.roomName}</span>
      )
    },
    {
      title: 'Suất chiếu',
      dataIndex: 'showtimeStart',
      key: 'showtimeStart',
      render: (date: string) => <span className="text-blue-600">{dayjs(date).format('HH:mm - DD/MM/YYYY')}</span>
    },
    {
      title: 'Ghế',
      key: 'seats',
      render: (_: any, record: BookingResponseDto) => (
        <Space size={[0, 4]} wrap>
          {record.tickets?.map(t => (
            <Tag key={t.id} className="font-mono m-0">{t.seatCode}</Tag>
          ))}
        </Space>
      )
    },
    {
      title: 'Tổng tiền',
      dataIndex: 'totalAmount',
      key: 'totalAmount',
      render: (amount: number) => (
        <span className="font-bold text-red-500">
          {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount)}
        </span>
      )
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
      render: (status: string) => {
        let color = 'default';
        let text = status;
        if (status === 'Confirmed') {
          color = 'success';
          text = 'Đã thanh toán';
        } else if (status === 'Canceled') {
          color = 'error';
          text = 'Đã huỷ';
        } else if (status === 'Holding') {
          color = 'warning';
          text = 'Chờ thanh toán';
        }
        return <Tag color={color}>{text}</Tag>;
      }
    },
    {
      title: 'Hành động',
      key: 'action',
      render: (_: any, record: BookingResponseDto) => {
        if (record.status === 'Holding') {
          return (
            <Button type="primary" size="small" onClick={() => router.push(`/checkout/${record.id}`)}>
              Thanh toán ngay
            </Button>
          );
        }
        return null;
      }
    }
  ];

  return (
    <div className="bg-gray-50 min-h-screen py-10 px-4">
      <div className="max-w-6xl mx-auto">
        <Title level={2} className="mb-8">Hồ sơ cá nhân</Title>
        
        <Card className="mb-6 shadow-sm rounded-xl">
          <Title level={4}>Thông tin tài khoản</Title>
          <div className="mt-4">
            <p><strong>Họ tên:</strong> {user?.fullName}</p>
            <p><strong>Email:</strong> {user?.email}</p>
            <p><strong>Vai trò:</strong> {user?.role}</p>
          </div>
        </Card>

        <Card className="shadow-sm rounded-xl">
          <Title level={4} className="mb-6">Lịch sử đặt vé</Title>
          <Table 
            columns={columns} 
            dataSource={bookings} 
            rowKey="id" 
            loading={loading}
            pagination={{ pageSize: 10 }}
          />
        </Card>
      </div>
    </div>
  );
}
