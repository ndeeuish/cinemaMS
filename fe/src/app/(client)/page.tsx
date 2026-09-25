'use client';

import { useEffect, useState } from 'react';
import { Card, Button, Spin, Carousel, Row, Col, Typography } from 'antd';
import { clientService } from '@/services/client.service';
import { MovieDto, ArticleDto } from '@/types/client.type';
import Link from 'next/link';
import { ClockCircleOutlined, CalendarOutlined, RightOutlined, LeftOutlined, LeftCircleOutlined, RightCircleOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';

const { Meta } = Card;
const { Title, Paragraph } = Typography;


export default function HomePage() {
  const [movies, setMovies] = useState<MovieDto[]>([]);
  const [articles, setArticles] = useState<ArticleDto[]>([]);
  const [loading, setLoading] = useState(true);

  const nowShowingMovies = movies.filter(m => dayjs(m.releaseDate).isBefore(dayjs().add(1, 'day')));
  const comingSoonMovies = movies.filter(m => dayjs(m.releaseDate).isAfter(dayjs()));

  useEffect(() => {
    fetchMovies();
    fetchArticles();
  }, []);

  const fetchMovies = async () => {
    try {
      const data = await clientService.getMovies();
      setMovies(data);
    } catch (error) {
      console.error('Failed to fetch movies:', error);
    } finally {
      setLoading(false);
    }
  };

  const fetchArticles = async () => {
    try {
      const data = await clientService.getArticles(1, 3);
      setArticles(data.items);
    } catch (error) {
      console.error('Failed to fetch articles:', error);
    }
  };

  const CustomPrevArrow = (props: any) => {
    const { onClick } = props;
    return (
      <button
        onClick={onClick}
        className="absolute top-[35%] -left-12 -translate-y-1/2 z-10 text-gray-400 hover:text-red-500 transition-all duration-300 hover:scale-110 focus:outline-none cursor-pointer opacity-60 hover:opacity-100"
        style={{ border: 'none', background: 'transparent' }}
      >
        <LeftCircleOutlined style={{ fontSize: '44px', filter: 'drop-shadow(1px 1px 2px rgba(255,255,255,0.6))' }} />
      </button>
    );
  };

  const CustomNextArrow = (props: any) => {
    const { onClick } = props;
    return (
      <button
        onClick={onClick}
        className="absolute top-[35%] -right-12 -translate-y-1/2 z-10 text-gray-400 hover:text-red-500 transition-all duration-300 hover:scale-110 focus:outline-none cursor-pointer opacity-60 hover:opacity-100"
        style={{ border: 'none', background: 'transparent' }}
      >
        <RightCircleOutlined style={{ fontSize: '44px', filter: 'drop-shadow(1px 1px 2px rgba(255,255,255,0.6))' }} />
      </button>
    );
  };

  const carouselSettings4 = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 4,
    slidesToScroll: 4,
    arrows: true,
    prevArrow: <CustomPrevArrow />,
    nextArrow: <CustomNextArrow />,
    responsive: [
      { breakpoint: 1024, settings: { slidesToShow: 3, slidesToScroll: 3 } },
      { breakpoint: 768, settings: { slidesToShow: 2, slidesToScroll: 2 } },
      { breakpoint: 480, settings: { slidesToShow: 1, slidesToScroll: 1 } }
    ]
  };

  const carouselSettings5 = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 5,
    slidesToScroll: 5,
    arrows: true,
    prevArrow: <CustomPrevArrow />,
    nextArrow: <CustomNextArrow />,
    responsive: [
      { breakpoint: 1024, settings: { slidesToShow: 4, slidesToScroll: 4 } },
      { breakpoint: 768, settings: { slidesToShow: 3, slidesToScroll: 3 } },
      { breakpoint: 480, settings: { slidesToShow: 2, slidesToScroll: 2 } }
    ]
  };

  return (
    <div className="bg-gray-50 min-h-screen">
      {/* Hero Banner Area */}
      <div className="relative bg-black">
        {articles.length > 0 ? (
          <Carousel autoplay effect="fade" dots={{ className: 'custom-dots' }} autoplaySpeed={4000}>
            {articles.slice(0, 4).map((article) => (
              <div key={article.id}>
                <Link href={`/articles/${article.id}`}>
                  <div 
                    className="h-[400px] md:h-[500px] w-full bg-cover bg-center relative cursor-pointer group"
                    style={{ 
                      backgroundImage: `url(${article.imageUrl ? (article.imageUrl.startsWith('http') ? article.imageUrl : `${process.env.NEXT_PUBLIC_API_URL?.replace('/api', '') || 'https://localhost:7212'}${article.imageUrl}`) : 'https://placehold.co/1200x500/1a1a1a/FFFFFF?text=No+Image'})` 
                    }}
                  >
                    <div className="absolute inset-0 bg-gradient-to-t from-black via-black/40 to-transparent opacity-80 group-hover:opacity-60 transition-opacity duration-300"></div>
                    <div className="absolute bottom-10 left-10 md:bottom-16 md:left-16 max-w-2xl">
                      <div className="bg-red-600 text-white text-xs font-bold px-3 py-1 inline-block mb-3 rounded">
                        {article.type === 1 ? 'SỰ KIỆN' : 'TIN TỨC'}
                      </div>
                      <h2 className="text-2xl md:text-4xl font-bold text-white mb-2 leading-tight drop-shadow-md">
                        {article.title}
                      </h2>
                      <p className="text-gray-300 line-clamp-2 md:text-lg hidden md:block drop-shadow-md">
                        {article.summary}
                      </p>
                    </div>
                  </div>
                </Link>
              </div>
            ))}
          </Carousel>
        ) : (
          <div className="h-[400px] md:h-[500px] w-full bg-gray-900 flex items-center justify-center">
            <Spin size="large" />
          </div>
        )}
      </div>

      {/* Main Content Area */}
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 space-y-16">
        
        {/* Now Showing Section */}
        <section id="now-showing" className="scroll-mt-20">
          <div className="flex items-center justify-between mb-8">
            <h2 className="text-3xl font-extrabold text-gray-900 border-l-4 border-red-600 pl-4 uppercase tracking-tight">Phim Đang Chiếu</h2>
          </div>

          {loading ? (
            <div className="flex justify-center py-20"><Spin size="large" /></div>
          ) : movies.length > 0 ? (
            <div className="movie-carousel-wrapper">
              <Carousel {...carouselSettings4} className="pb-8">
                {movies.map(movie => (
                  <div key={movie.id} className="px-3 pb-4">
                    <Link href={`/movies/${movie.id}`}>
                      <Card
                        hoverable
                        className="h-full overflow-hidden shadow-md hover:shadow-xl transition-shadow border-0 rounded-xl"
                        cover={
                          <div className="relative pt-[150%]">
                            <img
                              alt={movie.title}
                              src={movie.posterUrl ? (movie.posterUrl.startsWith('http') ? movie.posterUrl : `${process.env.NEXT_PUBLIC_API_URL?.replace('/api', '') || 'https://localhost:7212'}${movie.posterUrl}`) : 'https://placehold.co/300x450/EAEAEA/000000.png?text=No+Poster'}
                              onError={(e) => {
                                e.currentTarget.onerror = null;
                                e.currentTarget.src = 'https://placehold.co/300x450/EAEAEA/000000.png?text=No+Poster';
                              }}
                              className="absolute top-0 left-0 w-full h-full object-cover"
                            />
                            <div className="absolute top-3 right-3 bg-red-600 text-white text-sm font-black px-3 py-1.5 rounded-md shadow-lg border border-red-400 backdrop-blur-sm bg-opacity-90 tracking-wide z-10 flex items-center justify-center min-w-[48px]">
                              {movie.ageRestrictionCode || 'P'}
                            </div>
                          </div>
                        }
                      >
                        <Meta
                          title={<span className="text-lg font-bold truncate block">{movie.title}</span>}
                          description={
                            <div className="mt-2 space-y-1 text-sm text-gray-500">
                              <div className="flex items-center gap-2"><ClockCircleOutlined /> {movie.durationInMinutes} phút</div>
                              <div className="flex items-center gap-2"><CalendarOutlined /> {dayjs(movie.releaseDate).format('DD/MM/YYYY')}</div>
                            </div>
                          }
                        />
                        <div className="mt-4 pt-4 border-t border-gray-100">
                          <Button type="primary" danger block size="large" className="font-semibold rounded-lg">
                            MUA VÉ NGAY
                          </Button>
                        </div>
                      </Card>
                    </Link>
                  </div>
                ))}
              </Carousel>
            </div>
          ) : (
            <div className="w-full text-center py-20 bg-white rounded-xl shadow-sm border border-gray-100">
              <p className="text-gray-500 text-lg">Hiện tại không có phim nào đang chiếu.</p>
            </div>
          )}
        </section>

        {/* Coming Soon Section */}
        <section id="coming-soon" className="scroll-mt-20">
          <div className="flex items-center justify-between mb-8">
            <h2 className="text-3xl font-extrabold text-gray-900 border-l-4 border-blue-600 pl-4 uppercase tracking-tight">Phim Sắp Chiếu</h2>
          </div>
          
          <div className="movie-carousel-wrapper">
            <Carousel {...carouselSettings5} className="pb-8">
              {comingSoonMovies.map(movie => (
                <div key={movie.id} className="px-2 pb-4">
                  <Card 
                    hoverable 
                    className="h-full overflow-hidden border-0 shadow-sm hover:shadow-lg transition-shadow rounded-xl bg-white"
                    cover={
                      <div className="relative pt-[150%] overflow-hidden">
                        <img alt={movie.title} src={movie.posterUrl} className="absolute top-0 left-0 w-full h-full object-cover transition-transform duration-500 hover:scale-105" />
                      </div>
                    }
                    styles={{ body: { padding: '16px' } }}
                  >
                    <h3 className="text-sm font-bold text-gray-900 leading-tight hover:text-blue-600 transition-colors line-clamp-2 text-center">
                      {movie.title}
                    </h3>
                  </Card>
                </div>
              ))}
            </Carousel>
          </div>
        </section>

        {/* Events & News Section */}
        <section id="events-news" className="pt-8 border-t border-gray-200 scroll-mt-20">
          <div className="flex items-center justify-between mb-8">
            <h2 className="text-3xl font-extrabold text-gray-900 border-l-4 border-green-600 pl-4 uppercase tracking-tight">Khuyến Mãi & Tin Tức</h2>
          </div>
          
          <Row gutter={[24, 24]}>
            {articles.map(item => (
              <Col xs={24} md={8} key={item.id}>
                <Link href={`/articles/${item.id}`}>
                  <Card 
                    hoverable 
                    className="h-full overflow-hidden border-0 shadow-sm hover:shadow-lg transition-shadow rounded-xl bg-white"
                    cover={
                      <div className="overflow-hidden">
                        <img 
                          alt={item.title} 
                          src={item.imageUrl ? (item.imageUrl.startsWith('http') ? item.imageUrl : `${process.env.NEXT_PUBLIC_API_URL?.replace('/api', '') || 'https://localhost:7212'}${item.imageUrl}`) : 'https://placehold.co/400x250/EAEAEA/000000.png?text=No+Image'} 
                          className="w-full h-48 object-cover transition-transform duration-500 hover:scale-105" 
                        />
                      </div>
                    }
                    styles={{ body: { padding: '20px' } }}
                  >
                    <p className="text-xs text-gray-400 mb-2 font-medium">{dayjs(item.createdAt).format('DD/MM/YYYY')}</p>
                    <h3 className="text-lg font-bold text-gray-900 leading-tight hover:text-red-600 transition-colors line-clamp-2">
                      {item.title}
                    </h3>
                  </Card>
                </Link>
              </Col>
            ))}
          </Row>
        </section>

      </div>

      <style jsx global>{`
        /* Offset dots from overlapping content */
        .movie-carousel-wrapper .slick-dots {
          bottom: -5px;
        }
        .movie-carousel-wrapper .slick-dots li button {
          background: #d1d5db; /* gray-300 */
          height: 4px;
        }
        .movie-carousel-wrapper .slick-dots li.slick-active button {
          background: #dc2626; /* red-600 */
        }
      `}</style>
    </div>
  );
}
