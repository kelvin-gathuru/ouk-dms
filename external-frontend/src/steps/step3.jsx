import React, { useState,useEffect } from 'react';
import { Form, Button, Alert } from 'react-bootstrap';
import { toast } from 'react-toastify';

const Step3 = ({ onNext, onBack, formData, updateFormData,  onFilesChange }) => {
  const [selectedFiles, setSelectedFiles] = useState([]);
  const [errors, setErrors] = useState([]);
  const [agreeTerms, setAgreeTerms] = useState(false);
  const [agreePrivacy, setAgreePrivacy] = useState(false);

  const allowedTypes = [
    'application/pdf',
    'application/msword',
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
    'image/jpeg',
    'image/png',
  ];
  const maxSizeMB = 5;

 const handleFileChange = (e) => {
  const files = Array.from(e.target.files);
  const validFiles = [];
  const existingNames = selectedFiles.map((f) => f.name);

  files.forEach((file) => {
    const sizeMB = file.size / (1024 * 1024);
    if (!allowedTypes.includes(file.type)) {
      toast.error(`❌ ${file.name}: Unsupported file type.`);
    } else if (sizeMB > maxSizeMB) {
      toast.error(`❌ ${file.name}: File size exceeds ${maxSizeMB} MB.`);
    } else if (existingNames.includes(file.name)) {
      toast.warning(`⚠️ ${file.name} already uploaded.`);
    } else {
      validFiles.push(file);
      toast.success(`✅ ${file.name} uploaded successfully.`);
    }
  });

  

  const allFiles = [...selectedFiles, ...validFiles];
  setSelectedFiles(allFiles);
  updateFormData({ ...formData, supportingDocs: allFiles });

  if (typeof onFilesChange === 'function') {
    onFilesChange(allFiles);
  }
};
 
useEffect(() => {
  if (formData.supportingDocs?.length) {
    setSelectedFiles(formData.supportingDocs);
  }
}, [formData.supportingDocs]);

//Delete a file
const removeFile = (index) => {
  const updatedFiles = selectedFiles.filter((_, i) => i !== index);
  setSelectedFiles(updatedFiles);
  updateFormData({ ...formData, supportingDocs: updatedFiles });

  if (typeof onFilesChange === 'function') {
    onFilesChange(updatedFiles);
  }

  toast.info(` File removed.`);
};


  const handleNext = () => {
    if (!agreeTerms || !agreePrivacy) {
      toast.error('Please agree to the terms and privacy policy.');
      return;
    }

    onNext(); // Go to Step 4
  };

  return (
    <div>
      <h3 className="mb-4">3. Supporting Documents</h3>

      <Alert variant="info">
        <i className="bi bi-info-circle-fill me-2" />
        Please upload any supporting documents that provide additional context or evidence for your petition.
      </Alert>

      <Form.Group className="mb-4">
        <Form.Label>Supporting Documents (Optional)</Form.Label>
        <Form.Control type="file" multiple onChange={handleFileChange} />
        <Form.Text>
          You can upload multiple files (PDF, DOC, DOCX, JPG, PNG). Max file size: 5MB per file.
        </Form.Text>
      </Form.Group>

      {errors.map((err, idx) => (
        <Alert key={idx} variant="danger">{err}</Alert>
      ))}

      {selectedFiles.map((file, idx) => (
        <Alert key={idx} variant="success"  className="d-flex justify-content-between align-items-center">
          <span>✅ {file.name} ({(file.size / (1024 * 1024)).toFixed(2)} MB)</span>
          <Button variant="outline-danger" size="sm" onClick={() => removeFile(idx)}>
      Remove
    </Button>
        </Alert>
      ))}

      <Form.Group className="mb-3">
        <Form.Check
          type="checkbox"
          label="I confirm that all information provided is true and accurate."
          checked={agreeTerms}
          onChange={(e) => setAgreeTerms(e.target.checked)}
          required
        />
        <Form.Check
          className="mt-2"
          type="checkbox"
          label={
            <>
              I have read and agree to the{' '}
              <a href="#" onClick={(e) => e.preventDefault()}>Privacy Policy</a> and{' '}
              <a href="#" onClick={(e) => e.preventDefault()}>Terms of Service</a>.
            </>
          }
          checked={agreePrivacy}
          onChange={(e) => setAgreePrivacy(e.target.checked)}
          required
        />
      </Form.Group>

      <div className="d-flex justify-content-between mt-4">
        <Button variant="outline-secondary" onClick={onBack}>
          <i className="bi bi-arrow-left me-2" />
          Back
        </Button>

        <Button variant="success" onClick={handleNext}>
          Next: Review & Submit <i className="bi bi-arrow-right ms-2" />
        </Button>
      </div>
    </div>
  );
};

export default Step3;
