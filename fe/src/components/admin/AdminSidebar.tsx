'use client';

import { Layout, Menu } from 'antd';
import {
  DashboardOutlined,
  VideoCameraOutlined,
  CalendarOutlined,
  EnvironmentOutlined,
  UserOutlined,
} from '@ant-design/icons';
import { usePathname, useRouter } from 'next/navigation';

const { Sider } = Layout;

import { useAuthStore } from '@/stores/auth.store';

export default function AdminSidebar() {
  const router = useRouter();
  const pathname = usePathname();
  const { role } = useAuthStore();
  const isAdmin = role?.toLowerCase() === 'admin';

  const menuItems = [
    { key: '/admin', icon: <DashboardOutlined />, label: 'Thống kê' },
    { key: '/admin/movies', icon: <VideoCameraOutlined />, label: 'Quản lý Phim' },
    { key: '/admin/showtimes', icon: <CalendarOutlined />, label: 'Lịch chiếu' },
    { key: '/admin/bookings', icon: <CalendarOutlined />, label: 'Đặt vé' },
    ...(isAdmin ? [{ key: '/admin/cinemas', icon: <EnvironmentOutlined />, label: 'Cụm Rạp' }] : []),
    ...(isAdmin ? [{ key: '/admin/users', icon: <UserOutlined />, label: 'Người dùng' }] : []),
  ];

  return (
    <Sider
      theme="dark"
      breakpoint="lg"
      collapsedWidth="0"
      className="min-h-screen"
    >
      <div className="h-16 flex items-center justify-center m-4 bg-white/20 rounded text-white font-bold text-lg">
        CinemaMS
      </div>
      <Menu
        theme="dark"
        mode="inline"
        selectedKeys={[pathname]}
        items={menuItems}
        onClick={({ key }) => router.push(key)}
      />
    </Sider>
  );
}
