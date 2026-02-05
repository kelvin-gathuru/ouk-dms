import React, { useState, useEffect } from 'react';
import { Button, Spinner, Alert, Modal, Container } from 'react-bootstrap';
import { BsEye, BsDownload, BsX } from 'react-icons/bs';
import axios from 'axios';
// import { useNavigate } from 'react-router-dom';
// import { AuthContext } from '../context/AuthContext';
import config from '../config';
import styled from 'styled-components';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

// Styled Components
const PetitionCard = styled.div`
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  padding: 5px;
  margin-bottom: 16px;
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

const CardContent = styled.div`
  flex: 1;
`;

const PetitionTitle = styled.h5`
  margin: 0 0 8px 0;
  color: #2c3e50;
`;

const PetitionDate = styled.span`
  color: #7f8c8d;
  font-size: 0.9em;
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

const MyPetitions = () => {
  const [petitions, setPetitions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [showPreview, setShowPreview] = useState(false);
  const [previewLoading, setPreviewLoading] = useState(false);
  const [currentPetition, setCurrentPetition] = useState(null);
  // const navigate = useNavigate();
  // const { user } = useContext(AuthContext);

  useEffect(() => {
    const fetchPetitions = async () => {
      try {
        const token = localStorage.getItem('userToken');
        const clientId = localStorage.getItem('id');

        if (!token) {
          setError('Authentication required');
          setLoading(false);
          return;
        }

        if (!clientId) {
          setError('Client ID not found');
          setLoading(false);
          return;
        }

        const response = await axios.get(`${config.api.baseURL}/Client/${clientId}/Documents`, {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });

        // The API returns a DocumentList object, so we need to extract the data array
        // Based on DocumentList.cs, it seems it inherits from List<DocumentDto>, but usually serialized as an array or object with items
        // Let's handle both cases just to be safe, but based on Controller return Ok(result), it should be the list or object wrapping it
        // If it returns the DocumentList object properties (TotalCount, etc) + items, we might need to check how it's serialized.
        // Usually List<T> inheritance serializes as Array, but with extra properties it might be an object.
        // Let's assume it returns an array or an object with 'data' or just the array.
        // Checking GetDocuments in DocumentController, it returns Ok(result) where result is DocumentList.

        const data = Array.isArray(response.data) ? response.data : (response.data.data || []);
        setPetitions(data);
      } catch (err) {
        console.error('Error fetching petitions:', err);
        setError('Failed to fetch petitions. Please try again later.');
        toast.error('Failed to load petitions');
      } finally {
        setLoading(false);
      }
    };

    fetchPetitions();
  }, []);



  const formatDate = (dateString) => {
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  const handlePreview = async (petition) => {
    setPreviewLoading(true);
    setCurrentPetition(petition);
    setShowPreview(true);

    try {
      const token = localStorage.getItem('userToken');
      const clientId = localStorage.getItem('id');

      const response = await axios.get(
        `${config.api.baseURL}/Client/${clientId}/Documents/${petition.id}/download`,
        {
          responseType: 'blob',
          headers: {
            'Authorization': `Bearer ${token}`
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

    // Use the blob URL if available (from preview)
    if (currentPetition.pdfUrl) {
      const link = document.createElement('a');
      link.href = currentPetition.pdfUrl;
      link.download = currentPetition.name || `petition-${currentPetition.id}.pdf`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    } else {
      // Direct download if preview wasn't loaded
      const clientId = localStorage.getItem('id');
      const downloadUrl = `${config.api.baseURL}/Client/${clientId}/Documents/${currentPetition.id}/download`;
      window.open(downloadUrl, '_blank');
    }
  };

  const closePreview = () => {
    setShowPreview(false);
    // Clean up the object URL to avoid memory leaks
    if (currentPetition?.pdfUrl) {
      URL.revokeObjectURL(currentPetition.pdfUrl);
    }
    setCurrentPetition(null);
  };

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ minHeight: '50vh' }}>
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
      </div>
    );
  }

  if (error) {
    return <Alert variant="danger">{error}</Alert>;
  }

  return (
    <Container className="py-4">
      <h4 className="mb-4">My Petitions</h4>

      {petitions.length === 0 ? (
        <div className="alert alert-info">You haven't submitted any petitions yet.</div>
      ) : (
        <div>
          {petitions.map((petition) => (
            <PetitionCard key={petition.id}>
              <CardContent>
                <PetitionTitle className='mb-4'>{petition.description || petition.name}</PetitionTitle>
                <div className="d-flex align-items-center gap-2">
                  <PetitionDate>
                    Submitted on {formatDate(petition.createdDate)}
                  </PetitionDate>
                </div>
              </CardContent>

              <Button
                variant="primary"
                size="sm"
                onClick={() => handlePreview(petition)}
                disabled={previewLoading && currentPetition?.id === petition.id}
                className="d-flex align-items-center gap-2"
              >
                {previewLoading && currentPetition?.id === petition.id ? (
                  <>
                    <Spinner as="span" size="sm" animation="border" role="status" />
                    Loading...
                  </>
                ) : (
                  <>
                    <BsEye /> View Document
                  </>
                )}
              </Button>
            </PetitionCard>
          ))}
        </div>
      )}

      {/* Preview Modal */}
      <PreviewModal
        show={showPreview}
        onHide={closePreview}
        size="xl"
        centered
        fullscreen="lg-down"
      >
        <Modal.Header closeButton>
          <Modal.Title>
            {currentPetition?.description || currentPetition?.name || 'Document Preview'}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {previewLoading ? (
            <Loading>
              <Spinner animation="border" />
              <span className="ms-2">Loading document...</span>
            </Loading>
          ) : currentPetition?.isPdf ? (
            <iframe
              src={`${currentPetition.pdfUrl}#toolbar=0`}
              title="Document Preview"
            />
          ) : (
            <Loading>
              <div className="text-center">
                <p>This document cannot be previewed.</p>
              </div>
            </Loading>
          )}
        </Modal.Body>
        <div className="modal-actions">
          <Button
            variant="secondary"
            onClick={closePreview}
            className="d-flex align-items-center gap-1"
          >
            <BsX /> Close
          </Button>
          <Button
            variant="primary"
            onClick={handleDownload}
            disabled={!currentPetition}
            className="d-flex align-items-center gap-1"
          >
            <BsDownload /> Download
          </Button>
        </div>
      </PreviewModal>
    </Container>
  );
};

export default MyPetitions;
