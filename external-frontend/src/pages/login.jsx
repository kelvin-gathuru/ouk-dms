import React, { useState, useContext, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { storeToken, getToken } from '../service/tokenStorage';
import { toast } from 'react-toastify';
import { AuthContext } from '../context/AuthContext';
import { FaEye, FaEyeSlash, FaEnvelope, FaLock } from 'react-icons/fa';
import config from '../config';

export default function LoginScreen() {
  const [formData, setFormData] = useState({
    email: '',
    password: ''
  });
  const [errors, setErrors] = useState({});
  const [showPassword, setShowPassword] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const navigate = useNavigate();
  const { login, isAuthenticated } = useContext(AuthContext);

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

    if (!formData.password) {
      newErrors.password = 'Password is required';
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

    // Validate email in real-time as user types
    if (name === 'email') {
      const emailError = validateEmail(value);
      setErrors(prev => ({
        ...prev,
        email: emailError || ''
      }));
    } else if (errors[name]) {
      // Clear error for other fields when user starts typing
      setErrors(prev => ({
        ...prev,
        [name]: ''
      }));
    }
  };

  const handleBlur = (e) => {
    const { name } = e.target;
    if (name === 'email') {
      // Keep the existing error state on blur, it's already being handled in handleChange
      return;
    }
  };

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateForm()) return;

    setIsSubmitting(true);

    try {
      const response = await fetch(`${config.api.baseURL}/client/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData),
      });

      const data = await response.json();

      if (response.ok) {
        // Backend returns structured response with user data
        // The response structure is: { success: true, data: { token, id, email, companyName, contactPerson }, ... }
        // Handle both camelCase and PascalCase
        const userData = data.data || data.Data || data;
        const token = userData.token || userData.Token;
        const id = userData.id || userData.Id;
        const email = userData.email || userData.Email;
        const companyName = userData.companyName || userData.CompanyName;
        const contactPerson = userData.contactPerson || userData.ContactPerson;

        if (token) {
          // Store token using the storeToken function (expects object with token and name)
          storeToken({
            token: token,
            name: companyName || contactPerson || email
          });

          // Store each field independently in localStorage
          localStorage.setItem('userToken', token); // Changed from 'token' to 'userToken' to match getToken()
          localStorage.setItem('id', id || '');
          localStorage.setItem('email', email || '');
          localStorage.setItem('companyName', companyName || '');
          localStorage.setItem('contactPerson', contactPerson || '');

          // Also store as a single user object for convenience
          const userInfo = {
            token: token,
            id: id,
            email: email,
            companyName: companyName,
            contactPerson: contactPerson
          };
          localStorage.setItem('user', JSON.stringify(userInfo));

          login(token);
          toast.success('Login successful');
          navigate('/dashboard');
        } else {
          toast.error('Login failed. No token received.');
        }
      } else {
        const errorMessage = typeof data === 'string' ? data : (data.message || data.Message || (data.errors && data.errors[0]) || (data.Errors && data.Errors[0]) || 'Login failed. Please try again.');
        toast.error(errorMessage);
      }
    } catch (error) {
      toast.error(error.message || 'An error occurred. Please try again.');
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
                  <span>Welcome...</span>
                  <span className="cursor"></span>
                </div>
                <div className="screen-line">
                  <span className="prompt"></span>
                  <span>Public Petitions System</span>
                </div>
                <div className="screen-line">
                  <span className="prompt"></span>
                  <span>Secure Login Required</span>
                </div>
                <div className="screen-line">
                  <span className="prompt"></span>
                  <span>Initializing system...</span>
                </div>
              </div>
            </div>
            <div className="computer-base">
              <div className="computer-stand"></div>
            </div>
          </div>
          <div className="welcome-text">
            <h2>Secure Access Portal</h2>
            <p>Login to manage your petitions and track their progress</p>
          </div>
        </div>

        {/* Form Side */}
        <div className="form-side">
          <div className="form-header">
            <h2>Welcome Back</h2>
            <p>Sign in to your account to continue</p>
          </div>

          <form onSubmit={handleSubmit} className="login-form" noValidate>
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
                  onBlur={handleBlur}
                  disabled={isSubmitting}
                  autoComplete="username"
                  placeholder="Enter your email"
                />
              </div>
              {errors.email && (
                <div className="error-message">
                  {errors.email}
                </div>
              )}
            </div>

            <div className="form-group">
              <label htmlFor="password">Password</label>
              <div className="input-with-icon">
                <FaLock className="input-icon" />
                <input
                  type={showPassword ? 'text' : 'password'}
                  id="password"
                  name="password"
                  className={`form-control ${errors.password ? 'input-error' : ''}`}
                  value={formData.password}
                  onChange={handleChange}
                  disabled={isSubmitting}
                  autoComplete="current-password"
                  placeholder="Enter your password"
                />
                <button
                  type="button"
                  className="password-toggle"
                  onClick={togglePasswordVisibility}
                  disabled={isSubmitting}
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

            <div className="form-options">
              <div className="remember-me">
                <input type="checkbox" id="remember" />
                <label htmlFor="remember">Remember me</label>
              </div>
              <Link to="/forgot" className="forgot-password">
                Forgot Password?
              </Link>
            </div>

            <button
              type="submit"
              className="login-btn"
              disabled={isSubmitting}
            >
              {isSubmitting ? (
                <>
                  <span className="spinner"></span>
                  Signing in...
                </>
              ) : (
                'Sign In'
              )}
            </button>
          </form>

          <div className="signup-link">
            Don't have an account? <Link to="/signup" className="signup-text">Sign up here</Link>
          </div>
        </div>
      </div>

      <style jsx>{`
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

        .illustration-side::before {
          content: '';
          position: absolute;
          width: 200px;
          height: 200px;
          border-radius: 50%;
          background: rgba(255, 255, 255, 0.1);
          top: -50px;
          left: -50px;
        }

        .illustration-side::after {
          content: '';
          position: absolute;
          width: 150px;
          height: 150px;
          border-radius: 50%;
          background: rgba(255, 255, 255, 0.08);
          bottom: -50px;
          right: -50px;
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
        }

        .screen-content {
          padding: 20px;
          color: #4dccff;
          font-family: 'Courier New', monospace;
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

        @keyframes blink {
          0%, 100% { opacity: 1; }
          50% { opacity: 0; }
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
        }

        .logo {
          display: flex;
          align-items: center;
          margin-bottom: 30px;
        }

        .logo-icon {
          width: 40px;
          height: 40px;
          background: #006600;
          border-radius: 8px;
          display: flex;
          align-items: center;
          justify-content: center;
          margin-right: 12px;
          color: #ffffff;
          font-size: 1.2rem;
        }

        .logo-text h1 {
          font-size: 1.5rem;
          color: #212529;
          font-weight: 700;
        }

        .logo-text p {
          font-size: 0.85rem;
          color: #6c757d;
        }

        .form-header {
          margin-bottom: 30px;
        }

        .form-header h2 {
          font-size: 1.8rem;
          color: #212529;
          margin-bottom: 8px;
        }

        .form-header p {
          color: #6c757d;
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
          border-color: #dc3545;
        }

        .input-valid {
          border-color: #198754;
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
          z-index: 2;
        }

        .password-toggle:disabled {
          opacity: 0.6;
          cursor: not-allowed;
        }

        .error-message {
          color: #dc3545;
          font-size: 0.875rem;
          margin-top: 5px;
        }

        .form-options {
          display: flex;
          justify-content: space-between;
          align-items: center;
          margin-bottom: 25px;
        }

        .remember-me {
          display: flex;
          align-items: center;
        }

        .remember-me input {
          margin-right: 8px;
        }

        .remember-me label {
          margin-bottom: 0;
          color: #6c757d;
        }

        .forgot-password {
          color: #006600;
          text-decoration: none;
          font-weight: 500;
          font-size: 0.875rem;
          transition: color 0.3s;
        }

        .forgot-password:hover {
          color: #004d00;
          text-decoration: underline;
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
        }
      `}</style>
    </div>
  );
}