import React from 'react';
import { Link } from 'react-router-dom';

export default function Footer() {
  return (
    <footer className="bg-dark text-white py-4 mt-auto">
      <div className="container">
          <div className="row g-4">
            <div className="col-lg-4">
              <h3 className="h5 mb-3">Kenyan Parliament</h3>
              <p className="text-muted">The National Assembly of Kenya is the legislative body of the Republic of Kenya, representing the people and exercising oversight over the Executive.</p>
            </div>
            <div className="col-lg-2">
              <h3 className="h5 mb-3">Quick Links</h3>
              <ul className="list-unstyled">
                <li className="mb-2"><a href="#home" className="text-white text-decoration-none">Home</a></li>
                <li className="mb-2"><a href="#about" className="text-white text-decoration-none">About Petitions</a></li>
                <li className="mb-2"><a href="#procedure" className="text-white text-decoration-none">Procedure</a></li>
                <li><Link to="/create-petition" className="text-white text-decoration-none">Submit Petition</Link></li>
              </ul>
            </div>
            <div className="col-lg-3">
              <h3 className="h5 mb-3">Contact Information</h3>
              <ul className="list-unstyled text-white">
                <li className="mb-2"><i className="fas fa-user-tie me-2"></i> The Clerk, National Assembly</li>
                <li className="mb-2"><i className="fas fa-phone me-2"></i> Tel: +254 2 2221291 / 2848000</li>
                <li className="mb-2"><i className="fas fa-fax me-2"></i> Fax: +254 2 2243694</li>
                <li className="mb-2"><i className="fas fa-envelope me-2"></i> Email: cna@parliament.go.ke</li>
                <li><i className="fas fa-globe me-2"></i> www.parliament.go.ke</li>
              </ul>
            </div>
            <div className="col-lg-3">
              <h3 className="h5 mb-3">Follow Us</h3>
              <div className="d-flex gap-3">
                <a href="https://twitter.com/Parliament_KE" target="_blank" rel="noopener noreferrer" className="text-white">
                  <i className="fab fa-twitter fa-lg"></i>
                </a>
                <a href="https://facebook.com/ParliamentKE" target="_blank" rel="noopener noreferrer" className="text-white">
                  <i className="fab fa-facebook-f fa-lg"></i>
                </a>
                <a href="https://www.instagram.com/parliamentofkenya/" target="_blank" rel="noopener noreferrer" className="text-white">
                  <i className="fab fa-instagram fa-lg"></i>
                </a>
                <a href="https://www.youtube.com/channel/UCXuseB7juWB7DIgTJcwtHFQ" target="_blank" rel="noopener noreferrer" className="text-white">
                  <i className="fab fa-youtube fa-lg"></i>
                </a>
              </div>
            </div>
          </div>
          <hr className="my-4 bg-secondary" />
          <div className="text-center text-white">
            <p className="mb-0">&copy; {new Date().getFullYear()} Kenyan Parliament. All rights reserved.</p>
          </div>
        </div>
    </footer>
  );
}