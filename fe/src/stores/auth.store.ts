import { create } from 'zustand';
import Cookies from 'js-cookie';

interface AuthState {
  isAuthenticated: boolean;
  username: string | null;
  fullName: string | null;
  role: string | null;
  
  // Actions
  login: (token: string, username: string, fullName: string, role: string) => void;
  logout: () => void;
}

const getStoredItem = (key: string) => {
  if (typeof window !== 'undefined') {
    return localStorage.getItem(key);
  }
  return null;
};

export const useAuthStore = create<AuthState>((set) => ({
  isAuthenticated: !!Cookies.get('token'), // Check initial state from cookie
  username: getStoredItem('username'),
  fullName: getStoredItem('fullName'),
  role: getStoredItem('role'),

  login: (token: string, username: string, fullName: string, role: string) => {
    Cookies.set('token', token, { expires: 1 }); // Expires in 1 day
    if (typeof window !== 'undefined') {
      localStorage.setItem('username', username);
      localStorage.setItem('fullName', fullName);
      localStorage.setItem('role', role);
    }
    set({ isAuthenticated: true, username, fullName, role });
  },

  logout: () => {
    Cookies.remove('token');
    if (typeof window !== 'undefined') {
      localStorage.removeItem('username');
      localStorage.removeItem('fullName');
      localStorage.removeItem('role');
    }
    set({ isAuthenticated: false, username: null, fullName: null, role: null });
  },
}));
