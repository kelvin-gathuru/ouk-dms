import React, { useState } from 'react';
import styled from 'styled-components';
import { FaUpload, FaFilePdf, FaSpinner, FaCheckCircle } from 'react-icons/fa';
import axios from 'axios';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { getToken } from '../service/tokenStorage';
import config from '../config';

const Container = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: 30px 20px;
  min-height: calc(100vh - 200px);
`;

const UploadCard = styled.div`
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  padding: 30px;
  max-width: 800px;
  margin: 0 auto;
  text-align: center;
`;

const Title = styled.h1`
  color: #2c3e50;
  margin-bottom: 10px;
  font-size: 28px;
`;

const Subtitle = styled.p`
  color: #7f8c8d;
  margin-bottom: 30px;
  font-size: 16px;
`;

const UploadArea = styled.div`
  border: 2px dashed #e0e0e0;
  border-radius: 8px;
  padding: 40px 20px;
  margin-bottom: 25px;
  cursor: pointer;
  transition: all 0.3s ease;
  background-color: ${props => (props.isDragging ? '#f8f9fa' : '#fff')};
  
  &:hover {
    border-color: #4a6da7;
  }
`;

const UploadIcon = styled.div`
  font-size: 48px;
  color: #4a6da7;
  margin-bottom: 15px;
`;

const FileInput = styled.input`
  display: none;
`;

const FileInfo = styled.div`
  margin-top: 20px;
  padding: 15px;
  background-color: #f8f9fa;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: space-between;
`;

const FileIcon = styled.div`
  font-size: 24px;
  color: #4a6da7;
  margin-right: 15px;
`;

const FileDetails = styled.div`
  flex: 1;
  text-align: left;
`;

const FileName = styled.p`
  margin: 0;
  font-weight: 500;
  color: #2c3e50;
`;

const FileSize = styled.p`
  margin: 5px 0 0;
  font-size: 14px;
  color: #7f8c8d;
`;

const RemoveButton = styled.button`
  background: none;
  border: none;
  color: #e74c3c;
  cursor: pointer;
  font-size: 14px;
  font-weight: 500;
  padding: 5px 10px;
  border-radius: 4px;
  transition: background-color 0.2s;
  
  &:hover {
    background-color: #fde8e6;
  }
`;

const SubmitButton = styled.button`
  background-color: #4a6da7;
  color: white;
  border: none;
  padding: 12px 30px;
  font-size: 16px;
  font-weight: 600;
  border-radius: 6px;
  cursor: pointer;
  transition: background-color 0.3s;
  display: inline-flex;
  align-items: center;
  gap: 10px;
  margin-top: 20px;
  
  &:hover {
    background-color: #3a5a8f;
  }
  
  &:disabled {
    background-color: #bdc3c7;
    cursor: not-allowed;
  }
`;

const SuccessMessage = styled.div`
  color: #27ae60;
  margin-top: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  font-weight: 500;
`;

const DirectPetitionUpload = () => {
  const [file, setFile] = useState(null);
  const [isDragging, setIsDragging] = useState(false);
  const [isUploading, setIsUploading] = useState(false);
  const [isUploaded, setIsUploaded] = useState(false);
  const [uploadError, setUploadError] = useState(null);
  const [regarding, setRegarding] = useState('');
  const [errors, setErrors] = useState({});
  const fileInputRef = React.createRef();

  const convertPdfToDocx = async (file) => {
    if (!file || file.type !== 'application/pdf') {
      return file; // Return as is if not a PDF
    }

    try {
      const formData = new FormData();
      formData.append('pdf', file);

      const response = await axios.post('https://pdf2word.topdev.co.ke/api/convert', formData, {
        responseType: 'blob',
      });

      // Create a new File object from the response
      const docxFile = new File(
        [response.data],
        file.name.replace(/\.pdf$/i, '.docx'),
        { type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' }
      );

      return docxFile;
    } catch (error) {
      console.error('Error converting PDF to DOCX:', error);
      throw new Error('Failed to convert PDF to Word document');
    }
  };

  const handleDragOver = (e) => {
    e.preventDefault();
    setIsDragging(true);
  };

  const handleDragLeave = (e) => {
    e.preventDefault();
    setIsDragging(false);
  };

  const handleDrop = (e) => {
    e.preventDefault();
    setIsDragging(false);

    const droppedFile = e.dataTransfer.files[0];
    if (droppedFile) {
      handleFileSelect(droppedFile);
    }
  };

  const handleFileChange = (e) => {
    const selectedFile = e.target.files[0];
    if (selectedFile) {
      handleFileSelect(selectedFile);
    }
  };

  const handleFileSelect = (selectedFile) => {
    // Check file type (only PDF)
    if (selectedFile.type !== 'application/pdf') {
      setUploadError('Please upload a valid PDF file');
      toast.error('Invalid file type. Only PDF files are accepted.');
      return;
    }

    // Check file size (max 10MB)
    const maxSize = 10 * 1024 * 1024; // 10MB
    if (selectedFile.size > maxSize) {
      setUploadError('File size exceeds 10MB limit');
      toast.error('File is too large. Maximum size is 10MB.');
      return;
    }

    setFile(selectedFile);
    setUploadError(null);
  };

  const handleRemoveFile = () => {
    setFile(null);
    setUploadError(null);
    setErrors(prev => ({ ...prev, file: undefined }));
    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  const handleSubmit = async () => {
    // Validate form
    const newErrors = {};

    if (!file) {
      newErrors.file = 'Please select a file to upload';
    }

    if (!regarding.trim()) {
      newErrors.regarding = 'Please enter a subject for your petition';
    }

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      if (newErrors.file) toast.error('Please select a file to upload');
      if (newErrors.regarding) toast.error('Please enter a subject for your petition');
      return;
    }

    try {
      setIsUploading(true);
      setUploadError(null);

      const token = getToken();
      if (!token) {
        throw new Error('Authentication required. Please log in again.');
      }

      // Get clientId from localStorage
      const clientId = localStorage.getItem('id');
      if (!clientId) {
        throw new Error('Client ID not found. Please log in again.');
      }

      // Comment out PDF to DOCX conversion as requested
      // const fileToUpload = await convertPdfToDocx(file);

      const formData = new FormData();
      formData.append('Files', file); // Use 'Files' to match the command property
      formData.append('Name', file.name); // File name
      formData.append('Description', regarding); // Description from regarding field
      formData.append('CategoryId', config.PETITION_CATEGORY_ID); // Category ID from config
      formData.append('ClientId', clientId); // Client ID from localStorage
      formData.append('Extension', file.name.substring(file.name.lastIndexOf('.'))); // File extension
      formData.append('StorageSettingId', '00000000-0000-0000-0000-000000000000'); // Empty GUID for default local storage

      await axios.post(
        `${config.api.baseURL}/ClientDocument`,
        formData,
        {
          headers: {
            'Content-Type': 'multipart/form-data',
            'Authorization': `Bearer ${token}`
          },
        }
      );

      setIsUploaded(true);
      toast.success('Petition uploaded successfully!');

      // Reset form after successful upload
      setTimeout(() => {
        setFile(null);
        setRegarding('');
        setIsUploaded(false);
        if (fileInputRef.current) {
          fileInputRef.current.value = '';
        }
      }, 3000);

    } catch (error) {
      console.error('Error uploading file:', error);
      const errorMsg = error.response?.data?.message ||
        error.message ||
        'Failed to upload document. Please try again.';
      setUploadError(errorMsg);
      toast.error(errorMsg);
    } finally {
      setIsUploading(false);
    }
  };

  const formatFileSize = (bytes) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  };

  return (
    <Container>
      <UploadCard>
        <Title>Upload Your Petition</Title>
        <Subtitle>Upload a PDF document containing your completed petition (max 10MB)</Subtitle>

        <div style={{ marginBottom: '20px', textAlign: 'left' }}>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: '500', color: '#2c3e50' }}>
            Regarding (Subject of the Petition) *
          </label>
          <input
            type="text"
            value={regarding}
            onChange={(e) => {
              setRegarding(e.target.value);
              if (errors.regarding) {
                setErrors(prev => ({ ...prev, regarding: undefined }));
              }
            }}
            placeholder="Enter the subject of your petition..."
            style={{
              width: '100%',
              padding: '10px 12px',
              border: `1px solid ${errors.regarding ? '#dc3545' : '#ced4da'}`,
              borderRadius: '4px',
              fontSize: '16px',
              outline: 'none',
              transition: 'border-color 0.2s',
              ':focus': {
                borderColor: '#4a6da7',
              },
            }}
          />
          {errors.regarding && (
            <div style={{ color: '#dc3545', fontSize: '0.8em', marginTop: '5px' }}>
              {errors.regarding}
            </div>
          )}
        </div>

        <FileInput
          type="file"
          id="file-upload"
          accept=".pdf,application/pdf"
          onChange={handleFileChange}
          ref={fileInputRef}
        />

        <UploadArea
          onClick={() => document.getElementById('file-upload').click()}
          onDragOver={handleDragOver}
          onDragLeave={handleDragLeave}
          onDrop={handleDrop}
          isDragging={isDragging}
        >
          <UploadIcon>
            <FaUpload />
          </UploadIcon>
          <h3>Drag & Drop your PDF file here</h3>
          <p>or click to browse files (PDF only, max 10MB)</p>
        </UploadArea>

        {file && (
          <FileInfo>
            <div style={{ display: 'flex', alignItems: 'center' }}>
              <FileIcon><FaFilePdf /></FileIcon>
              <FileDetails>
                <FileName>{file.name}</FileName>
                <FileSize>{formatFileSize(file.size)}</FileSize>
              </FileDetails>
            </div>
            <RemoveButton onClick={handleRemoveFile}>Remove</RemoveButton>
          </FileInfo>
        )}

        {errors.file && <p style={{ color: '#e74c3c', marginTop: '10px' }}>{errors.file}</p>}
        {uploadError && !errors.file && <p style={{ color: '#e74c3c', marginTop: '10px' }}>{uploadError}</p>}

        {isUploaded ? (
          <SuccessMessage>
            <FaCheckCircle /> Upload successful! Your petition will be reviewed shortly.
          </SuccessMessage>
        ) : (
          <SubmitButton
            onClick={handleSubmit}
            disabled={!file || isUploading}
          >
            {isUploading ? (
              <>
                <FaSpinner className="fa-spin" /> Uploading...
              </>
            ) : (
              'Upload Petition'
            )}
          </SubmitButton>
        )}
      </UploadCard>
    </Container>
  );
};

export default DirectPetitionUpload;
