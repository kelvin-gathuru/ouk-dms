import React, { useState, useEffect } from 'react';
import Sidebar from './sidebar';
import Topbar from './TopBar';
import './DashboardLayout.css';

const DashboardLayout = ({ children }) => {
  const [showSidebar, setShowSidebar] = useState(window.innerWidth >= 992);
  const [isMobileView, setIsMobileView] = useState(window.innerWidth < 992);

  useEffect(() => {
    const handleResize = () => {
      const isMobile = window.innerWidth < 992;
      setIsMobileView(isMobile);
      setShowSidebar(!isMobile);
    };

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  const toggleSidebar = () => {
    setShowSidebar(prev => !prev);
  };

  return (
    <div className={`dashboard-layout ${showSidebar ? 'sidebar-visible' : ''} ${isMobileView ? 'mobile-view' : ''}`}>
      <div className={`sidebar-overlay ${showSidebar && isMobileView ? 'active' : ''}`} onClick={toggleSidebar}></div>

      <Sidebar showSidebar={showSidebar} toggleSidebar={toggleSidebar} isMobileView={isMobileView} />

      <div className="main-content">
        <Topbar toggleSidebar={toggleSidebar} showSidebar={showSidebar} />
        <main className="content-wrapper">
          {children}
        </main>
      </div>
    </div>
  );
};

export default DashboardLayout;
