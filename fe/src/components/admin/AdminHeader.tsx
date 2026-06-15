'use client';

import { Layout, Dropdown, MenuProps, Avatar } from 'antd';
import { UserOutlined, LogoutOutlined } from '@ant-design/icons';
import { useAuthStore } from '@/stores/auth.store';
import { useRouter } from 'next/navigation';

const { Header } = Layout;

export default function AdminHeader() {
  const { fullName, logout } = useAuthStore();
  const router = useRouter();

  const handleLogout = () => {
    logout();
    router.push('/login');
  };

  const userMenuItems: MenuProps['items'] = [
    {
      key: 'logout',
      danger: true,
      icon: <LogoutOutlined />,
      label: 'Đăng xuất',
      onClick: handleLogout,
    },
  ];

  return (
    <Header className="bg-white px-6 flex justify-end items-center shadow-sm">
      <Dropdown menu={{ items: userMenuItems }} placement="bottomRight">
        <div className="cursor-pointer flex items-center gap-2 hover:bg-gray-50 px-3 rounded transition-colors">
          <Avatar icon={<UserOutlined />} />
          <span className="font-medium text-gray-700">{fullName || 'Admin'}</span>
        </div>
      </Dropdown>
    </Header>
  );
}
