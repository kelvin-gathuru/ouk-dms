import React from 'react';
import { Link } from 'react-router-dom';
import logo from '../../assets/court of arms.png';

// Inline styles for better control
const styles = {
  navbar: {
    backgroundColor: 'var(--bs-success)',
    padding: '0.5rem 0',
    boxShadow: '0 2px 8px rgba(0,0,0,0.2)',
    position: 'sticky',
    top: 0,
    zIndex: 1000,
    minHeight: '90px'
  },
  navContainer: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
    width: '100%',
    padding: '0 1rem'
  },
  logoContainer: {
    display: 'flex',
    alignItems: 'center',
    gap: '1rem',
    flexShrink: 0
  },
  logoText: {
    color: '#ffffff',
    fontWeight: '700',
    margin: 0,
    lineHeight: '1.2',
    fontSize: '1.25rem'
  },
  logoSubtext: {
    color: 'rgba(255, 255, 255, 0.95)',
    fontSize: '0.85rem',
    display: 'block',
    fontWeight: '400'
  },
  navLinks: {
    display: 'flex',
    gap: '0.5rem',
    margin: '0 1.5rem',
    flexGrow: 1,
    justifyContent: 'center'
  },
  navLink: {
    color: '#ffffff',
    fontWeight: '500',
    padding: '0.75rem 1.25rem !important',
    whiteSpace: 'nowrap',
    transition: 'all 0.2s',
    borderRadius: '4px',
    position: 'relative',
    textDecoration: 'none',
    '&:after': {
      content: '""',
      position: 'absolute',
      width: '100%',
      height: '2px',
      bottom: '8px',
      left: 0,
      backgroundColor: '#ffffff',
      transform: 'scaleX(0.8)',
      transformOrigin: 'center',
      transition: 'transform 0.2s ease-in-out',
      opacity: 0.7
    },
    '&:hover': {
      backgroundColor: 'rgba(255, 255, 255, 0.15)',
      transform: 'translateY(-1px)',
      '&:after': {
        transform: 'scaleX(1)',
        opacity: 1
      }
    }
  },
  authButtons: {
    display: 'flex',
    gap: '0.75rem',
    alignItems: 'center',
    flexShrink: 0
  },
  logoImg: {
    height: '80px',
    width: 'auto',
    objectFit: 'contain',
    maxWidth: '180px'
  }
};

export default function Header() {
  return (
    <nav className="navbar navbar-expand-lg" style={styles.navbar}>
      <div className="container-fluid" style={{ padding: '0 1rem' }}>
        <div style={styles.logoContainer}>
          <Link to="/" className="navbar-brand p-0">
            <img 
              src={logo} 
              alt="Kenyan Parliament Logo" 
              style={styles.logoImg}
            />
          </Link>
          <div className="d-none d-lg-block">
            <h1 style={styles.logoText}>
              Kenyan Parliament
              <small style={styles.logoSubtext}>Public Petitions Portal</small>
            </h1>
          </div>
        </div>

        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
          style={{ border: '1px solid rgba(255,255,255,0.3)' }}
        >
          <i className="fas fa-bars text-white"></i>
        </button>

        <div className="collapse navbar-collapse" id="navbarNav">
          <div style={styles.navContainer}>
            <div style={{ flexGrow: 1, display: 'flex', justifyContent: 'center' }}>
              <ul className="navbar-nav" style={styles.navLinks}>
                <li className="nav-item">
                  <a href="#home" className="nav-link" style={styles.navLink}>Home</a>
                </li>
                <li className="nav-item">
                  <a href="#about" className="nav-link" style={styles.navLink}>About Petitions</a>
                </li>
                <li className="nav-item">
                  <a href="#procedure" className="nav-link" style={styles.navLink}>Procedure</a>
                </li>
                <li className="nav-item">
                  <Link to="/create-petition" className="nav-link" style={styles.navLink}>
                    Submit Petition
                  </Link>
                </li>
              </ul>
            </div>
            
            <div style={styles.authButtons} className="mt-3 mt-lg-0">
              <Link to="/login" className="btn btn-outline-light px-3" style={{ whiteSpace: 'nowrap', fontWeight: '500' }}>
                Login
              </Link>
              <Link to="/signup" className="btn btn-light px-3" style={{ whiteSpace: 'nowrap', fontWeight: '600', color: '#004d00' }}>
                Sign Up
              </Link>
            </div>
          </div>
        </div>
      </div>
    </nav>
  );
}
