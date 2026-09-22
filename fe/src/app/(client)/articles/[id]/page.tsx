'use client';

import { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';
import { Spin, Typography, Tag, Divider, Breadcrumb } from 'antd';
import { clientService } from '@/services/client.service';
import { ArticleDto, ArticleType } from '@/types/client.type';
import dayjs from 'dayjs';
import Link from 'next/link';
import { HomeOutlined, ClockCircleOutlined } from '@ant-design/icons';

const { Title, Paragraph } = Typography;

export default function ArticleDetailPage() {
  const params = useParams();
  const id = params?.id as string;
  const [article, setArticle] = useState<ArticleDto | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (id) {
      fetchArticleDetail(id);
    }
  }, [id]);

  const fetchArticleDetail = async (articleId: string) => {
    try {
      const data = await clientService.getArticleById(articleId);
      setArticle(data);
    } catch (error) {
      console.error('Failed to fetch article detail:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 flex justify-center items-center">
        <Spin size="large" />
      </div>
    );
  }

  if (!article) {
    return (
      <div className="min-h-screen bg-gray-50 flex flex-col justify-center items-center">
        <Title level={3}>Không tìm thấy bài viết</Title>
        <Link href="/" className="text-red-600 hover:underline">Quay về trang chủ</Link>
      </div>
    );
  }

  const imageUrl = article.imageUrl 
    ? (article.imageUrl.startsWith('http') ? article.imageUrl : `${process.env.NEXT_PUBLIC_API_URL?.replace('/api', '') || 'https://localhost:7212'}${article.imageUrl}`) 
    : 'https://placehold.co/1200x400/EAEAEA/000000.png?text=No+Image';

  return (
    <div className="bg-gray-50 min-h-screen pb-16">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 py-8">
        <Breadcrumb className="mb-6"
          items={[
            { title: <Link href="/"><HomeOutlined /> Trang chủ</Link> },
            { title: <Link href="/#events-news">Khuyến mãi / Tin tức</Link> },
            { title: article.title }
          ]}
        />

        <div className="bg-white rounded-2xl shadow-sm overflow-hidden">
          <div className="w-full h-[300px] md:h-[400px] relative">
            <img 
              src={imageUrl} 
              alt={article.title}
              className="w-full h-full object-cover"
            />
          </div>

          <div className="p-6 md:p-10">
            <div className="flex items-center gap-3 mb-4">
              <Tag color={article.type === ArticleType.News ? "blue" : "green"} className="text-sm px-3 py-1 font-semibold rounded-full border-0">
                {article.type === ArticleType.News ? 'Tin tức' : 'Sự kiện'}
              </Tag>
              <div className="text-gray-500 text-sm flex items-center gap-1 font-medium">
                <ClockCircleOutlined /> {dayjs(article.createdAt).format('DD/MM/YYYY')}
              </div>
            </div>

            <Title level={1} className="!mt-0 !mb-4 !text-3xl md:!text-4xl !font-extrabold !text-gray-900 leading-tight">
              {article.title}
            </Title>

            <Paragraph className="text-lg text-gray-600 font-medium italic mb-8">
              {article.summary}
            </Paragraph>

            <Divider />

            <div 
              className="prose prose-lg max-w-none prose-img:rounded-xl prose-a:text-red-600 hover:prose-a:text-red-700"
              dangerouslySetInnerHTML={{ __html: article.content }} 
            />
          </div>
        </div>
      </div>
    </div>
  );
}
