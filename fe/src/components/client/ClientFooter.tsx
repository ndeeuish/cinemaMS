import Link from 'next/link';
import { FacebookOutlined, InstagramOutlined, TwitterOutlined, YoutubeOutlined } from '@ant-design/icons';

export default function ClientFooter() {
  return (
    <footer className="bg-gray-900 text-gray-300 pt-16 pb-8 border-t border-gray-800">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8 mb-12">
          {/* Column 1: About */}
          <div>
            <h3 className="text-xl font-bold text-white mb-4">CinemaMS</h3>
            <p className="text-sm text-gray-400 mb-4 leading-relaxed">
              Hệ thống đặt vé xem phim trực tuyến hàng đầu, mang lại trải nghiệm điện ảnh tuyệt vời nhất với hệ thống rạp hiện đại trên toàn quốc.
            </p>
            <div className="flex gap-4">
              <a href="#" className="w-10 h-10 rounded-full bg-gray-800 flex items-center justify-center hover:bg-red-500 hover:text-white transition-colors">
                <FacebookOutlined className="text-lg" />
              </a>
              <a href="#" className="w-10 h-10 rounded-full bg-gray-800 flex items-center justify-center hover:bg-red-500 hover:text-white transition-colors">
                <InstagramOutlined className="text-lg" />
              </a>
              <a href="#" className="w-10 h-10 rounded-full bg-gray-800 flex items-center justify-center hover:bg-red-500 hover:text-white transition-colors">
                <TwitterOutlined className="text-lg" />
              </a>
              <a href="#" className="w-10 h-10 rounded-full bg-gray-800 flex items-center justify-center hover:bg-red-500 hover:text-white transition-colors">
                <YoutubeOutlined className="text-lg" />
              </a>
            </div>
          </div>

          {/* Column 2: Quick Links */}
          <div>
            <h3 className="text-lg font-bold text-white mb-4">Danh mục</h3>
            <ul className="space-y-2">
              <li><Link href="/" className="hover:text-red-500 transition-colors">Trang chủ</Link></li>
              <li><Link href="#" className="hover:text-red-500 transition-colors">Phim đang chiếu</Link></li>
              <li><Link href="#" className="hover:text-red-500 transition-colors">Phim sắp ra mắt</Link></li>
              <li><Link href="#" className="hover:text-red-500 transition-colors">Rạp chiếu phim</Link></li>
              <li><Link href="#" className="hover:text-red-500 transition-colors">Tin tức & Khuyến mãi</Link></li>
            </ul>
          </div>

          {/* Column 3: Policy */}
          <div>
            <h3 className="text-lg font-bold text-white mb-4">Điều khoản</h3>
            <ul className="space-y-2">
              <li><Link href="#" className="hover:text-red-500 transition-colors">Điều khoản sử dụng</Link></li>
              <li><Link href="#" className="hover:text-red-500 transition-colors">Chính sách bảo mật</Link></li>
              <li><Link href="#" className="hover:text-red-500 transition-colors">Quy định đổi trả</Link></li>
              <li><Link href="#" className="hover:text-red-500 transition-colors">Câu hỏi thường gặp</Link></li>
            </ul>
          </div>

          {/* Column 4: Contact */}
          <div>
            <h3 className="text-lg font-bold text-white mb-4">Chăm sóc khách hàng</h3>
            <ul className="space-y-3">
              <li>
                <div className="text-sm text-gray-500">Hotline:</div>
                <div className="text-lg font-bold text-red-500">1900 1234</div>
              </li>
              <li>
                <div className="text-sm text-gray-500">Giờ làm việc:</div>
                <div>8:00 - 22:00 (Tất cả các ngày)</div>
              </li>
              <li>
                <div className="text-sm text-gray-500">Email hỗ trợ:</div>
                <div>support@cinemams.vn</div>
              </li>
            </ul>
          </div>
        </div>

        <div className="pt-8 border-t border-gray-800 text-center text-sm text-gray-500">
          <p>&copy; {new Date().getFullYear()} CinemaMS. All rights reserved.</p>
        </div>
      </div>
    </footer>
  );
}
