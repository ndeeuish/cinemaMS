'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { useAuthStore } from '@/stores/auth.store';
import { Button, Dropdown, MenuProps, Avatar } from 'antd';
import { UserOutlined, LogoutOutlined } from '@ant-design/icons';
import { useRouter } from 'next/navigation';

export default function ClientNavbar() {
  const { isAuthenticated, fullName, role, logout } = useAuthStore();
  const router = useRouter();
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  const handleLogout = () => {
    logout();
    router.push('/');
  };

  const handleScroll = (e: React.MouseEvent<HTMLAnchorElement>, targetId: string) => {
    if (window.location.pathname === '/') {
      e.preventDefault();
      const targetElement = document.getElementById(targetId);
      if (targetElement) {
        const targetPosition = targetElement.getBoundingClientRect().top + window.scrollY;
        const startPosition = window.scrollY;
        const distance = targetPosition - startPosition;
        const duration = 1200; // Slower scroll duration (1.2 seconds)
        let start: number | null = null;

        // Custom easing function for smooth acceleration and deceleration (easeInOutCubic)
        const ease = (t: number, b: number, c: number, d: number) => {
          t /= d / 2;
          if (t < 1) return (c / 2) * t * t * t + b;
          t -= 2;
          return (c / 2) * (t * t * t + 2) + b;
        };

        const animation = (currentTime: number) => {
          if (start === null) start = currentTime;
          const timeElapsed = currentTime - start;
          const run = ease(timeElapsed, startPosition, distance, duration);
          window.scrollTo(0, run);
          if (timeElapsed < duration) {
            requestAnimationFrame(animation);
          } else {
            // Update URL hash without jumping
            window.history.pushState(null, '', `/#${targetId}`);
          }
        };

        requestAnimationFrame(animation);
      }
    }
  };

  const userMenuItems: MenuProps['items'] = [
    {
      key: 'profile',
      label: <Link href="/profile">Vé của tôi</Link>,
    },
    ...(role?.toLowerCase() === 'admin' ? [{
      key: 'admin',
      label: <Link href="/admin">Quản trị Admin</Link>,
    }] : []),
    {
      type: 'divider',
    },
    {
      key: 'logout',
      danger: true,
      icon: <LogoutOutlined />,
      label: 'Đăng xuất',
      onClick: handleLogout,
    },
  ];

  return (
    <nav className="bg-gray-900 text-white sticky top-0 z-50 shadow-md">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          <div className="flex-shrink-0">
            <Link href="/" className="text-2xl font-bold tracking-wider text-red-500">
              CINEMA<span className="text-white">MS</span>
            </Link>
          </div>
          
          <div className="hidden md:block">
            <div className="ml-10 flex items-baseline space-x-8">
              <Link href="/" className="hover:text-red-400 px-3 py-2 rounded-md font-medium transition-colors">Trang chủ</Link>
              <Link href="/#now-showing" onClick={(e) => handleScroll(e, 'now-showing')} className="hover:text-red-400 px-3 py-2 rounded-md font-medium transition-colors">Phim đang chiếu</Link>
              <Link href="/#coming-soon" onClick={(e) => handleScroll(e, 'coming-soon')} className="hover:text-red-400 px-3 py-2 rounded-md font-medium transition-colors">Phim sắp chiếu</Link>
              <Link href="/#events-news" onClick={(e) => handleScroll(e, 'events-news')} className="hover:text-red-400 px-3 py-2 rounded-md font-medium transition-colors">Khuyến mãi/Tin tức</Link>
            </div>
          </div>

          <div>
            {mounted ? (
              isAuthenticated ? (
                <Dropdown menu={{ items: userMenuItems }} placement="bottomRight">
                  <div className="cursor-pointer flex items-center gap-2 hover:bg-gray-800 px-3 py-2 rounded transition-colors">
                    <Avatar icon={<UserOutlined />} className="bg-red-500" />
                    <span className="font-medium hidden sm:block">{fullName || 'User'}</span>
                  </div>
                </Dropdown>
              ) : (
                <Link href="/login">
                  <Button type="primary" danger shape="round">Đăng nhập</Button>
                </Link>
              )
            ) : (
              <div className="w-[100px] h-[32px]"></div>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}
