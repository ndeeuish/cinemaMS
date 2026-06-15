export interface FilterBase {
  pageIndex: number;
  pageSize: number;
  keyword?: string;
  roleId?: number; // Specifically for Users filter
  genreId?: number; // Specifically for Movies filter
}

export interface PagedResult<T> {
  items: T[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
  pageSize: number;
}

export interface CinemaDto {
  id: number;
  name: string;
  address: string;
  hotline: string;
  createdAt?: string;
}

export interface RoomDto {
  id: number;
  cinemaId: number;
  name: string;
  capacity: number;
  roomTypeId: number;
  roomTypeName?: string;
  createdAt?: string;
}

export interface MovieDto {
  id: number;
  title: string;
  description: string;
  durationInMinutes: number;
  releaseDate: string;
  director: string;
  casts: string;
  posterUrl: string;
  trailerUrl: string;
  ageRestrictionId: number;
  ageRestrictionCode: string;
  genreIds: number[];
  genres: string[];
  createdAt?: string;
}

export interface ShowtimeDto {
  id: number;
  movieId: number;
  roomId: number;
  startTime: string;
  endTime: string;
  basePrice: number;
  createdAt?: string;
}

export interface UserDto {
  id: number;
  username: string;
  email: string;
  fullName: string;
  phoneNumber: string;
  roleId: number;
  roleName?: string;
  isDeleted: boolean;
  createdAt?: string;
}

export interface GenreDto {
  id: number;
  name: string;
}

export interface AgeRestrictionDto {
  id: number;
  code: string;
  description: string;
}

export interface RoomTypeDto {
  id: number;
  name: string;
  surcharge: number;
}

export interface SeatTypeDto {
  id: number;
  name: string;
  surcharge: number;
}

export interface SeatDto {
  id: number;
  roomId: number;
  rowIndex: string;
  seatNumber: number;
  columnIndex: number;
  seatTypeId: number;
  seatTypeName: string;
  code: string;
}

export interface BookingAdminDto {
  id: number;
  userId: number;
  userName: string;
  userEmail: string;
  totalAmount: number;
  status: string;
  createdAt: string;
  movieTitle: string;
  cinemaName: string;
  roomName: string;
  showtimeStart?: string;
  ticketCount: number;
  seatCodes: string;
}
