export interface MovieDto {
  id: number;
  title: string;
  description: string;
  durationInMinutes: number;
  releaseDate: string;
  posterUrl: string;
  trailerUrl: string;
  language?: string;
  ageRestrictionCode: string;
  ageRestrictionName?: string;
  director: string;
  casts: string;
  genres: string[];
}

export interface ShowtimeDto {
  id: number;
  movieId: number;
  roomId: number;
  roomName: string;
  cinemaName: string;
  startTime: string;
  endTime: string;
  basePrice: number;
}

export interface SeatDto {
  id: number;
  roomId: number;
  rowIndex: string;
  columnIndex: number;
  seatNumber: number;
  seatTypeName: string;
  priceModifier: number;
  isActive?: boolean;
}

export interface TicketDto {
  id: number;
  showtimeId: number;
  movieTitle: string;
  seatCode: string;
  price: number;
}

export interface BookingResponseDto {
  id: number;
  status: string;
  holdExpiration: string;
  totalAmount: number;
  movieTitle: string;
  cinemaName: string;
  roomName: string;
  showtimeStart: string;
  tickets: TicketDto[];
}
