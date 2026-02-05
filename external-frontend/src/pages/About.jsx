import React, { useEffect, useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { getToken } from '../service/tokenStorage';
import { AuthContext } from '../context/AuthContext';

export default function About() {
  const navigate = useNavigate();
  const { isAuthenticated } = useContext(AuthContext);

  // Redirect if already authenticated
  useEffect(() => {
    const token = getToken();
    if (token || isAuthenticated) {
      navigate('/dashboard');
    }
  }, [navigate, isAuthenticated]);

  return (
    <div>
      <header className="bg-light py-5">
        <div className="container text-center">
          <h1 className="display-4">About the Petition Process</h1>
          <p className="lead">
            Learn how public petitions work and their importance in the legislative process
          </p>
        </div>
      </header>

      <main className="py-5">
        <div className="container">
          <div className="row">
            <div className="col-lg-8 mx-auto">
              <div className="card shadow-sm mb-5">
                <div className="card-body p-5">
                  <h2 className="mb-4">What is a Public Petition?</h2>
                  <p className="lead">
                    A public petition is a formal written request, typically signed by many people, appealing to authority in respect of a particular cause.
                  </p>
                  <p>
                    In the context of the Parliament of Kenya, public petitions are a vital tool for citizens to bring their concerns to the attention of their elected representatives.
                  </p>

                  <h3 className="mt-5 mb-3">Legal Basis</h3>
                  <p>
                    The right to petition Parliament is enshrined in Article 119 of the Constitution of Kenya 2010 and is further operationalized through Standing Order 223.
                  </p>
                  <div className="alert alert-info">
                    <h5>Standing Order 223</h5>
                    <p className="mb-0">
                      Any member of the public may petition the National Assembly on matters of public policy or grievance.
                    </p>
                  </div>

                  <h3 className="mt-5 mb-3">The Petition Process</h3>
                  <div className="row g-4">
                    {[
                      { step: "1. Submission", text: "Submit your petition through this platform or directly to the Clerk of the National Assembly." },
                      { step: "2. Acknowledgment", text: "You'll receive an acknowledgment and a reference number for tracking." },
                      { step: "3. Committee Review", text: "The petition is referred to the relevant committee for consideration." },
                      { step: "4. Hearing", text: "You may be invited to present your petition before the committee." },
                      { step: "5. Report", text: "The committee prepares a report with recommendations." },
                      { step: "6. Action", text: "The House considers the report and takes appropriate action." }
                    ].map((item, idx) => (
                      <div key={idx} className="col-md-6">
                        <div className="card h-100 border-0 shadow-sm">
                          <div className="card-body">
                            <h5 className="card-title">{item.step}</h5>
                            <p className="card-text">{item.text}</p>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>

                  <h3 className="mt-5 mb-3">Requirements for a Valid Petition</h3>
                  <ul className="list-group list-group-flush mb-4">
                    <li className="list-group-item">Must be in writing and in English or Swahili</li>
                    <li className="list-group-item">Must state the petitioner's name and contact details</li>
                    <li className="list-group-item">Must clearly state the grievance or matter of concern</li>
                    <li className="list-group-item">Must indicate the action being sought from Parliament</li>
                    <li className="list-group-item">Must be signed by the petitioner(s)</li>
                  </ul>

                  <div className="alert alert-warning mt-5">
                    <h5>Important Note</h5>
                    <p className="mb-0">
                      Petitions that are frivolous, defamatory, or sub judice will not be considered.
                    </p>
                  </div>

                  <div className="text-center mt-5">
                    <Link to="/create-petition" className="btn btn-success btn-lg">Start Your Petition</Link>
                      
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>

    </div>
  );
}