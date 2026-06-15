'use client';

import { Layout } from 'antd';
import AdminGuard from '@/components/shared/AdminGuard';
import AdminSidebar from '@/components/admin/AdminSidebar';
import AdminHeader from '@/components/admin/AdminHeader';

const { Content } = Layout;

export default function AdminLayout({ children }: { children: React.ReactNode }) {
  return (
    <AdminGuard>
      <Layout className="min-h-screen">
        <AdminSidebar />
        <Layout>
          <AdminHeader />
          <Content className="m-6 p-6 bg-white rounded-lg shadow-sm">
            {children}
          </Content>
        </Layout>
      </Layout>
    </AdminGuard>
  );
}
