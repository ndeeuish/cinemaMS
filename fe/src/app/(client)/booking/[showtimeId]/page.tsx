'use client';

import { useEffect, useState, use } from 'react';
import { Spin, Button, message, Divider, Alert } from 'antd';
import { clientService } from '@/services/client.service';
import { SeatDto, ShowtimeDto, MovieDto } from '@/types/client.type';
import { useBookingStore } from '@/stores/booking.store';
import { useAuthStore } from '@/stores/auth.store';
import { useRouter } from 'next/navigation';
import dayjs from 'dayjs';

export default function BookingPage({ params }: { params: Promise<{ showtimeId: string }> }) {
  const resolvedParams = use(params);
  const showtimeId = parseInt(resolvedParams.showtimeId);
  const router = useRouter();

  const [loading, setLoading] = useState(true);
  const [showtime, setShowtime] = useState<ShowtimeDto | null>(null);
  const [movie, setMovie] = useState<MovieDto | null>(null);
  const [seats, setSeats] = useState<SeatDto[]>([]);
  const [reservedSeatIds, setReservedSeatIds] = useState<number[]>([]);
  
  const { selectedSeats, toggleSeat, setShowtime: setStoreShowtime, getTotalPrice } = useBookingStore();

  useEffect(() => {
    if (showtimeId) {
      setStoreShowtime(showtimeId);
      fetchData();
      
      // Poll for reserved seats every 10 seconds to avoid conflicts
      const interval = setInterval(fetchReservedSeats, 10000);
      return () => clearInterval(interval);
    }
  }, [showtimeId, setStoreShowtime]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const stData = await clientService.getShowtimeById(showtimeId);
      setShowtime(stData);
      
      const [movieData, seatsData, reservedData] = await Promise.all([
        clientService.getMovieById(stData.movieId),
        clientService.getSeatsByRoom(stData.roomId),
        clientService.getReservedSeats(showtimeId)
      ]);
      
      setMovie(movieData);
      setSeats(seatsData);
      setReservedSeatIds(reservedData);
    } catch (error) {
      message.error('Không thể tải dữ liệu sơ đồ ghế!');
    } finally {
      setLoading(false);
    }
  };

  const fetchReservedSeats = async () => {
    try {
      const reservedData = await clientService.getReservedSeats(showtimeId);
      setReservedSeatIds(reservedData);
    } catch (error) {
      console.error('Failed to update reserved seats');
    }
  };

  const handleSeatClick = (seat: SeatDto) => {
    if (seat.isActive === false || reservedSeatIds.includes(seat.id)) return;
    
    // Auto-select paired seat if Sweetbox
    if (seat.seatTypeName === 'Sweetbox') {
      // Find the pair (assuming Sweetbox comes in pairs, e.g., J1-J2)
      // This is a simplified approach. In a real scenario, you'd match by logic.
      // For this MVP, we just let them pick one by one or implement pair logic if backend supports it.
      toggleSeat(seat);
    } else {
      toggleSeat(seat);
    }
  };

  const { isAuthenticated } = useAuthStore();

  const handleCheckout = async () => {
    if (!isAuthenticated) {
      message.warning('Vui lòng đăng nhập để tiếp tục đặt vé!');
      localStorage.setItem('returnUrl', `/booking/${showtimeId}`);
      router.push('/login');
      return;
    }

    if (selectedSeats.length === 0) {
      message.warning('Vui lòng chọn ít nhất 1 ghế!');
      return;
    }
    
    setLoading(true);
    try {
      const seatIds = selectedSeats.map(s => s.id);
      const booking = await clientService.createBooking(showtimeId, seatIds);
      message.success('Giữ ghế thành công! Đang chuyển đến thanh toán...');
      router.push(`/checkout/${booking.id}`);
    } catch (error: any) {
      const errorMsg = error.response?.data?.message || 'Ghế đã bị đặt hoặc xảy ra lỗi!';
      message.error(errorMsg);
      // Refresh reserved seats to reflect the conflict
      fetchReservedSeats();
    } finally {
      setLoading(false);
    }
  };

  // Group seats by row
  const rowNames = Array.from(new Set(seats.map(s => s.rowIndex))).sort();

  const getSeatColor = (seat: SeatDto) => {
    if (seat.isActive === false) return 'bg-gray-300 cursor-not-allowed opacity-30';
    if (reservedSeatIds.includes(seat.id)) return 'bg-red-500 cursor-not-allowed border-red-700 text-transparent'; // Reserved
    
    const isSelected = selectedSeats.some(s => s.id === seat.id);
    if (isSelected) return 'bg-green-500 border-green-600 text-white shadow-lg transform scale-110';
    
    if (seat.seatTypeName === 'VIP') return 'bg-yellow-400 border-yellow-600 hover:bg-yellow-300';
    if (seat.seatTypeName === 'Sweetbox') return 'bg-pink-400 border-pink-600 hover:bg-pink-300'; // Width handled in render
    
    return 'bg-white border-gray-400 hover:bg-gray-100'; // Standard
  };

  if (loading && !showtime) return <div className="flex justify-center py-32"><Spin size="large" /></div>;

  return (
    <div className="min-h-screen bg-gray-900 text-gray-200 pb-20">
      {/* Header Info */}
      <div className="bg-gray-800 shadow-lg py-4 px-6 sticky top-0 z-40">
        <div className="max-w-6xl mx-auto flex flex-col md:flex-row justify-between items-center gap-4">
          <div>
            <h1 className="text-2xl font-bold text-white m-0">{movie?.title}</h1>
            <div className="text-gray-400 text-sm mt-1">
              {showtime?.cinemaName} - Phòng {showtime?.roomName} | {dayjs(showtime?.startTime).format('HH:mm - DD/MM/YYYY')}
            </div>
          </div>
          <div className="text-right">
            <div className="text-sm text-gray-400">Tạm tính</div>
            <div className="text-3xl font-bold text-red-500">
              {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(showtime ? getTotalPrice(showtime.basePrice) : 0)}
            </div>
            <Button 
              type="primary" 
              danger 
              size="large" 
              className="mt-2 font-bold px-8" 
              disabled={selectedSeats.length === 0}
              loading={loading}
              onClick={handleCheckout}
            >
              MUA VÉ
            </Button>
          </div>
        </div>
      </div>

      <div className="max-w-4xl mx-auto mt-8 px-4">
        {selectedSeats.length >= 8 && (
          <Alert type="warning" message="Bạn chỉ được chọn tối đa 8 ghế cho mỗi lần đặt" className="mb-4" showIcon />
        )}

        {/* Screen */}
        <div className="mb-16">
          <div className="w-full h-8 bg-gradient-to-b from-gray-300 to-transparent rounded-t-[50%] blur-[2px] opacity-70 border-t-4 border-white mb-2"></div>
          <div className="text-center tracking-[0.5em] text-gray-400 font-bold text-sm">MÀN HÌNH</div>
        </div>

        {/* Seat Matrix */}
        <div className="overflow-x-auto pb-8">
          <div className="min-w-[600px] flex flex-col items-center gap-3">
            {rowNames.map((row, index) => {
              const rowSeats = seats.filter(s => s.rowIndex === row);
              const maxCol = Math.max(...seats.map(s => s.columnIndex));
              
              return (
                <div key={`row-${row || index}`} className="flex items-center gap-3 w-full justify-center">
                  <div className="w-6 font-bold text-gray-500 text-center">{row}</div>
                  <div className="flex gap-2 justify-center">
                    {Array.from({ length: maxCol }).map((_, colIndex) => {
                      const colNum = colIndex + 1;
                      const seat = rowSeats.find(s => s.columnIndex === colNum);
                      
                      if (!seat) {
                        return <div key={`empty-${row}-${colNum}`} className="w-8 h-8 pointer-events-none"></div>;
                      }
                      
                      return (
                        <div 
                          key={seat.id}
                          onClick={() => handleSeatClick(seat)}
                          className={`
                            h-8 rounded-t-lg rounded-b-sm border-2 flex items-center justify-center text-xs font-semibold
                            transition-all duration-200 cursor-pointer
                            ${getSeatColor(seat)}
                            ${seat.seatTypeName === 'Sweetbox' ? 'w-[72px]' : 'w-8'}
                          `}
                          title={`${seat.rowIndex}${seat.seatNumber} - ${seat.seatTypeName}`}
                        >
                          {/* Hide number if reserved */}
                          {reservedSeatIds.includes(seat.id) ? 'X' : seat.seatNumber}
                        </div>
                      );
                    })}
                  </div>
                  <div className="w-6 font-bold text-gray-500 text-center">{row}</div>
                </div>
              );
            })}
          </div>
        </div>

        <Divider className="border-gray-700" />

        {/* Legend */}
        <div className="flex flex-wrap justify-center gap-6 text-sm">
          <div className="flex items-center gap-2">
            <div className="w-6 h-6 bg-white border-2 border-gray-400 rounded"></div> Thường
          </div>
          <div className="flex items-center gap-2">
            <div className="w-6 h-6 bg-yellow-400 border-2 border-yellow-600 rounded"></div> VIP
          </div>
          <div className="flex items-center gap-2">
            <div className="w-10 h-6 bg-pink-400 border-2 border-pink-600 rounded"></div> Sweetbox
          </div>
          <div className="flex items-center gap-2">
            <div className="w-6 h-6 bg-red-500 border-2 border-red-700 rounded flex items-center justify-center text-white text-xs">X</div> Đã đặt
          </div>
          <div className="flex items-center gap-2">
            <div className="w-6 h-6 bg-green-500 border-2 border-green-600 rounded"></div> Ghế đang chọn
          </div>
        </div>
      </div>
    </div>
  );
}
