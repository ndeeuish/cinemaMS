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
      } else if (error.response.status >= 500) {
        if (typeof window !== 'undefined') {
          message.error('Lỗi hệ thống từ máy chủ (500)!');
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
