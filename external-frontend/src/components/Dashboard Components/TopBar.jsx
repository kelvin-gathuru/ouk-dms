import React, { useState, useRef, useEffect } from 'react';
import { Form, Button } from 'react-bootstrap';
import {
  FiMenu,
  FiSearch,
  FiBell,
  FiUser,
  FiLogOut,
  FiChevronDown,
  FiMail,
  FiAlertCircle,
  FiCheckCircle
} from 'react-icons/fi';
import { Link } from 'react-router-dom';
import { logout, getUserName } from '../../service/tokenStorage';
import { useNavigate } from 'react-router-dom';
import './TopBar.css';

const TopBar = ({ toggleSidebar, showSidebar }) => {
  const [showDropdown, setShowDropdown] = useState(false);
  const [showUserDropdown, setShowUserDropdown] = useState(false);
  const [notifications] = useState([
    // { id: 1, title: 'New response received', message: 'Your petition has a new response', time: '2 min ago', read: false, type: 'response' },
    // { id: 2, title: 'System update', message: 'New features available in your dashboard', time: '1 hour ago', read: true, type: 'system' },
    // { id: 3, title: 'Reminder', message: 'Your petition is awaiting review', time: '3 hours ago', read: true, type: 'reminder' },
  ]);

  const navigate = useNavigate();
  const dropdownRef = useRef(null);
  const userDropdownRef = useRef(null);
  const userName = localStorage.getItem('contactPerson') || localStorage.getItem('companyName') || getUserName() || 'User';
  const userInitial = userName.charAt(0).toUpperCase();

  const unreadCount = notifications.filter(n => !n.read).length;

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  // Close dropdowns when clicking outside
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setShowDropdown(false);
      }
      if (userDropdownRef.current && !userDropdownRef.current.contains(event.target)) {
        setShowUserDropdown(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  return (
    <header className="topbar">
      <div className="d-flex align-items-center">
        <button
          className="sidebar-toggle me-3"
          onClick={toggleSidebar}
          aria-label="Toggle sidebar"
        >
          <FiMenu size={20} />
        </button>

        <div className="search-bar">
          <FiSearch className="search-icon" />
          <Form.Control
            type="search"
            placeholder="Search petitions..."
            className="search-input"
            onKeyDown={(e) => {
              if (e.key === 'Enter') {
                e.preventDefault();
                navigate(`/dashboard/petitions?search=${e.target.value}`);
              }
            }}
          />
        </div>
      </div>

      <div className="topbar-actions">
        <div className="dropdown" ref={dropdownRef}>
          <button
            className="notification-btn"
            onClick={() => setShowDropdown(!showDropdown)}
            aria-label="Notifications"
          >
            <FiBell size={20} />
            {unreadCount > 0 && (
              <span className="notification-badge">{unreadCount}</span>
            )}
          </button>

          <div className={`dropdown-menu dropdown-menu-end notification-dropdown ${showDropdown ? 'show' : ''}`}>
            <div className="dropdown-header d-flex justify-content-between align-items-center">
              <h6 className="mb-0">Notifications</h6>
              <Button
                variant="link"
                size="sm"
                className="text-primary p-0"
                onClick={() => { }}
              >
                Mark all as read
              </Button>
            </div>
            <div className="dropdown-divider my-0"></div>
            <div className="notification-list">
              {notifications.map(notification => (
                <div
                  key={notification.id}
                  className={`notification-item ${!notification.read ? 'unread' : ''}`}
                  onClick={() => { }}
                >
                  <div className="notification-icon">
                    {notification.type === 'response' ? (
                      <FiMail size={18} />
                    ) : notification.type === 'system' ? (
                      <FiAlertCircle size={18} />
                    ) : (
                      <FiCheckCircle size={18} />
                    )}
                  </div>
                  <div className="notification-content">
                    <div className="notification-title">{notification.title}</div>
                    <div className="notification-message">{notification.message}</div>
                    <div className="notification-time">{notification.time}</div>
                  </div>
                </div>
              ))}
            </div>
            <div className="dropdown-footer text-center">
              <Button variant="link" size="sm" className="text-primary">
                View All Notifications
              </Button>
            </div>
          </div>
        </div>

        <div className="dropdown user-dropdown" ref={userDropdownRef}>
          <button
            className="user-dropdown-toggle"
            onClick={() => setShowUserDropdown(!showUserDropdown)}
            aria-label="User menu"
          >
            <div className="user-avatar">
              {userInitial}
            </div>
            <div className="user-info d-none d-md-flex flex-column">
              <span className="user-name">{userName}</span>
              <span className="user-role">Petitioner</span>
            </div>
            <FiChevronDown className="ms-1" />
          </button>

          <div className={`dropdown-menu dropdown-menu-end user-dropdown-menu ${showUserDropdown ? 'show' : ''}`}>
            <div className="dropdown-header">
              <h6 className="mb-0">Signed in as</h6>
              <div className="text-truncate">{userName}</div>
            </div>
            <div className="dropdown-divider"></div>
            <Link to="/profile" className="dropdown-item">
              <FiUser className="me-2" /> Profile
            </Link>
            {/* <Link to="/settings" className="dropdown-item">
              <FiSettings className="me-2" /> Settings
            </Link> */}
            <div className="dropdown-divider"></div>
            <button className="dropdown-item text-danger" onClick={handleLogout}>
              <FiLogOut className="me-2" /> Logout
            </button>
          </div>
        </div>
      </div>
    </header>
  );
};

export default TopBar;
