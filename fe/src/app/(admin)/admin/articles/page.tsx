'use client';

import { useEffect, useState } from 'react';
import { Table, Button, Modal, Form, Input, DatePicker, Space, Popconfirm, message, Select, Tag, Row, Col, Typography, Upload, Switch, Divider } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, EyeOutlined, LoadingOutlined } from '@ant-design/icons';
import { adminService } from '@/services/admin.service';
import { ArticleDto, ArticleType } from '@/types/admin.type';
import dayjs from 'dayjs';
import { getErrorMessage } from '@/utils/error.util';

const { Option } = Select;
const { Title, Paragraph, Text } = Typography;

const extractPublicId = (url: string) => {
  try {
    const parts = url.split('/');
    const uploadIndex = parts.findIndex(p => p === 'upload');
    if (uploadIndex !== -1 && parts.length > uploadIndex + 2) {
      const pathWithExtension = parts.slice(uploadIndex + 2).join('/');
      return pathWithExtension.split('.')[0];
    }
  } catch (e) {}
  return null;
};

export default function AdminArticlesPage() {
  const [articles, setArticles] = useState<ArticleDto[]>([]);
  const [loading, setLoading] = useState(false);
  
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingArticle, setEditingArticle] = useState<ArticleDto | null>(null);
  const [form] = Form.useForm();

  const [uploadingImage, setUploadingImage] = useState(false);
  const [imagePreview, setImagePreview] = useState<string>('');

  const [isDetailsVisible, setIsDetailsVisible] = useState(false);
  const [detailsArticle, setDetailsArticle] = useState<ArticleDto | null>(null);

  const [pagination, setPagination] = useState({ current: 1, pageSize: 10, total: 0 });
  const [keyword, setKeyword] = useState<string>('');

  const fetchArticles = async (page = pagination.current, pageSize = pagination.pageSize, kw = keyword) => {
    setLoading(true);
    try {
      const data = await adminService.getArticles({
        pageIndex: page,
        pageSize,
        keyword: kw || undefined
      });
      setArticles(data.items);
      setPagination({
        current: data.pageIndex,
        pageSize: data.pageSize,
        total: data.totalItems
      });
    } catch (error) {
      message.error('Lỗi khi tải danh sách bài viết');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchArticles(1, pagination.pageSize, keyword);
  }, [keyword]);

  const handleTableChange = (newPagination: any) => {
    fetchArticles(newPagination.current, newPagination.pageSize, keyword);
  };

  const onSearch = (value: string) => {
    setKeyword(value);
  };

  const showModal = (article?: ArticleDto) => {
    setEditingArticle(article || null);
    if (article) {
      form.setFieldsValue({
        ...article,
        startDate: article.startDate ? dayjs(article.startDate) : null,
        endDate: article.endDate ? dayjs(article.endDate) : null,
      });
      setImagePreview(article.imageUrl || '');
    } else {
      form.resetFields();
      form.setFieldsValue({ type: ArticleType.News, isActive: true });
      setImagePreview('');
    }
    setIsModalVisible(true);
  };

  const showDetails = (article: ArticleDto) => {
    setDetailsArticle(article);
    setIsDetailsVisible(true);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    form.resetFields();
    setEditingArticle(null);
    setImagePreview('');
  };

  const handleFinish = async (values: any) => {
    try {
      const payload = {
        ...values,
        startDate: values.startDate?.format('YYYY-MM-DD'),
        endDate: values.endDate?.format('YYYY-MM-DD')
      };

      if (editingArticle) {
        await adminService.updateArticle(editingArticle.id, payload);
        message.success('Cập nhật bài viết thành công');

        // Xóa ảnh cũ nếu bị thay đổi
        if (editingArticle.imageUrl && editingArticle.imageUrl !== payload.imageUrl) {
          const publicId = extractPublicId(editingArticle.imageUrl);
          if (publicId) {
            adminService.deleteImage(publicId).catch(() => console.log('Failed to delete old image'));
          }
        }
      } else {
        await adminService.createArticle(payload);
        message.success('Thêm bài viết thành công');
      }
      setIsModalVisible(false);
      fetchArticles();
    } catch (error: any) {
      message.error(getErrorMessage(error, 'Có lỗi xảy ra'));
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await adminService.deleteArticle(id);
      message.success('Xóa bài viết thành công');
      fetchArticles();
    } catch (error: any) {
      message.error('Không thể xóa bài viết này');
    }
  };

  const customUpload = async (options: any) => {
    const { file, onSuccess, onError } = options;
    try {
      setUploadingImage(true);
      const res = await adminService.uploadImage(file as File, 'CinemaMS/Articles');
      setImagePreview(res.url);
      form.setFieldsValue({ imageUrl: res.url });
      onSuccess?.(res, new XMLHttpRequest());
    } catch (err) {
      message.error('Tải ảnh lên thất bại');
      onError?.(err as Error);
    } finally {
      setUploadingImage(false);
    }
  };
  
  const uploadButton = (
    <div>
      {uploadingImage ? <LoadingOutlined /> : <PlusOutlined />}
      <div style={{ marginTop: 8 }}>Tải ảnh lên</div>
    </div>
  );

  const columns = [
    { 
      title: 'Ảnh bìa', 
      dataIndex: 'imageUrl', 
      key: 'imageUrl', 
      render: (url: string) => <img src={url} alt="cover" className="w-16 h-10 object-cover rounded" /> 
    },
    { title: 'Tiêu đề', dataIndex: 'title', key: 'title' },
    { 
      title: 'Loại', 
      dataIndex: 'type', 
      key: 'type', 
      render: (type: ArticleType) => <Tag color={type === ArticleType.News ? 'blue' : 'green'}>{type === ArticleType.News ? 'Tin tức' : 'Sự kiện'}</Tag> 
    },
    { 
      title: 'Trạng thái', 
      dataIndex: 'isActive', 
      key: 'isActive', 
      render: (isActive: boolean) => <Tag color={isActive ? 'success' : 'default'}>{isActive ? 'Hiển thị' : 'Đã ẩn'}</Tag> 
    },
    {
      title: 'Ngày tạo',
      dataIndex: 'createdAt',
      key: 'createdAt',
      render: (date: string) => dayjs(date).format('DD/MM/YYYY')
    },
    {
      title: 'Hành động',
      key: 'action',
      render: (_: any, record: ArticleDto) => (
        <Space size="middle">
          <Button type="text" icon={<EyeOutlined />} onClick={() => showDetails(record)} />
          <Button type="text" icon={<EditOutlined />} onClick={() => showModal(record)} />
          <Popconfirm
            title="Bạn có chắc chắn muốn xóa bài viết này?"
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
        <h2 className="text-2xl font-bold">Quản lý Bài viết & Sự kiện</h2>
        <Space>
          <Input.Search
            placeholder="Tìm kiếm tiêu đề..."
            allowClear
            onSearch={onSearch}
            style={{ width: 250 }}
          />
          <Button type="primary" icon={<PlusOutlined />} onClick={() => showModal()}>
            Thêm bài viết mới
          </Button>
        </Space>
      </div>

      <Table
        columns={columns}
        dataSource={articles}
        rowKey="id"
        loading={loading}
        pagination={pagination}
        onChange={handleTableChange}
      />

      <Modal
        title={editingArticle ? 'Cập nhật Bài viết' : 'Thêm Bài viết mới'}
        open={isModalVisible}
        onCancel={handleCancel}
        onOk={() => form.submit()}
        width={900}
      >
        <Form form={form} layout="vertical" onFinish={handleFinish}>
          <Row gutter={16}>
            <Col span={16}>
              <Form.Item name="title" label="Tiêu đề" rules={[{ required: true }]}>
                <Input placeholder="Nhập tiêu đề bài viết/sự kiện" />
              </Form.Item>
            </Col>
            <Col span={4}>
              <Form.Item name="type" label="Loại bài viết" rules={[{ required: true }]}>
                <Select>
                  <Option value={ArticleType.News}>Tin tức</Option>
                  <Option value={ArticleType.Event}>Sự kiện</Option>
                </Select>
              </Form.Item>
            </Col>
            <Col span={4}>
              <Form.Item name="isActive" label="Trạng thái" valuePropName="checked">
                <Switch checkedChildren="Hiển thị" unCheckedChildren="Ẩn" />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="startDate" label="Ngày bắt đầu (áp dụng cho Sự kiện)">
                <DatePicker format="DD/MM/YYYY" className="w-full" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="endDate" label="Ngày kết thúc (áp dụng cho Sự kiện)">
                <DatePicker format="DD/MM/YYYY" className="w-full" />
              </Form.Item>
            </Col>
          </Row>

          <Form.Item name="summary" label="Mô tả ngắn" rules={[{ required: true }]}>
            <Input.TextArea rows={3} placeholder="Đoạn giới thiệu ngắn hiển thị ngoài trang chủ" />
          </Form.Item>

          <Form.Item name="imageUrl" label="Ảnh bìa" rules={[{ required: true }]}>
            <Upload
              name="cover"
              listType="picture-card"
              className="avatar-uploader"
              showUploadList={false}
              customRequest={customUpload}
              accept="image/*"
            >
              {imagePreview ? <img src={imagePreview} alt="cover" style={{ width: '100%', height: '100%', objectFit: 'cover' }} /> : uploadButton}
            </Upload>
          </Form.Item>

          <Form.Item name="content" label="Nội dung chi tiết (HTML)" rules={[{ required: true }]}>
            <Input.TextArea rows={8} placeholder="Nhập nội dung bài viết dạng HTML" />
          </Form.Item>
        </Form>
      </Modal>

      <Modal
        title={<span className="text-xl font-bold">Chi tiết Bài viết</span>}
        open={isDetailsVisible}
        onCancel={() => setIsDetailsVisible(false)}
        footer={[
          <Button key="close" onClick={() => setIsDetailsVisible(false)}>Đóng</Button>
        ]}
        width={900}
      >
        {detailsArticle && (
          <div className="mt-4">
            <img
              src={detailsArticle.imageUrl || 'https://via.placeholder.com/800x400'}
              alt={detailsArticle.title}
              className="w-full h-64 object-cover rounded-xl shadow-md mb-6"
            />
            
            <div className="flex gap-2 mb-4">
              <Tag color={detailsArticle.type === ArticleType.News ? 'blue' : 'green'} className="font-bold">
                {detailsArticle.type === ArticleType.News ? 'Tin tức' : 'Sự kiện'}
              </Tag>
              {detailsArticle.startDate && <Tag color="cyan">Bắt đầu: {dayjs(detailsArticle.startDate).format('DD/MM/YYYY')}</Tag>}
              {detailsArticle.endDate && <Tag color="orange">Kết thúc: {dayjs(detailsArticle.endDate).format('DD/MM/YYYY')}</Tag>}
            </div>

            <Title level={3} className="mb-4">{detailsArticle.title}</Title>
            
            <Text strong className="block mb-2">Mô tả ngắn:</Text>
            <Paragraph className="text-gray-600 italic border-l-4 border-gray-300 pl-4">
              {detailsArticle.summary}
            </Paragraph>

            <Divider />
            
            <Text strong className="block mb-4">Nội dung chi tiết:</Text>
            <div className="bg-gray-50 p-6 rounded-lg border border-gray-100 prose max-w-none" dangerouslySetInnerHTML={{ __html: detailsArticle.content }} />
          </div>
        )}
      </Modal>
    </div>
  );
}
