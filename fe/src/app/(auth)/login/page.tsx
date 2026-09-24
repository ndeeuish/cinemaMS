'use client';

import { useState } from 'react';
import { Form, Input, Button, Card, message } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { authService } from '@/services/auth.service';
import { useAuthStore } from '@/stores/auth.store';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { getErrorMessage } from '@/utils/error.util';

export default function LoginPage() {
  const [loading, setLoading] = useState(false);
  const login = useAuthStore((state) => state.login);
  const router = useRouter();

  const onFinish = async (values: any) => {
    setLoading(true);
    try {
      const data = await authService.login(values);
      // Save to global state (Zustand) and cookies
      login(data.token, data.username, data.fullName, data.role);
      
      message.success('Đăng nhập thành công!');
      
      // Redirect based on role or returnUrl
      const returnUrl = localStorage.getItem('returnUrl');
      if (returnUrl) {
        localStorage.removeItem('returnUrl');
        router.push(returnUrl);
      } else if (data.role === 'Admin') {
        router.push('/admin');
      } else {
        router.push('/');
      }
    } catch (err: any) {
      message.error(getErrorMessage(err, 'Đăng nhập thất bại!'));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <Card title={<h2 className="text-center text-2xl font-bold m-0">Đăng nhập CinemaMS</h2>} className="w-full max-w-md shadow-lg">
        <Form
          name="login_form"
          initialValues={{ remember: true }}
          onFinish={onFinish}
          layout="vertical"
          size="large"
        >
          <Form.Item
            name="username"
            rules={[{ required: true, message: 'Vui lòng nhập tên tài khoản!' }]}
          >
            <Input prefix={<UserOutlined />} placeholder="Tên tài khoản" />
          </Form.Item>

          <Form.Item
            name="password"
            rules={[{ required: true, message: 'Vui lòng nhập mật khẩu!' }]}
          >
            <Input.Password prefix={<LockOutlined />} placeholder="Mật khẩu" />
          </Form.Item>

          <div className="flex justify-end mb-4 -mt-2">
            <a href="/forgot-password" className="text-blue-600 hover:underline text-sm">Quên mật khẩu?</a>
          </div>

          <Form.Item>
            <Button type="primary" htmlType="submit" className="w-full" loading={loading}>
              Đăng nhập
            </Button>
          </Form.Item>

          <div className="text-center mt-4">
            Chưa có tài khoản? <a href="/register" className="text-blue-600 hover:underline">Đăng ký ngay</a>
          </div>
        </Form>
      </Card>
    </div>
  );
}
