import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { FaCheck, FaArrowRight } from 'react-icons/fa';
import config from '../config';

export default function Activate() {
    const location = useLocation();
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [activationCode, setActivationCode] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);

    useEffect(() => {
        if (location.state && location.state.email) {
            setEmail(location.state.email);
        }
    }, [location]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!email || !activationCode) {
            toast.error('Please enter both email and activation code.');
            return;
        }

        setIsSubmitting(true);
        try {
            const response = await fetch(`${config.api.baseURL}/client/activate`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ email, activationCode })
            });

            const data = await response.json();

            if (response.ok) {
                toast.success('Account activated successfully! Please login.');
                navigate('/login');
            } else {
                const errorMessage = typeof data === 'string' ? data : (data.message || data.errors?.[0] || 'Activation failed. Please check your code.');
                toast.error(errorMessage);
            }
        } catch (error) {
            toast.error('An error occurred. Please try again.');
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="activation-container">
            <div className="activation-card">
                <h2>Activate Account</h2>
                <p>Please enter the activation code sent to your email.</p>

                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label>Email Address</label>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            placeholder="Enter your email"
                            disabled={isSubmitting}
                            required
                        />
                    </div>

                    <div className="form-group">
                        <label>Activation Code</label>
                        <input
                            type="text"
                            value={activationCode}
                            onChange={(e) => setActivationCode(e.target.value)}
                            placeholder="Enter activation code"
                            disabled={isSubmitting}
                            required
                        />
                    </div>

                    <button type="submit" className="btn-activate" disabled={isSubmitting}>
                        {isSubmitting ? 'Activating...' : 'Activate Account'}
                    </button>
                </form>
            </div>

            <style jsx>{`
                .activation-container {
                    min-height: 100vh;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    background: linear-gradient(135deg, #f5f7fa 0%, #e4edf5 100%);
                    padding: 20px;
                }
                
                .activation-card {
                    background: white;
                    padding: 40px;
                    border-radius: 20px;
                    box-shadow: 0 10px 40px rgba(0, 0, 0, 0.08);
                    max-width: 500px;
                    width: 100%;
                    text-align: center;
                }
                
                h2 {
                    color: #212529;
                    margin-bottom: 10px;
                }
                
                p {
                    color: #6c757d;
                    margin-bottom: 30px;
                }
                
                .form-group {
                    margin-bottom: 20px;
                    text-align: left;
                }
                
                label {
                    display: block;
                    margin-bottom: 8px;
                    color: #212529;
                    font-weight: 500;
                }
                
                input {
                    width: 100%;
                    padding: 15px;
                    border: 1.5px solid #e9ecef;
                    border-radius: 10px;
                    font-size: 1rem;
                    transition: all 0.3s;
                }
                
                input:focus {
                    outline: none;
                    border-color: #006600;
                }
                
                .btn-activate {
                    width: 100%;
                    padding: 15px;
                    background: #006600;
                    color: white;
                    border: none;
                    border-radius: 10px;
                    font-size: 1rem;
                    font-weight: 600;
                    cursor: pointer;
                    transition: all 0.3s;
                }
                
                .btn-activate:hover:not(:disabled) {
                    background: #004d00;
                }
                
                .btn-activate:disabled {
                    opacity: 0.7;
                    cursor: not-allowed;
                }
            `}</style>
        </div>
    );
}
