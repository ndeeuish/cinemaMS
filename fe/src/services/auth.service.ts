import api from './api';
import { AuthResponseDto } from '../types/auth.type';

export const authService = {
  login: async (credentials: any): Promise<AuthResponseDto> => {
    const response = await api.post<AuthResponseDto>('/Auth/login', credentials);
    return response.data;
  },
  register: async (data: any): Promise<any> => {
    const response = await api.post('/Auth/register', data);
    return response.data;
  },
  forgotPassword: async (data: { email: string }): Promise<any> => {
    const response = await api.post('/Auth/forgot-password', data);
    return response.data;
  },
  resetPassword: async (data: any): Promise<any> => {
    const response = await api.post('/Auth/reset-password', data);
    return response.data;
  },
};
