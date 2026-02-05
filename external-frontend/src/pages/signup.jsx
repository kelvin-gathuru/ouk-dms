import React, { useState, useContext, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { FaEye, FaEyeSlash, FaArrowRight, FaArrowLeft, FaUser, FaEnvelope, FaPhone, FaIdCard, FaLock, FaCheck } from 'react-icons/fa';
import { getToken } from '../service/tokenStorage';
import { AuthContext } from '../context/AuthContext';
import config from '../config';

export default function SignUp() {
    const [formData, setFormData] = useState({
        companyName: '',
        firstName: '',
        lastName: '',
        email: '',
        phone: '',
        identification: '',
        password: '',
        confirmPassword: ''
    });

    const [errors, setErrors] = useState({});
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [termsAccepted, setTermsAccepted] = useState(false);
    const [currentStep, setCurrentStep] = useState(1);
    const [animation, setAnimation] = useState('fadeIn');

    const navigate = useNavigate();
    const { isAuthenticated } = useContext(AuthContext);

    // Redirect if already authenticated
    useEffect(() => {
        const token = getToken();
        if (token || isAuthenticated) {
            navigate('/dashboard');
        }
    }, [navigate, isAuthenticated]);

    // Animation handler
    const changeStep = (newStep) => {
        setAnimation('fadeOut');
        setTimeout(() => {
            setCurrentStep(newStep);
            setAnimation('fadeIn');
        }, 300);
    };

    const validateEmail = (email) => {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!email) return 'Email is required';
        if (!emailRegex.test(email)) return 'Please enter a valid email address';
        return '';
    };

    const validatePassword = (password) => {
        if (!password) return 'Password is required';
        if (password.length < 8) return 'Password must be at least 8 characters long';
        if (!/\d/.test(password)) return 'Password must include at least one number';
        if (!/[!@#$%^&*]/.test(password)) return 'Password must include at least one special character';
        return '';
    };

    const validateStep = (step) => {
        const newErrors = {};

        if (step === 1) {
            if (!formData.companyName.trim()) newErrors.companyName = 'Company name is required';
            if (!formData.firstName.trim()) newErrors.firstName = 'First name is required';
            if (!formData.lastName.trim()) newErrors.lastName = 'Last name is required';
            if (!formData.phone) {
                newErrors.phone = 'Phone number is required';
            } else if (!/^\d{9}$/.test(formData.phone)) {
                newErrors.phone = 'Please enter 9 digits after +254';
            }
        }

        if (step === 2) {
            const emailError = validateEmail(formData.email);
            if (emailError) newErrors.email = emailError;

            if (!formData.identification.trim()) newErrors.identification = 'Identification is required';
        }

        if (step === 3) {
            const passwordError = validatePassword(formData.password);
            if (passwordError) newErrors.password = passwordError;

            if (formData.password !== formData.confirmPassword) {
                newErrors.confirmPassword = 'Passwords do not match';
            }

            if (!termsAccepted) {
                newErrors.terms = 'You must accept the terms and conditions';
            }
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleNext = () => {
        if (validateStep(currentStep)) {
            if (currentStep < 3) {
                changeStep(currentStep + 1);
            } else {
                handleSubmit();
            }
        }
    };

    const handlePrevious = () => {
        if (currentStep > 1) {
            changeStep(currentStep - 1);
        }
    };

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;

        if (type === 'checkbox') {
            setTermsAccepted(checked);
            if (errors.terms) {
                setErrors(prev => ({
                    ...prev,
                    terms: ''
                }));
            }
            return;
        }

        // Format phone number as user types
        if (name === 'phone') {
            // Remove all non-digit characters
            const numbers = value.replace(/\D/g, '');

            // If user is typing, ensure we only take up to 9 digits after +254
            let formattedNumber = numbers;
            if (numbers.length > 9) {
                formattedNumber = numbers.substring(0, 9);
            }

            setFormData(prev => ({
                ...prev,
                [name]: formattedNumber
            }));

            // Clear any previous phone errors when user starts typing
            if (errors.phone) {
                setErrors(prev => ({
                    ...prev,
                    phone: ''
                }));
            }
            return;
        }

        setFormData(prev => ({
            ...prev,
            [name]: value
        }));

        // Real-time validation for email and password
        if (name === 'email') {
            const emailError = validateEmail(value);
            setErrors(prev => ({
                ...prev,
                email: emailError || ''
            }));
        } else if (name === 'password') {
            const passwordError = validatePassword(value);
            setErrors(prev => ({
                ...prev,
                password: passwordError || '',
                confirmPassword: value !== formData.confirmPassword ? 'Passwords do not match' : ''
            }));
        } else if (name === 'confirmPassword') {
            setErrors(prev => ({
                ...prev,
                confirmPassword: value !== formData.password ? 'Passwords do not match' : ''
            }));
        } else if (errors[name]) {
            // Clear error when user starts typing in other fields
            setErrors(prev => ({
                ...prev,
                [name]: ''
            }));
        }
    };

    const handleSubmit = async () => {
        setIsSubmitting(true);

        try {
            const response = await fetch(`${config.api.baseURL}/client/signup`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    companyName: formData.companyName.trim(),
                    contactPerson: `${formData.firstName.trim()} ${formData.lastName.trim()}`,
                    email: formData.email.trim(),
                    phoneNumber: `254${formData.phone}`,
                    address: formData.identification.trim(),
                    password: formData.password
                })
            });

            const data = await response.json();

            if (response.ok) {
                // Success based on 2xx status code
                setFormData({
                    companyName: '',
                    firstName: '',
                    lastName: '',
                    email: '',
                    phone: '',
                    identification: '',
                    password: '',
                    confirmPassword: ''
                });
                setTermsAccepted(false);
                toast.success('Signup successful! Please check your email for activation code.');
                navigate("/activation", { state: { email: formData.email } });
            } else {
                // Error response
                const errorMessage = typeof data === 'string' ? data : (data.message || data.errors?.[0] || 'Signup failed. Please try again.');
                toast.error(errorMessage);
            }
        } catch (error) {
            toast.error(error.message || 'An error occurred. Please try again.');
            console.error('Error:', error);
        } finally {
            setIsSubmitting(false);
        }
    };

    const renderStepIndicator = () => (
        <div className="step-indicator">
            {[1, 2, 3].map((step) => (
                <div
                    key={step}
                    className={`step ${currentStep === step ? 'active' : ''} ${currentStep > step ? 'completed' : ''}`}
                    onClick={() => currentStep > step && changeStep(step)}
                >
                    {currentStep > step ? <FaCheck /> : step}
                </div>
            ))}
            <div className="progress-bar">
                <div
                    className="progress"
                    style={{ width: `${((currentStep - 1) / 2) * 100}%` }}
                ></div>
            </div>
        </div>
    );

    const renderStep1 = () => (
        <div className={`form-step ${animation}`}>
            <h2>Personal Information</h2>
            <p className="step-description">Let's start with your basic details</p>

            <div className="form-group">
                <div className="input-with-icon">
                    <FaUser className="input-icon" />
                    <input
                        type="text"
                        name="companyName"
                        value={formData.companyName}
                        onChange={handleChange}
                        placeholder="Company Name"
                        className={errors.companyName ? 'input-error' : ''}
                        disabled={isSubmitting}
                    />
                </div>
                {errors.companyName && <span className="error-message">{errors.companyName}</span>}
            </div>

            <div className="form-group">
                <div className="input-with-icon">
                    <FaUser className="input-icon" />
                    <input
                        type="text"
                        name="firstName"
                        value={formData.firstName}
                        onChange={handleChange}
                        placeholder="First Name"
                        className={errors.firstName ? 'input-error' : ''}
                        disabled={isSubmitting}
                    />
                </div>
                {errors.firstName && <span className="error-message">{errors.firstName}</span>}
            </div>

            <div className="form-group">
                <div className="input-with-icon">
                    <FaUser className="input-icon" />
                    <input
                        type="text"
                        name="lastName"
                        value={formData.lastName}
                        onChange={handleChange}
                        placeholder="Last Name"
                        className={errors.lastName ? 'input-error' : ''}
                        disabled={isSubmitting}
                    />
                </div>
                {errors.lastName && <span className="error-message">{errors.lastName}</span>}
            </div>

            <div className="form-group">
                <div className="input-with-icon">
                    <FaPhone className="input-icon" />
                    <div className="phone-input-wrapper">
                        <span className="phone-prefix">+254</span>
                        <input
                            type="tel"
                            name="phone"
                            value={formData.phone}
                            onChange={handleChange}
                            placeholder="7XXXXXXXX"
                            className={`${errors.phone ? 'input-error' : ''} phone-input`}
                            maxLength={9}
                            disabled={isSubmitting}
                        />
                    </div>
                </div>
                {errors.phone && <span className="error-message">{errors.phone}</span>}
            </div>
        </div>
    );

    const renderStep2 = () => (
        <div className={`form-step ${animation}`}>
            <h2>Account Details</h2>
            <p className="step-description">We'll use this to secure your account</p>

            <div className="form-group">
                <div className="input-with-icon">
                    <FaEnvelope className="input-icon" />
                    <input
                        type="email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        placeholder="Email Address"
                        className={errors.email ? 'input-error' : ''}
                        disabled={isSubmitting}
                        autoComplete="username"
                    />
                </div>
                {errors.email && <span className="error-message">{errors.email}</span>}
            </div>

            <div className="form-group">
                <div className="input-with-icon">
                    <FaIdCard className="input-icon" />
                    <input
                        type="text"
                        name="identification"
                        value={formData.identification}
                        onChange={handleChange}
                        placeholder="National ID/Passport"
                        className={errors.identification ? 'input-error' : ''}
                        disabled={isSubmitting}
                    />
                </div>
                {errors.identification && <span className="error-message">{errors.identification}</span>}
            </div>
        </div>
    );

    const renderStep3 = () => (
        <div className={`form-step ${animation}`}>
            <h2>Create Password</h2>
            <p className="step-description">Make it strong and secure</p>

            <div className="form-group">
                <div className="input-with-icon">
                    <FaLock className="input-icon" />
                    <input
                        type={showPassword ? 'text' : 'password'}
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                        placeholder="Create Password"
                        className={errors.password ? 'input-error' : ''}
                        disabled={isSubmitting}
                        autoComplete="new-password"
                    />
                    <button
                        type="button"
                        className="password-toggle"
                        onClick={() => setShowPassword(!showPassword)}
                        disabled={isSubmitting}
                    >
                        {showPassword ? <FaEyeSlash /> : <FaEye />}
                    </button>
                </div>
                {errors.password && <span className="error-message">{errors.password}</span>}

                <div className="password-strength">
                    <div
                        className={`strength-bar ${formData.password.length > 0 ? 'active' : ''} 
                        ${formData.password.length >= 8 && /\d/.test(formData.password) && /[!@#$%^&*]/.test(formData.password) ? 'strong' : ''}`}
                    ></div>
                    <div
                        className={`strength-text ${formData.password.length > 0 ? 'show' : ''}`}
                    >
                        {formData.password.length === 0 ? '' :
                            formData.password.length < 8 ? 'Weak' :
                                !/\d/.test(formData.password) || !/[!@#$%^&*]/.test(formData.password) ? 'Good' : 'Strong'}
                    </div>
                </div>
            </div>

            <div className="form-group">
                <div className="input-with-icon">
                    <FaLock className="input-icon" />
                    <input
                        type={showConfirmPassword ? 'text' : 'password'}
                        name="confirmPassword"
                        value={formData.confirmPassword}
                        onChange={handleChange}
                        placeholder="Confirm Password"
                        className={errors.confirmPassword ? 'input-error' : ''}
                        disabled={isSubmitting}
                        autoComplete="new-password"
                    />
                    <button
                        type="button"
                        className="password-toggle"
                        onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                        disabled={isSubmitting}
                    >
                        {showConfirmPassword ? <FaEyeSlash /> : <FaEye />}
                    </button>
                </div>
                {errors.confirmPassword && <span className="error-message">{errors.confirmPassword}</span>}
            </div>

            <div className="form-group terms-checkbox">
                <label className="checkbox-container">
                    <input
                        type="checkbox"
                        checked={termsAccepted}
                        onChange={handleChange}
                        name="terms"
                        disabled={isSubmitting}
                    />
                    <span className="checkmark"></span>
                    I agree to the <Link to="/terms" className="terms-link">Terms & Conditions</Link> and
                    <Link to="/privacy" className="terms-link"> Privacy Policy</Link>
                </label>
                {errors.terms && <span className="error-message">{errors.terms}</span>}
            </div>
        </div>
    );

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
                        <h2>Join Our Community</h2>
                        <p>Create an account to access all features and start your journey with us.</p>

                        <div className="features-list">
                            <div className="feature">
                                <div className="feature-icon">
                                    <FaCheck />
                                </div>
                                <span>Secure and Private</span>
                            </div>
                            <div className="feature">
                                <div className="feature-icon">
                                    <FaCheck />
                                </div>
                                <span>Easy to Use</span>
                            </div>
                            <div className="feature">
                                <div className="feature-icon">
                                    <FaCheck />
                                </div>
                                <span>24/7 Support</span>
                            </div>
                        </div>

                        <div className="already-have-account">
                            Already have an account? <Link to="/login" className="login-link">Sign In</Link>
                        </div>
                    </div>
                </div>

                {/* Right Side - Form */}
                <div className="form-side">
                    <div className="form-container">
                        {renderStepIndicator()}

                        {currentStep === 1 && renderStep1()}
                        {currentStep === 2 && renderStep2()}
                        {currentStep === 3 && renderStep3()}

                        <div className="form-navigation">
                            {currentStep > 1 && (
                                <button
                                    type="button"
                                    className="btn btn-outline"
                                    onClick={handlePrevious}
                                    disabled={isSubmitting}
                                >
                                    <FaArrowLeft /> Back
                                </button>
                            )}

                            <button
                                type="button"
                                className="btn btn-primary"
                                onClick={handleNext}
                                disabled={isSubmitting}
                            >
                                {currentStep === 3 ? (
                                    <>{isSubmitting ? 'Creating Account...' : 'Create Account'}
                                        {!isSubmitting && <FaArrowRight className="btn-icon" />}
                                    </>
                                ) : (
                                    <>
                                        Next <FaArrowRight className="btn-icon" />
                                    </>
                                )}
                                {isSubmitting && <span className="btn-spinner"></span>}
                            </button>
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
                
                .logo {
                    display: block;
                    margin-bottom: 40px;
                    text-align: center;
                }
                
                .logo img {
                    max-height: 40px;
                }
                
                /* Step Indicator */
                .step-indicator {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    margin-bottom: 40px;
                    position: relative;
                    padding: 0 30px;
                }
                
                .step {
                    width: 36px;
                    height: 36px;
                    border-radius: 50%;
                    background: #f0f0f0;
                    color: #999;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-weight: 600;
                    position: relative;
                    z-index: 2;
                    transition: all 0.3s ease;
                    cursor: default;
                }
                
                .step.active {
                    background: #006600;
                    color: white;
                    transform: scale(1.1);
                    box-shadow: 0 4px 12px rgba(0, 102, 0, 0.2);
                }
                
                .step.completed {
                    background: #28a745;
                    color: white;
                    cursor: pointer;
                }
                
                .progress-bar {
                    position: absolute;
                    top: 50%;
                    left: 0;
                    right: 0;
                    height: 4px;
                    background: #f0f0f0;
                    transform: translateY(-50%);
                    z-index: 1;
                    border-radius: 2px;
                    overflow: hidden;
                }
                
                .progress {
                    height: 100%;
                    background: #006600;
                    transition: width 0.3s ease;
                    border-radius: 2px;
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
                }
                
                .step-description {
                    color: #6c757d;
                    margin-bottom: 30px;
                    font-size: 1rem;
                }
                
                /* Form Elements */
                .form-group {
                    margin-bottom: 20px;
                    position: relative;
                }
                
                .input-with-icon {
                    position: relative;
                    margin-bottom: 15px;
                }
                
                .input-icon {
                    position: absolute;
                    left: 15px;
                    top: 50%;
                    transform: translateY(-50%);
                    color: #6c757d;
                    z-index: 2;
                }
                
                input[type="text"],
                input[type="email"],
                input[type="tel"],
                input[type="password"] {
                    width: 100%;
                    padding: 15px 15px 15px 45px;
                    border: 1.5px solid #e9ecef;
                    border-radius: 10px;
                    font-size: 1rem;
                    transition: all 0.3s;
                    background: #ffffff;
                    height: 52px;
                }
                
                input:focus {
                    outline: none;
                    border-color: #006600;
                    box-shadow: 0 0 0 3px rgba(0, 102, 0, 0.1);
                }
                
                .input-error {
                    border-color: #dc3545 !important;
                }
                
                .input-valid {
                    border-color: #28a745 !important;
                }
                
                .error-message {
                    color: #dc3545;
                    font-size: 0.85rem;
                    margin-top: 5px;
                    display: block;
                }
                
                /* Password Strength */
                .password-strength {
                    margin-top: 10px;
                    height: 4px;
                    background: #f0f0f0;
                    border-radius: 2px;
                    overflow: hidden;
                    position: relative;
                }
                
                .strength-bar {
                    height: 100%;
                    width: 0%;
                    background: #dc3545;
                    transition: all 0.3s ease;
                }
                
                .strength-bar.active {
                    width: 30%;
                    background: #dc3545;
                }
                
                .strength-bar.strong {
                    width: 100%;
                    background: #28a745;
                }
                
                .strength-text {
                    font-size: 0.75rem;
                    color: #6c757d;
                    margin-top: 5px;
                    text-align: right;
                    height: 16px;
                    opacity: 0;
                    transition: opacity 0.3s ease;
                }
                
                .strength-text.show {
                    opacity: 1;
                }
                
                /* Password Toggle */
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
                    display: flex;
                    align-items: center;
                    justify-content: center;
                }
                
                .password-toggle:focus {
                    outline: none;
                    color: #006600;
                }
                
                /* Terms Checkbox */
                .terms-checkbox {
                    margin-top: 20px;
                }
                
                .checkbox-container {
                    display: block;
                    position: relative;
                    padding-left: 35px;
                    margin-bottom: 12px;
                    cursor: pointer;
                    font-size: 0.95rem;
                    color: #495057;
                    -webkit-user-select: none;
                    -moz-user-select: none;
                    -ms-user-select: none;
                    user-select: none;
                }
                
                .checkbox-container input {
                    position: absolute;
                    opacity: 0;
                    cursor: pointer;
                    height: 0;
                    width: 0;
                }
                
                .checkmark {
                    position: absolute;
                    top: 0;
                    left: 0;
                    height: 24px;
                    width: 24px;
                    background-color: #f8f9fa;
                    border: 1.5px solid #dee2e6;
                    border-radius: 6px;
                    transition: all 0.2s ease;
                }
                
                .checkbox-container:hover input ~ .checkmark {
                    border-color: #006600;
                }
                
                .checkbox-container input:checked ~ .checkmark {
                    background-color: #006600;
                    border-color: #006600;
                }
                
                .checkmark:after {
                    content: "";
                    position: absolute;
                    display: none;
                }
                
                .checkbox-container input:checked ~ .checkmark:after {
                    display: block;
                }
                
                .checkbox-container .checkmark:after {
                    left: 8px;
                    top: 4px;
                    width: 5px;
                    height: 10px;
                    border: solid white;
                    border-width: 0 2px 2px 0;
                    transform: rotate(45deg);
                }
                
                .terms-link {
                    color: #006600;
                    text-decoration: none;
                    font-weight: 500;
                    transition: all 0.2s ease;
                }
                
                .terms-link:hover {
                    text-decoration: underline;
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
                    
                    .step-indicator {
                        padding: 0 15px;
                    }
                }
                
                @media (max-width: 400px) {
                    .illustration-side h2 {
                        font-size: 1.6rem;
                    }
                    
                    .illustration-side p {
                        font-size: 1rem;
                    }
                    
                    input[type="text"],
                    input[type="email"],
                    input[type="tel"],
                    input[type="password"] {
                        padding-left: 40px;
                        font-size: 0.95rem;
                    }
                    
                    .input-icon {
                        left: 12px;
                    }
                }
                
                /* Animations */
                .fadeIn {
                    animation: fadeIn 0.3s ease forwards;
                }
                
                .fadeOut {
                    animation: fadeOut 0.3s ease forwards;
                }
                
                .phone-input-wrapper {
                    display: flex;
                    align-items: center;
                    border: 1.5px solid #e9ecef;
                    border-radius: 10px;
                    overflow: hidden;
                    position: relative;
                    padding-left: 40px;
                }
                
                .phone-prefix {
                    padding: 0 10px;
                    background: #f8f9fa;
                    height: 52px;
                    display: flex;
                    align-items: center;
                    border-right: 1px solid #e9ecef;
                    color: #495057;
                    font-size: 0.9rem;
                }
                
                .phone-input {
                    flex: 1;
                    border: none !important;
                    height: 52px;
                    padding: 15px !important;
                    width: 100%;
                }
                
                /* Adjust the input padding when there's an error */
                .input-error {
                    border-color: #dc3545 !important;
                }
            `}</style>
        </div>
    );
}