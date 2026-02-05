import React, { useState, useEffect, useContext } from "react";
import { Link, useNavigate } from "react-router-dom";
import { toast } from 'react-toastify';
import { getToken } from '../service/tokenStorage';
import { AuthContext } from '../context/AuthContext';
import config from '../config';
import { FaEnvelope, FaArrowLeft } from 'react-icons/fa';

export default function ForgotPassword() {
    const [formData, setFormData] = useState({
        email: ''
    });
    const [errors, setErrors] = useState({});
    const [isSubmitting, setIsSubmitting] = useState(false);
    const navigate = useNavigate();
    const { isAuthenticated } = useContext(AuthContext);

    // Redirect if already authenticated
    useEffect(() => {
        const token = getToken();
        if (token || isAuthenticated) {
            navigate('/dashboard');
        }
    }, [navigate, isAuthenticated]);

    const validateEmail = (email) => {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!email) return 'Email is required';
        if (!emailRegex.test(email)) return 'Please enter a valid email address';
        return '';
    };

    const validateForm = () => {
        const newErrors = {};

        const emailError = validateEmail(formData.email);
        if (emailError) newErrors.email = emailError;

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));

        if (name === 'email' && errors.email) {
            setErrors(prev => ({
                ...prev,
                email: validateEmail(value)
            }));
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!validateForm() || isSubmitting) return;

        setIsSubmitting(true);

        try {
            const response = await fetch(`${config.api.baseURL}/Client/forgot-password`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ email: formData.email })
            });

            const data = await response.json();

            if (response.ok) {
                toast.success(data.message || 'Password reset code sent successfully!');
                navigate('/reset', { state: { email: formData.email } });
            } else {
                toast.error(data.message || 'Failed to send reset code. Please try again.');
            }
        } catch (error) {
            console.error('Error:', error);
            toast.error('An error occurred. Please try again.');
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="login-container">
            <div className="login-content">
                {/* Illustration Side */}
                <div className="illustration-side">
                    <div className="computer-container">
                        <div className="computer-screen">
                            <div className="screen-content">
                                <div className="screen-line">
                                    <span className="prompt"></span>
                                    <span>Password Recovery</span>
                                    <span className="cursor"></span>
                                </div>
                                <div className="screen-line">
                                    <span className="prompt"></span>
                                    <span>Enter your email to receive</span>
                                </div>
                                <div className="screen-line">
                                    <span className="prompt"></span>
                                    <span>password reset instructions</span>
                                </div>
                                <div className="screen-line">
                                    <span className="prompt"></span>
                                    <span>Initializing recovery...</span>
                                </div>
                            </div>
                        </div>
                        <div className="computer-base">
                            <div className="computer-stand"></div>
                        </div>
                    </div>
                    <div className="welcome-text">
                        <h2>Account Recovery</h2>
                        <p>We'll help you get back to your account</p>
                    </div>
                </div>

                {/* Form Side */}
                <div className="form-side">
                    <div className="back-to-login">
                        <Link to="/login" className="back-link">
                            <FaArrowLeft className="back-icon" />
                            Back to Login
                        </Link>
                    </div>

                    <div className="form-header">
                        <h2>Reset Password</h2>
                        <p>Enter your email to receive password reset instructions</p>
                    </div>

                    <form onSubmit={handleSubmit} className="login-form">
                        <div className="form-group">
                            <label htmlFor="email">Email Address</label>
                            <div className="input-with-icon">
                                <FaEnvelope className="input-icon" />
                                <input
                                    type="email"
                                    id="email"
                                    name="email"
                                    className={`form-control ${errors.email ? 'input-error' : formData.email && 'input-valid'}`}
                                    value={formData.email}
                                    onChange={handleChange}
                                    disabled={isSubmitting}
                                    autoComplete="email"
                                    placeholder="Enter your email"
                                />
                            </div>
                            {errors.email && (
                                <div className="error-message">
                                    {errors.email}
                                </div>
                            )}
                        </div>

                        <button
                            type="submit"
                            className="login-btn"
                            disabled={isSubmitting}
                        >
                            {isSubmitting ? (
                                <>
                                    <span className="spinner"></span>
                                    Sending Reset Link...
                                </>
                            ) : (
                                'Send Reset Link'
                            )}
                        </button>
                    </form>

                    <div className="signup-link">
                        Remember your password?{' '}
                        <Link to="/login" className="signup-text">Sign in here</Link>
                    </div>
                </div>
            </div>

            <style jsx>{`
                @keyframes blink {
                    0%, 100% { opacity: 1; }
                    50% { opacity: 0; }
                }

                .login-container {
                    background: linear-gradient(135deg, #f5f7fa 0%, #e4edf5 100%);
                    min-height: 100vh;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    padding: 20px 20px 40px;
                }

                .login-content {
                    display: flex;
                    width: 100%;
                    max-width: 1000px;
                    height: auto;
                    min-height: 600px;
                    background: #ffffff;
                    border-radius: 20px;
                    overflow: hidden;
                    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
                    margin-top: 20px;
                }

                /* Illustration Side Styles */
                .illustration-side {
                    flex: 1;
                    background: linear-gradient(135deg, #006600 0%, #004d00 100%);
                    display: flex;
                    flex-direction: column;
                    align-items: center;
                    justify-content: center;
                    padding: 40px;
                    color: #ffffff;
                    position: relative;
                    overflow: hidden;
                }

                .illustration-side::before, .illustration-side::after {
                    content: '';
                    position: absolute;
                    border-radius: 50%;
                    background: rgba(255, 255, 255, 0.1);
                }

                .illustration-side::before {
                    width: 200px;
                    height: 200px;
                    top: -50px;
                    left: -50px;
                }

                .illustration-side::after {
                    width: 150px;
                    height: 150px;
                    bottom: -50px;
                    right: -50px;
                    background: rgba(255, 255, 255, 0.08);
                }

                .computer-container {
                    position: relative;
                    width: 100%;
                    max-width: 350px;
                    z-index: 2;
                }

                .computer-screen {
                    width: 100%;
                    height: 220px;
                    background: #1a1a2e;
                    border-radius: 12px 12px 0 0;
                    position: relative;
                    overflow: hidden;
                    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
                    padding: 20px;
                    color: #4dccff;
                    font-family: 'Courier New', monospace;
                }

                .screen-content {
                    height: 100%;
                    display: flex;
                    flex-direction: column;
                    justify-content: center;
                }

                .screen-line {
                    margin-bottom: 8px;
                    display: flex;
                    align-items: center;
                }

                .prompt {
                    color: #00ff9d;
                    margin-right: 10px;
                }

                .cursor {
                    display: inline-block;
                    width: 8px;
                    height: 16px;
                    background: #4dccff;
                    margin-left: 5px;
                    animation: blink 1s infinite;
                }

                .computer-base {
                    width: 100%;
                    height: 20px;
                    background: #d1d1d1;
                    border-radius: 0 0 8px 8px;
                    position: relative;
                }

                .computer-stand {
                    width: 60px;
                    height: 30px;
                    background: #b8b8b8;
                    margin: 0 auto;
                    border-radius: 0 0 5px 5px;
                }

                .welcome-text {
                    text-align: center;
                    margin-top: 40px;
                    z-index: 2;
                }

                .welcome-text h2 {
                    font-size: 1.8rem;
                    margin-bottom: 10px;
                    font-weight: 600;
                }

                .welcome-text p {
                    opacity: 0.9;
                    font-size: 1rem;
                }

                /* Form Side Styles */
                .form-side {
                    flex: 1;
                    display: flex;
                    flex-direction: column;
                    justify-content: center;
                    padding: 50px;
                    background: #ffffff;
                    position: relative;
                }

                .back-to-login {
                    position: absolute;
                    top: 30px;
                    left: 50px;
                }

                .back-link {
                    display: flex;
                    align-items: center;
                    color: #006600;
                    text-decoration: none;
                    font-weight: 500;
                    font-size: 0.9rem;
                    transition: color 0.3s;
                }

                .back-link:hover {
                    color: #004d00;
                    text-decoration: underline;
                }

                .back-icon {
                    margin-right: 8px;
                }

                .form-header {
                    margin-bottom: 30px;
                    text-align: center;
                }

                .form-header h2 {
                    font-size: 1.8rem;
                    color: #212529;
                    margin-bottom: 8px;
                }

                .form-header p {
                    color: #6c757d;
                    margin-bottom: 0;
                }

                .login-form {
                    width: 100%;
                }

                .form-group {
                    margin-bottom: 20px;
                    position: relative;
                }

                .form-group label {
                    display: block;
                    margin-bottom: 8px;
                    color: #212529;
                    font-weight: 500;
                }

                .input-with-icon {
                    position: relative;
                }

                .input-icon {
                    position: absolute;
                    left: 15px;
                    top: 50%;
                    transform: translateY(-50%);
                    color: #6c757d;
                    z-index: 2;
                }

                .form-control {
                    width: 100%;
                    padding: 15px 15px 15px 45px;
                    border: 1.5px solid #e9ecef;
                    border-radius: 10px;
                    font-size: 1rem;
                    transition: all 0.3s;
                    background: #ffffff;
                }

                .form-control:focus {
                    outline: none;
                    border-color: #006600;
                    box-shadow: 0 0 0 3px rgba(0, 102, 0, 0.1);
                }

                .input-error {
                    border-color: #dc3545 !important;
                }

                .input-valid {
                    border-color: #198754 !important;
                }

                .error-message {
                    color: #dc3545;
                    font-size: 0.875rem;
                    margin-top: 5px;
                }

                .login-btn {
                    display: block;
                    width: 100%;
                    padding: 15px;
                    background: #006600;
                    color: #ffffff;
                    border: none;
                    border-radius: 10px;
                    font-size: 1rem;
                    font-weight: 600;
                    cursor: pointer;
                    transition: all 0.3s;
                    margin-top: 10px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    gap: 8px;
                }

                .login-btn:hover:not(:disabled) {
                    background: #004d00;
                    transform: translateY(-2px);
                    box-shadow: 0 5px 15px rgba(0, 102, 0, 0.2);
                }

                .login-btn:disabled {
                    opacity: 0.7;
                    cursor: not-allowed;
                    transform: none;
                }

                .spinner {
                    width: 16px;
                    height: 16px;
                    border: 2px solid transparent;
                    border-top: 2px solid #ffffff;
                    border-radius: 50%;
                    animation: spin 1s linear infinite;
                }

                @keyframes spin {
                    0% { transform: rotate(0deg); }
                    100% { transform: rotate(360deg); }
                }

                .signup-link {
                    text-align: center;
                    margin-top: 30px;
                    color: #6c757d;
                }

                .signup-text {
                    color: #006600;
                    text-decoration: none;
                    font-weight: 500;
                }

                .signup-text:hover {
                    text-decoration: underline;
                }

                /* Responsive Design */
                @media (max-width: 768px) {
                    .login-content {
                        flex-direction: column;
                        height: auto;
                    }
                    
                    .illustration-side {
                        padding: 30px;
                        min-height: 300px;
                    }
                    
                    .computer-container {
                        max-width: 250px;
                    }
                    
                    .computer-screen {
                        height: 160px;
                    }
                    
                    .form-side {
                        padding: 40px 30px;
                    }

                    .back-to-login {
                        position: static;
                        margin-bottom: 20px;
                        text-align: center;
                    }
                }
            `}</style>
        </div>
    );
}