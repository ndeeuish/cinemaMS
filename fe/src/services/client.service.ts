import api from './api';
import { MovieDto, ShowtimeDto, SeatDto, BookingResponseDto } from '../types/client.type';

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
  
  getBookingById: async (bookingId: number): Promise<BookingResponseDto> => {
    // Fallback since BE has no getById. Get all my bookings and find the one.
    const response = await api.get<BookingResponseDto[]>(`/Bookings/get-my-bookings`);
    const booking = response.data.find(b => b.id === bookingId);
    if (!booking) throw new Error("Booking not found");
    return booking;
  },

  confirmPayment: async (bookingId: number): Promise<{ success: boolean; message: string }> => {
    const response = await api.post<{ success: boolean; message: string }>(`/Bookings/${bookingId}/confirm-payment`);
    return response.data;
  },
};
