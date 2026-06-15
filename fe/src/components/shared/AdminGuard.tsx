'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { useAuthStore } from '@/stores/auth.store';
import { Spin } from 'antd';

export default function AdminGuard({ children }: { children: React.ReactNode }) {
  const { isAuthenticated, role } = useAuthStore();
  const router = useRouter();
  const [isAuthorized, setIsAuthorized] = useState(false);

  useEffect(() => {
    if (!isAuthenticated) {
      // Not logged in -> Redirect to login
      router.push('/login');
    } else if (role?.toLowerCase() !== 'admin' && role?.toLowerCase() !== 'manager') {
      // Logged in but not admin/manager -> Redirect to home
      router.push('/');
    } else {
      // Admin -> Grant access
      setIsAuthorized(true);
    }
  }, [isAuthenticated, role, router]);

  if (!isAuthorized) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <Spin size="large" description="Đang kiểm tra quyền truy cập..." />
      </div>
    );
  }

  return <>{children}</>;
}
