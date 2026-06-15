import ClientNavbar from '@/components/client/ClientNavbar';

import ClientFooter from '@/components/client/ClientFooter';

export default function ClientLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <ClientNavbar />
      <main className="flex-grow">
        {children}
      </main>
      <ClientFooter />
    </div>
  );
}
