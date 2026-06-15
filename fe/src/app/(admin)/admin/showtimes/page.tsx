'use client';

import { useEffect, useState } from 'react';
import { Table, Button, Modal, Form, Select, DatePicker, InputNumber, Space, Popconfirm, message, Input } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { adminService } from '@/services/admin.service';
import { ShowtimeDto, CinemaDto, MovieDto, RoomDto } from '@/types/admin.type';
import dayjs from 'dayjs';

const { Option } = Select;

export default function AdminShowtimesPage() {
  const [cinemas, setCinemas] = useState<CinemaDto[]>([]);
  const [selectedCinema, setSelectedCinema] = useState<number | null>(null);
  const [showtimes, setShowtimes] = useState<ShowtimeDto[]>([]);
  const [movies, setMovies] = useState<MovieDto[]>([]);
  const [rooms, setRooms] = useState<RoomDto[]>([]);
  
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingShowtime, setEditingShowtime] = useState<ShowtimeDto | null>(null);
  
  const [form] = Form.useForm();

  const fetchInitialData = async () => {
    try {
      const [cinemasData, moviesData] = await Promise.all([
        adminService.getCinemas({ pageIndex: 1, pageSize: 100 }),
        adminService.getMovies({ pageIndex: 1, pageSize: 100 }),
      ]);
      setCinemas(cinemasData.items);
      setMovies(moviesData.items);
      if (cinemasData.items && cinemasData.items.length > 0) {
        setSelectedCinema(cinemasData.items[0].id);
      }
    } catch (error) {
      message.error('Lỗi khi tải dữ liệu khởi tạo');
    }
  };

  useEffect(() => {
    fetchInitialData();
  }, []);

  const [pagination, setPagination] = useState({ current: 1, pageSize: 10, total: 0 });
  const [keyword, setKeyword] = useState<string>('');

  const fetchShowtimesAndRooms = async (cinemaId: number, page = pagination.current, pageSize = pagination.pageSize, kw = keyword) => {
    setLoading(true);
    try {
      const [showtimesData, roomsData] = await Promise.all([
        adminService.getShowtimesByCinema(cinemaId, { pageIndex: page, pageSize, keyword: kw || undefined }),
        adminService.getRoomsByCinema(cinemaId, { pageIndex: 1, pageSize: 100 }), // Load all rooms for dropdown
      ]);
      setShowtimes(showtimesData.items);
      setRooms(roomsData.items);
      setPagination({
        current: showtimesData.pageIndex,
        pageSize: showtimesData.pageSize,
        total: showtimesData.totalItems
      });
    } catch (error) {
      message.error('Lỗi khi tải lịch chiếu của rạp này');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (selectedCinema) {
      fetchShowtimesAndRooms(selectedCinema, 1, pagination.pageSize, keyword);
    }
  }, [selectedCinema, keyword]);

  const handleTableChange = (newPagination: any) => {
    if (selectedCinema) {
      fetchShowtimesAndRooms(selectedCinema, newPagination.current, newPagination.pageSize, keyword);
    }
  };

  const onSearch = (value: string) => {
    setKeyword(value);
  };

  const showModal = (showtime?: ShowtimeDto) => {
    if (showtime) {
      setEditingShowtime(showtime);
      form.setFieldsValue({
        ...showtime,
        startTime: dayjs(showtime.startTime),
      });
    } else {
      setEditingShowtime(null);
      form.resetFields();
    }
    setIsModalVisible(true);
  };

  const handleFinish = async (values: any) => {
    try {
      const payload = {
        ...values,
        startTime: values.startTime.format('YYYY-MM-DDTHH:mm:ss'),
      };

      if (editingShowtime) {
        await adminService.updateShowtime(editingShowtime.id, payload);
        message.success('Cập nhật lịch chiếu thành công');
      } else {
        await adminService.createShowtime(payload);
        message.success('Thêm lịch chiếu thành công');
      }
      setIsModalVisible(false);
      if (selectedCinema) fetchShowtimesAndRooms(selectedCinema);
    } catch (error: any) {
      message.error(error.response?.data?.details || 'Có lỗi xảy ra');
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await adminService.deleteShowtime(id);
      message.success('Xóa lịch chiếu thành công');
      if (selectedCinema) fetchShowtimesAndRooms(selectedCinema);
    } catch (error: any) {
      message.error(error.response?.data?.details || 'Không thể xóa lịch chiếu này');
    }
  };

  const columns = [
    { 
      title: 'Phim', 
      key: 'movie', 
      render: (_: any, record: ShowtimeDto) => {
        const movie = movies.find(m => m.id === record.movieId);
        return movie?.title || `Phim ID: ${record.movieId}`;
      }
    },
    { 
      title: 'Phòng chiếu', 
      key: 'room', 
      render: (_: any, record: ShowtimeDto) => {
        const room = rooms.find(r => r.id === record.roomId);
        return room?.name || `Phòng ID: ${record.roomId}`;
      }
    },
    { 
      title: 'Bắt đầu', 
      dataIndex: 'startTime', 
      key: 'startTime',
      render: (date: string) => dayjs(date).format('HH:mm DD/MM/YYYY')
    },
    { 
      title: 'Giá vé', 
      dataIndex: 'basePrice', 
      key: 'basePrice',
      render: (value: number) => `${new Intl.NumberFormat('vi-VN').format(value || 0)}đ`
    },
    {
      title: 'Hành động',
      key: 'action',
      render: (_: any, record: ShowtimeDto) => (
        <Space size="middle">
          <Button type="text" icon={<EditOutlined />} onClick={() => showModal(record)} />
          <Popconfirm
            title="Bạn có chắc chắn muốn xóa lịch chiếu này?"
            onConfirm={() => handleDelete(record.id)}
            okText="Xóa"
            cancelText="Hủy"
          >
            <Button type="text" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold">Quản lý Lịch chiếu</h2>
        <Space>
          <Input.Search 
            placeholder="Tìm phim, phòng chiếu..." 
            allowClear 
            onSearch={onSearch} 
            style={{ width: 250 }}
            disabled={!selectedCinema}
          />
          <span className="font-medium">Chọn Cụm Rạp:</span>
          <Select 
            style={{ width: 250 }} 
            value={selectedCinema} 
            onChange={setSelectedCinema}
          >
            {cinemas.map(c => (
              <Option key={c.id} value={c.id}>{c.name}</Option>
            ))}
          </Select>
          <Button type="primary" icon={<PlusOutlined />} onClick={() => showModal()} disabled={!selectedCinema}>
            Thêm Lịch Chiếu
          </Button>
        </Space>
      </div>

      <Table
        columns={columns}
        dataSource={showtimes}
        rowKey="id"
        loading={loading}
        pagination={pagination}
        onChange={handleTableChange}
      />

      <Modal
        title={editingShowtime ? 'Cập nhật Lịch Chiếu' : 'Thêm Lịch Chiếu Mới'}
        open={isModalVisible}
        onCancel={() => setIsModalVisible(false)}
        onOk={() => form.submit()}
      >
        <Form form={form} layout="vertical" onFinish={handleFinish}>
          <Form.Item name="movieId" label="Phim" rules={[{ required: true }]}>
            <Select showSearch optionFilterProp="children">
              {movies.map(m => (
                <Option key={m.id} value={m.id}>{m.title}</Option>
              ))}
            </Select>
          </Form.Item>

          <Form.Item name="roomId" label="Phòng chiếu" rules={[{ required: true }]}>
            <Select>
              {rooms.map(r => (
                <Option key={r.id} value={r.id}>{r.name}</Option>
              ))}
            </Select>
          </Form.Item>

          <Form.Item name="startTime" label="Giờ bắt đầu" rules={[{ required: true }]} tooltip="Giờ kết thúc sẽ được hệ thống tự động tính dựa trên thời lượng phim + 30 phút dọn dẹp">
            <DatePicker showTime format="YYYY-MM-DD HH:mm" className="w-full" />
          </Form.Item>

          <Form.Item name="basePrice" label="Giá vé cơ bản (VNĐ)" rules={[{ required: true }]}>
            <InputNumber min={0} step={10000} className="w-full" />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
