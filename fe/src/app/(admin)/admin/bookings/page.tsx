'use client';

import { useEffect, useState } from 'react';
import { Table, Tag, Input, Space, message, Typography } from 'antd';
import { adminService } from '@/services/admin.service';
import { BookingAdminDto } from '@/types/admin.type';
import dayjs from 'dayjs';

const { Text } = Typography;

export default function AdminBookingsPage() {
  const [bookings, setBookings] = useState<BookingAdminDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [pagination, setPagination] = useState({ current: 1, pageSize: 10, total: 0 });
  const [keyword, setKeyword] = useState<string>('');

  const fetchBookings = async (page = pagination.current, pageSize = pagination.pageSize, kw = keyword) => {
    setLoading(true);
    try {
      const data = await adminService.getBookings({ pageIndex: page, pageSize, keyword: kw || undefined });
      setBookings(data.items);
      setPagination({
        current: data.pageIndex,
        pageSize: data.pageSize,
        total: data.totalItems
      });
    } catch (error) {
      message.error('Lỗi khi tải danh sách đặt vé');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchBookings(1, pagination.pageSize, keyword);
  }, [keyword]);

  const handleTableChange = (newPagination: any) => {
    fetchBookings(newPagination.current, newPagination.pageSize, keyword);
  };

  const onSearch = (value: string) => {
    setKeyword(value);
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Confirmed': return 'green';
      case 'Holding': return 'orange';
      case 'PendingPayment': return 'blue';
      case 'Canceled': return 'red';
      default: return 'default';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'Confirmed': return 'Đã thanh toán';
      case 'Holding': return 'Đang giữ ghế';
      case 'PendingPayment': return 'Chờ thanh toán';
      case 'Canceled': return 'Đã hủy';
      default: return status;
    }
  };

  const columns = [
    { 
      title: 'Mã Đặt vé', 
      dataIndex: 'id', 
      key: 'id',
      render: (id: number) => <Text strong>#MDV-{id}</Text>
    },
    { 
      title: 'Khách hàng', 
      key: 'user', 
      render: (_: any, record: BookingAdminDto) => (
        <div>
          <div>{record.userName}</div>
          <Text type="secondary" className="text-xs">{record.userEmail}</Text>
        </div>
      )
    },
    { 
      title: 'Phim / Lịch chiếu', 
      key: 'movie', 
      render: (_: any, record: BookingAdminDto) => (
        <div>
          <div className="font-medium text-blue-600">{record.movieTitle}</div>
          <Text type="secondary" className="text-xs">
            {record.showtimeStart ? dayjs(record.showtimeStart).format('HH:mm DD/MM/YYYY') : ''} 
          </Text>
        </div>
      )
    },
    { 
      title: 'Rạp / Phòng', 
      key: 'cinema', 
      render: (_: any, record: BookingAdminDto) => (
        <div>
          <div>{record.cinemaName}</div>
          <Text type="secondary" className="text-xs">{record.roomName}</Text>
        </div>
      )
    },
    { 
      title: 'Số vé / Mã ghế', 
      key: 'ticketCount',
      render: (_: any, record: BookingAdminDto) => (
        <div className="text-center">
          <div className="font-bold">{record.ticketCount} vé</div>
          {record.seatCodes && (
            <Text type="secondary" className="text-xs break-words block max-w-[120px]">
              {record.seatCodes}
            </Text>
          )}
        </div>
      )
    },
    { 
      title: 'Tổng tiền', 
      dataIndex: 'totalAmount', 
      key: 'totalAmount',
      render: (amount: number) => <Text strong className="text-red-500">{new Intl.NumberFormat('vi-VN').format(amount)}đ</Text>
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
      render: (status: string) => (
        <Tag color={getStatusColor(status)}>{getStatusText(status)}</Tag>
      ),
    },
    { 
      title: 'Ngày đặt', 
      dataIndex: 'createdAt', 
      key: 'createdAt',
      render: (date: string) => dayjs(date).format('HH:mm DD/MM/YYYY')
    }
  ];

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold">Lịch sử Đặt vé</h2>
        <Space>
          <Input.Search 
            placeholder="Tìm theo mã MDV, tên, email khách hàng..." 
            allowClear 
            onSearch={onSearch} 
            style={{ width: 350 }}
          />
        </Space>
      </div>

      <Table
        columns={columns}
        dataSource={bookings}
        rowKey="id"
        loading={loading}
        pagination={pagination}
        onChange={handleTableChange}
      />
    </div>
  );
}
