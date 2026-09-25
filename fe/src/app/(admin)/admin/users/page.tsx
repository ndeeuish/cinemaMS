'use client';
import { getErrorMessage } from '@/utils/error.util';

import { useEffect, useState } from 'react';
import { Table, Button, Space, Tag, Popconfirm, message, Modal, Form, Input, Select, Row, Col, Typography } from 'antd';
import { EditOutlined, DeleteOutlined, EyeOutlined } from '@ant-design/icons';
import { adminService } from '@/services/admin.service';
import { UserDto } from '@/types/admin.type';
import dayjs from 'dayjs';

const { Option } = Select;
const { Title, Text } = Typography;

const ROLES = [
  { id: 1, name: 'Admin' },
  { id: 3, name: 'Manager' },
  { id: 4, name: 'Staff' },
  { id: 2, name: 'Customer' }
];

export default function AdminUsersPage() {
  const [users, setUsers] = useState<UserDto[]>([]);
  const [loading, setLoading] = useState(false);

  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingUser, setEditingUser] = useState<UserDto | null>(null);
  const [form] = Form.useForm();

  const [isDetailsVisible, setIsDetailsVisible] = useState(false);
  const [detailsUser, setDetailsUser] = useState<UserDto | null>(null);

  const [selectedRole, setSelectedRole] = useState<number | null>(null);

  const [pagination, setPagination] = useState({ current: 1, pageSize: 10, total: 0 });
  const [keyword, setKeyword] = useState<string>('');

  const fetchUsers = async (page = pagination.current, pageSize = pagination.pageSize, roleId = selectedRole, kw = keyword) => {
    setLoading(true);
    try {
      const data = await adminService.getUsers({ pageIndex: page, pageSize, roleId: roleId || undefined, keyword: kw || undefined });
      setUsers(data.items);
      setPagination({
        current: data.pageIndex,
        pageSize: data.pageSize,
        total: data.totalItems
      });
    } catch (error) {
      message.error('Lỗi khi tải danh sách người dùng');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers(1, pagination.pageSize, selectedRole, keyword);
  }, [selectedRole, keyword]);

  const handleTableChange = (newPagination: any) => {
    fetchUsers(newPagination.current, newPagination.pageSize, selectedRole, keyword);
  };

  const onSearch = (value: string) => {
    setKeyword(value);
  };

  const showModal = (user: UserDto) => {
    setEditingUser(user);
    form.setFieldsValue({
      email: user.email,
      fullName: user.fullName,
      phoneNumber: user.phoneNumber,
      roleId: user.roleId
    });
    setIsModalVisible(true);
  };

  const showDetails = (user: UserDto) => {
    setDetailsUser(user);
    setIsDetailsVisible(true);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    form.resetFields();
    setEditingUser(null);
  };

  const handleFinish = async (values: any) => {
    try {
      if (editingUser) {
        await adminService.updateUser(editingUser.id, values);
        message.success('Cập nhật người dùng thành công');
      }
      setIsModalVisible(false);
      fetchUsers();
    } catch (error: any) {
      message.error(getErrorMessage(error, 'Có lỗi xảy ra'));
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await adminService.deleteUser(id);
      message.success('Xóa người dùng thành công');
      fetchUsers();
    } catch (error: any) {
      message.error(getErrorMessage(error, 'Không thể xóa người dùng này'));
    }
  };

  const columns = [
    { title: 'Tài khoản', dataIndex: 'username', key: 'username' },
    { title: 'Họ tên', dataIndex: 'fullName', key: 'fullName' },
    { title: 'Email', dataIndex: 'email', key: 'email' },
    {
      title: 'Vai trò',
      dataIndex: 'roleName',
      key: 'role',
      render: (role: string) => {
        let color = 'blue';
        if (role === 'Admin') color = 'red';
        else if (role === 'Manager') color = 'orange';
        else if (role === 'Staff') color = 'green';
        return <Tag color={color}>{role || 'Customer'}</Tag>;
      }
    },
    {
      title: 'Ngày tham gia',
      dataIndex: 'createdAt',
      key: 'createdAt',
      render: (date: string) => dayjs(date).format('DD/MM/YYYY HH:mm')
    },
    {
      title: 'Trạng thái',
      key: 'status',
      render: (_: any, record: UserDto) => (
        <Tag color={record.isDeleted ? 'default' : 'success'}>
          {record.isDeleted ? 'Đã khóa/Xóa' : 'Hoạt động'}
        </Tag>
      ),
    },
    {
      title: 'Hành động',
      key: 'action',
      render: (_: any, record: UserDto) => (
        <Space size="middle">
          <Button type="text" icon={<EyeOutlined />} onClick={() => showDetails(record)} />
          {!record.isDeleted && (
            <>
              <Button type="text" icon={<EditOutlined />} onClick={() => showModal(record)} />
              <Popconfirm
                title="Bạn có chắc chắn muốn xóa (vô hiệu hóa) người dùng này?"
                onConfirm={() => handleDelete(record.id)}
                okText="Xóa"
                cancelText="Hủy"
              >
                <Button type="text" danger icon={<DeleteOutlined />} />
              </Popconfirm>
            </>
          )}
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold">Quản lý Người Dùng</h2>
        <Space>
          <Input.Search 
            placeholder="Tìm kiếm tài khoản, tên, email..." 
            allowClear 
            onSearch={onSearch} 
            style={{ width: 250 }}
          />
          <Select 
            placeholder="Lọc theo vai trò" 
            allowClear 
            style={{ width: 200 }}
            onChange={(val) => setSelectedRole(val)}
          >
            {ROLES.map(role => (
              <Option key={role.id} value={role.id}>{role.name}</Option>
            ))}
          </Select>
        </Space>
      </div>

      <Table
        columns={columns}
        dataSource={users}
        rowKey="id"
        loading={loading}
        pagination={pagination}
        onChange={handleTableChange}
      />

      {/* Edit User Modal */}
      <Modal
        title="Cập nhật Thông tin & Vai trò"
        open={isModalVisible}
        onCancel={handleCancel}
        onOk={() => form.submit()}
      >
        <Form form={form} layout="vertical" onFinish={handleFinish}>
          <Form.Item name="fullName" label="Họ tên" rules={[{ required: true }]}>
            <Input />
          </Form.Item>

          <Form.Item name="email" label="Email" rules={[{ required: true, type: 'email' }]}>
            <Input />
          </Form.Item>

          <Form.Item name="phoneNumber" label="Số điện thoại">
            <Input />
          </Form.Item>

          <Form.Item name="roleId" label="Vai trò" rules={[{ required: true }]}>
            <Select>
              {ROLES.map(role => (
                <Option key={role.id} value={role.id}>{role.name}</Option>
              ))}
            </Select>
          </Form.Item>
        </Form>
      </Modal>

      {/* User Details Modal */}
      <Modal
        title={<span className="text-xl font-bold">Chi tiết Người Dùng</span>}
        open={isDetailsVisible}
        onCancel={() => setIsDetailsVisible(false)}
        footer={[
          <Button key="close" onClick={() => setIsDetailsVisible(false)}>Đóng</Button>
        ]}
      >
        {detailsUser && (
          <div className="mt-4 flex flex-col gap-3">
            <Row>
              <Col span={8}><Text strong>Tài khoản:</Text></Col>
              <Col span={16}>{detailsUser.username}</Col>
            </Row>
            <Row>
              <Col span={8}><Text strong>Họ tên:</Text></Col>
              <Col span={16}>{detailsUser.fullName}</Col>
            </Row>
            <Row>
              <Col span={8}><Text strong>Email:</Text></Col>
              <Col span={16}>{detailsUser.email}</Col>
            </Row>
            <Row>
              <Col span={8}><Text strong>Số điện thoại:</Text></Col>
              <Col span={16}>{detailsUser.phoneNumber || 'Trống'}</Col>
            </Row>
            <Row>
              <Col span={8}><Text strong>Vai trò:</Text></Col>
              <Col span={16}>
                <Tag color={
                  detailsUser.roleName === 'Admin' ? 'red' :
                    detailsUser.roleName === 'Manager' ? 'orange' :
                      detailsUser.roleName === 'Staff' ? 'green' : 'blue'
                }>
                  {detailsUser.roleName || 'Customer'}
                </Tag>
              </Col>
            </Row>
            <Row>
              <Col span={8}><Text strong>Trạng thái:</Text></Col>
              <Col span={16}>
                <Tag color={detailsUser.isDeleted ? 'default' : 'success'}>
                  {detailsUser.isDeleted ? 'Đã khóa/Xóa' : 'Hoạt động'}
                </Tag>
              </Col>
            </Row>
            <Row>
              <Col span={8}><Text strong>Ngày tham gia:</Text></Col>
              <Col span={16}>{dayjs(detailsUser.createdAt).format('DD/MM/YYYY HH:mm')}</Col>
            </Row>
          </div>
        )}
      </Modal>
    </div>
  );
}
