export interface AuthResponseDto {
  token: string;
  username: string;
  email: string;
  fullName: string;
  role: string;
}

export interface UserDto {
  id: number;
  username: string;
  email: string;
  fullName: string;
  phoneNumber?: string;
  roleId: number;
  roleName: string;
  isDeleted: boolean;
  createdAt: string;
}
