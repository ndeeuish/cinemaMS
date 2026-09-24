'use client';

import { useEffect, useState } from 'react';
import { Table, Button, Modal, Form, Input, InputNumber, DatePicker, Space, Popconfirm, message, Select, Tag, Row, Col, Typography, Upload } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, EyeOutlined, LoadingOutlined } from '@ant-design/icons';
import { adminService } from '@/services/admin.service';
import { MovieDto, AgeRestrictionDto, GenreDto } from '@/types/admin.type';
import dayjs from 'dayjs';

const { Option } = Select;
const { Title, Paragraph, Text } = Typography;

const getAgeRestrictionColor = (code: string) => {
  if (code.includes('18')) return 'red';
  if (code.includes('16')) return 'orange';
  if (code.includes('13')) return 'blue';
  if (code === 'P') return 'green';
  return 'default';
};

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

export default function AdminMoviesPage() {
  const [movies, setMovies] = useState<MovieDto[]>([]);

  // Master data state
  const [ageRestrictions, setAgeRestrictions] = useState<AgeRestrictionDto[]>([]);
  const [genres, setGenres] = useState<GenreDto[]>([]);

  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingMovie, setEditingMovie] = useState<MovieDto | null>(null);
  const [form] = Form.useForm();

  const [uploadingPoster, setUploadingPoster] = useState(false);
  const [posterPreview, setPosterPreview] = useState<string>('');

  const [isDetailsVisible, setIsDetailsVisible] = useState(false);
  const [detailsMovie, setDetailsMovie] = useState<MovieDto | null>(null);

  const [pagination, setPagination] = useState({ current: 1, pageSize: 10, total: 0 });
  const [keyword, setKeyword] = useState<string>('');
  const [selectedGenreId, setSelectedGenreId] = useState<number | undefined>(undefined);

  const fetchMovies = async (page = pagination.current, pageSize = pagination.pageSize, kw = keyword, genreId = selectedGenreId) => {
    setLoading(true);
    try {
      const data = await adminService.getMovies({
        pageIndex: page,
        pageSize,
        keyword: kw || undefined,
        genreId: genreId
      });
      setMovies(data.items);
      setPagination({
        current: data.pageIndex,
        pageSize: data.pageSize,
        total: data.totalItems
      });
    } catch (error) {
      message.error('Lỗi khi tải danh sách phim');
    } finally {
      setLoading(false);
    }
  };

  const fetchFilters = async () => {
    try {
      const [genresData, ageRestrictionsData] = await Promise.all([
        adminService.getGenres(),
        adminService.getAgeRestrictions()
      ]);
      setGenres(genresData);
      setAgeRestrictions(ageRestrictionsData);
    } catch (error) {
      message.error('Lỗi khi tải dữ liệu bộ lọc');
    }
  };

  useEffect(() => {
    fetchMovies(1, pagination.pageSize, keyword, selectedGenreId);
  }, [keyword, selectedGenreId]);

  useEffect(() => {
    fetchFilters();
  }, []);

  const handleTableChange = (newPagination: any) => {
    fetchMovies(newPagination.current, newPagination.pageSize, keyword, selectedGenreId);
  };

  const onSearch = (value: string) => {
    setKeyword(value);
  };

  const onGenreChange = (value: number | undefined) => {
    setSelectedGenreId(value);
  };

  const showModal = (movie?: MovieDto) => {
    setEditingMovie(movie || null);
    if (movie) {
      form.setFieldsValue({
        ...movie,
        releaseDate: movie.releaseDate ? dayjs(movie.releaseDate) : null,
        genreIds: movie.genreIds || []
      });
      setPosterPreview(movie.posterUrl || '');
    } else {
      form.resetFields();
      setPosterPreview('');
    }
    setIsModalVisible(true);
  };

  const showDetails = (movie: MovieDto) => {
    setDetailsMovie(movie);
    setIsDetailsVisible(true);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    form.resetFields();
    setEditingMovie(null);
    setPosterPreview('');
  };

  const handleFinish = async (values: any) => {
    try {
      const payload = {
        ...values,
        releaseDate: values.releaseDate?.format('YYYY-MM-DD')
      };

      if (editingMovie) {
        await adminService.updateMovie(editingMovie.id, payload);
        message.success('Cập nhật phim thành công');

        // Xóa ảnh cũ nếu poster bị thay đổi
        if (editingMovie.posterUrl && editingMovie.posterUrl !== payload.posterUrl) {
          const publicId = extractPublicId(editingMovie.posterUrl);
          if (publicId) {
            adminService.deleteImage(publicId).catch(() => console.log('Failed to delete old poster'));
          }
        }
      } else {
        await adminService.createMovie(payload);
        message.success('Thêm phim thành công');
      }
      setIsModalVisible(false);
      fetchMovies();
    } catch (error: any) {
      message.error(getErrorMessage(error, 'Có lỗi xảy ra'));
    }
  };

  const handleDelete = async (id: number) => {
    try {
      // Note: Ideally we should delete the poster from Cloudinary here too
      // But we need the movie's posterUrl first. Assuming we just do soft delete or let backend handle it.
      await adminService.deleteMovie(id);
      message.success('Xóa phim thành công');
      fetchMovies();
    } catch (error: any) {
      message.error('Không thể xóa phim này');
    }
  };

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const customUpload = async (options: any) => {
    const { file, onSuccess, onError } = options;
    try {
      setUploadingPoster(true);
      const res = await adminService.uploadImage(file as File, 'CinemaMS/Movies/Posters');
      setPosterPreview(res.url);
      form.setFieldsValue({ posterUrl: res.url });
      onSuccess?.(res, new XMLHttpRequest());
    } catch (err) {
      message.error('Tải ảnh lên thất bại');
      onError?.(err as Error);
    } finally {
      setUploadingPoster(false);
    }
  };
  
  const uploadButton = (
    <div>
      {uploadingPoster ? <LoadingOutlined /> : <PlusOutlined />}
      <div style={{ marginTop: 8 }}>Tải ảnh lên</div>
    </div>
  );

  const columns = [
    { title: 'Tên phim', dataIndex: 'title', key: 'title' },
    { title: 'Thời lượng', dataIndex: 'durationInMinutes', key: 'duration', render: (val: number) => `${val} phút` },
    {
      title: 'Độ tuổi',
      dataIndex: 'ageRestrictionCode',
      key: 'age',
      render: (code: string) => <Tag color={getAgeRestrictionColor(code)} className="font-bold">{code}</Tag>
    },
    {
      title: 'Thể loại',
      dataIndex: 'genres',
      key: 'genres',
      render: (genres: string[]) => (
        <Space size={[0, 4]} wrap>
          {genres?.map(g => <Tag key={g}>{g}</Tag>)}
        </Space>
      )
    },
    {
      title: 'Ngày khởi chiếu',
      dataIndex: 'releaseDate',
      key: 'releaseDate',
      render: (date: string) => dayjs(date).format('DD/MM/YYYY')
    },
    {
      title: 'Hành động',
      key: 'action',
      render: (_: any, record: MovieDto) => (
        <Space size="middle">
          <Button type="text" icon={<EyeOutlined />} onClick={() => showDetails(record)} />
          <Button type="text" icon={<EditOutlined />} onClick={() => showModal(record)} />
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
        <h2 className="text-2xl font-bold">Quản lý Phim</h2>
        <Space>
          <Select
            placeholder="Lọc theo thể loại"
            allowClear
            style={{ width: 150 }}
            value={selectedGenreId}
            onChange={onGenreChange}
          >
            {genres.map(g => (
              <Option key={g.id} value={g.id}>{g.name}</Option>
            ))}
          </Select>
          <Input.Search
            placeholder="Tìm kiếm phim..."
            allowClear
            onSearch={onSearch}
            style={{ width: 250 }}
          />
          <Button type="primary" icon={<PlusOutlined />} onClick={() => showModal()}>
            Thêm phim mới
          </Button>
        </Space>
      </div>

      <Table
        columns={columns}
        dataSource={movies}
        rowKey="id"
        loading={loading}
        pagination={pagination}
        onChange={handleTableChange}
      />

      {/* Edit/Create Modal */}
      <Modal
        title={editingMovie ? 'Cập nhật Phim' : 'Thêm Phim Mới'}
        open={isModalVisible}
        onCancel={handleCancel}
        onOk={() => form.submit()}
        width={800}
      >
        <Form form={form} layout="vertical" onFinish={handleFinish}>
          <Form.Item name="title" label="Tên phim" rules={[{ required: true }]}>
            <Input />
          </Form.Item>

          <Row gutter={16}>
            <Col span={8}>
              <Form.Item name="durationInMinutes" label="Thời lượng (phút)" rules={[{ required: true }]}>
                <InputNumber min={1} className="w-full" />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item name="releaseDate" label="Ngày khởi chiếu" rules={[{ required: true }]}>
                <DatePicker format="DD/MM/YYYY" className="w-full" />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item name="ageRestrictionId" label="Giới hạn độ tuổi" rules={[{ required: true }]}>
                <Select placeholder="Chọn độ tuổi">
                  {ageRestrictions.map(age => (
                    <Option key={age.id} value={age.id}>{age.code} - {age.description}</Option>
                  ))}
                </Select>
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="director" label="Đạo diễn" rules={[{ required: true }]}>
                <Input placeholder="Tên đạo diễn" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="genreIds" label="Thể loại" rules={[{ required: true }]}>
                <Select mode="multiple" placeholder="Chọn thể loại">
                  {genres.map(g => (
                    <Option key={g.id} value={g.id}>{g.name}</Option>
                  ))}
                </Select>
              </Form.Item>
            </Col>
          </Row>

          <Form.Item name="casts" label="Diễn viên" rules={[{ required: true }]}>
            <Input.TextArea rows={2} placeholder="Danh sách diễn viên, cách nhau bằng dấu phẩy" />
          </Form.Item>

          <Form.Item name="posterUrl" label="Poster Phim" rules={[{ required: true }]}>
            <Upload
              name="poster"
              listType="picture-card"
              className="avatar-uploader"
              showUploadList={false}
              customRequest={customUpload}
              accept="image/*"
            >
              {posterPreview ? <img src={posterPreview} alt="poster" style={{ width: '100%', height: '100%', objectFit: 'cover' }} /> : uploadButton}
            </Upload>
          </Form.Item>

          <Form.Item name="trailerUrl" label="Link Trailer (URL Youtube)">
            <Input />
          </Form.Item>

          <Form.Item name="description" label="Nội dung tóm tắt" rules={[{ required: true }]}>
            <Input.TextArea rows={4} />
          </Form.Item>
        </Form>
      </Modal>

      {/* Details Modal */}
      <Modal
        title={<span className="text-xl font-bold">Chi tiết Phim</span>}
        open={isDetailsVisible}
        onCancel={() => setIsDetailsVisible(false)}
        footer={[
          <Button key="close" onClick={() => setIsDetailsVisible(false)}>Đóng</Button>
        ]}
        width={900}
      >
        {detailsMovie && (
          <Row gutter={32} className="mt-4">
            <Col span={8}>
              <img
                src={detailsMovie.posterUrl || 'https://via.placeholder.com/300x450'}
                alt={detailsMovie.title}
                className="w-full rounded-xl shadow-md"
              />
            </Col>
            <Col span={16}>
              <Title level={3} className="mb-2">{detailsMovie.title}</Title>
              <div className="flex flex-wrap gap-2 mb-6">
                <Tag color={getAgeRestrictionColor(detailsMovie.ageRestrictionCode)} className="font-bold">
                  {detailsMovie.ageRestrictionCode}
                </Tag>
                <Tag color="blue">{detailsMovie.durationInMinutes} phút</Tag>
                <Tag color="cyan">Khởi chiếu: {dayjs(detailsMovie.releaseDate).format('DD/MM/YYYY')}</Tag>
              </div>

              <div className="mb-4">
                <Text strong>Thể loại: </Text>
                {detailsMovie.genres?.map(g => <Tag key={g} className="ml-1">{g}</Tag>)}
              </div>

              <div className="mb-4">
                <Text strong>Đạo diễn: </Text>
                <Text>{detailsMovie.director}</Text>
              </div>

              <div className="mb-4">
                <Text strong>Diễn viên: </Text>
                <Text>{detailsMovie.casts}</Text>
              </div>

              <div className="mb-6">
                <Text strong>Nội dung: </Text>
                <Paragraph className="mt-1 text-gray-600 text-justify">
                  {detailsMovie.description}
                </Paragraph>
              </div>

              {detailsMovie.trailerUrl && (
                <Button type="primary" danger onClick={() => window.open(detailsMovie.trailerUrl, '_blank')}>
                  Xem Trailer trên Youtube
                </Button>
              )}
            </Col>
          </Row>
        )}
      </Modal>
    </div>
  );
}
