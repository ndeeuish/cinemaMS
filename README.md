# 🎬 CinemaMS - Modern Cinema Management System

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Next.js](https://img.shields.io/badge/Next.js-14-000000?style=flat-square&logo=nextdotjs&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC292B?style=flat-square&logo=microsoftsqlserver&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=flat-square&logo=redis&logoColor=white)
![Clean Architecture](https://img.shields.io/badge/Architecture-Clean_Architecture-brightgreen?style=flat-square)

## 💡 Giới thiệu dự án

**CinemaMS** là một hệ thống quản lý rạp chiếu phim bao gồm đầy đủ phân hệ dành cho Người dùng và Quản trị viên.
Dự án được xây dựng với mục tiêu áp dụng các kiến trúc phần mềm chuẩn mực (Clean Architecture) và giải quyết các bài toán khó trong thực tế của hệ thống thương mại điện tử / đặt vé (ví dụ: xử lý tranh chấp dữ liệu khi đặt ghế).

## 🚀 Điểm nhấn Kỹ thuật & Kiến trúc (Technical Highlights)

Dự án này không chỉ là một ứng dụng CRUD thông thường mà còn tập trung sâu vào chất lượng hệ thống:

- **Clean Architecture & CQRS:** Backend được thiết kế theo chuẩn Clean Architecture, tách biệt hoàn toàn Domain, Application, Infrastructure và Presentation. Sử dụng `MediatR` để triển khai mô hình CQRS (Command Query Responsibility Segregation), giúp luồng đọc/ghi dữ liệu độc lập, dễ dàng bảo trì và mở rộng.
- **Xử lý Tranh chấp (Concurrency Handling):** Giải quyết triệt để tình trạng Race Condition (nhiều người dùng chọn cùng 1 ghế tại cùng 1 thời điểm) bằng cách áp dụng **Redis Distributed Lock**.
- **Tối ưu Hiệu năng (Caching):** Ứng dụng **Redis Distributed Caching** cho các API có lượng truy cập lớn (như danh sách phim, lịch chiếu), kết hợp phân trang, giúp giảm tải đáng kể cho Database.
- **Xử lý Tác vụ ngầm (Background Processing):** Tích hợp `IHostedService` (.NET Background Service) để tự động theo dõi thời gian giữ ghế của người dùng. Hệ thống tự động giải phóng các ghế chưa được thanh toán sau 10 phút mà không cần sự can thiệp thủ công.

## ✨ Tính năng chính (Key Features)

### 🙍‍♂️ Phân hệ Khách hàng (Client)

- Xem danh sách Phim đang chiếu, sắp chiếu chi tiết.
- Xem Lịch chiếu theo ngày và theo từng rạp.
- **Chọn ghế Real-time:** Xem sơ đồ ghế (Ghế thường, VIP, Sweetbox) và trạng thái ghế trống/đã đặt/đang được giữ.
- Thanh toán và lưu trữ lịch sử đặt vé.

### 👨‍💻 Phân hệ Quản trị (Admin)

- Dashboard thống kê tổng quan (Doanh thu, số lượng vé...).
- Quản lý Rạp chiếu, Phòng chiếu và Sơ đồ ghế.
- Quản lý Phim, Thể loại và Lịch chiếu.
- Quản lý Người dùng và Phân quyền (Role-based Authorization bằng JWT).

## 🛠 Công nghệ sử dụng (Tech Stack)

**Backend (.NET 8):**

- C# 12, ASP.NET Core Web API
- Entity Framework Core 8, SQL Server
- Redis (StackExchange.Redis)
- MediatR, FluentValidation, AutoMapper
- Authentication & Authorization: JWT Bearer

**Frontend (Next.js):**

- Next.js (App Router), React 19
- Styling: Tailwind CSS, Ant Design (antd)
- State Management: Zustand
- Data Fetching: Axios

## 📁 Cấu trúc thư mục (Folder Structure)

Hệ thống được chia thành 2 thư mục chính:

```text
cinemaMS/
├── be/ (Backend - .NET Clean Architecture)
│   ├── CinemaMS.Domain/           # Core Entities, Interfaces
│   ├── CinemaMS.Application/      # CQRS Handlers, DTOs, Business Rules
│   ├── CinemaMS.Infrastructure/   # EF Core DbContext, Repositories, Redis Services
│   └── CinemaMS.API/              # Controllers, Middlewares, Dependency Injection
│
└── fe/ (Frontend - Next.js)
    ├── src/app/                   # App Router (client, admin, auth pages)
    ├── src/components/            # UI Components
    ├── src/services/              # Axios API calls
    └── src/stores/                # Zustand state management
```

## 💻 Hướng dẫn cài đặt & Khởi chạy (Getting Started)

### Yêu cầu hệ thống (Prerequisites)

- [Node.js](https://nodejs.org/) (v18+)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server
- Redis Server (Hoặc dùng Docker: `docker run -p 6379:6379 -d redis`)

### 1. Setup Backend

```bash
cd be/CinemaMS.API
```

- Cập nhật chuỗi kết nối Database và Redis trong file `appsettings.json` (hoặc `appsettings.Development.json`).
- Chạy Entity Framework Migrations để tạo Database:

```bash
dotnet ef database update --project ../CinemaMS.Infrastructure --startup-project .
```

- Chạy server:

```bash
dotnet run
```

_API sẽ chạy tại: `https://localhost:<port>`_

### 2. Setup Frontend

```bash
cd fe
```

- Trỏ `NEXT_PUBLIC_API_URL` tới URL của Backend trong file môi trường (nếu có).
- Cài đặt thư viện và chạy:

```bash
npm install
npm run dev
```

_Web sẽ chạy tại: `http://localhost:3000`_

## 📚 Tài liệu API (API Documentation)

Khi chạy Backend ở môi trường Development, hệ thống đã tích hợp sẵn Swagger.
Bạn có thể truy cập `https://localhost:<port>/swagger` để xem tài liệu API chi tiết và test trực tiếp các endpoint.

---

_Dự án được phát triển bởi Nguyễn Đức Hiếu - 2026_
