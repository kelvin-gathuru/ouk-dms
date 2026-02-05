import React, { useEffect, useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { getToken } from '../service/tokenStorage';
import { AuthContext } from '../context/AuthContext';

export default function Index() {
  const navigate = useNavigate();
  const { isAuthenticated } = useContext(AuthContext);

  // Redirect if already authenticated
  useEffect(() => {
    const token = getToken();
    if (token || isAuthenticated) {
      navigate('/dashboard');
    }
  }, [navigate, isAuthenticated]);

  // FAQ Toggle Functionality
  useEffect(() => {
    const handleFaqClick = (event) => {
      if (event.target.closest('.faq-question')) {
        event.target.closest('.faq-item').classList.toggle('active');
      }
    };

    // Mobile Menu Toggle
    const handleMenuClick = () => {
      const nav = document.querySelector('nav ul');
      if (nav) {
        nav.style.display = nav.style.display === 'flex' ? 'none' : 'flex';
      }
    };

    document.addEventListener('click', handleFaqClick);
    const mobileMenu = document.querySelector('.mobile-menu');
    if (mobileMenu) {
      mobileMenu.addEventListener('click', handleMenuClick);
    }

    return () => {
      document.removeEventListener('click', handleFaqClick);
      if (mobileMenu) {
        mobileMenu.removeEventListener('click', handleMenuClick);
      }
    };
  }, []);

  return (
    <div className="landing-page">
      {/* Hero Section */}
      <section className="hero py-5" id="home" style={{
        background: 'linear-gradient(rgba(0, 0, 0, 0.6), rgba(0, 0, 0, 0.6)), url(https://images.unsplash.com/photo-1586769852836-bc069f19e1b6?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1800&q=80) no-repeat center center/cover',
        color: 'white',
        textAlign: 'center',
        padding: '100px 20px',
        minHeight: '80vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center'
      }}>
        <div className="container">
          <div className="hero-content">
            <h2 className="display-4 fw-normal mb-4">Your Voice in Parliament. Start a Public Petition.</h2>
            <p className="lead mb-5">Empower change. Learn how to formally present your issues to the Kenyan National Assembly and hold your leaders accountable.</p>
            <div className="d-flex justify-content-center gap-3">
              <Link to="/create-petition" className="btn btn-warning">Start Your Petition Journey</Link>
              <a href="#procedure" className="btn btn-outline">View Petition Procedure</a>
            </div>
          </div>
        </div>
      </section>

      {/* What is Petition Section */}
      <section className="py-5 bg-light" id="about">
        <div className="container">
          <div className="section-title">
            <h2>What is a Public Petition?</h2>
            <p>Understanding your constitutional right to petition Parliament</p>
          </div>
          <div className="row align-items-center">
            <div className="col-lg-6 mb-4 mb-lg-0">
              <h3 className="h2 text-success mb-4">Exercising Your Constitutional Right</h3>
              <p className="lead">A public petition is a formal written request made by you or your community to the National Assembly of Kenya. It is a fundamental right that allows citizens to directly demand action from Parliament on issues of national, county, or local concern.</p>
              <p>The petition process is governed by the <strong>Standing Orders of the National Assembly</strong>, ensuring your voice is heard and formally addressed.</p>
              <a href="#procedure" className="btn btn-success">Learn More</a>
            </div>
            <div className="col-lg-6">
              <img src="https://via.placeholder.com/600x400/006600/ffffff?text=Petition+Process" alt="Petition Process" className="img-fluid rounded shadow" />
            </div>
          </div>
        </div>
      </section>

      {/* Petition Issues Section */}
      <section className="py-5">
        <div className="container">
          <div className="section-title">
            <h2>What Can You Petition About?</h2>
            <p>Issues worthy of a formal petition to Parliament</p>
          </div>
          <div className="row g-4">
            <div className="col-md-4">
              <div className="card h-100 border-0 shadow-sm">
                <div className="card-body text-center p-4">
                  <div className="text-primary mb-3">
                    <i className="fas fa-users fa-3x"></i>
                  </div>
                  <h3 className="h4">Matters of Public Interest</h3>
                  <p>Advocate for policy changes, new laws, or amendments to existing laws that affect the public.</p>
                </div>
              </div>
            </div>
            <div className="col-md-4">
              <div className="card h-100 border-0 shadow-sm">
                <div className="card-body text-center p-4">
                  <div className="text-primary mb-3">
                    <i className="fas fa-balance-scale fa-3x"></i>
                  </div>
                  <h3 className="h4">Grievances & Redress</h3>
                  <p>Seek redress for a personal or community grievance where existing legal avenues have been exhausted.</p>
                </div>
              </div>
            </div>
            <div className="col-md-4">
              <div className="card h-100 border-0 shadow-sm">
                <div className="card-body text-center p-4">
                  <div className="text-primary mb-3">
                    <i className="fas fa-shield-alt fa-3x"></i>
                  </div>
                  <h3 className="h4">Accountability</h3>
                  <p>Petition concerning the conduct of public officers or state organs to ensure accountability.</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Track Petitions Section */}
      <section className="py-5 bg-white">
        <div className="container">
          <div className="row align-items-center">
            <div className="col-lg-6 order-lg-2 mb-4 mb-lg-0">
              <div className="position-relative overflow-hidden rounded shadow-lg" style={{ height: '300px' }}>
                <div className="position-absolute w-100 h-100 d-flex align-items-center justify-content-center" style={{ background: 'rgba(0, 100, 0, 0.1)' }}>
                  <i className="fas fa-chart-line text-success" style={{ fontSize: '5rem', opacity: 0.8 }}></i>
                </div>
              </div>
            </div>
            <div className="col-lg-6 order-lg-1 pe-lg-5">
              <h2 className="h2 mb-4">Track Petitions in Real-Time</h2>
              <p className="lead mb-4">Stay informed about the progress of petitions submitted to the Kenyan Parliament.</p>
              <div className="d-flex flex-column flex-md-row gap-3">
                <Link to="/petition-tracker" className="btn btn-success btn-md d-flex align-items-center justify-content-center gap-2">
                  <i className="fas fa-external-link-alt"></i>
                  Visit Petition Tracker
                </Link>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Procedure Section */}
      <section className="py-5 bg-light" id="procedure">
        <div className="container">
          <div className="section-title">
            <h2>The Petition Procedure</h2>
            <p>How to submit your petition in 4 clear steps</p>
          </div>
          <div className="timeline position-relative">
            <div className="timeline-item position-relative mb-5">
              <div className="timeline-content p-4 bg-white rounded shadow-sm">
                <h3 className="h4">1. Draft Your Petition</h3>
                <p>Write a clear, formal letter addressed to the <strong>Clerk of the National Assembly</strong>. Include your full name, ID number, postal address, a clear statement of the issue, background facts, and a specific "prayer" (what you want Parliament to do).</p>
              </div>
            </div>
            <div className="timeline-item position-relative mb-5">
              <div className="timeline-content p-4 bg-white rounded shadow-sm">
                <h3 className="h4">2. Get Signatures (Optional)</h3>
                <p>While one person can petition, gathering signatures from other concerned citizens strengthens your petition's impact and shows widespread support for your cause.</p>
              </div>
            </div>
            <div className="timeline-item position-relative mb-5">
              <div className="timeline-content p-4 bg-white rounded shadow-sm">
                <h3 className="h4">3. Submit the Signed Petition</h3>
                <p>Choose between two submission methods: directly upload your already signed PDF, or use our guided form to structure your petition before printing, signing, and uploading.</p>
              </div>
            </div>
            <div className="timeline-item position-relative">
              <div className="timeline-content p-4 bg-white rounded shadow-sm">
                <h3 className="h4">4. Track & Follow Up</h3>
                <p>Once submitted, the Office of Clerk will acknowledge receipt. If compliant with Standing Orders, your petition will be tabled in Parliament and assigned to a relevant committee for consideration.</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Submission Methods Section */}
      <section className="py-5" id="submit">
        <div className="container">
          <div className="section-title">
            <h2>Two Ways to Submit Your Petition</h2>
            <p>Choose the submission method that works best for you</p>
          </div>
          <div className="row g-4">
            <div className="col-lg-6">
              <div className="card h-100 border-0 shadow-sm">
                <div className="card-body p-4 text-center">
                  <div className="text-primary mb-3">
                    <i className="fas fa-file-upload fa-3x"></i>
                  </div>
                  <h3 className="h4">Upload a Ready Petition (Fastest)</h3>
                  <p className="mb-4">Ideal for individuals/groups who have already drafted and signed their petition document.</p>
                  <p className="text-muted"><strong>Process:</strong> Click the button below, fill in your contact details, and attach your signed PDF.</p>
                  <Link to="/create-petition" className="btn btn-success">Upload Signed PDF Now</Link>
                </div>
              </div>
            </div>
            <div className="col-lg-6">
              <div className="card h-100 border-0 shadow-sm">
                <div className="card-body p-4 text-center">
                  <div className="text-primary mb-3">
                    <i className="fas fa-file-signature fa-3x"></i>
                  </div>
                  <h3 className="h4">Use Our Guided Form</h3>
                  <p className="mb-4">Ideal for first-time petitioners who want a structured template to ensure they include all required information.</p>
                  <p className="text-muted"><strong>Process:</strong> Fill in an online form, download the generated PDF, print it, sign it, and then upload it.</p>
                  <Link to="/create-petition" className="btn btn-success">Start Guided Form</Link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Requirements Section */}
      <section className="py-5 bg-light">
        <div className="container">
          <div className="section-title">
            <h2>Key Requirements & Important Notes</h2>
            <p>What you need to know before submitting your petition</p>
          </div>
          <div className="row g-4">
            <div className="col-md-6 col-lg-3">
              <div className="d-flex h-100">
                <div className="me-3 text-primary">
                  <i className="fas fa-signature fa-2x"></i>
                </div>
                <div>
                  <h4>Signature is Mandatory</h4>
                  <p className="text-muted mb-0">An unsigned petition will be rejected. Ensure your petition is properly signed before submission.</p>
                </div>
              </div>
            </div>
            <div className="col-md-6 col-lg-3">
              <div className="d-flex h-100">
                <div className="me-3 text-primary">
                  <i className="fas fa-check-circle fa-2x"></i>
                </div>
                <div>
                  <h4>Be Factual</h4>
                  <p className="text-muted mb-0">Ensure all information provided is accurate and truthful. Petitions with false information will be rejected.</p>
                </div>
              </div>
            </div>
            <div className="col-md-6 col-lg-3">
              <div className="d-flex h-100">
                <div className="me-3 text-primary">
                  <i className="fas fa-road fa-2x"></i>
                </div>
                <div>
                  <h4>Exhaust Other Avenues</h4>
                  <p className="text-muted mb-0">For grievances, show that you have tried other legal or administrative remedies first before petitioning Parliament.</p>
                </div>
              </div>
            </div>
            <div className="col-md-6 col-lg-3">
              <div className="d-flex h-100">
                <div className="me-3 text-primary">
                  <i className="fas fa-language fa-2x"></i>
                </div>
                <div>
                  <h4>No Language Barriers</h4>
                  <p className="text-muted mb-0">Petitions can be submitted in either English or Kiswahili, Kenya's official languages.</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* FAQ Section */}
      <section className="py-5" id="faq">
        <div className="container">
          <div className="section-title">
            <h2>Frequently Asked Questions</h2>
            <p>Common questions about the petition process</p>
          </div>
          <div className="row justify-content-center">
            <div className="col-lg-8">
              <div className="accordion" id="faqAccordion">
                {[
                  {
                    question: 'How long does the process take?',
                    answer: 'There is no fixed timeline. It depends on the complexity of the issue and the Parliamentary calendar. The process ensures thorough consideration of each petition.'
                  },
                  {
                    question: 'Can my petition be rejected?',
                    answer: 'Yes, if it does not comply with the Standing Orders (e.g., unsigned, frivolous, or contains unparliamentary language). The Speaker will determine if a petition meets the requirements.'
                  },
                  {
                    question: 'Do I need a lawyer?',
                    answer: 'No, you do not. The process is designed for ordinary citizens. However, you may seek legal assistance if you wish to ensure your petition is properly structured.'
                  },
                  {
                    question: 'Where can I read the official Standing Orders?',
                    answer: 'You can find them on the official Kenyan Parliament website: <a href="http://www.parliament.go.ke" target="_blank" rel="noopener noreferrer">www.parliament.go.ke</a>.'
                  }
                ].map((item, index) => (
                  <div className="accordion-item mb-3 border-0 shadow-sm" key={index}>
                    <h3 className="accordion-header" id={`heading${index}`}>
                      <button
                        className="accordion-button collapsed bg-white text-dark"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target={`#collapse${index}`}
                        aria-expanded="false"
                        aria-controls={`collapse${index}`}
                      >
                        {item.question}
                      </button>
                    </h3>
                    <div
                      id={`collapse${index}`}
                      className="accordion-collapse collapse"
                      aria-labelledby={`heading${index}`}
                      data-bs-parent="#faqAccordion"
                    >
                      <div className="accordion-body">
                        <div dangerouslySetInnerHTML={{ __html: item.answer }} />
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="py-5 text-white" style={{
        background: 'linear-gradient(rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0.7)), url(https://images.unsplash.com/photo-1582213782179-e0d53f98f2ca?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1800&q=80) no-repeat center center/cover'
      }}>
        <div className="container text-center py-5">
          <h2 className="display-5 fw-normal mb-4">Ready to Make a Difference?</h2>
          <p className="lead mb-5">Your petition is the first step towards official parliamentary action. Submit yours today and be part of the democratic process.</p>
          <div className="d-flex justify-content-center gap-3">
            <Link to="/create-petition" className="btn btn-warning">Upload Your Signed Petition PDF</Link>
            <a
              href="/petition-template.pdf"
              className="btn btn-outline"
              target="_blank"
              rel="noopener noreferrer"
            >
              <i className="fas fa-file-pdf me-2"></i>View/Download Petition Template
            </a>
          </div>
        </div>
      </section>


      {/* Add some custom styles for the timeline */}
      <style jsx>{`
        .timeline::before {
          content: '';
          position: absolute;
          width: 2px;
          background-color: var(--primary);
          top: 0;
          bottom: 0;
          left: 50%;
          margin-left: -1px;
        }
        
        .timeline-item {
          padding: 10px 0;
          position: relative;
          width: 50%;
          box-sizing: border-box;
        }
        
        .timeline-item:nth-child(odd) {
          left: 0;
          padding-right: 40px;
          text-align: right;
        }
        
        .timeline-item:nth-child(even) {
          left: 50%;
          padding-left: 40px;
          text-align: left;
        }
        
        .timeline-item::after {
          content: '';
          position: absolute;
          width: 20px;
          height: 20px;
          background-color: white;
          border: 3px solid var(--primary);
          border-radius: 50%;
          top: 20px;
          z-index: 1;
        }
        
        .timeline-item:nth-child(odd)::after {
          right: -10px;
        }
        
        .timeline-item:nth-child(even)::after {
          left: -10px;
        }
        
        @media (max-width: 768px) {
          .timeline::before {
            left: 31px;
          }
          
          .timeline-item {
            width: 100%;
            padding-left: 70px;
            padding-right: 0;
            text-align: left;
          }
          
          .timeline-item:nth-child(even) {
            left: 0;
            padding-left: 70px;
            text-align: left;
          }
          
          .timeline-item::after {
            left: 20px !important;
          }
          
          .mobile-menu {
            display: block !important;
          }
          
          nav ul {
            display: none;
            position: absolute;
            top: 100%;
            left: 0;
            right: 0;
            background-color: var(--primary);
            flex-direction: column;
            padding: 15px 0;
            margin: 0;
          }
          
          nav ul.show {
            display: flex;
          }
          
          nav ul li {
            margin: 5px 0;
            padding: 0 15px;
          }
        }
      `}</style>
    </div>
  );
}
