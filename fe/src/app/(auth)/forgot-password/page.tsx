'use client';

import { useState, useEffect } from 'react';
import { Form, Input, Button, Card, message } from 'antd';
import { MailOutlined, LockOutlined, KeyOutlined } from '@ant-design/icons';
import { authService } from '@/services/auth.service';
import { useRouter } from 'next/navigation';

export default function ForgotPasswordPage() {
  const [loading, setLoading] = useState(false);
  const [step, setStep] = useState<1 | 2>(1);
  const [email, setEmail] = useState('');
  const [countdown, setCountdown] = useState(0);
  const router = useRouter();
  const [form] = Form.useForm();

  useEffect(() => {
    let timer: NodeJS.Timeout;
    if (countdown > 0) {
      timer = setInterval(() => {
        setCountdown((prev) => prev - 1);
      }, 1000);
    }
    return () => clearInterval(timer);
  }, [countdown]);

  const onSendOtp = async (values: { email: string }) => {
    if (countdown > 0) {
      message.warning(`Vui lòng đợi ${countdown}s để nhận lại mã.`);
      return;
    }
    
    setLoading(true);
    try {
      await authService.forgotPassword({ email: values.email });
      setEmail(values.email);
      setStep(2);
      setCountdown(60);
      message.success('Mã OTP đã được gửi đến email của bạn.');
    } catch (error: any) {
      const errorMsg = error.response?.data?.details || error.response?.data?.message || 'Có lỗi xảy ra, vui lòng thử lại.';
      message.error(errorMsg);
    } finally {
      setLoading(false);
    }
  };

  const onResetPassword = async (values: any) => {
    setLoading(true);
    try {
      await authService.resetPassword({
        email: email,
        otp: values.otp,
        newPassword: values.newPassword
      });
      message.success('Đổi mật khẩu thành công! Vui lòng đăng nhập lại.');
      router.push('/login');
    } catch (error: any) {
      const errorMsg = error.response?.data?.details || error.response?.data?.message || 'Mã OTP không hợp lệ hoặc đã hết hạn.';
      message.error(errorMsg);
    } finally {
      setLoading(false);
    }
  };

  const resendOtp = async () => {
    if (countdown > 0) return;
    
    setLoading(true);
    try {
      await authService.forgotPassword({ email: email });
      setCountdown(60);
      message.success('Mã OTP đã được gửi lại.');
    } catch (error: any) {
      const errorMsg = error.response?.data?.details || error.response?.data?.message || 'Vui lòng chờ 60s trước khi gửi lại yêu cầu.';
      message.error(errorMsg);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <Card title={<h2 className="text-center text-2xl font-bold m-0">Quên Mật Khẩu</h2>} className="w-full max-w-md shadow-lg">
        {step === 1 && (
          <Form
            name="forgot_password_form"
            onFinish={onSendOtp}
            layout="vertical"
            size="large"
          >
            <p className="text-gray-600 mb-4 text-center">
              Nhập email đăng ký tài khoản của bạn. Hệ thống sẽ gửi mã xác nhận để đặt lại mật khẩu.
            </p>
            <Form.Item
              name="email"
              rules={[
                { required: true, message: 'Vui lòng nhập email!' },
                { type: 'email', message: 'Email không hợp lệ!' }
              ]}
            >
              <Input prefix={<MailOutlined />} placeholder="Email của bạn" />
            </Form.Item>

            <Form.Item>
              <Button type="primary" htmlType="submit" className="w-full" loading={loading}>
                Nhận mã xác nhận (OTP)
              </Button>
            </Form.Item>
            
            <div className="text-center mt-4">
              <a href="/login" className="text-blue-600 hover:underline">Quay lại Đăng nhập</a>
            </div>
          </Form>
        )}

        {step === 2 && (
          <Form
            form={form}
            name="reset_password_form"
            onFinish={onResetPassword}
            layout="vertical"
            size="large"
          >
            <p className="text-gray-600 mb-4 text-center">
              Mã OTP đã được gửi đến <span className="font-bold">{email}</span>. Mã có hiệu lực trong 5 phút.
            </p>

            <Form.Item
              name="otp"
              rules={[
                { required: true, message: 'Vui lòng nhập mã OTP!' },
                { len: 6, message: 'Mã OTP phải có 6 chữ số!' }
              ]}
            >
              <Input prefix={<KeyOutlined />} placeholder="Nhập mã OTP 6 số" maxLength={6} />
            </Form.Item>

            <Form.Item
              name="newPassword"
              rules={[
                { required: true, message: 'Vui lòng nhập mật khẩu mới!' },
                { min: 6, message: 'Mật khẩu phải có ít nhất 6 ký tự!' }
              ]}
            >
              <Input.Password prefix={<LockOutlined />} placeholder="Mật khẩu mới" />
            </Form.Item>

            <Form.Item>
              <Button type="primary" htmlType="submit" className="w-full" loading={loading}>
                Đặt lại mật khẩu
              </Button>
            </Form.Item>
            
            <div className="text-center mt-4 text-sm">
              Chưa nhận được mã?{' '}
              <Button 
                type="link" 
                className="p-0 border-0 h-auto" 
                disabled={countdown > 0} 
                onClick={resendOtp}
                loading={loading && countdown === 0}
              >
                {countdown > 0 ? `Gửi lại sau ${countdown}s` : 'Gửi lại mã'}
              </Button>
            </div>
            <div className="text-center mt-2">
              <a href="/login" className="text-gray-500 hover:underline text-sm">Hủy bỏ</a>
            </div>
          </Form>
        )}
      </Card>
    </div>
  );
}
