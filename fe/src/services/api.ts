import axios from 'axios';
import Cookies from 'js-cookie';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'https://localhost:7212/api';
const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request Interceptor: Auto-attach JWT token
api.interceptors.request.use(
  (config) => {
    const token = Cookies.get('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

import { message } from 'antd';

// Response Interceptor: Handle errors globally
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response) {
      if (error.response.status === 401) {
        Cookies.remove('token');
        if (typeof window !== 'undefined') {
          window.location.href = '/login';
        }
      } else {
        // Show toast notification for 400, 500, etc.
        const errorMsg = error.response.data?.message || 'Có lỗi xảy ra từ máy chủ!';
        if (typeof window !== 'undefined') {
          message.error(errorMsg);
        }
      }
    } else {
      if (typeof window !== 'undefined') {
        message.error('Không thể kết nối đến máy chủ!');
      }
    }
    return Promise.reject(error);
  }
);

export default api;
