'use client';

import { Suspense } from 'react';
import { useSearchParams } from 'next/navigation';
import { Card, Button, Result, Spin, Typography } from 'antd';

const { Text } = Typography;

function CheckoutResultContent() {
  const searchParams = useSearchParams();
  const status = searchParams.get('status');
  const bookingId = searchParams.get('bookingId');
  const txnId = searchParams.get('txnId');
  const message = searchParams.get('message');

  if (status === 'success') {
    return (
      <Card className="shadow-lg rounded-xl text-center">
        <Result
          status="success"
          title="Thanh toán thành công!"
          subTitle={`Mã giao dịch VNPAY: ${txnId}. Đơn hàng #${bookingId}. Vé của bạn đã được xác nhận.`}
          extra={[
            <Button type="primary" key="home" href="/">
              Về Trang Chủ
            </Button>,
            <Button key="tickets" href="/profile">
              Xem Vé Của Tôi
            </Button>,
          ]}
        />
      </Card>
    );
  }

  return (
    <Card className="shadow-lg rounded-xl text-center">
      <Result
        status="error"
        title="Thanh toán thất bại"
        subTitle={message || "Giao dịch không thành công hoặc đã bị huỷ."}
        extra={[
          <Button type="primary" key="home" href="/">
            Về Trang Chủ
          </Button>
        ]}
      />
    </Card>
  );
}

export default function CheckoutResultPage() {
  return (
    <div className="max-w-2xl mx-auto py-16 px-4">
      <Suspense fallback={<div className="flex justify-center py-20"><Spin size="large" /></div>}>
        <CheckoutResultContent />
      </Suspense>
    </div>
  );
}
