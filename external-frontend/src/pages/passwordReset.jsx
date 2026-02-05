import React, { useState, useRef, useEffect, useContext } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { toast } from 'react-toastify';
import { FaEye, FaEyeSlash, FaArrowLeft, FaLock, FaKey } from 'react-icons/fa';
import { getToken } from '../service/tokenStorage';
import { AuthContext } from '../context/AuthContext';
import config from '../config';

export default function PasswordReset() {
    const [digits, setDigits] = useState(Array(6).fill(''));
    const [formData, setFormData] = useState({
        password: '',
        confirmPassword: ''
    });
    const [errors, setErrors] = useState({});
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const inputRefs = useRef(Array(6).fill(null));
    const navigate = useNavigate();
    const location = useLocation();
    const email = location.state?.email;
    const { isAuthenticated } = useContext(AuthContext);

    useEffect(() => {
        const token = getToken();
        if (token || isAuthenticated) {
            navigate('/dashboard');
            return;
        }

        if (!email) {
            toast.error('No email provided. Please request a new password reset link.');
            navigate('/forgot');
        } else {
            inputRefs.current[0]?.focus();
        }
    }, [email, navigate, isAuthenticated]);

    const handleDigitChange = (index, value) => {
        if (value && !/^\d$/.test(value)) return;

        const newDigits = [...digits];
        newDigits[index] = value;
        setDigits(newDigits);

        if (value && index < 5) {
            inputRefs.current[index + 1]?.focus();
        }
    };

    const handleKeyDown = (index, e) => {
        if (e.key === 'Backspace' && !digits[index] && index > 0) {
            const newDigits = [...digits];
            newDigits[index - 1] = '';
            setDigits(newDigits);
            inputRefs.current[index - 1]?.focus();
        }
    };

    const validateForm = () => {
        const newErrors = {};

        if (digits.some(d => !d)) {
            newErrors.code = 'Please enter the 6-digit verification code';
        }

        if (!formData.password) {
            newErrors.password = 'Password is required';
        } else if (formData.password.length < 8) {
            newErrors.password = 'Password must be at least 8 characters long';
        } else if (!/\d/.test(formData.password)) {
            newErrors.password = 'Password must contain at least one number';
        } else if (!/[!@#$%^&*]/.test(formData.password)) {
            newErrors.password = 'Password must contain at least one special character';
        }

        if (!formData.confirmPassword) {
            newErrors.confirmPassword = 'Please confirm your password';
        } else if (formData.password !== formData.confirmPassword) {
            newErrors.confirmPassword = 'Passwords do not match';
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));

        if (errors[name]) {
            setErrors(prev => ({
                ...prev,
                [name]: ''
            }));
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!validateForm()) return;

        setIsSubmitting(true);

        try {
            const response = await fetch(
                `${config.api.baseURL}/Client/reset-password`,
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        email: email,
                        token: digits.join(''),
                        newPassword: formData.password
                    })
                }
            );

            const data = await response.json();

            if (response.ok) {
                toast.success(data.message || 'Password reset successfully!');
                navigate('/login');
            } else {
                toast.error(data.message || 'Failed to reset password. Please try again.');
                setDigits(Array(6).fill(''));
                inputRefs.current[0]?.focus();
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
                                    <span>Password Reset</span>
                                    <span className="cursor"></span>
                                </div>
                                <div className="screen-line">
                                    <span className="prompt"></span>
                                    <span>Verification Code Sent</span>
                                </div>
                                <div className="screen-line">
                                    <span className="prompt"></span>
                                    <span>Enter the 6-digit code</span>
                                </div>
                                <div className="screen-line">
                                    <span className="prompt"></span>
                                    <span>And set a new password</span>
                                </div>
                            </div>
                        </div>
                        <div className="computer-base">
                            <div className="computer-stand"></div>
                        </div>
                    </div>
                    <div className="welcome-text">
                        <h2>Reset Your Password</h2>
                        <p>Create a new secure password to access your account</p>
                    </div>
                </div>

                {/* Form Side */}
                <div className="form-side">
                    <div className="back-to-login">
                        <Link to="/forgot" className="back-link">
                            <FaArrowLeft className="back-icon" />
                            Back to Forgot Password
                        </Link>
                    </div>

                    <div className="form-header">
                        <h2>Reset Password</h2>
                        <p>We've sent a 6-digit code to <strong>{email}</strong></p>
                    </div>

                    <form onSubmit={handleSubmit} className="login-form">
                        <div className="form-group">
                            <label>Verification Code</label>
                            <div className="verification-code-container">
                                {digits.map((digit, index) => (
                                    <input
                                        key={index}
                                        ref={el => inputRefs.current[index] = el}
                                        type="text"
                                        className={`verification-digit ${errors.code ? 'input-error' : digit && 'input-valid'}`}
                                        value={digit}
                                        onChange={(e) => handleDigitChange(index, e.target.value)}
                                        onKeyDown={(e) => handleKeyDown(index, e)}
                                        maxLength={1}
                                        disabled={isSubmitting}
                                        inputMode="numeric"
                                        pattern="[0-9]*"
                                        autoComplete="one-time-code"
                                    />
                                ))}
                            </div>
                            {errors.code && (
                                <div className="error-message">
                                    {errors.code}
                                </div>
                            )}
                        </div>

                        <div className="form-group">
                            <label htmlFor="password">New Password</label>
                            <div className="input-with-icon">
                                <FaLock className="input-icon" />
                                <input
                                    type={showPassword ? 'text' : 'password'}
                                    id="password"
                                    name="password"
                                    className={`form-control ${errors.password ? 'input-error' : formData.password && 'input-valid'}`}
                                    value={formData.password}
                                    onChange={handleChange}
                                    disabled={isSubmitting}
                                    placeholder="Enter your new password"
                                />
                                <button
                                    type="button"
                                    className="password-toggle"
                                    onClick={() => setShowPassword(!showPassword)}
                                    tabIndex="-1"
                                >
                                    {showPassword ? <FaEyeSlash /> : <FaEye />}
                                </button>
                            </div>
                            {errors.password && (
                                <div className="error-message">
                                    {errors.password}
                                </div>
                            )}
                        </div>

                        <div className="form-group">
                            <label htmlFor="confirmPassword">Confirm New Password</label>
                            <div className="input-with-icon">
                                <FaKey className="input-icon" />
                                <input
                                    type={showConfirmPassword ? 'text' : 'password'}
                                    id="confirmPassword"
                                    name="confirmPassword"
                                    className={`form-control ${errors.confirmPassword ? 'input-error' : formData.confirmPassword && 'input-valid'}`}
                                    value={formData.confirmPassword}
                                    onChange={handleChange}
                                    disabled={isSubmitting}
                                    placeholder="Confirm your new password"
                                />
                                <button
                                    type="button"
                                    className="password-toggle"
                                    onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                                    tabIndex="-1"
                                >
                                    {showConfirmPassword ? <FaEyeSlash /> : <FaEye />}
                                </button>
                            </div>
                            {errors.confirmPassword && (
                                <div className="error-message">
                                    {errors.confirmPassword}
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
                                    Resetting Password...
                                </>
                            ) : (
                                'Reset Password'
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

                .verification-code-container {
                    display: flex;
                    justify-content: center;
                    gap: 10px;
                    margin-bottom: 5px;
                }

                .verification-digit {
                    width: 45px;
                    height: 60px;
                    text-align: center;
                    font-size: 1.5rem;
                    border: 1.5px solid #e9ecef;
                    border-radius: 10px;
                    transition: all 0.3s;
                    caret-color: transparent;
                }

                .verification-digit:focus {
                    outline: none;
                    border-color: #006600;
                    box-shadow: 0 0 0 3px rgba(0, 102, 0, 0.1);
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

                .password-toggle {
                    position: absolute;
                    right: 15px;
                    top: 50%;
                    transform: translateY(-50%);
                    background: none;
                    border: none;
                    color: #6c757d;
                    cursor: pointer;
                    padding: 5px;
                    z-index: 2;
                }

                .password-toggle:focus {
                    outline: none;
                    color: #006600;
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

                    .verification-digit {
                        width: 40px;
                        height: 55px;
                        font-size: 1.3rem;
                    }
                }

                @media (max-width: 480px) {
                    .verification-digit {
                        width: 35px;
                        height: 50px;
                        font-size: 1.1rem;
                    }
                }
            `}</style>
        </div>
    );
}