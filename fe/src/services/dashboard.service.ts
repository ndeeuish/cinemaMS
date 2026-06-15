import api from './api';
import { DashboardOverviewDto, MovieRevenueDto } from '../types/dashboard.type';

export const dashboardService = {
  getOverview: async (params?: any): Promise<DashboardOverviewDto> => {
    const response = await api.get<DashboardOverviewDto>('/dashboards/overview', { params });
    return response.data;
  },

  getTopMovies: async (params?: any): Promise<MovieRevenueDto[]> => {
    const response = await api.get<MovieRevenueDto[]>('/dashboards/top-movies', { params });
    return response.data;
  },
};
