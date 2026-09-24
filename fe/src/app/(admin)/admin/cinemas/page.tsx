'use client';

import { useEffect, useState } from 'react';
import { Table, Button, Modal, Form, Input, Space, Popconfirm, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, SettingOutlined, AppstoreAddOutlined } from '@ant-design/icons';
import { adminService } from '@/services/admin.service';
import { CinemaDto } from '@/types/admin.type';
import { useRouter } from 'next/navigation';

export default function AdminCinemasPage() {
  const [cinemas, setCinemas] = useState<CinemaDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingCinema, setEditingCinema] = useState<CinemaDto | null>(null);
  const [form] = Form.useForm();
  const router = useRouter();

  const [pagination, setPagination] = useState({ current: 1, pageSize: 10, total: 0 });
  const [keyword, setKeyword] = useState<string>('');

  const fetchCinemas = async (page = pagination.current, pageSize = pagination.pageSize, kw = keyword) => {
    setLoading(true);
    try {
      const data = await adminService.getCinemas({ pageIndex: page, pageSize, keyword: kw || undefined });
      setCinemas(data.items);
      setPagination({
        current: data.pageIndex,
        pageSize: data.pageSize,
        total: data.totalItems
      });
    } catch (error) {
      message.error('Lỗi khi tải danh sách cụm rạp');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCinemas(1, pagination.pageSize, keyword);
  }, [keyword]);

  const handleTableChange = (newPagination: any) => {
    fetchCinemas(newPagination.current, newPagination.pageSize, keyword);
  };

  const onSearch = (value: string) => {
    setKeyword(value);
  };

  const showModal = (cinema?: CinemaDto) => {
    setEditingCinema(cinema || null);
    if (cinema) {
      form.setFieldsValue(cinema);
    } else {
      form.resetFields();
    }
    setIsModalVisible(true);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    form.resetFields();
    setEditingCinema(null);
  };

  const handleFinish = async (values: any) => {
    try {
      if (editingCinema) {
        await adminService.updateCinema(editingCinema.id, values);
        message.success('Cập nhật cụm rạp thành công');
      } else {
        await adminService.createCinema(values);
        message.success('Thêm cụm rạp thành công');
      }
      setIsModalVisible(false);
      fetchCinemas();
    } catch (error: any) {
      message.error(getErrorMessage(error, 'Có lỗi xảy ra'));
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await adminService.deleteCinema(id);
      message.success('Xóa cụm rạp thành công');
      fetchCinemas();
    } catch (error: any) {
      message.error('Không thể xóa cụm rạp này vì đã có phòng/lịch chiếu liên quan');
    }
  };

  const columns = [
    { title: 'Tên rạp', dataIndex: 'name', key: 'name' },
    { title: 'Địa chỉ', dataIndex: 'address', key: 'address' },
    { title: 'Hotline', dataIndex: 'hotline', key: 'hotline' },
    {
      title: 'Hành động',
      key: 'action',
      render: (_: any, record: CinemaDto) => (
        <Space size="middle">
          <Button type="text" icon={<EditOutlined />} onClick={() => showModal(record)} />
          <Button 
            type="text" 
            icon={<AppstoreAddOutlined />} 
            onClick={() => router.push(`/admin/cinemas/${record.id}/rooms`)}
          />
          <Popconfirm
            title="Bạn có chắc chắn muốn xóa?"
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
        <h2 className="text-2xl font-bold">Quản lý Cụm Rạp</h2>
        <Space>
          <Input.Search 
            placeholder="Tìm rạp, địa chỉ..." 
            allowClear 
            onSearch={onSearch} 
            style={{ width: 250 }}
          />
          <Button type="primary" icon={<PlusOutlined />} onClick={() => showModal()}>
            Thêm cụm rạp
          </Button>
        </Space>
      </div>

      <Table
        columns={columns}
        dataSource={cinemas}
        rowKey="id"
        loading={loading}
        pagination={pagination}
        onChange={handleTableChange}
      />

      <Modal
        title={editingCinema ? 'Cập nhật Cụm Rạp' : 'Thêm Cụm Rạp Mới'}
        open={isModalVisible}
        onCancel={handleCancel}
        onOk={() => form.submit()}
      >
        <Form form={form} layout="vertical" onFinish={handleFinish}>
          <Form.Item name="name" label="Tên Rạp" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          
          <Form.Item name="address" label="Vị trí / Địa chỉ" rules={[{ required: true }]}>
            <Input />
          </Form.Item>

          <Form.Item name="hotline" label="Hotline" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
