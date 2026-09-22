import api from './api';
import { CinemaDto, MovieDto, RoomDto, ShowtimeDto, UserDto, FilterBase, PagedResult, BookingAdminDto } from '../types/admin.type';

export const adminService = {
  // Cinemas
  getCinemas: async (params?: FilterBase): Promise<PagedResult<CinemaDto>> => {
    const response = await api.get<PagedResult<CinemaDto>>('/Cinemas/Pagging-cinema', { params });
    return response.data;
  },
  createCinema: async (data: Partial<CinemaDto>): Promise<number> => {
    const response = await api.post('/Cinemas/Create-cinema', data);
    return response.data;
  },
  updateCinema: async (id: number, data: Partial<CinemaDto>): Promise<void> => {
    await api.put(`/Cinemas/Update-cinema/${id}`, data);
  },
  deleteCinema: async (id: number): Promise<void> => {
    await api.delete(`/Cinemas/Delete-cinema/${id}`);
  },

  // Rooms
  getRoomsByCinema: async (cinemaId: number, params?: FilterBase): Promise<PagedResult<RoomDto>> => {
    const response = await api.get<PagedResult<RoomDto>>(`/Rooms/Get-room-by-cinema/${cinemaId}`, { params });
    return response.data;
  },
  createRoom: async (data: Partial<RoomDto>): Promise<number> => {
    const response = await api.post('/Rooms/Create-room', data);
    return response.data;
  },
  updateRoom: async (id: number, data: Partial<RoomDto>): Promise<void> => {
    await api.put(`/Rooms/Update-room/${id}`, data);
  },
  deleteRoom: async (id: number): Promise<void> => {
    await api.delete(`/Rooms/Delete-room/${id}`);
  },

  // Seats Bulk
  getSeatsByRoom: async (roomId: number): Promise<any[]> => {
    const response = await api.get<any[]>(`/Seats/Get-seat-by-room/${roomId}`);
    return response.data;
  },
  generateSeatsBulk: async (roomId: number, matrix: number[][]): Promise<void> => {
    await api.post('/Seats/bulk', { roomId, matrix });
  },

  // Movies
  getMovies: async (params?: FilterBase): Promise<PagedResult<MovieDto>> => {
    const response = await api.get<PagedResult<MovieDto>>('/Movies/Pagging-movie', { params });
    return response.data;
  },
  createMovie: async (data: Partial<MovieDto>): Promise<number> => {
    const response = await api.post('/Movies/Create-movie', data);
    return response.data;
  },
  updateMovie: async (id: number, data: Partial<MovieDto>): Promise<void> => {
    await api.put(`/Movies/Update-movie/${id}`, data);
  },
  deleteMovie: async (id: number): Promise<void> => {
    await api.delete(`/Movies/Delete-movie/${id}`);
  },

  // Showtimes
  getShowtimesByCinema: async (cinemaId: number, params?: FilterBase): Promise<PagedResult<ShowtimeDto>> => {
    const response = await api.get<PagedResult<ShowtimeDto>>(`/Showtimes/Get-showtime-by-cinema/${cinemaId}`, { params });
    return response.data;
  },
  createShowtime: async (data: Partial<ShowtimeDto>): Promise<number> => {
    const response = await api.post('/Showtimes/Create-showtime', data);
    return response.data;
  },
  updateShowtime: async (id: number, data: Partial<ShowtimeDto>): Promise<void> => {
    await api.put(`/Showtimes/Update-showtime/${id}`, data);
  },
  deleteShowtime: async (id: number): Promise<void> => {
    await api.delete(`/Showtimes/Delete-showtime/${id}`);
  },

  // Users
  getUsers: async (params?: FilterBase): Promise<PagedResult<UserDto>> => {
    const response = await api.get<PagedResult<UserDto>>('/users', { params });
    return response.data;
  },
  updateUser: async (id: number, data: Partial<UserDto>): Promise<void> => {
    await api.put(`/users/${id}`, data);
  },
  deleteUser: async (id: number): Promise<void> => {
    await api.delete(`/users/${id}`);
  },

  // Catalog
  getGenres: async (): Promise<any[]> => {
    const response = await api.get<any[]>('/Catalog/genres');
    return response.data;
  },
  getAgeRestrictions: async (): Promise<any[]> => {
    const response = await api.get<any[]>('/Catalog/age-restrictions');
    return response.data;
  },
  getRoomTypes: async (): Promise<any[]> => {
    const response = await api.get<any[]>('/Catalog/room-types');
    return response.data;
  },
  getSeatTypes: async (): Promise<any[]> => {
    const response = await api.get<any[]>('/Catalog/seat-types');
    return response.data;
  },

  // Bookings
  getBookings: async (params?: FilterBase): Promise<PagedResult<BookingAdminDto>> => {
    const response = await api.get<PagedResult<BookingAdminDto>>('/Bookings/Get-all-bookings', { params });
    return response.data;
  },

  // Uploads
  uploadImage: async (file: File, folder: string, tag?: string): Promise<{ url: string, publicId: string }> => {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('folder', folder);
    if (tag) {
      formData.append('tag', tag);
    }
    const response = await api.post<{ url: string, publicId: string }>('/Uploads/image', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
    return response.data;
  },
  deleteImage: async (publicId: string): Promise<void> => {
    await api.delete(`/Uploads/image?publicId=${encodeURIComponent(publicId)}`);
  }
};
