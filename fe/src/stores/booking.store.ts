import { create } from 'zustand';
import { SeatDto } from '../types/client.type';

interface BookingState {
  selectedSeats: SeatDto[];
  showtimeId: number | null;
  
  toggleSeat: (seat: SeatDto) => void;
  setShowtime: (id: number) => void;
  clearBooking: () => void;
  getTotalPrice: (basePrice: number) => number;
}

export const useBookingStore = create<BookingState>((set, get) => ({
  selectedSeats: [],
  showtimeId: null,

  toggleSeat: (seat) => set((state) => {
    const isSelected = state.selectedSeats.some(s => s.id === seat.id);
    if (isSelected) {
      return { selectedSeats: state.selectedSeats.filter(s => s.id !== seat.id) };
    } else {
      // Limit to 8 seats per booking for example
      if (state.selectedSeats.length >= 8) {
        return state;
      }
      return { selectedSeats: [...state.selectedSeats, seat] };
    }
  }),

  setShowtime: (id) => set({ showtimeId: id, selectedSeats: [] }), // clear seats when changing showtime

  clearBooking: () => set({ selectedSeats: [], showtimeId: null }),

  getTotalPrice: (basePrice) => {
    const { selectedSeats } = get();
    return selectedSeats.reduce((total, seat) => total + basePrice + seat.priceModifier, 0);
  }
}));
