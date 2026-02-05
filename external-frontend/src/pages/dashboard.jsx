import React, { useState, useEffect } from 'react';
import { Card, Row, Col, Table, Spinner, Alert, Button, Badge, Modal } from 'react-bootstrap';
import {
  BsClockHistory,
  BsCheckCircle,
  BsXCircle,
  BsExclamationTriangle,
  BsFileEarmarkText,
  BsArrowRepeat,
  BsPlusCircle,
  BsEye,
  BsDownload,
  BsX
} from 'react-icons/bs';
import axios from 'axios';
import { toast } from 'react-toastify';
import { getUserName, getToken } from '../service/tokenStorage';
import config from '../config';
import styled from 'styled-components';
import { useNavigate } from 'react-router-dom';

// Styled Components
const DashboardContainer = styled.div`
  padding: 2rem 0;
`;

const WelcomeSection = styled.div`
  margin-bottom: 2rem;
  h4 {
    color: #2c3e50;
    font-weight: 600;
  }
  p {
    color: #7f8c8d;
  }
`;

const StatCard = styled(Card)`
  border: none;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
  transition: transform 0.2s;
  height: 100%;
  
  &:hover {
    transform: translateY(-5px);
  }

  .card-body {
    padding: 1.5rem;
  }

  .icon-wrapper {
    width: 48px;
    height: 48px;
    border-radius: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 24px;
    margin-bottom: 1rem;
  }
`;

const RecentTable = styled(Table)`
  th {
    background-color: #f8f9fa;
    border-bottom: 2px solid #dee2e6;
    color: #6c757d;
    font-weight: 600;
    font-size: 0.85rem;
    text-transform: uppercase;
    letter-spacing: 0.5px;
  }
  
  td {
    vertical-align: middle;
    padding: 1rem 0.75rem;
  }
`;

const TimelineItem = styled.div`
  position: relative;
  padding-left: 2rem;
  padding-bottom: 1.5rem;
  border-left: 2px solid #e9ecef;

  &:last-child {
    border-left: 2px solid transparent;
  }

  &::before {
    content: '';
    position: absolute;
    left: -0.4rem;
    top: 0;
    width: 14px;
    height: 14px;
    border-radius: 50%;
    background: #fff;
    border: 3px solid ${props => props.color || '#0d6efd'};
  }
`;

const QuickActionsCard = styled(Card)`
  border: none;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
  margin-bottom: 1.5rem;
  background: white;

  .card-header {
    background: white;
    border-bottom: none;
    padding: 1.25rem 1.5rem 0.5rem;
    font-weight: 600;
    color: #2c3e50;
    font-size: 1rem;
  }

  .card-body {
    padding: 1.5rem;
  }

  .action-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 1rem;
    justify-items: center;
  }

  .action-item {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 0.5rem;
    cursor: pointer;
    transition: transform 0.2s;

    &:hover {
      transform: translateY(-3px);
      
      .icon-circle {
        background-color: #e9ecef;
        color: #0d6efd;
      }
    }
  }

  .icon-circle {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    background-color: #f8f9fa;
    color: #6c757d;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.25rem;
    transition: all 0.2s;
  }

  .action-label {
    font-size: 0.75rem;
    color: #6c757d;
    text-align: center;
    font-weight: 500;
  }
`;

const PreviewModal = styled(Modal)`
  .modal-dialog {
    max-width: 90%;
    width: 90%;
    height: 90%;
    max-height: 90%;
    margin: 2% auto;
  }
  
  .modal-content {
    height: 100%;
    display: flex;
    flex-direction: column;
  }
  
  .modal-body {
    flex: 1;
    padding: 0;
    iframe {
      width: 100%;
      height: 100%;
      border: none;
    }
  }
  
  .modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem;
    border-bottom: 1px solid #dee2e6;
  }
  
  .modal-actions {
    display: flex;
    gap: 0.5rem;
    padding: 1rem;
    border-top: 1px solid #dee2e6;
    justify-content: flex-end;
  }
`;

const Loading = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  color: #7f8c8d;
`;

export default function DashboardHome() {
  const [dashboardData, setDashboardData] = useState({
    totalPetitions: 0,
    pendingCount: 0,
    approvedCount: 0,
    rejectedCount: 0,
    recentDocuments: [],
    recentActivities: []
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const userName = localStorage.getItem('contactPerson') || localStorage.getItem('companyName') || getUserName() || 'User';
  const token = getToken();
  const navigate = useNavigate();
  const [showPreview, setShowPreview] = useState(false);
  const [previewLoading, setPreviewLoading] = useState(false);
  const [currentPetition, setCurrentPetition] = useState(null);



  const getStatusBadge = (status) => {
    const statusUpper = String(status || '').toUpperCase();

    const statusMap = {
      'SUBMITTED': { variant: 'info', icon: <BsFileEarmarkText />, label: 'Submitted' },
      'RECEIVED': { variant: 'primary', icon: <BsCheckCircle />, label: 'Received' },
      'UNDER_REVIEW': { variant: 'warning', icon: <BsClockHistory />, label: 'Under Review' },
      'APPROVED': { variant: 'success', icon: <BsCheckCircle />, label: 'Approved' },
      'PRESENTED': { variant: 'success', icon: <BsCheckCircle />, label: 'Presented' },
      'REJECTED': { variant: 'danger', icon: <BsXCircle />, label: 'Rejected' },
      'UPDATED': { variant: 'secondary', icon: <BsArrowRepeat />, label: 'Updated' }
    };

    const config = statusMap[statusUpper] || { variant: 'secondary', icon: <BsExclamationTriangle />, label: status || 'Unknown' };

    return (
      <Badge bg={config.variant} className="d-inline-flex align-items-center gap-1 px-3 py-2">
        {config.icon} {config.label}
      </Badge>
    );
  };

  const formatDate = (dateString) => {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString(undefined, {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  const handlePreview = async (petition) => {
    setPreviewLoading(true);
    setCurrentPetition(petition);
    setShowPreview(true);

    try {
      const clientId = localStorage.getItem('id');
      const authToken = getToken();

      const response = await axios.get(
        `${config.api.baseURL}/Client/${clientId}/Documents/${petition.id}/download`,
        {
          responseType: 'blob',
          headers: {
            'Authorization': `Bearer ${authToken}`
          }
        }
      );

      const file = new Blob([response.data], { type: 'application/pdf' });
      const fileURL = URL.createObjectURL(file);
      setCurrentPetition(prev => ({
        ...prev,
        pdfUrl: fileURL,
        isPdf: response.headers['content-type'] === 'application/pdf'
      }));
    } catch (error) {
      console.error('Error loading document:', error);
      toast.error('Failed to load document. Please try downloading the file.');
      setCurrentPetition(prev => ({
        ...prev,
        pdfUrl: null,
        isPdf: false
      }));
    } finally {
      setPreviewLoading(false);
    }
  };

  const handleDownload = () => {
    if (!currentPetition) return;

    if (currentPetition.pdfUrl) {
      const link = document.createElement('a');
      link.href = currentPetition.pdfUrl;
      link.download = currentPetition.name || `petition-${currentPetition.id}.pdf`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    } else {
      const clientId = localStorage.getItem('id');
      const downloadUrl = `${config.api.baseURL}/Client/${clientId}/Documents/${currentPetition.id}/download`;
      window.open(downloadUrl, '_blank');
    }
  };

  const closePreview = () => {
    setShowPreview(false);
    if (currentPetition?.pdfUrl) {
      URL.revokeObjectURL(currentPetition.pdfUrl);
    }
    setCurrentPetition(null);
  };

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        const clientId = localStorage.getItem('id');
        if (!clientId) throw new Error('Client ID not found');

        const response = await fetch(`${config.api.baseURL}/Client/${clientId}/Dashboard`, {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        });

        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        setDashboardData(data);
      } catch (err) {
        console.error('Error fetching dashboard data:', err);
        setError('Failed to load dashboard data. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    fetchDashboardData();
  }, [token]);

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ height: '60vh' }}>
        <Spinner animation="border" variant="primary" />
      </div>
    );
  }

  if (error) {
    return (
      <DashboardContainer className="container">
        <Alert variant="danger">{error}</Alert>
      </DashboardContainer>
    );
  }

  return (
    <DashboardContainer className="container">
      {/* Welcome Section */}
      <WelcomeSection className="d-flex justify-content-between align-items-center">
        <div>
          <h4>Welcome back, {userName}</h4>
          <p className="mb-0">Here's what's happening with your petitions today.</p>
        </div>
        <Button onClick={() => navigate('/create-petition')} variant="primary" className="d-flex align-items-center gap-2">
          <BsPlusCircle /> New Petition
        </Button>
      </WelcomeSection>

      {/* Stats Grid */}
      <Row className="g-4 mb-5">
        <Col md={3}>
          <StatCard>
            <Card.Body>
              <div className="icon-wrapper bg-primary bg-opacity-10 text-primary">
                <BsFileEarmarkText />
              </div>
              <h6 className="text-muted mb-2">Total Petitions</h6>
              <h2 className="mb-0 fw-bold">{dashboardData.totalPetitions}</h2>
            </Card.Body>
          </StatCard>
        </Col>
        <Col md={3}>
          <StatCard>
            <Card.Body>
              <div className="icon-wrapper bg-warning bg-opacity-10 text-warning">
                <BsClockHistory />
              </div>
              <h6 className="text-muted mb-2">Pending Review</h6>
              <h2 className="mb-0 fw-bold">{dashboardData.pendingCount}</h2>
            </Card.Body>
          </StatCard>
        </Col>
        <Col md={3}>
          <StatCard>
            <Card.Body>
              <div className="icon-wrapper bg-success bg-opacity-10 text-success">
                <BsCheckCircle />
              </div>
              <h6 className="text-muted mb-2">Approved</h6>
              <h2 className="mb-0 fw-bold">{dashboardData.approvedCount}</h2>
            </Card.Body>
          </StatCard>
        </Col>
        <Col md={3}>
          <StatCard>
            <Card.Body>
              <div className="icon-wrapper bg-danger bg-opacity-10 text-danger">
                <BsXCircle />
              </div>
              <h6 className="text-muted mb-2">Rejected</h6>
              <h2 className="mb-0 fw-bold">{dashboardData.rejectedCount}</h2>
            </Card.Body>
          </StatCard>
        </Col>
      </Row>





      <Row className="g-4">
        {/* Recent Petitions */}
        <Col lg={8}>
          <Card className="border-0 shadow-sm h-100">
            <Card.Header className="bg-white border-0 py-3 d-flex justify-content-between align-items-center">
              <h5 className="mb-0 fw-bold">Recent Petitions</h5>
              <Button variant="link" className="text-decoration-none" onClick={() => navigate('/dashboard/petitions')}>
                View All
              </Button>
            </Card.Header>
            <Card.Body className="p-0">
              <div className="table-responsive">
                <RecentTable hover className="mb-0">
                  <thead>
                    <tr>
                      <th className="ps-4">Petition Details</th>
                      <th>Status</th>
                      <th>Date</th>
                      <th className="text-end pe-4">Action</th>
                    </tr>
                  </thead>
                  <tbody>
                    {dashboardData.recentDocuments.length > 0 ? (
                      dashboardData.recentDocuments.map((doc) => (
                        <tr key={doc.id}>
                          <td className="ps-4">
                            <div className="fw-bold text-dark">{doc.description || doc.name}</div>
                            <small className="text-muted">{doc.documentNumber || 'No Ref'}</small>
                          </td>
                          <td>{getStatusBadge(doc.documentStatus?.name || doc.petitionStatus)}</td>
                          <td>{formatDate(doc.createdDate)}</td>
                          <td className="text-end pe-4">
                            <Button
                              variant="light"
                              size="sm"
                              className="text-primary"
                              onClick={() => handlePreview(doc)}
                            >
                              <BsEye /> View
                            </Button>
                          </td>
                        </tr>
                      ))
                    ) : (
                      <tr>
                        <td colSpan="4" className="text-center py-5 text-muted">
                          No petitions found. Start by creating one!
                        </td>
                      </tr>
                    )}
                  </tbody>
                </RecentTable>
              </div>
            </Card.Body>
          </Card>
        </Col>

        {/* Recent Activity */}
        {/* Right Sidebar */}
        <Col lg={4}>
          {/* Quick Launch */}
          <QuickActionsCard>
            <Card.Header>Quick Launch</Card.Header>
            <Card.Body>
              <div className="action-grid">
                <div className="action-item" onClick={() => navigate('/create-petition')}>
                  <div className="icon-circle">
                    <BsPlusCircle />
                  </div>
                  <span className="action-label">Create</span>
                </div>
                <div className="action-item" onClick={() => navigate('/dashboard/upload-petition')}>
                  <div className="icon-circle">
                    <BsDownload />
                  </div>
                  <span className="action-label">Upload</span>
                </div>
                <div className="action-item" onClick={() => navigate('/dashboard/petitions')}>
                  <div className="icon-circle">
                    <BsFileEarmarkText />
                  </div>
                  <span className="action-label">View All</span>
                </div>
              </div>
            </Card.Body>
          </QuickActionsCard>

          <Card className="border-0 shadow-sm h-100">
            <Card.Header className="bg-white border-0 py-3">
              <h5 className="mb-0 fw-bold">Recent Activity</h5>
            </Card.Header>
            <Card.Body>
              <div className="activity-list">
                {dashboardData.recentActivities.length > 0 ? (
                  dashboardData.recentActivities.map((activity, index) => (
                    <TimelineItem
                      key={index}
                      color={activity.type === 'SUBMISSION' ? '#0d6efd' : '#ffc107'}
                    >
                      <div className="d-flex justify-content-between mb-1">
                        <small className="text-muted">{formatDate(activity.timestamp)}</small>
                      </div>
                      <p className="mb-0 text-dark">{activity.description}</p>
                      <Badge
                        bg={activity.type === 'SUBMISSION' ? 'primary' : 'warning'}
                        className="mt-2 text-xs"
                      >
                        {activity.type}
                      </Badge>
                    </TimelineItem>
                  ))
                ) : (
                  <div className="text-center py-4 text-muted">
                    No recent activity.
                  </div>
                )}
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </DashboardContainer >
  );
}
