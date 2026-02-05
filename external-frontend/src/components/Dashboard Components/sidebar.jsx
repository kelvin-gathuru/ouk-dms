import React, { useContext, useEffect, useRef, useState } from 'react';
import { Nav } from 'react-bootstrap';
import { NavLink, useLocation } from 'react-router-dom';
import { AuthContext } from '../../context/AuthContext';
import {
  FiHome,
  FiFileText,
  FiFolder,
  FiUser,
  FiHelpCircle,
  FiLogOut,
  FiUpload,
  FiChevronRight,
  FiChevronLeft,
  FiChevronDown
} from 'react-icons/fi';
import { BsHouseDoorFill } from 'react-icons/bs';
import './Sidebar.css';

const Sidebar = ({ toggleSidebar, showSidebar, isMobileView }) => {
  const { logout } = useContext(AuthContext);
  const location = useLocation();
  const [activeSubmenu, setActiveSubmenu] = useState(null);
  // const [setCollapsedSections] = useState({});
  const sidebarRef = useRef(null);

  // Navigation items
  const navItems = [
    {
      title: 'Main',
      items: [
        {
          to: '/dashboard',
          icon: <FiHome size={20} />,
          label: 'Dashboard',
          exact: true
        },
        {
          to: '/create-petition',
          icon: <FiFileText size={20} />,
          label: 'Create Petition'
        },
        {
          to: '/create-petition-kiswahili',
          icon: <FiFileText size={20} />,
          label: 'Petition(swahili)'
        },
        {
          to: '/dashboard/upload-petition',
          icon: <FiUpload size={20} />,
          label: 'Upload Petition'
        },
        {
          to: '/dashboard/petitions',
          icon: <FiFolder size={20} />,
          label: 'My Petitions',

        },
      ]
    },
    {
      title: 'Account',
      items: [
        { to: '/profile', icon: <FiUser size={20} />, label: 'Profile' },
        { to: '/help-support', icon: <FiHelpCircle size={20} />, label: 'Help & Support' },
      ]
    }
  ];

  const handleLogout = (e) => {
    e.preventDefault();
    logout();
    if (isMobileView) {
      toggleSidebar();
    }
  };

  // const toggleSubmenu = (index, e) => {
  //   e.preventDefault();
  //   e.stopPropagation();
  //   setCollapsedSections(prev => ({
  //     ...prev,
  //     [index]: !prev[index]
  //   }));
  // };

  // Close sidebar when clicking outside on mobile
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (isMobileView && showSidebar && sidebarRef.current && !sidebarRef.current.contains(event.target)) {
        // Check if the click is not on the menu toggle button
        const menuToggle = document.querySelector('.sidebar-toggle');
        if (!menuToggle || !menuToggle.contains(event.target)) {
          toggleSidebar();
        }
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [isMobileView, showSidebar, toggleSidebar]);

  // Auto-close submenus when location changes
  useEffect(() => {
    setActiveSubmenu(null);
  }, [location.pathname]);

  // Close sidebar when route changes on mobile
  useEffect(() => {
    if (isMobileView) {
      toggleSidebar();
    }
  }, [location.pathname]);

  return (
    <>
      <div
        ref={sidebarRef}
        className={`sidebar ${showSidebar ? 'show' : ''} ${isMobileView ? 'mobile' : ''} ${!isMobileView && !showSidebar ? 'collapsed' : ''}`}
      >
        <div className="sidebar-header">
          <div className="d-flex align-items-center">
            <div className="sidebar-logo">
              <BsHouseDoorFill size={24} />
            </div>
            <h5 className="mb-0 ms-2">Petition Platform</h5>
          </div>
          {!isMobileView && (
            <button className="sidebar-close" onClick={toggleSidebar}>
              <FiChevronLeft size={20} />
            </button>
          )}
        </div>

        <div className="sidebar-content">
          {navItems.map((section, sectionIndex) => (
            <div key={sectionIndex} className="sidebar-section">
              <div className="section-title">{section.title}</div>
              <Nav className="flex-column">
                {section.items.map((item, itemIndex) => (
                  <React.Fragment key={item.to}>
                    <NavLink
                      to={item.to}
                      className={({ isActive }) =>
                        `nav-link ${isActive ? 'active' : ''} ${item.submenu ? 'has-submenu' : ''}`
                      }
                      onClick={(e) => {
                        if (item.submenu) {
                          e.preventDefault();
                          setActiveSubmenu(activeSubmenu === item.to ? null : item.to);
                        }
                      }}
                      end={item.exact}
                    >
                      <span className="nav-icon">{item.icon}</span>
                      <span className="nav-label">{item.label}</span>
                      {item.submenu && (
                        <span className="submenu-toggle">
                          {activeSubmenu === item.to ? (
                            <FiChevronDown size={16} />
                          ) : (
                            <FiChevronRight size={16} />
                          )}
                        </span>
                      )}
                    </NavLink>

                    {item.submenu && (
                      <div
                        className={`submenu ${activeSubmenu === item.to ? 'show' : ''}`}
                        style={{
                          maxHeight: activeSubmenu === item.to ? `${item.submenu.length * 44}px` : '0'
                        }}
                      >
                        {item.submenu.map((subItem) => (
                          <NavLink
                            key={subItem.to}
                            to={subItem.to}
                            className={({ isActive }) =>
                              `nav-link submenu-item ${isActive ? 'active' : ''}`
                            }
                          >
                            <span className="submenu-icon">•</span>
                            <span className="submenu-label">{subItem.label}</span>
                          </NavLink>
                        ))}
                      </div>
                    )}
                  </React.Fragment>
                ))}
              </Nav>
            </div>
          ))}
        </div>

        <div className="sidebar-footer">
          <NavLink
            to="#"
            className="nav-link"
            onClick={handleLogout}
          >
            <span className="nav-icon"><FiLogOut size={20} /></span>
            <span className="nav-label">Logout</span>
          </NavLink>
        </div>
      </div>
    </>
  );
};

export default Sidebar;
