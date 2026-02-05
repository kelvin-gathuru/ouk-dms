import React, { useState, useEffect, useRef } from 'react';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import styled from 'styled-components';
import { FaEye, FaSpinner } from 'react-icons/fa';
import axios from 'axios';
import config from '../config';
import { getToken } from '../service/tokenStorage';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

// Styled Components
const Container = styled.div`
  max-width: 1550px;
  margin: 0 auto;
  padding: 5px;
  display: flex;
  flex-direction: column;
  gap: 5px;
  
  @media (min-width: 1024px) {
    flex-direction: row;
    align-items: flex-start;
  }
`;

const FormSection = styled.div`
  flex: 1;
  min-width: 0;
  
  @media (min-width: 1024px) {
    padding-right: 20px;
  }
`;

const PreviewSection = styled.div`
  flex: 1;
  min-width: 0;
  position: relative;
  border: 1px dashed #e0e0e0;
  border-radius: 8px;
  min-height: 600px;
  display: flex;
  flex-direction: column;
  max-height: calc(100vh - 100px);
  
  @media (max-width: 1023px) {
    margin-top: 30px;
    min-height: 500px;
  }
`;

const PreviewToggle = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  color: #4a6da7;
  cursor: pointer;
  padding: 15px;
  font-weight: 500;
  background-color: #f8f9fa;
  border-radius: 8px 8px 0 0;
  border-bottom: 1px solid #e0e0e0;
  
  &:hover {
    background-color: #f1f3f5;
  }
  
  svg {
    font-size: 18px;
  }
`;

const PreviewPlaceholder = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  flex: 1;
  color: #6c757d;
  padding: 20px;
  text-align: center;
  
  p {
    margin-top: 10px;
    margin-bottom: 15px;
  }
`;

const PreviewContent = styled.div`
  flex: 1;
  overflow: auto;
  display: flex;
  flex-direction: column;
  height: 100%;
`;

const PreviewHeader = styled.div`
  background: #4a6da7;
  color: white;
  padding: 12px 16px;
  font-weight: 500;
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

const PreviewIframe = styled.iframe`
  width: 100%;
  flex: 1;
  border: none;
  min-height: 500px;
`;

const FormHeader = styled.div`
  position: relative;
  margin-bottom: 20px;
  padding: 15px;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  background: #f8f9fa;
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: center;
  gap: 15px;
`;

const RadioGroup = styled.div`
  display: flex;
  gap: 20px;
  flex-wrap: wrap;
`;

const PrimaryButton = styled.button`
  background: #4a6da7;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  font-weight: 500;
  transition: background-color 0.2s;
  min-width: 120px;
  text-align: center;
  
  &:hover {
    background: #3a5a8c;
  }
  
  &:disabled {
    background: #cccccc;
    cursor: not-allowed;
  }
`;

const SecondaryButton = styled(PrimaryButton)`
  background: #6c757d;
  
  &:hover {
    background: #5a6268;
  }
`;

const PreviewButton = styled(PrimaryButton)`
  background: #28a745;
  
  &:hover {
    background: #218838;
  }
`;

const PreviewDialog = styled.div`
  width: 100%;
  height: 600px;
  border: 1px solid #e0e0e0;
  background: #fff;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
  display: ${({ show }) => (show ? 'block' : 'none')};
`;

const DownloadButton = styled.button`
  padding: 6px 12px;
  font-size: 14px;
  cursor: pointer;
  background: #28a745;
  color: white;
  border: none;
  border-radius: 4px;
  display: flex;
  align-items: center;
  gap: 6px;
  transition: background-color 0.2s;
  
  &:hover {
    background: #218838;
  }
`;

const FormContainer = styled.div`
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 20px;
  background: #fff;
`;

const FormGroup = styled.div`
  margin-bottom: 20px;
  
  label {
    display: block;
    margin-bottom: 6px;
    font-weight: 500;
    color: #333;
  }
`;

const StyledInput = styled.input`
  width: 100%;
  padding: 10px 12px;
  border: 1px solid #ced4da;
  border-radius: 4px;
  font-size: 14px;
  transition: border-color 0.15s, box-shadow 0.15s;
  
  &:focus {
    border-color: #80bdff;
    outline: 0;
    box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
  }
`;

const StyledTextarea = styled.textarea`
  width: 100%;
  padding: 10px 12px;
  border: 1px solid #ced4da;
  border-radius: 4px;
  font-size: 14px;
  min-height: 100px;
  resize: vertical;
  transition: border-color 0.15s, box-shadow 0.15s;
  
  &:focus {
    border-color: #80bdff;
    outline: 0;
    box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
  }
  
  &.dotted {
    border: none;
    border-bottom: 1px dotted #ced4da;
    border-radius: 0;
    padding: 5px 0;
    min-height: auto;
    
    &:focus {
      border-bottom-color: #80bdff;
      box-shadow: none;
    }
  }
  
  &.small {
    width: 80px;
    display: inline-block;
    margin: 0 5px;
  }
  
  &.tiny {
    width: 50px;
    display: inline-block;
    margin: 0 5px;
  }
`;

const Table = styled.table`
  width: 100%;
  border-collapse: collapse;
  margin: 20px 0;
  font-size: 14px;
  
  th, td {
    border: 1px solid #dee2e6;
    padding: 10px;
    text-align: left;
  }
  
  th {
    background-color: #f8f9fa;
    font-weight: 600;
  }
  
  input, textarea {
    width: 100%;
    padding: 6px 8px;
    border: 1px solid #ced4da;
    border-radius: 4px;
    font-size: 14px;
    
    &:focus {
      border-color: #80bdff;
      outline: 0;
      box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
    }
  }
  
  textarea {
    min-height: 60px;
    resize: vertical;
  }
`;

const ActionButton = styled.button`
  padding: 6px 12px;
  border: 1px solid #6c757d;
  background: #fff;
  color: #6c757d;
  border-radius: 4px;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 5px;
  font-size: 13px;
  transition: all 0.2s;
  
  &:hover {
    background: #f8f9fa;
  }
  
  &.primary {
    background: #4a6da7;
    color: white;
    border-color: #4a6da7;
    
    &:hover {
      background: #3a5a8c;
      border-color: #3a5a8c;
    }
  }
  
  &.danger {
    background: #dc3545;
    color: white;
    border-color: #dc3545;
    
    &:hover {
      background: #c82333;
      border-color: #bd2130;
    }
  }
`;

const ButtonGroup = styled.div`
  display: flex;
  gap: 10px;
  margin: 15px 0;
  flex-wrap: wrap;
`;

const Card = styled.div`
  background: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 20px;
  
  h3 {
    margin-top: 0;
    margin-bottom: 20px;
    color: #333;
    font-size: 18px;
    padding-bottom: 10px;
    border-bottom: 1px solid #eee;
  }
`;

const FullPreviewDialog = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: white;
  z-index: 1000;
  display: flex;
  flex-direction: column;
`;

const PreviewToolbar = styled.div`
  padding: 15px;
  background: #f8f9fa;
  border-bottom: 1px solid #e0e0e0;
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

const ActionButtons = styled.div`
  display: flex;
  gap: 10px;
`;

const SubmitDialog = styled.div`
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background: white;
  padding: 30px;
  border-radius: 8px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
  z-index: 1001;
  max-width: 500px;
  width: 90%;
`;

const Overlay = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  z-index: 1000;
`;

const CreatePetition = () => {
  const [currentStep, setCurrentStep] = useState(1);
  const [formData, setFormData] = useState({
    petitionType: 'individual',
    regarding: '',
    petitioners: '',
    reasons: [''],
    efforts: [''],
    that4: 'the issues in respect of which this Petition is raised are not pending before any court of law or any constitutional or legal body.',
    prayers: [''],
    day: new Date().getDate(),
    month: new Date().toLocaleString('default', { month: 'long' }),
    year: new Date().getFullYear().toString().slice(-2),
    indName: '',
    indSign: '',
    indContact: '',
    groupPetitioners: [
      { name: '', id: '', contact: '', signature: '' }
    ]
  });

  const [showPreview, setShowPreview] = useState(false);
  const [pdfUrl, setPdfUrl] = useState('');
  const iframeRef = useRef(null);

  const [showFullPreview, setShowFullPreview] = useState(false);
  const [showSubmitDialog, setShowSubmitDialog] = useState(false);
  const [signedPetition, setSignedPetition] = useState(null);

  const [isUploading, setIsUploading] = useState(false);
  const [uploadError, setUploadError] = useState(null);
  const [documentUrl, setDocumentUrl] = useState('');

  const [errors, setErrors] = useState({});

  // Steps configuration
  const steps = [
    { id: 1, title: 'Petition Type' },
    { id: 2, title: 'Petition Details' },
    { id: 3, title: 'Reasons' },
    { id: 4, title: 'Efforts Made' },
    { id: 5, title: 'Court Details & Prayers' },
    { id: 6, title: 'Date & Petitioner Info' },
    { id: 7, title: 'Review & Submit' }
  ];

  // Navigation functions
  const nextStep = () => {
    // If we're on step 2 (index 1), validate the 'regarding' field
    if (currentStep === 2 && !formData.regarding.trim()) {
      setErrors({
        ...errors,
        regarding: 'Please enter a subject for your petition'
      });
      return;
    }

    // Clear any previous errors when proceeding
    setErrors({});

    if (currentStep < steps.length) {
      setCurrentStep(currentStep + 1);
    }
  };

  const prevStep = () => {
    if (currentStep > 1) {
      setCurrentStep(currentStep - 1);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleGroupPetitionerChange = (index, field, value) => {
    const updatedPetitioners = [...formData.groupPetitioners];
    updatedPetitioners[index] = {
      ...updatedPetitioners[index],
      [field]: value
    };
    setFormData(prev => ({
      ...prev,
      groupPetitioners: updatedPetitioners
    }));
  };

  const addPetitioner = () => {
    setFormData(prev => ({
      ...prev,
      groupPetitioners: [
        ...prev.groupPetitioners,
        { name: '', id: '', contact: '', signature: '' }
      ]
    }));
  };

  const removePetitioner = (index) => {
    if (formData.groupPetitioners.length <= 1) return;
    const updatedPetitioners = formData.groupPetitioners.filter((_, i) => i !== index);
    setFormData(prev => ({
      ...prev,
      groupPetitioners: updatedPetitioners
    }));
  };

  const addPrayer = () => {
    setFormData(prev => ({
      ...prev,
      prayers: [...prev.prayers, '']
    }));
  };

  const removePrayer = (index) => {
    if (formData.prayers.length <= 1) return;
    const updatedPrayers = formData.prayers.filter((_, i) => i !== index);
    setFormData(prev => ({
      ...prev,
      prayers: updatedPrayers
    }));
  };

  const handlePrayerChange = (index, value) => {
    const updatedPrayers = [...formData.prayers];
    updatedPrayers[index] = value;
    setFormData(prev => ({
      ...prev,
      prayers: updatedPrayers
    }));
  };

  const addReason = () => {
    setFormData(prev => ({
      ...prev,
      reasons: [...prev.reasons, '']
    }));
  };

  const removeReason = (index) => {
    if (formData.reasons.length <= 1) return;
    const updatedReasons = formData.reasons.filter((_, i) => i !== index);
    setFormData(prev => ({
      ...prev,
      reasons: updatedReasons
    }));
  };

  const handleReasonChange = (index, value) => {
    const updatedReasons = [...formData.reasons];
    updatedReasons[index] = value;
    setFormData(prev => ({
      ...prev,
      reasons: updatedReasons
    }));
  };

  const addEffort = () => {
    setFormData(prev => ({
      ...prev,
      efforts: [...prev.efforts, '']
    }));
  };

  const removeEffort = (index) => {
    if (formData.efforts.length <= 1) return;
    const updatedEfforts = formData.efforts.filter((_, i) => i !== index);
    setFormData(prev => ({
      ...prev,
      efforts: updatedEfforts
    }));
  };

  const handleEffortChange = (index, value) => {
    const updatedEfforts = [...formData.efforts];
    updatedEfforts[index] = value;
    setFormData(prev => ({
      ...prev,
      efforts: updatedEfforts
    }));
  };

  const handleDateChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const togglePreview = () => {
    setShowPreview(!showPreview);
  };

  const generatePDF = () => {
    // Sample payload for creating a PDF
    console.log('Sample PDF Generation Payload:', {
      formData: {
        petitionType: formData.petitionType,
        regarding: formData.regarding,
        petitioners: formData.petitioners,
        reasons: formData.reasons.filter(r => r.trim() !== ''),
        efforts: formData.efforts.filter(e => e.trim() !== ''),
        that4: formData.that4,
        prayers: formData.prayers.filter(p => p.trim() !== ''),
        day: formData.day,
        month: formData.month,
        year: formData.year,
        ...(formData.petitionType === 'individual' ? {
          indName: formData.indName,
          indSign: formData.indSign,
          indContact: formData.indContact
        } : {
          groupPetitioners: formData.groupPetitioners.map(p => ({
            name: p.name,
            id: p.id,
            contact: p.contact,
            signature: p.signature
          }))
        })
      },
      metadata: {
        generatedAt: new Date().toISOString(),
        pageSize: 'A4',
        unit: 'pt',
        font: 'times',
        fontSize: 12
      }
    });

    const doc = new jsPDF({ unit: 'pt', format: 'a4' });
    let y = 50;
    const marginLeft = 50;
    const maxWidth = 500;

    doc.setFont('times', 'normal');
    doc.setFontSize(12);

    // Address
    [
      'The Clerk of the National Assembly',
      'P. O. Box 41842-00100',
      'Nairobi, Kenya',
      'Main Parliament Buildings'
    ].forEach(line => {
      y = addWrappedText(doc, line, 50, y, maxWidth);
    });

    // Title
    doc.setFont('times', 'bold');
    doc.setFontSize(14);
    const title = `PUBLIC PETITION TO THE NATIONAL ASSEMBLY REGARDING ${formData.regarding || '____'}`;
    y = addWrappedText(doc, title, marginLeft, y, maxWidth, 18);

    // Undersigned
    doc.setFont('times', 'normal');
    doc.setFontSize(12);
    const undersigned = `${formData.petitionType === 'individual' ? 'I, the UNDERSIGNED,' : 'WE, the UNDERSIGNED,'} ${formData.petitioners || ''}`;
    y = addWrappedText(doc, undersigned, marginLeft, y, maxWidth);

    // Attention line
    // y = addWrappedText(doc, 'DRAW the attention of the House to the following:', marginLeft, y, maxWidth);

    // REASONS section
    if (formData.reasons.some(reason => reason.trim() !== '')) {
      y = addWrappedText(doc, 'DRAW the attention of the House to the following:', marginLeft, y, maxWidth);

      // Add reasons
      formData.reasons.forEach((reason) => {
        if (reason.trim()) {
          y = addWrappedText(doc, `THAT, ${reason}`, marginLeft, y, maxWidth);
        }
      });
    }

    // EFFORTS section
    if (formData.efforts.some(effort => effort.trim() !== '')) {
      // Add efforts with same formatting as reasons
      formData.efforts.forEach((effort) => {
        if (effort.trim()) {
          y = addWrappedText(doc, `THAT, ${effort}`, marginLeft, y, maxWidth);
        }
      });
    }

    // COURT DETAILS section
    if (formData.that4 && formData.that4.trim() !== '') {
      y = addWrappedText(doc, `THAT, ${formData.that4}`, marginLeft, y, maxWidth);
    }

    // THEREFORE
    if (formData.prayers.some(prayer => prayer.trim() !== '')) {
      y = addWrappedText(doc, 'THEREFORE, your humble Petitioners pray that the National Assembly through the Public Petitions Committee to:', marginLeft, y, maxWidth);

      // Add each prayer as a numbered item with small Roman numerals in brackets
      formData.prayers.forEach((prayer, index) => {
        if (prayer.trim()) {
          const romanNumeral = toRoman(index + 1).toLowerCase();
          // Add the prayer with small Roman numeral in brackets
          y = addWrappedText(doc, `(${romanNumeral}) ${prayer}`, marginLeft + 20, y, maxWidth - 20);
        }
      });
      y += 10;
    }

    // Date
    const dateStr = `Dated this ${formData.day} day of ${formData.month} 20${formData.year}`;
    y = addWrappedText(doc, dateStr, marginLeft, y, maxWidth);

    // Add "And your PETITIONERS will ever pray"
    y = addWrappedText(doc, 'And your PETITIONERS will ever pray', marginLeft, y, maxWidth);
    y += 10; // Add some space

    // Add "Presented by" title
    y = addWrappedText(doc, 'Presented by:', marginLeft, y, maxWidth);
    y += 5; // Add small space

    // Individual or Group
    if (formData.petitionType === 'individual') {
      [
        `Name: ${formData.indName || '__________________'}`,
        `Signature: ${formData.indSign || '__________________'}`,
        `Contact: ${formData.indContact || '__________________'}`
      ].forEach(text => {
        y = addWrappedText(doc, text, marginLeft + 20, y, maxWidth); // Indent the details
      });
    } else {
      const rows = formData.groupPetitioners.map((p, i) => [
        i + 1,
        p.name || '',
        p.id || '',
        p.contact || '',
        p.signature || ''
      ]);

      autoTable(doc, {
        head: [['S/No', 'NAME OF PETITIONER', 'ID NUMBER', 'CONTACT', 'SIGNATURE']],
        body: rows,
        startY: y,
        theme: 'grid',
        styles: { font: 'times', fontSize: 10 },
        headStyles: { fillColor: [220, 220, 220] }
      });

      y = doc.lastAutoTable.finalY + 20;
    }

    const pdfDataUri = doc.output('datauristring');
    setPdfUrl(pdfDataUri);

    if (iframeRef.current) {
      iframeRef.current.src = pdfDataUri;
    }

    return doc;
  };

  const addWrappedText = (doc, text, x, y, maxWidth, lineHeight = 15) => {
    const pageHeight = doc.internal.pageSize.height;
    const wrapped = doc.splitTextToSize(text, maxWidth);
    const blockHeight = wrapped.length * lineHeight;

    if (y + blockHeight > pageHeight - 50) {
      doc.addPage();
      y = 50;
    }

    doc.text(wrapped, x, y);
    return y + blockHeight + 10;
  };

  const toRoman = (num) => {
    const roman = {
      M: 1000, CM: 900, D: 500, CD: 400, C: 100, XC: 90,
      L: 50, XL: 40, X: 10, IX: 9, V: 5, IV: 4, I: 1
    };
    let str = '';
    for (let i of Object.keys(roman)) {
      const q = Math.floor(num / roman[i]);
      num -= q * roman[i];
      str += i.repeat(q);
    }
    return str;
  };

  const handleDownload = () => {
    const doc = generatePDF();
    doc.save('Petition.pdf');
  };

  const resetForm = () => {
    setFormData({
      petitionType: 'individual',
      regarding: '',
      petitioners: '',
      reasons: [''],
      efforts: [''],
      that4: 'the issues in respect of which this Petition is raised are not pending before any court of law or any constitutional or legal body.',
      prayers: [''],
      day: new Date().getDate(),
      month: new Date().toLocaleString('default', { month: 'long' }),
      year: new Date().getFullYear().toString().slice(-2),
      indName: '',
      indSign: '',
      indContact: '',
      groupPetitioners: [
        { name: '', id: '', contact: '', signature: '' }
      ]
    });
    setCurrentStep(1);
    setSignedPetition(null);
    setDocumentUrl('');
  };

  const handleSubmit = async () => {
    if (!signedPetition) {
      setUploadError('Please select a file to upload');
      toast.error('Please select a file to upload');
      return;
    }

    try {
      setIsUploading(true);
      const uploadResponse = await handleFileUpload(signedPetition);

      // Show success toast
      toast.success('Petition submitted successfully!');

      // Reset form and go back to step 1 after a short delay
      setTimeout(() => {
        resetForm();
        setShowSubmitDialog(false);
      }, 2000);

      return uploadResponse;
    } catch (error) {
      // Error toast will be shown by handleFileUpload
      console.error('Submission error:', error);
      // The form data remains intact on error
    } finally {
      setIsUploading(false);
    }
  };

  const convertPdfToDocx = async (file) => {
    if (!file || !file.type.includes('pdf')) {
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

  const handleFileUpload = async (file) => {
    if (!file) return;

    const token = getToken();
    if (!token) {
      const errorMsg = 'Authentication required. Please log in again.';
      setUploadError(errorMsg);
      toast.error(errorMsg);
      return Promise.reject(new Error(errorMsg));
    }

    // Get clientId from localStorage
    const clientId = localStorage.getItem('id');
    if (!clientId) {
      const errorMsg = 'Client ID not found. Please log in again.';
      setUploadError(errorMsg);
      toast.error(errorMsg);
      return Promise.reject(new Error(errorMsg));
    }

    setUploadError(null);

    try {
      // Comment out PDF to DOCX conversion as requested
      // const fileToUpload = await convertPdfToDocx(file);

      const formDataObj = new FormData();
      formDataObj.append('Files', file); // Use 'Files' to match the command property
      formDataObj.append('Name', file.name); // File name
      formDataObj.append('Description', formData.regarding || 'NEW-PETITION'); // Description from regarding field
      formDataObj.append('CategoryId', config.PETITION_CATEGORY_ID); // Category ID from config
      formDataObj.append('ClientId', clientId); // Client ID from localStorage
      formDataObj.append('Extension', file.name.substring(file.name.lastIndexOf('.'))); // File extension
      formDataObj.append('StorageSettingId', '00000000-0000-0000-0000-000000000000'); // Empty GUID for default local storage

      const response = await axios.post(
        `${config.api.baseURL}/ClientDocument`,
        formDataObj,
        {
          headers: {
            'Content-Type': 'multipart/form-data',
            'Authorization': `Bearer ${token}`
          },
        }
      );

      // setDocumentUrl(response.data.url); // API might not return url in the same format, but keeping if needed or can be removed if unused
      return response.data;
    } catch (error) {
      console.error('Error processing document:', error);
      const errorMsg = error.response?.data?.message ||
        error.message ||
        'Failed to process document. Please try again.';
      setUploadError(errorMsg);
      toast.error(errorMsg);
      throw error;
    }
  };
  // Update PDF when form changes
  useEffect(() => {
    if (showPreview) {
      generatePDF();
    }
  }, [formData, showPreview]);

  // Render step content
  const renderStepContent = (step) => {
    switch (step) {
      case 1:
        return (
          <Card>
            <h3>Type of Petition</h3>
            <RadioGroup>
              <label>
                <input
                  type="radio"
                  name="petitionType"
                  value="individual"
                  checked={formData.petitionType === 'individual'}
                  onChange={handleChange}
                />
                Individual Petition
              </label>
              <label>
                <input
                  type="radio"
                  name="petitionType"
                  value="group"
                  checked={formData.petitionType === 'group'}
                  onChange={handleChange}
                />
                Group Petition
              </label>
            </RadioGroup>
          </Card>
        );
      case 2:
        return (
          <>
            <Card>
              <h3>Petition Details</h3>
              <FormGroup>
                <label>Regarding (Subject of the Petition) *</label>
                <StyledInput
                  type="text"
                  name="regarding"
                  value={formData.regarding}
                  onChange={handleChange}
                  placeholder="Enter the subject of your petition..."
                  style={errors.regarding ? { borderColor: '#dc3545' } : {}}
                />
                {errors.regarding && (
                  <div style={{ color: '#dc3545', fontSize: '0.8em', marginTop: '5px' }}>
                    {errors.regarding}
                  </div>
                )}
              </FormGroup>
              <FormGroup>
                <label>Here, identify in general terms, who the petitioner or petitioners are,
                  for example, citizens of Kenya, residents of county or region, workers of
                  industry, etc. *</label>
                <StyledTextarea
                  name="petitioners"
                  value={formData.petitioners}
                  onChange={handleChange}
                  placeholder="Enter petitioners' information"
                  required
                />
              </FormGroup>
            </Card>
          </>
        );
      case 3:
        return (
          <Card>
            <h3>Reasons for Petition</h3>
            <p style={{ color: '#6c757d', marginTop: '-15px', marginBottom: '15px', fontSize: '0.9em' }}>
              Here, briefly state the reasons underlying the request for the intervention of the House by outlining the grievances or problems by summarizing the facts which the petitioner or petitioners wish the House to consider.
            </p>
            {formData.reasons.map((reason, index) => (
              <FormGroup key={`reason-${index}`}>
                <div style={{ display: 'flex', gap: '10px', marginBottom: '5px' }}>
                  <span>Reason {index + 1}</span>
                  {formData.reasons.length > 1 && (
                    <button
                      onClick={() => removeReason(index)}
                      style={{
                        background: '#dc3545',
                        color: 'white',
                        border: 'none',
                        borderRadius: '4px',
                        width: '24px',
                        height: '24px',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        cursor: 'pointer',
                        flexShrink: 0,
                        fontSize: '12px',
                        padding: 0
                      }}
                      title="Remove reason"
                    >
                      ×
                    </button>
                  )}
                </div>
                <StyledTextarea
                  value={reason}
                  onChange={(e) => handleReasonChange(index, e.target.value)}
                  placeholder={`Enter reason ${index + 1}...`}
                  style={{ minHeight: '80px' }}
                />
              </FormGroup>
            ))}
            <ButtonGroup>
              <ActionButton onClick={addReason} className="primary">
                + Add Another Reason
              </ActionButton>
            </ButtonGroup>
          </Card>
        );
      case 4:
        return (
          <Card>
            <h3>Efforts Made</h3>
            <p style={{ color: '#6c757d', marginTop: '-15px', marginBottom: '15px', fontSize: '0.9em' }}>
              Here confirm that efforts have been made to have the matter addressed by the relevant body, and it failed to give satisfactory response.
            </p>
            {formData.efforts.map((effort, index) => (
              <FormGroup key={`effort-${index}`}>
                <div style={{ display: 'flex', gap: '10px', marginBottom: '5px' }}>
                  <span>Effort {index + 1}</span>
                  {formData.efforts.length > 1 && (
                    <button
                      onClick={() => removeEffort(index)}
                      style={{
                        background: '#dc3545',
                        color: 'white',
                        border: 'none',
                        borderRadius: '4px',
                        width: '24px',
                        height: '24px',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        cursor: 'pointer',
                        flexShrink: 0,
                        fontSize: '12px',
                        padding: 0
                      }}
                      title="Remove effort"
                    >
                      ×
                    </button>
                  )}
                </div>
                <StyledTextarea
                  value={effort}
                  onChange={(e) => handleEffortChange(index, e.target.value)}
                  placeholder={`Describe effort ${index + 1}...`}
                  style={{ minHeight: '80px' }}
                />
              </FormGroup>
            ))}
            <ButtonGroup>
              <ActionButton onClick={addEffort} className="primary">
                + Add Another Effort
              </ActionButton>
            </ButtonGroup>
          </Card>
        );
      case 5:
        return (
          <>
            <Card>
              <h3>Court Details</h3>
              <p style={{ color: '#6c757d', marginTop: '-15px', marginBottom: '15px', fontSize: '0.9em' }}>
                Here confirm that the issues in respect of which the petition is made are not pending before any court of law, or constitutional or legal body.
              </p>
              <FormGroup>
                <StyledTextarea
                  name="that4"
                  value={formData.that4}
                  onChange={handleChange}
                  style={{ minHeight: '100px' }}
                />
              </FormGroup>
            </Card>
            <Card>
              <h3>Prayers</h3>
              <p style={{ color: '#6c757d', marginTop: '-15px', marginBottom: '15px', fontSize: '0.9em' }}>
                Here, set out the prayer by stating in summary what action the petitioners wish Parliament to take or refrain from.
              </p>
              {formData.prayers.map((prayer, index) => (
                <div key={index} style={{ marginBottom: '10px', position: 'relative' }}>
                  <div style={{ display: 'flex', gap: '10px', marginBottom: '5px' }}>
                    <span>{index + 1}.</span>
                    <StyledTextarea
                      value={prayer}
                      onChange={(e) => handlePrayerChange(index, e.target.value)}
                      placeholder={`Enter prayer point ${index + 1}...`}
                      style={{ flex: 1, minHeight: '80px' }}
                    />
                    {formData.prayers.length > 1 && (
                      <button
                        onClick={() => removePrayer(index)}
                        style={{
                          background: '#dc3545',
                          color: 'white',
                          border: 'none',
                          borderRadius: '4px',
                          width: '30px',
                          height: '30px',
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'center',
                          cursor: 'pointer',
                          flexShrink: 0
                        }}
                        title="Remove prayer"
                      >
                        ×
                      </button>
                    )}
                  </div>
                </div>
              ))}
              <button
                onClick={addPrayer}
                style={{
                  background: '#28a745',
                  color: 'white',
                  border: 'none',
                  borderRadius: '4px',
                  padding: '8px 16px',
                  cursor: 'pointer',
                  marginTop: '10px',
                  display: 'flex',
                  alignItems: 'center',
                  gap: '8px'
                }}
              >
                + Add Prayer
              </button>
            </Card>
          </>
        );
      case 6:
        return (
          <Card>
            <h3>Date & Petitioner Information</h3>
            <FormGroup>
              <label>Date of Petition</label>
              <div style={{ display: 'flex', gap: '10px' }}>
                <StyledInput
                  type="number"
                  name="day"
                  value={formData.day}
                  onChange={handleDateChange}
                  min="1"
                  max="31"
                  style={{ width: '60px' }}
                />
                <StyledInput
                  type="text"
                  name="month"
                  value={formData.month}
                  onChange={handleDateChange}
                  style={{ width: '120px' }}
                />
                <span style={{ lineHeight: '40px' }}>20</span>
                <StyledInput
                  type="number"
                  name="year"
                  value={formData.year}
                  onChange={handleDateChange}
                  min="0"
                  max="99"
                  style={{ width: '60px' }}
                />
              </div>
            </FormGroup>

            {formData.petitionType === 'individual' ? (
              <>
                <FormGroup>
                  <label>Your Full Name</label>
                  <StyledInput
                    type="text"
                    name="indName"
                    value={formData.indName}
                    onChange={handleChange}
                    placeholder="Enter your full name"
                    required
                  />
                </FormGroup>
                <FormGroup>
                  <label>Your Signature</label>
                  <StyledInput
                    type="text"
                    name="indSign"
                    value={formData.indSign}
                    readOnly
                    style={{ backgroundColor: '#f5f5f5', cursor: 'not-allowed' }}
                    placeholder="Signature will be added here"
                  />
                </FormGroup>
                <FormGroup>
                  <label>Contact Information</label>
                  <StyledInput
                    type="text"
                    name="indContact"
                    value={formData.indContact}
                    onChange={handleChange}
                    placeholder="Phone/Email"
                    required
                  />
                </FormGroup>
              </>
            ) : (
              <>
                <h4>Group Petitioners</h4>
                {formData.groupPetitioners.map((petitioner, index) => (
                  <div key={index} style={{ marginBottom: '20px', padding: '15px', border: '1px solid #eee', borderRadius: '5px' }}>
                    <FormGroup>
                      <label>Petitioner {index + 1} Name</label>
                      <StyledInput
                        type="text"
                        value={petitioner.name}
                        onChange={(e) => handleGroupPetitionerChange(index, 'name', e.target.value)}
                        placeholder="Full name"
                        required
                      />
                    </FormGroup>
                    <FormGroup>
                      <label>ID/Passport Number</label>
                      <StyledInput
                        type="text"
                        value={petitioner.id}
                        onChange={(e) => handleGroupPetitionerChange(index, 'id', e.target.value)}
                        placeholder="ID/Passport number"
                      />
                    </FormGroup>
                    <FormGroup>
                      <label>Contact Information</label>
                      <StyledInput
                        type="text"
                        value={petitioner.contact}
                        onChange={(e) => handleGroupPetitionerChange(index, 'contact', e.target.value)}
                        placeholder="Phone/Email"
                        required
                      />
                    </FormGroup>
                    <FormGroup>
                      <label>Signature</label>
                      <StyledInput
                        type="text"
                        value={petitioner.signature}
                        readOnly
                        style={{ backgroundColor: '#f5f5f5', cursor: 'not-allowed' }}
                        placeholder="Signature will be added here"
                      />
                    </FormGroup>
                    {formData.groupPetitioners.length > 1 && (
                      <button
                        type="button"
                        onClick={() => removePetitioner(index)}
                        style={{ backgroundColor: '#dc3545' }}
                      >
                        Remove Petitioner
                      </button>
                    )}
                  </div>
                ))}
                <button
                  type="button"
                  onClick={addPetitioner}
                  style={{ marginTop: '10px' }}
                >
                  Add Another Petitioner
                </button>
              </>
            )}
          </Card>
        );
      case 7:
        return (
          <Card>
            <h3>Review Your Petition</h3>
            <p>Please review all the information before submitting.</p>
            <FormGroup>
              <label>Petition Type:</label>
              <p>{formData.petitionType === 'individual' ? 'Individual' : 'Group'} Petition</p>
            </FormGroup>
            <FormGroup>
              <label>Regarding:</label>
              <p>{formData.regarding}</p>
            </FormGroup>
            <FormGroup>
              <label>Petitioners:</label>
              <p>{formData.petitioners}</p>
            </FormGroup>
            <FormGroup>
              <label>Date:</label>
              <p>{`Dated this ${formData.day} day of ${formData.month} 20${formData.year}`}</p>
            </FormGroup>
            <FormGroup>
              <label>Reasons:</label>
              <ul>
                {formData.reasons.map((reason, index) => (
                  <li key={index} style={{ marginBottom: '10px' }}>{reason}</li>
                ))}
              </ul>
            </FormGroup>
            <FormGroup>
              <label>Efforts Made:</label>
              <ul>
                {formData.efforts.map((effort, index) => (
                  <li key={index} style={{ marginBottom: '10px' }}>{effort}</li>
                ))}
              </ul>
            </FormGroup>
            <FormGroup>
              <label>Court Details:</label>
              <p>{formData.that4}</p>
            </FormGroup>
            <FormGroup>
              <label>Prayers:</label>
              <ol>
                {formData.prayers.map((prayer, index) => (
                  <li key={index} style={{ marginBottom: '10px' }}>{prayer}</li>
                ))}
              </ol>
            </FormGroup>
            {/* <div style={{ marginTop: '30px', display: 'flex', justifyContent: 'flex-end' }}>
              <PreviewButton 
                onClick={() => setShowFullPreview(true)}
                style={{ width: 'auto', padding: '10px 24px' }}
              >
                Preview Full Document
              </PreviewButton>
            </div> */}
          </Card>
        );
      default:
        return null;
    }
  };

  const renderFullPreview = () => (
    <FullPreviewDialog>
      <PreviewToolbar>
        <h3>Document Preview</h3>
        <ActionButtons>
          <SecondaryButton onClick={() => setShowFullPreview(false)}>
            Back to Edit
          </SecondaryButton>
          <PrimaryButton onClick={handleDownload}>
            Download PDF
          </PrimaryButton>
          <PreviewButton
            onClick={() => {
              setShowFullPreview(false);
              setShowSubmitDialog(true);
            }}
          >
            Proceed to Submit
          </PreviewButton>
        </ActionButtons>
      </PreviewToolbar>
      <PreviewContent>
        <iframe
          src={pdfUrl}
          style={{ width: '100%', height: '100%', border: 'none' }}
          title="PDF Preview"
        />
      </PreviewContent>
    </FullPreviewDialog>
  );

  const renderSubmitDialog = () => (
    <>
      <Overlay onClick={() => !isUploading && setShowSubmitDialog(false)} />
      <SubmitDialog>
        <h3 style={{ marginTop: 0, color: '#2c3e50' }}>Upload Signed Petition</h3>
        <p style={{ marginBottom: '20px', color: '#6c757d' }}>Please upload the signed copy of your petition.</p>

        <div style={{
          border: '2px dashed #ced4da',
          borderRadius: '8px',
          padding: '30px',
          textAlign: 'center',
          marginBottom: '20px',
          backgroundColor: isUploading ? '#f8f9fa' : '#ffffff',
          opacity: isUploading ? 0.7 : 1,
          position: 'relative'
        }}>
          {isUploading && (
            <div style={{
              position: 'absolute',
              top: 0,
              left: 0,
              right: 0,
              bottom: 0,
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              backgroundColor: 'rgba(255, 255, 255, 0.8)'
            }}>
              <FaSpinner className="fa-spin" style={{ fontSize: '24px', color: '#007bff' }} />
              <span style={{ marginLeft: '10px' }}>Uploading...</span>
            </div>
          )}
          <input
            type="file"
            accept=".pdf,.jpg,.jpeg,.png"
            onChange={(e) => {
              if (e.target.files[0]) {
                setSignedPetition(e.target.files[0]);
                setUploadError(null);
              }
            }}
            id="file-upload"
            disabled={isUploading}
            style={{ display: 'none' }}
          />
          <label
            htmlFor="file-upload"
            style={{
              display: 'block',
              cursor: isUploading ? 'not-allowed' : 'pointer',
              padding: '15px',
              borderRadius: '4px',
              backgroundColor: isUploading ? '#e9ecef' : '#f8f9fa',
              color: isUploading ? '#6c757d' : '#495057',
              transition: 'all 0.2s',
              border: '1px solid #ced4da',
              pointerEvents: isUploading ? 'none' : 'auto'
            }}
          >
            {signedPetition ?
              `Selected: ${signedPetition.name}` :
              'Click to select file or drag and drop'}
          </label>
          {uploadError && (
            <div style={{ color: '#dc3545', marginTop: '10px', fontSize: '14px' }}>
              {uploadError}
            </div>
          )}
        </div>

        <div style={{
          margin: '20px 0',
          padding: '15px',
          backgroundColor: '#fff3cd',
          borderRadius: '6px',
          borderLeft: '4px solid #ffc107',
          color: '#856404'
        }}>
          <strong>⚠️ Important:</strong> Once you submit, you won't be able to make further changes to this petition.
        </div>

        <ActionButtons style={{ justifyContent: 'flex-end', marginTop: '25px' }}>
          <SecondaryButton
            onClick={() => !isUploading && setShowSubmitDialog(false)}
            style={{
              padding: '10px 20px',
              backgroundColor: '#6c757d',
              color: 'white',
              border: 'none',
              borderRadius: '4px',
              cursor: isUploading ? 'not-allowed' : 'pointer',
              opacity: isUploading ? 0.7 : 1,
              pointerEvents: isUploading ? 'none' : 'auto'
            }}
            disabled={isUploading}
          >
            Cancel
          </SecondaryButton>
          <button
            onClick={handleSubmit}
            disabled={!signedPetition || isUploading}
            style={{
              padding: '10px 20px',
              backgroundColor: signedPetition && !isUploading ? '#28a745' : '#6c757d',
              color: 'white',
              border: 'none',
              borderRadius: '4px',
              cursor: signedPetition && !isUploading ? 'pointer' : 'not-allowed',
              opacity: isUploading ? 0.7 : 1,
              display: 'flex',
              alignItems: 'center',
              gap: '8px'
            }}
          >
            {isUploading ? (
              <>
                <FaSpinner className="fa-spin" />
                Uploading...
              </>
            ) : 'Upload Document'}
          </button>
        </ActionButtons>
      </SubmitDialog>
    </>
  );

  return (
    <>
      <ToastContainer
        position="top-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
      />
      <Container>
        <FormSection>
          <FormHeader>
            <h4>Create New Petition</h4>
            <div style={{ display: 'flex', gap: '10px' }}>
              <PreviewButton onClick={() => setShowPreview(!showPreview)}>
                {showPreview ? 'Hide Preview' : 'Show Preview'}
              </PreviewButton>
            </div>
          </FormHeader>

          {/* Progress Steps */}
          <div style={{ marginBottom: '20px', overflowX: 'auto' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', minWidth: '100%' }}>
              {steps.map((step, index) => (
                <div
                  key={step.id}
                  style={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    flex: 1,
                    position: 'relative',
                    minWidth: '100px'
                  }}
                >
                  <div
                    style={{
                      width: '30px',
                      height: '30px',
                      borderRadius: '50%',
                      backgroundColor: currentStep >= step.id ? '#4a6da7' : '#e0e0e0',
                      color: currentStep >= step.id ? 'white' : '#666',
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      marginBottom: '5px',
                      fontWeight: 'bold',
                      fontSize: '14px'
                    }}
                  >
                    {step.id}
                  </div>
                  <div
                    style={{
                      fontSize: '12px',
                      textAlign: 'center',
                      color: currentStep >= step.id ? '#4a6da7' : '#666',
                      fontWeight: currentStep === step.id ? 'bold' : 'normal'
                    }}
                  >
                    {step.title}
                  </div>
                  {index < steps.length - 1 && (
                    <div
                      style={{
                        position: 'absolute',
                        top: '15px',
                        left: 'calc(50% + 15px)',
                        right: 'calc(-50% + 15px)',
                        height: '2px',
                        backgroundColor: currentStep > step.id ? '#4a6da7' : '#e0e0e0',
                        zIndex: -1
                      }}
                    />
                  )}
                </div>
              ))}
            </div>
          </div>

          {/* Form Content */}
          {renderStepContent(currentStep)}

          {/* Navigation Buttons */}
          <div style={{
            display: 'flex',
            justifyContent: 'space-between',
            marginTop: '20px',
            padding: '15px 0',
            borderTop: '1px solid #e0e0e0'
          }}>
            <ActionButton
              onClick={prevStep}
              disabled={currentStep === 1}
              style={{ visibility: currentStep === 1 ? 'hidden' : 'visible' }}
            >
              Previous
            </ActionButton>

            {currentStep < steps.length ? (
              <ActionButton
                onClick={nextStep}
                className="primary"
              >
                Next
              </ActionButton>
            ) : (
              <PreviewButton
                onClick={() => setShowFullPreview(true)}
                style={{ width: 'auto', padding: '10px 24px' }}
              >
                Preview Full Document
              </PreviewButton>
            )}
          </div>
        </FormSection>

        {/* Preview Section */}
        <PreviewSection>
          <PreviewToggle onClick={() => setShowPreview(!showPreview)}>
            <FaEye />
            <span>{showPreview ? 'Hide Preview' : 'Show Preview'}</span>
          </PreviewToggle>

          {showPreview ? (
            <PreviewContent>
              <PreviewHeader>
                <h3>Petition Preview</h3>
                <ActionButton onClick={handleDownload}>
                  Generate PDF
                </ActionButton>
              </PreviewHeader>
              <PreviewIframe
                src={pdfUrl}
                title="Petition Preview"
                ref={iframeRef}
              />
            </PreviewContent>
          ) : (
            <PreviewPlaceholder>
              <FaEye size={48} color="#e0e0e0" />
              <p>Click "Show Preview" to see how your petition will look</p>
            </PreviewPlaceholder>
          )}
        </PreviewSection>
        {showFullPreview && renderFullPreview()}
        {showSubmitDialog && renderSubmitDialog()}
      </Container>
    </>
  );
};

export default CreatePetition;