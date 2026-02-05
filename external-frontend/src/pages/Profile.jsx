import React, { useState, useEffect } from 'react';
import { Card, Form, Button, Container, Row, Col, Tabs, Tab, ListGroup } from 'react-bootstrap';
import { toast } from 'react-toastify';
import { jwtDecode } from 'jwt-decode';
import config from '../config';
import axios from 'axios';

export default function Profile() {
  const [userInfo, setUserInfo] = useState({ name: '', email: '' });
  const [passwordData, setPasswordData] = useState({
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  });
  const [errors, setErrors] = useState({});
  const [isLoading, setIsLoading] = useState(false);
  const [passwordRequirements, setPasswordRequirements] = useState({
    length: false,
    uppercase: false,
    lowercase: false,
    number: false,
    specialChar: false,
    notSameAsOld: true
  });
  const [showPassword, setShowPassword] = useState({
    oldPassword: false,
    newPassword: false,
    confirmPassword: false
  });

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const token = localStorage.getItem('userToken');
        const userId = localStorage.getItem('id');

        // Just proceed with the call, if it fails the backend will handle it
        const response = await axios.get(`${config.api.baseURL}/Client/${userId}/Profile`, {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });

        const data = response.data;
        // Handle both camelCase and PascalCase
        const profileData = data.data || data.Data || data;
        const contactPerson = profileData.contactPerson || profileData.ContactPerson;
        const companyName = profileData.companyName || profileData.CompanyName;
        const email = profileData.email || profileData.Email;

        setUserInfo({
          name: contactPerson || companyName || 'User',
          email: email || '',
          ...profileData
        });
      } catch (error) {
        console.error('Error fetching profile:', error);
        toast.error('Error loading user information');

        // Fallback to localStorage if API fails, but don't log out
        setUserInfo({
          name: localStorage.getItem('contactPerson') || localStorage.getItem('companyName') || 'User',
          email: localStorage.getItem('email') || ''
        });
      }
    };

    fetchProfile();
  }, []);

  const validatePassword = (password, oldPassword = '') => {
    const requirements = {
      length: password.length >= 8,
      uppercase: /[A-Z]/.test(password),
      lowercase: /[a-z]/.test(password),
      number: /[0-9]/.test(password),
      specialChar: /[!@#$%^&*(),.?":{}|<>]/.test(password),
      notSameAsOld: password !== oldPassword
    };
    setPasswordRequirements(requirements);

    // Return true only if all requirements are met
    return Object.values(requirements).every(Boolean);
  };

  const handlePasswordChange = (e) => {
    const { name, value } = e.target;
    setPasswordData(prev => ({
      ...prev,
      [name]: value
    }));

    // Real-time validation for new password
    if (name === 'newPassword') {
      validatePassword(value, passwordData.oldPassword);
    } else if (name === 'oldPassword' && passwordData.newPassword) {
      validatePassword(passwordData.newPassword, value);
    }

    // Clear error when user types
    if (errors[name]) {
      setErrors(prev => ({
        ...prev,
        [name]: null
      }));
    }
  };

  const togglePasswordVisibility = (field) => {
    setShowPassword(prev => ({
      ...prev,
      [field]: !prev[field]
    }));
  };

  const validateForm = () => {
    const newErrors = {};

    if (!passwordData.oldPassword) {
      newErrors.oldPassword = 'Current password is required';
    }

    if (!passwordData.newPassword) {
      newErrors.newPassword = 'New password is required';
    } else if (!Object.values(passwordRequirements).every(Boolean)) {
      newErrors.newPassword = 'Please meet all password requirements';
    }

    if (!passwordData.confirmPassword) {
      newErrors.confirmPassword = 'Please confirm your new password';
    } else if (passwordData.newPassword !== passwordData.confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateForm()) return;

    setIsLoading(true);

    try {
      const token = localStorage.getItem('userToken');
      const userId = localStorage.getItem('id');

      if (!token) {
        toast.error('Authentication required');
        return;
      }

      const response = await axios.post(
        `${config.api.baseURL}/Client/${userId}/change-password`,
        {
          oldPassword: passwordData.oldPassword,
          newPassword: passwordData.newPassword
        },
        {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        }
      );

      toast.success('Password updated successfully');
      setPasswordData({
        oldPassword: '',
        newPassword: '',
        confirmPassword: ''
      });
    } catch (error) {
      console.error('Error updating password:', error);
      const errorMessage = error.response?.data?.message || error.response?.data?.Message ||
        (error.response?.data?.errors && error.response?.data?.errors[0]) ||
        (error.response?.data?.Errors && error.response?.data?.Errors[0]) ||
        'Failed to update password';
      toast.error(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Container className="py-5">
      <Row className="justify-content-center">
        <Col lg={8}>
          <Card className="shadow-sm border-0">
            <Card.Header className="bg-success text-white text-center py-4">
              <div className="d-flex justify-content-center mb-3">
                <div className="position-relative">
                  <div
                    className="rounded-circle bg-white d-flex align-items-center justify-content-center"
                    style={{ width: '120px', height: '120px', border: '4px solid white' }}
                  >
                    <i className="bi bi-person-fill text-primary" style={{ fontSize: '4rem' }}></i>
                  </div>
                  <button
                    className="btn btn-sm btn-light rounded-circle position-absolute"
                    style={{ bottom: '5px', right: '5px' }}
                    title="Change profile picture"
                  >
                    <i className="bi bi-camera"></i>
                  </button>
                </div>
              </div>
              <h3 className="mb-0">{userInfo?.name || 'User Profile'}</h3>
              <p className="mb-0">{userInfo?.email || ''}</p>
            </Card.Header>

            <Card.Body className="p-4">
              <Tabs
                defaultActiveKey="profile"
                id="profile-tabs"
                className="mb-4"
                fill
              >
                <Tab eventKey="profile" title="Profile Information">
                  <div className="mt-4">
                    <div className="d-flex justify-content-between align-items-center mb-4">
                      <h5 className="mb-0">Personal Information</h5>
                      <Button variant="outline-success" size="sm">
                        <i className="bi bi-pencil me-1"></i> Edit Profile
                      </Button>
                    </div>

                    <ListGroup variant="flush" className="mb-4">
                      <ListGroup.Item className="d-flex justify-content-between align-items-center">
                        <div>
                          <h6 className="mb-0">Full Name</h6>
                          <small className="text-muted">Your full name as it appears on your account</small>
                        </div>
                        <span>{userInfo?.name || 'Not provided'}</span>
                      </ListGroup.Item>
                      <ListGroup.Item className="d-flex justify-content-between align-items-center">
                        <div>
                          <h6 className="mb-0">Email Address</h6>
                          <small className="text-muted">Your primary email address</small>
                        </div>
                        <span>{userInfo?.email || 'Not provided'}</span>
                      </ListGroup.Item>
                      <ListGroup.Item className="d-flex justify-content-between align-items-center">
                        <div>
                          <h6 className="mb-0">Member Since</h6>
                          <small className="text-muted">When you joined our platform</small>
                        </div>
                        <span>{new Date().toLocaleDateString()}</span>
                      </ListGroup.Item>
                    </ListGroup>
                  </div>
                </Tab>

                <Tab eventKey="password" title="Change Password">
                  <div className="mt-4">
                    <h5 className="mb-4">Change Your Password</h5>
                    <Form onSubmit={handleSubmit}>
                      <Form.Group className="mb-4">
                        <Form.Label className="d-flex align-items-center">
                          <i className="bi bi-lock me-2"></i> Current Password
                        </Form.Label>
                        <div className="input-group">
                          <Form.Control
                            type={showPassword.oldPassword ? "text" : "password"}
                            name="oldPassword"
                            value={passwordData.oldPassword}
                            onChange={handlePasswordChange}
                            isInvalid={!!errors.oldPassword}
                            placeholder="Enter current password"
                            className="py-2"
                          />
                          <button
                            className="btn btn-outline-secondary"
                            type="button"
                            onClick={() => togglePasswordVisibility('oldPassword')}
                          >
                            <i className={`bi bi-eye${showPassword.oldPassword ? '-slash' : ''}`}></i>
                          </button>
                          <Form.Control.Feedback type="invalid">
                            {errors.oldPassword}
                          </Form.Control.Feedback>
                        </div>
                      </Form.Group>

                      <Form.Group className="mb-4">
                        <Form.Label className="d-flex align-items-center">
                          <i className="bi bi-key me-2"></i> New Password
                        </Form.Label>
                        <div className="input-group">
                          <Form.Control
                            type={showPassword.newPassword ? "text" : "password"}
                            name="newPassword"
                            value={passwordData.newPassword}
                            onChange={handlePasswordChange}
                            isInvalid={!!errors.newPassword}
                            placeholder="Enter new password"
                            className="py-2"
                          />
                          <button
                            className="btn btn-outline-secondary"
                            type="button"
                            onClick={() => togglePasswordVisibility('newPassword')}
                          >
                            <i className={`bi bi-eye${showPassword.newPassword ? '-slash' : ''}`}></i>
                          </button>
                        </div>
                        <div className="mt-3 p-3 bg-light rounded">
                          <small className="d-block text-muted mb-2">Password must contain:</small>
                          <div className="row">
                            <div className="col-6">
                              <div className={`d-flex align-items-center mb-2 ${passwordRequirements.length ? 'text-success' : 'text-muted'}`}>
                                <i className={`bi ${passwordRequirements.length ? 'bi-check-circle-fill' : 'bi-circle'} me-2`}></i>
                                <small>8+ characters</small>
                              </div>
                              <div className={`d-flex align-items-center mb-2 ${passwordRequirements.uppercase ? 'text-success' : 'text-muted'}`}>
                                <i className={`bi ${passwordRequirements.uppercase ? 'bi-check-circle-fill' : 'bi-circle'} me-2`}></i>
                                <small>Uppercase letter</small>
                              </div>
                              <div className={`d-flex align-items-center ${passwordRequirements.lowercase ? 'text-success' : 'text-muted'}`}>
                                <i className={`bi ${passwordRequirements.lowercase ? 'bi-check-circle-fill' : 'bi-circle'} me-2`}></i>
                                <small>Lowercase letter</small>
                              </div>
                            </div>
                            <div className="col-6">
                              <div className={`d-flex align-items-center mb-2 ${passwordRequirements.number ? 'text-success' : 'text-muted'}`}>
                                <i className={`bi ${passwordRequirements.number ? 'bi-check-circle-fill' : 'bi-circle'} me-2`}></i>
                                <small>Number (0-9)</small>
                              </div>
                              <div className={`d-flex align-items-center mb-2 ${passwordRequirements.specialChar ? 'text-success' : 'text-muted'}`}>
                                <i className={`bi ${passwordRequirements.specialChar ? 'bi-check-circle-fill' : 'bi-circle'} me-2`}></i>
                                <small>Special character</small>
                              </div>
                              <div className={`d-flex align-items-center ${passwordRequirements.notSameAsOld ? 'text-success' : 'text-muted'}`}>
                                <i className={`bi ${passwordRequirements.notSameAsOld ? 'bi-check-circle-fill' : 'bi-circle'} me-2`}></i>
                                <small>Different from current</small>
                              </div>
                            </div>
                          </div>
                        </div>
                        <Form.Control.Feedback type="invalid" className="d-block mt-2">
                          {errors.newPassword}
                        </Form.Control.Feedback>
                      </Form.Group>

                      <Form.Group className="mb-4">
                        <Form.Label className="d-flex align-items-center">
                          <i className="bi bi-shield-lock me-2"></i> Confirm New Password
                        </Form.Label>
                        <div className="input-group">
                          <Form.Control
                            type={showPassword.confirmPassword ? "text" : "password"}
                            name="confirmPassword"
                            value={passwordData.confirmPassword}
                            onChange={handlePasswordChange}
                            isInvalid={!!errors.confirmPassword}
                            placeholder="Confirm new password"
                            className="py-2"
                          />
                          <button
                            className="btn btn-outline-secondary"
                            type="button"
                            onClick={() => togglePasswordVisibility('confirmPassword')}
                          >
                            <i className={`bi bi-eye${showPassword.confirmPassword ? '-slash' : ''}`}></i>
                          </button>
                          <Form.Control.Feedback type="invalid">
                            {errors.confirmPassword}
                          </Form.Control.Feedback>
                        </div>
                      </Form.Group>

                      <div className="d-grid gap-2">
                        <Button
                          variant="success"
                          type="submit"
                          disabled={isLoading}
                          className="py-2"
                        >
                          {isLoading ? 'Updating...' : 'Update Password'}
                        </Button>
                      </div>
                    </Form>
                  </div>
                </Tab>
              </Tabs>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
}