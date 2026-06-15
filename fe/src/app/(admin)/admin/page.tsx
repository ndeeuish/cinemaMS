'use client';

import { useEffect, useState } from 'react';
import { Card, Col, Row, Statistic, Table, Typography, message, Spin } from 'antd';
import { DollarOutlined, BarcodeOutlined, UserOutlined, VideoCameraOutlined } from '@ant-design/icons';
import { dashboardService } from '@/services/dashboard.service';
import { DashboardOverviewDto, MovieRevenueDto } from '@/types/dashboard.type';

const { Title } = Typography;

export default function DashboardPage() {
  const [loading, setLoading] = useState(true);
  const [overview, setOverview] = useState<DashboardOverviewDto | null>(null);
  const [topMovies, setTopMovies] = useState<MovieRevenueDto[]>([]);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const [overviewData, topMoviesData] = await Promise.all([
        dashboardService.getOverview(),
        dashboardService.getTopMovies({ topN: 5 }),
      ]);
      setOverview(overviewData);
      setTopMovies(topMoviesData);
    } catch (error) {
      message.error('Lỗi khi tải dữ liệu thống kê');
    } finally {
      setLoading(false);
    }
  };

  const tableColumns = [
    {
      title: 'Tên Phim',
      dataIndex: 'movieTitle',
      key: 'movieTitle',
    },
    {
      title: 'Số Vé Bán Ra',
      dataIndex: 'ticketsSold',
      key: 'ticketsSold',
      align: 'right' as const,
    },
    {
      title: 'Doanh Thu',
      dataIndex: 'revenue',
      key: 'revenue',
      align: 'right' as const,
      render: (value: number) => (
        <span className="font-bold text-green-600">
          {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value)}
        </span>
      ),
    },
  ];

  if (loading || !overview) {
    return <div className="flex justify-center py-20"><Spin size="large" /></div>;
  }

  return (
    <div>
      <Title level={2} className="mb-6">Tổng quan hệ thống</Title>
      
      {/* 4 Cards KPI */}
      <Row gutter={[16, 16]} className="mb-8">
        <Col xs={24} sm={12} lg={6}>
          <Card variant="borderless" className="shadow-sm">
            <Statistic
              title="Tổng Doanh Thu"
              value={overview.totalRevenue}
              precision={0}
              styles={{ content: { color: '#3f8600', fontWeight: 'bold' } }}
              prefix={<DollarOutlined />}
              suffix="đ"
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card variant="borderless" className="shadow-sm">
            <Statistic
              title="Vé Đã Bán"
              value={overview.totalTicketsSold}
              styles={{ content: { color: '#1677ff', fontWeight: 'bold' } }}
              prefix={<BarcodeOutlined />}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card variant="borderless" className="shadow-sm">
            <Statistic
              title="Tổng số người dùng"
              value={overview.totalUsers}
              styles={{ content: { color: '#faad14', fontWeight: 'bold' } }}
              prefix={<UserOutlined />}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card variant="borderless" className="shadow-sm">
            <Statistic
              title="Phim Đang Chiếu"
              value={overview.totalMovies}
              styles={{ content: { color: '#eb2f96', fontWeight: 'bold' } }}
              prefix={<VideoCameraOutlined />}
            />
          </Card>
        </Col>
      </Row>

      {/* Top Movies Table */}
      <Card title="Top Phim Doanh Thu Cao Nhất" variant="borderless" className="shadow-sm">
        <Table 
          dataSource={topMovies} 
          columns={tableColumns} 
          rowKey="movieId"
          pagination={false}
        />
      </Card>
    </div>
  );
}
