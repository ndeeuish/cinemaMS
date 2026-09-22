import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  allowedDevOrigins: [
    process.env.ALLOWED_ORIGIN || "192.168.1.5",
    "localhost",
    "127.0.0.1",
  ],
};

export default nextConfig;
