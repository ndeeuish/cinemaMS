import api from './api';
import { MovieDto, ShowtimeDto, SeatDto, BookingResponseDto, ArticleDto, PagedResult } from '../types/client.type';

export const clientService = {
  getMovies: async (): Promise<MovieDto[]> => {
    const response = await api.get<any>('/Movies/Pagging-movie');
    return response.data.items || response.data; // Fallback to response.data in case it's still an array somehow
  },

  getMovieById: async (id: number): Promise<MovieDto> => {
    const response = await api.get<MovieDto>(`/Movies/Get-movie/${id}`);
    return response.data;
  },

  getShowtimesByMovie: async (movieId: number): Promise<ShowtimeDto[]> => {
    const response = await api.get<ShowtimeDto[]>(`/Showtimes/Get-showtime-by-movie/${movieId}`);
    return response.data;
  },
  
  getShowtimeById: async (showtimeId: number): Promise<ShowtimeDto> => {
    const response = await api.get<ShowtimeDto>(`/Showtimes/Get-showtime/${showtimeId}`);
    return response.data;
  },

  getSeatsByRoom: async (roomId: number): Promise<SeatDto[]> => {
    const response = await api.get<SeatDto[]>(`/Seats/Get-seat-by-room/${roomId}`);
    return response.data;
  },

  getReservedSeats: async (showtimeId: number): Promise<number[]> => {
    const response = await api.get<number[]>(`/Bookings/showtime/${showtimeId}/get-reserved-seats`);
    return response.data;
  },

  createBooking: async (showtimeId: number, seatIds: number[]): Promise<BookingResponseDto> => {
    const response = await api.post<BookingResponseDto>('/Bookings/Create-booking', { showtimeId, seatIds });
    return response.data;
  },
  
  getMyBookings: async (): Promise<BookingResponseDto[]> => {
    const response = await api.get<BookingResponseDto[]>('/Bookings/get-my-bookings');
    return response.data;
  },

  getBookingById: async (bookingId: number): Promise<BookingResponseDto> => {
    // Fallback since BE has no getById. Get all my bookings and find the one.
    const response = await api.get<BookingResponseDto[]>('/Bookings/get-my-bookings');
    const booking = response.data.find(b => b.id === bookingId);
    if (!booking) throw new Error("Booking not found");
    return booking;
  },

  createPaymentUrl: async (bookingId: number): Promise<{ url: string }> => {
    const response = await api.post<{ url: string }>('/Payments/create-url', bookingId);
    return response.data;
  },

  getArticles: async (pageIndex = 1, pageSize = 6, type?: number, isActive: boolean = true): Promise<PagedResult<ArticleDto>> => {
    let url = `/Articles/Pagging-article?PageIndex=${pageIndex}&PageSize=${pageSize}&IsActive=${isActive}`;
    if (type !== undefined) {
      url += `&Type=${type}`;
    }
    const response = await api.get<PagedResult<ArticleDto>>(url);
    return response.data;
  },

  getArticleById: async (id: number | string): Promise<ArticleDto> => {
    const response = await api.get<ArticleDto>(`/Articles/Get-article/${id}`);
    return response.data;
  },
};
