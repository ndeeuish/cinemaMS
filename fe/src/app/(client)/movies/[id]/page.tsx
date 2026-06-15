'use client';

import { useEffect, useState, use } from 'react';
import { Card, Button, Spin, Typography, Tag, Divider, Row, Col, Alert } from 'antd';
import { clientService } from '@/services/client.service';
import { MovieDto, ShowtimeDto } from '@/types/client.type';
import Link from 'next/link';
import { ClockCircleOutlined, CalendarOutlined, GlobalOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import isSameOrAfter from 'dayjs/plugin/isSameOrAfter';
dayjs.extend(isSameOrAfter);

const { Title, Paragraph } = Typography;

export default function MovieDetailPage({ params }: { params: Promise<{ id: string }> }) {
  // In Next.js 15, params is a Promise
  const resolvedParams = use(params);
  const movieId = parseInt(resolvedParams.id);
  
  const [movie, setMovie] = useState<MovieDto | null>(null);
  const [showtimes, setShowtimes] = useState<ShowtimeDto[]>([]);
  const [loading, setLoading] = useState(true);
  
  // Generate 7 days starting from today with localized labels
  const sevenDays = Array.from({ length: 7 }).map((_, i) => {
    const d = dayjs().add(i, 'day');
    const daysVi = ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'];
    return {
      value: d.format('DD/MM/YYYY'),
      label: `${daysVi[d.day()]} ${d.format('DD/MM')}`
    };
  });
  
  const [selectedDate, setSelectedDate] = useState<string>(sevenDays[0].value);

  useEffect(() => {
    if (movieId) {
      fetchData();
    }
  }, [movieId]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const [movieData, showtimesData] = await Promise.all([
        clientService.getMovieById(movieId),
        clientService.getShowtimesByMovie(movieId)
      ]);
      setMovie(movieData);
      
      // Keep showtimes from today onwards
      const validShowtimes = showtimesData
        .filter(s => dayjs(s.startTime).startOf('day').isSameOrAfter(dayjs().startOf('day')))
        .sort((a, b) => dayjs(a.startTime).valueOf() - dayjs(b.startTime).valueOf());
        
      setShowtimes(validShowtimes);
    } catch (error) {
      console.error('Failed to fetch movie details:', error);
    } finally {
      setLoading(false);
    }
  };

  // Group showtimes by date
  const groupedShowtimes = showtimes.reduce((acc, showtime) => {
    const date = dayjs(showtime.startTime).format('DD/MM/YYYY');
    if (!acc[date]) {
      acc[date] = [];
    }
    acc[date].push(showtime);
    return acc;
  }, {} as Record<string, ShowtimeDto[]>);

  if (loading) return <div className="flex justify-center py-20"><Spin size="large" /></div>;
  if (!movie) return <div className="text-center py-20"><Alert type="error" title="Không tìm thấy phim!" /></div>;

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-10">
      <Row gutter={[32, 32]}>
        {/* Poster Column */}
        <Col xs={24} md={8} lg={6}>
          <div className="sticky top-24">
            <img 
              src={movie.posterUrl || 'https://via.placeholder.com/300x450'} 
              alt={movie.title} 
              className="w-full rounded-xl shadow-lg mb-4"
            />
            <Button type="primary" danger block size="large" onClick={() => {
              const trailerElement = document.getElementById('trailer-section');
              trailerElement?.scrollIntoView({ behavior: 'smooth' });
            }}>
              XEM TRAILER
            </Button>
          </div>
        </Col>

        {/* Info Column */}
        <Col xs={24} md={16} lg={18}>
          <Title level={1} className="mb-2">{movie.title}</Title>
          <div className="flex flex-wrap gap-2 mb-6">
            <Tag color="red" className="font-bold text-sm px-2 py-1">{movie.ageRestrictionCode || movie.ageRestrictionName}</Tag>
            <Tag icon={<ClockCircleOutlined />} color="blue" className="text-sm px-2 py-1">{movie.durationInMinutes} phút</Tag>
            {movie.language && <Tag icon={<GlobalOutlined />} color="cyan" className="text-sm px-2 py-1">{movie.language}</Tag>}
            <Tag icon={<CalendarOutlined />} className="text-sm px-2 py-1">Khởi chiếu: {dayjs(movie.releaseDate).format('DD/MM/YYYY')}</Tag>
          </div>

          <div className="mb-6 space-y-3">
            <div className="flex text-base">
              <span className="w-24 flex-shrink-0 font-semibold text-gray-800">Đạo diễn:</span>
              <span className="text-gray-800">{movie.director || 'Đang cập nhật'}</span>
            </div>
            <div className="flex text-base">
              <span className="w-24 flex-shrink-0 font-semibold text-gray-800">Diễn viên:</span>
              <span className="text-gray-800">{movie.casts || 'Đang cập nhật'}</span>
            </div>
            <div className="flex text-base">
              <span className="w-24 flex-shrink-0 font-semibold text-gray-800">Thể loại:</span>
              <span className="text-gray-800">{movie.genres?.length ? movie.genres.join(', ') : 'Đang cập nhật'}</span>
            </div>
          </div>

          <Title level={4}>Nội dung phim</Title>
          <Paragraph className="text-gray-600 text-base leading-relaxed mb-8">
            {movie.description}
          </Paragraph>

          <Divider />

          {/* Showtimes Section */}
          <Title level={3} className="mb-6">Lịch Chiếu</Title>
          
          <div className="space-y-6">
            {/* Date Selector */}
            <div className="flex gap-3 overflow-x-auto pb-2">
              {sevenDays.map(dateObj => (
                <Button 
                  key={dateObj.value}
                  type={selectedDate === dateObj.value ? 'primary' : 'default'}
                  danger={selectedDate === dateObj.value}
                  className="min-w-[110px] font-semibold"
                  onClick={() => setSelectedDate(dateObj.value)}
                >
                  {dateObj.label}
                </Button>
              ))}
            </div>

            {/* Showtimes for selected date */}
            {selectedDate && (
              (!groupedShowtimes[selectedDate] || groupedShowtimes[selectedDate].length === 0) ? (
                <Alert type="info" title={`Hiện tại chưa có lịch chiếu nào trong ngày ${selectedDate}.`} showIcon className="mt-4" />
              ) : (
                <Card className="shadow-sm border-0 bg-white" styles={{ body: { padding: '20px' } }}>
                  {Object.entries(
                    groupedShowtimes[selectedDate].reduce((acc, st) => {
                      if (!acc[st.cinemaName]) acc[st.cinemaName] = [];
                      acc[st.cinemaName].push(st);
                      return acc;
                    }, {} as Record<string, ShowtimeDto[]>)
                  ).map(([cinemaName, cinemaShowtimes]) => (
                    <div key={cinemaName} className="mb-6 last:mb-0">
                      <div className="font-semibold text-lg text-gray-800 mb-3 border-l-4 border-red-500 pl-3">
                        {cinemaName}
                      </div>
                      <div className="flex flex-wrap gap-3">
                        {cinemaShowtimes.map(st => {
                          const isPast = dayjs(st.startTime).isBefore(dayjs());
                          
                          if (isPast) {
                            return (
                              <Button 
                                key={st.id}
                                size="large" 
                                disabled
                                className="font-mono"
                                title="Đã qua giờ chiếu"
                              >
                                {dayjs(st.startTime).format('HH:mm')}
                              </Button>
                            );
                          }
                          
                          return (
                            <Link key={st.id} href={`/booking/${st.id}`}>
                              <Button 
                                size="large" 
                                className="font-mono text-gray-800 hover:text-red-500 hover:border-red-500"
                              >
                                {dayjs(st.startTime).format('HH:mm')}
                              </Button>
                            </Link>
                          );
                        })}
                      </div>
                    </div>
                  ))}
                </Card>
              )
            )}
          </div>

          {/* Trailer Section */}
          {movie.trailerUrl && (
            <div id="trailer-section" className="mt-12">
              <Title level={3}>Trailer</Title>
              <div className="aspect-w-16 aspect-h-9 rounded-xl overflow-hidden shadow-lg bg-black">
                <iframe 
                  src={movie.trailerUrl.replace('watch?v=', 'embed/')} 
                  title="Trailer"
                  frameBorder="0" 
                  allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" 
                  allowFullScreen
                  className="w-full h-96"
                ></iframe>
              </div>
            </div>
          )}

        </Col>
      </Row>
    </div>
  );
}
