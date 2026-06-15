export interface DashboardOverviewDto {
  totalRevenue: number;
  totalTicketsSold: number;
  totalUsers: number;
  totalMovies: number;
}

export interface MovieRevenueDto {
  movieId: number;
  movieTitle: string;
  ticketsSold: number;
  revenue: number;
}
