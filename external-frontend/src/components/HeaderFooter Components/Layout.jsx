import React from 'react';
import Header from './Header2';
import Footer from './Footer2';

export default function Layout({ children }) {
  return (
    <div className="d-flex flex-column min-vh-100">
      <Header />
      <main className="flex-grow-1">
        {children}
      </main>
      <Footer />
    </div>
  );
}
