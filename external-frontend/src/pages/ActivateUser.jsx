import React, { useState, useEffect, useRef, useContext } from 'react';
import { useNavigate, useLocation, Link } from 'react-router-dom';
import { toast } from 'react-toastify';
import { FaArrowLeft, FaEnvelope, FaCheck, FaRedo } from 'react-icons/fa';
import { getToken } from '../service/tokenStorage';
import { AuthContext } from '../context/AuthContext';
import config from '../config';

export default function ActivateUser() {
    const [digits, setDigits] = useState(Array(6).fill(''));
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [animation,] = useState('fadeIn');
    const inputRefs = useRef(Array(6).fill(null));
    const navigate = useNavigate();
    const location = useLocation();
    const email = location.state?.email;
    const { isAuthenticated } = useContext(AuthContext);

    useEffect(() => {
        // Check if user is already authenticated
        const token = getToken();
        if (token || isAuthenticated) {
            navigate('/dashboard');
            return;
        }
        
        // Original email check
        if (!email) {
            toast.error('No email provided. Please complete the registration process.');
            navigate('/signup');
        } else {
            // Focus first input on mount
            inputRefs.current[0]?.focus();
        }
    }, [email, navigate, isAuthenticated]);

    const handleChange = (index, value) => {
        // Only allow single digit
        if (value && !/^\d$/.test(value)) return;
        
        const newDigits = [...digits];
        newDigits[index] = value;
        setDigits(newDigits);

        // Auto-focus next input if current input has a value
        if (value && index < 5) {
            inputRefs.current[index + 1]?.focus();
        }

        // Auto-submit if all digits are filled
        if (newDigits.every(digit => digit !== '') && index === 5) {
            handleSubmit(newDigits.join(''));
        }
    };

    const handleKeyDown = (index, e) => {
        // Handle backspace to move to previous input
        if (e.key === 'Backspace' && !digits[index] && index > 0) {
            const newDigits = [...digits];
            newDigits[index - 1] = '';
            setDigits(newDigits);
            inputRefs.current[index - 1]?.focus();
        }
    };

    const handlePaste = (e) => {
        e.preventDefault();
        const pasteData = e.clipboardData.getData('text/plain').trim();
        const pasteDigits = pasteData.split('').slice(0, 6);
        
        if (pasteDigits.every(digit => /^\d$/.test(digit))) {
            const newDigits = [...digits];
            pasteDigits.forEach((digit, i) => {
                if (i < 6) newDigits[i] = digit;
            });
            setDigits(newDigits);
            
            // If exactly 6 digits pasted, submit
            if (pasteDigits.length === 6) {
                handleSubmit(pasteDigits.join(''));
            } else {
                // Focus next empty input after paste
                const nextIndex = Math.min(pasteDigits.length, 5);
                inputRefs.current[nextIndex]?.focus();
            }
        }
    };

    const handleSubmit = async (code) => {
        const activationCode = code || digits.join('');
        
        if (activationCode.length !== 6) {
            toast.error('Please enter a 6-digit code');
            return;
        }

        setIsSubmitting(true);

        try {
            const response = await fetch(
                `${config.api.baseURL}/user/activate?email=${encodeURIComponent(email)}&activationCode=${activationCode}`,
                { 
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                }
            );

            const data = await response.json();

            if (response.ok) {
                toast.success(data.message || 'Account activated successfully!');
                navigate('/login');
            } else {
                toast.error(data.message || 'Activation failed. Please try again.');
                // Clear all inputs on error
                setDigits(Array(6).fill(''));
                inputRefs.current[0]?.focus();
            }
        } catch (error) {
            toast.error('An error occurred. Please try again.');
            console.error('Activation error:', error);
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleResendCode = () => {
        // TODO: Implement resend activation code functionality
        toast.info('Resend functionality coming soon');
    };

    return (
        <div className="signup-container">
            <div className="signup-content">
                {/* Left Side - Illustration */}
                <div className="illustration-side">
                    <div className="floating-shapes">
                        <div className="shape shape-1"></div>
                        <div className="shape shape-2"></div>
                        <div className="shape shape-3"></div>
                    </div>
                    <div className="illustration-content">
                        <h2>Verify Your Account</h2>
                        <p>Enter the 6-digit code sent to your email to complete your registration and start using our services.</p>
                        
                        <div className="features-list">
                            <div className="feature">
                                <div className="feature-icon">
                                    <FaCheck />
                                </div>
                                <span>Secure Verification</span>
                            </div>
                            <div className="feature">
                                <div className="feature-icon">
                                    <FaCheck />
                                </div>
                                <span>Instant Activation</span>
                            </div>
                            <div className="feature">
                                <div className="feature-icon">
                                    <FaCheck />
                                </div>
                                <span>24/7 Support</span>
                            </div>
                        </div>
                        
                        <div className="already-have-account">
                            Remember your password? <Link to="/login" className="login-link">Sign In</Link>
                        </div>
                    </div>
                </div>
                
                {/* Right Side - Form */}
                <div className="form-side">
                    <div className="form-container">
                        <div className={`form-step ${animation}`}>
                            <div className="text-center mb-4">
                                <div className="verification-icon">
                                    <FaEnvelope />
                                </div>
                                <h2>Verify Your Email</h2>
                                <p className="step-description">
                                    We've sent a 6-digit verification code to<br />
                                    <strong>{email}</strong>
                                </p>
                            </div>
                            
                            <div className="form-group">
                                <div className="otp-container">
                                    {digits.map((digit, index) => (
                                        <input
                                            key={index}
                                            ref={el => inputRefs.current[index] = el}
                                            type="text"
                                            className={`otp-input ${isSubmitting ? 'disabled' : ''}`}
                                            value={digit}
                                            onChange={(e) => handleChange(index, e.target.value)}
                                            onKeyDown={(e) => handleKeyDown(index, e)}
                                            onPaste={handlePaste}
                                            maxLength={1}
                                            disabled={isSubmitting}
                                            inputMode="numeric"
                                            pattern="[0-9]*"
                                            autoComplete="one-time-code"
                                        />
                                    ))}
                                </div>
                                
                                {isSubmitting && (
                                    <div className="verification-loading">
                                        <span className="btn-spinner"></span>
                                        <span>Verifying your code...</span>
                                    </div>
                                )}
                            </div>

                            <div className="form-navigation">
                                <button 
                                    type="button" 
                                    className="btn btn-outline"
                                    onClick={() => navigate('/signup')}
                                    disabled={isSubmitting}
                                >
                                    <FaArrowLeft /> Back to Sign Up
                                </button>
                                
                                <button 
                                    type="button" 
                                    className="btn btn-primary"
                                    onClick={() => handleSubmit()}
                                    disabled={isSubmitting || digits.some(digit => digit === '')}
                                >
                                    {isSubmitting ? 'Verifying...' : 'Verify Account'}
                                    {!isSubmitting && <FaCheck className="btn-icon" />}
                                    {isSubmitting && <span className="btn-spinner"></span>}
                                </button>
                            </div>

                            <div className="resend-section">
                                <p>Didn't receive the code?</p>
                                <button
                                    className="resend-btn"
                                    onClick={handleResendCode}
                                    disabled={isSubmitting}
                                    type="button"
                                >
                                    <FaRedo className="resend-icon" />
                                    Resend Code
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <style jsx>{`
                @keyframes fadeIn {
                    from { opacity: 0; transform: translateY(10px); }
                    to { opacity: 1; transform: translateY(0); }
                }
                
                @keyframes fadeOut {
                    from { opacity: 1; transform: translateY(0); }
                    to { opacity: 0; transform: translateY(-10px); }
                }
                
                @keyframes float {
                    0% { transform: translateY(0px); }
                    50% { transform: translateY(-10px); }
                    100% { transform: translateY(0px); }
                }
                
                @keyframes pulse {
                    0% { transform: scale(1); }
                    50% { transform: scale(1.05); }
                    100% { transform: scale(1); }
                }
                
                .signup-container {
                    min-height: 100vh;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    background: linear-gradient(135deg, #f5f7fa 0%, #e4edf5 100%);
                    padding: 20px;
                }
                
                .signup-content {
                    display: flex;
                    width: 100%;
                    max-width: 1200px;
                    background: #ffffff;
                    border-radius: 20px;
                    overflow: hidden;
                    box-shadow: 0 10px 40px rgba(0, 0, 0, 0.08);
                    min-height: 700px;
                }
                
                /* Illustration Side */
                .illustration-side {
                    flex: 1;
                    background: linear-gradient(135deg, #006600 0%, #004d00 100%);
                    color: white;
                    padding: 60px 40px;
                    position: relative;
                    overflow: hidden;
                    display: flex;
                    flex-direction: column;
                    justify-content: center;
                }
                
                .illustration-side::before {
                    content: '';
                    position: absolute;
                    top: -50%;
                    right: -50%;
                    width: 100%;
                    height: 200%;
                    background: radial-gradient(circle, rgba(255,255,255,0.1) 0%, rgba(255,255,255,0) 70%);
                    border-radius: 50%;
                }
                
                .illustration-content {
                    position: relative;
                    z-index: 2;
                    max-width: 500px;
                    margin: 0 auto;
                    width: 100%;
                }
                
                .illustration-side h2 {
                    font-size: 2.2rem;
                    margin-bottom: 20px;
                    font-weight: 700;
                    line-height: 1.3;
                }
                
                .illustration-side p {
                    font-size: 1.1rem;
                    opacity: 0.9;
                    margin-bottom: 30px;
                    line-height: 1.6;
                }
                
                .features-list {
                    margin: 40px 0;
                }
                
                .feature {
                    display: flex;
                    align-items: center;
                    margin-bottom: 15px;
                    font-size: 1rem;
                }
                
                .feature-icon {
                    width: 24px;
                    height: 24px;
                    background: rgba(255, 255, 255, 0.2);
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    margin-right: 12px;
                    flex-shrink: 0;
                }
                
                .feature-icon svg {
                    font-size: 12px;
                    color: white;
                }
                
                .already-have-account {
                    margin-top: 40px;
                    font-size: 0.95rem;
                    opacity: 0.9;
                }
                
                .login-link {
                    color: white;
                    font-weight: 600;
                    text-decoration: none;
                    margin-left: 5px;
                    transition: all 0.3s ease;
                    border-bottom: 1px solid transparent;
                }
                
                .login-link:hover {
                    border-bottom-color: white;
                }
                
                /* Form Side */
                .form-side {
                    flex: 1;
                    padding: 60px 0;
                    display: flex;
                    flex-direction: column;
                    justify-content: center;
                    position: relative;
                    background: white;
                }
                
                .form-container {
                    max-width: 450px;
                    width: 100%;
                    margin: 0 auto;
                    padding: 0 40px;
                }
                
                /* Form Steps */
                .form-step {
                    animation: fadeIn 0.3s ease forwards;
                }
                
                .form-step.fadeOut {
                    animation: fadeOut 0.3s ease forwards;
                }
                
                .form-step h2 {
                    font-size: 1.8rem;
                    color: #212529;
                    margin-bottom: 8px;
                    text-align: center;
                }
                
                .step-description {
                    color: #6c757d;
                    margin-bottom: 30px;
                    font-size: 1rem;
                    text-align: center;
                    line-height: 1.5;
                }
                
                /* Verification Icon */
                .verification-icon {
                    width: 80px;
                    height: 80px;
                    background: linear-gradient(135deg, #006600, #004d00);
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    margin: 0 auto 20px;
                    color: white;
                    font-size: 2rem;
                    box-shadow: 0 8px 25px rgba(0, 102, 0, 0.2);
                }
                
                /* OTP Inputs */
                .otp-container {
                    display: flex;
                    justify-content: center;
                    gap: 12px;
                    margin: 40px 0;
                }
                
                .otp-input {
                    width: 55px;
                    height: 65px;
                    border: 2px solid #e9ecef;
                    border-radius: 12px;
                    text-align: center;
                    font-size: 1.5rem;
                    font-weight: 600;
                    color: #212529;
                    background: #ffffff;
                    transition: all 0.3s ease;
                    caret-color: transparent;
                }
                
                .otp-input:focus {
                    outline: none;
                    border-color: #006600;
                    box-shadow: 0 0 0 3px rgba(0, 102, 0, 0.1);
                    transform: translateY(-2px);
                }
                
                .otp-input.disabled {
                    background-color: #f8f9fa;
                    border-color: #dee2e6;
                    opacity: 0.7;
                }
                
                .otp-input:not(:placeholder-shown) {
                    border-color: #006600;
                    background-color: rgba(0, 102, 0, 0.02);
                }
                
                /* Verification Loading */
                .verification-loading {
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    gap: 10px;
                    margin: 20px 0;
                    color: #6c757d;
                    font-size: 0.9rem;
                }
                
                /* Form Navigation */
                .form-navigation {
                    display: flex;
                    justify-content: space-between;
                    margin-top: 40px;
                }
                
                .btn {
                    display: inline-flex;
                    align-items: center;
                    justify-content: center;
                    padding: 12px 24px;
                    border-radius: 8px;
                    font-weight: 600;
                    font-size: 1rem;
                    cursor: pointer;
                    transition: all 0.3s ease;
                    border: 2px solid transparent;
                }
                
                .btn:disabled {
                    opacity: 0.7;
                    cursor: not-allowed;
                }
                
                .btn-primary {
                    background: #006600;
                    color: white;
                    box-shadow: 0 4px 12px rgba(0, 102, 0, 0.2);
                }
                
                .btn-primary:hover:not(:disabled) {
                    background: #004d00;
                    transform: translateY(-2px);
                    box-shadow: 0 6px 16px rgba(0, 102, 0, 0.25);
                }
                
                .btn-primary:active:not(:disabled) {
                    transform: translateY(0);
                    box-shadow: 0 2px 8px rgba(0, 102, 0, 0.2);
                }
                
                .btn-outline {
                    background: transparent;
                    color: #006600;
                    border-color: #006600;
                }
                
                .btn-outline:hover:not(:disabled) {
                    background: rgba(0, 102, 0, 0.05);
                }
                
                .btn-icon {
                    margin-left: 8px;
                    font-size: 0.9em;
                }
                
                .btn-spinner {
                    width: 16px;
                    height: 16px;
                    border: 2px solid rgba(255, 255, 255, 0.3);
                    border-radius: 50%;
                    border-top-color: white;
                    animation: spin 1s ease-in-out infinite;
                    margin-left: 8px;
                }
                
                @keyframes spin {
                    to { transform: rotate(360deg); }
                }
                
                /* Resend Section */
                .resend-section {
                    text-align: center;
                    margin-top: 30px;
                    padding-top: 20px;
                    border-top: 1px solid #e9ecef;
                }
                
                .resend-section p {
                    color: #6c757d;
                    margin-bottom: 10px;
                    font-size: 0.95rem;
                }
                
                .resend-btn {
                    background: none;
                    border: none;
                    color: #006600;
                    font-weight: 600;
                    cursor: pointer;
                    display: inline-flex;
                    align-items: center;
                    gap: 8px;
                    padding: 8px 16px;
                    border-radius: 6px;
                    transition: all 0.3s ease;
                    font-size: 0.95rem;
                }
                
                .resend-btn:hover:not(:disabled) {
                    background: rgba(0, 102, 0, 0.05);
                    transform: translateY(-1px);
                }
                
                .resend-btn:disabled {
                    opacity: 0.5;
                    cursor: not-allowed;
                }
                
                .resend-icon {
                    font-size: 0.9em;
                }
                
                /* Floating Shapes */
                .floating-shapes {
                    position: absolute;
                    width: 100%;
                    height: 100%;
                    top: 0;
                    left: 0;
                    overflow: hidden;
                    z-index: 1;
                    pointer-events: none;
                }
                
                .shape {
                    position: absolute;
                    border-radius: 50%;
                    background: rgba(255, 255, 255, 0.1);
                    pointer-events: none;
                }
                
                .shape-1 {
                    width: 200px;
                    height: 200px;
                    top: -50px;
                    left: -50px;
                    animation: float 15s infinite ease-in-out;
                }
                
                .shape-2 {
                    width: 300px;
                    height: 300px;
                    bottom: -100px;
                    right: -100px;
                    animation: float 20s infinite ease-in-out;
                    animation-delay: 2s;
                }
                
                .shape-3 {
                    width: 150px;
                    height: 150px;
                    top: 50%;
                    right: -50px;
                    animation: float 18s infinite ease-in-out;
                    animation-delay: 1s;
                }
                
                /* Responsive Design */
                @media (max-width: 992px) {
                    .signup-content {
                        flex-direction: column;
                        max-width: 600px;
                        min-height: auto;
                    }
                    
                    .illustration-side, .form-side {
                        padding: 40px 30px;
                    }
                    
                    .illustration-side {
                        text-align: center;
                    }
                    
                    .illustration-content {
                        max-width: 100%;
                    }
                    
                    .form-container {
                        padding: 0 20px;
                    }
                    
                    .otp-container {
                        gap: 10px;
                    }
                    
                    .otp-input {
                        width: 50px;
                        height: 60px;
                    }
                }
                
                @media (max-width: 576px) {
                    .signup-content {
                        border-radius: 10px;
                    }
                    
                    .illustration-side h2 {
                        font-size: 1.8rem;
                    }
                    
                    .form-step h2 {
                        font-size: 1.5rem;
                    }
                    
                    .form-navigation {
                        flex-direction: column;
                        gap: 15px;
                    }
                    
                    .btn {
                        width: 100%;
                    }
                    
                    .form-container {
                        padding: 0 15px;
                    }
                    
                    .otp-container {
                        gap: 8px;
                    margin: 30px 0;
                    }
                    
                    .otp-input {
                        width: 45px;
                        height: 55px;
                        font-size: 1.3rem;
                    }
                    
                    .verification-icon {
                        width: 70px;
                        height: 70px;
                        font-size: 1.7rem;
                    }
                }
                
                @media (max-width: 400px) {
                    .illustration-side h2 {
                        font-size: 1.6rem;
                    }
                    
                    .illustration-side p {
                        font-size: 1rem;
                    }
                    
                    .otp-container {
                        gap: 6px;
                    }
                    
                    .otp-input {
                        width: 40px;
                        height: 50px;
                        font-size: 1.2rem;
                    }
                }
                
                /* Animations */
                .fadeIn {
                    animation: fadeIn 0.3s ease forwards;
                }
                
                .fadeOut {
                    animation: fadeOut 0.3s ease forwards;
                }
            `}</style>
        </div>
    );
}