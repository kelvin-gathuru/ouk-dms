import React from 'react';
import { Card, Button, Form, Alert } from 'react-bootstrap';
import { toast } from 'react-toastify';

export default function Step4({ formData, uploadedFiles, onBack, onSubmit, loading }) {
  const [confirmed, setConfirmed] = React.useState(false);

  // Format full name based on type
  const getFullName = () => {
    if (formData.petitionerType === 'individual') {
      return `${formData.firstName || ''} ${formData.lastName || ''}`.trim();
    } else {
      return formData.representativeName || formData.orgName || 'N/A';
    }
  };

  // Handle form submission
  const handleSubmit = async () => {
    if (!confirmed) {
      toast.error('Please confirm that all information is accurate.');
      return;
    }

    try {
      //  API call
      await new Promise((resolve) => setTimeout(resolve, 2000));
      toast.success('Petition submitted successfully!');
      onSubmit();
    } catch (error) {
      toast.error('Failed to submit petition. Please try again later.');
    }
  }

  return (
    <div>

      <h3 className="mb-4">4. Review & Submit</h3>

      <Alert variant="warning">
        <i className="bi bi-exclamation-triangle-fill me-2"></i>
        Please review your petition carefully before submitting. You will not be able to make changes after submission.
      </Alert>

      {/* Petitioner Information */}
      <Card className="mb-4">
        <Card.Header className="bg-light">
          <h5 className="mb-0">Petitioner Information</h5>
        </Card.Header>
        <Card.Body>
          <p><strong>Petitioner Type:</strong> {formData.petitionerType}</p>
          <p><strong>Full Name:</strong> {getFullName()}</p>
          <p><strong>Email:</strong> {formData.email}</p>
          <p><strong>Phone:</strong> {formData.phone}</p>
        </Card.Body>
      </Card>

      {/* Petition Details */}
      <Card className="mb-4">
        <Card.Header className="bg-light">
          <h5 className="mb-0">Petition Details</h5>
        </Card.Header>
        <Card.Body>
          <p><strong>Title:</strong> {formData.petitionTitle}</p>
          <p><strong>Grievance Statement:</strong> {formData.grievanceStatement}</p>
          <p><strong>Description:</strong> {formData.detailedDescription}</p>
          <p><strong>Prayer:</strong> {formData.prayer}</p>
        </Card.Body>
      </Card>

      {/* Supporting Documents */}
      <Card className="mb-4">
        <Card.Header className="bg-light d-flex justify-content-between align-items-center">
          <h5 className="mb-0">Supporting Documents</h5>
          <span className="badge bg-secondary">{uploadedFiles.length} files</span>
        </Card.Header>
        <Card.Body>
          {uploadedFiles.length === 0 ? (
            <p>No documents uploaded.</p>
          ) : (
            <ul className="list-group">
              {uploadedFiles.map((file, idx) => (
                <li key={idx} className="list-group-item d-flex justify-content-between align-items-center">
                  {file.name}
                  <span className="badge bg-light text-dark">{(file.size / 1024).toFixed(2)} KB</span>
                </li>
              ))}
            </ul>
          )}
        </Card.Body>
      </Card>

      {/* Confirmation Checkbox */}
      <Form.Group className="mb-4">
        <Form.Check
          required
          type="checkbox"
          id="finalConfirmation"
          label="I certify that all information provided is accurate and complete to the best of my knowledge."
          checked={confirmed}
          onChange={(e) => setConfirmed(e.target.checked)}
        />
      </Form.Group>

      {/* Navigation Buttons */}
      <div className="d-flex justify-content-between">
        <Button variant="outline-secondary" onClick={onBack}>
          <i className="bi bi-arrow-left me-2"></i>Back
        </Button>
        <Button
          variant="success"
          onClick={handleSubmit}
          disabled={!confirmed || loading}
        >
          {loading && (
            <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
          )}
          Submit Petition
        </Button>
      </div>
    </div>
  );
}
