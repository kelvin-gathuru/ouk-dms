import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { fetchPetitions, getPetitionStatus, calculateDueDate, formatDate } from '../services/petitionService';
import { FaSpinner, FaEye } from 'react-icons/fa';
import config from '../config';

const Container = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: 30px 20px;
`;

const Header = styled.div`
  margin-bottom: 30px;
  text-align: center;
`;

const Title = styled.h1`
  color: #2c3e50;
  margin-bottom: 10px;
`;

const Subtitle = styled.p`
  color: #7f8c8d;
  font-size: 16px;
`;

const SearchContainer = styled.div`
  margin: 20px 0;
  display: flex;
  gap: 10px;
`;

const SearchInput = styled.input`
  flex: 1;
  padding: 10px 15px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 16px;
  max-width: 500px;
  margin: 0 auto;
  display: block;
`;

const TableContainer = styled.div`
  overflow-x: auto;
  margin-top: 20px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  border-radius: 8px;
`;

const Table = styled.table`
  width: 100%;
  border-collapse: collapse;
  min-width: 1000px;
`;

const Th = styled.th`
  background-color: var(--bs-success);
  color: white;
  padding: 15px;
  text-align: left;
  font-weight: 600;
  &:first-child { border-top-left-radius: 8px; }
  &:last-child { border-top-right-radius: 8px; }
`;

const Td = styled.td`
  padding: 15px;
  border-bottom: 1px solid #eee;
  vertical-align: middle;
`;

const Tr = styled.tr`
  &:hover {
    background-color: #f8f9fa;
  }
  &:last-child td {
    border-bottom: none;
  }
`;

const StatusBadge = styled.span`
  display: inline-block;
  padding: 5px 10px;
  border-radius: 15px;
  font-size: 12px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  background-color: ${props => {
    switch (props.status) {
      case 'SUBMITTED': return '#e3f2fd';
      case 'UNDER_REVIEW': return '#fff3e0';
      case 'RESOLVED': return '#e8f5e9';
      case 'REJECTED': return '#ffebee';
      default: return '#f5f5f5';
    }
  }};
  color: ${props => {
    switch (props.status) {
      case 'SUBMITTED': return '#0d47a1';
      case 'UNDER_REVIEW': return '#e65100';
      case 'RESOLVED': return '#2e7d32';
      case 'REJECTED': return '#c62828';
      default: return '#212121';
    }
  }};
`;

const LoadingContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 40px;
  color: #666;
`;

const ErrorMessage = styled.div`
  background-color: #ffebee;
  color: #c62828;
  padding: 15px;
  border-radius: 4px;
  margin: 20px 0;
  text-align: center;
`;

const ViewButton = styled.button`
  background-color: #3498db;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 5px;
  font-size: 14px;
  transition: background-color 0.2s;

  &:hover {
    background-color: #2980b9;
  }
`;

const PetitionTracker = () => {
  const [petitions, setPetitions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    const loadPetitions = async () => {
      try {
        setLoading(true);
        const data = await fetchPetitions();
        // Handle both array and object with data property
        const petitionsData = Array.isArray(data) ? data : (data.data || []);
        setPetitions(petitionsData);
      } catch (err) {
        console.error('Failed to load petitions:', err);
        setError('Failed to load petitions. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    loadPetitions();
  }, []);

  const getPetitionerName = (petition) => {
    if (petition.client) {
      return petition.client.companyName || petition.client.contactPerson || 'Unknown';
    }
    return 'Unknown';
  };

  const getPetitionerId = (petition) => {
    if (petition.client) {
      return petition.client.id;
    }
    return '';
  }

  const filteredPetitions = petitions.filter(petition => {
    const searchLower = searchTerm.toLowerCase();
    const regarding = petition.description || '';
    const petitioner = getPetitionerName(petition);
    const petitionNo = petition.documentNumber || ''; // Use documentNumber as Petition No.
    const status = petition.documentStatus || 'SUBMITTED';

    return (
      regarding.toLowerCase().includes(searchLower) ||
      petitioner.toLowerCase().includes(searchLower) ||
      petitionNo.toLowerCase().includes(searchLower) ||
      status.toLowerCase().includes(searchLower)
    );
  });

  const handleView = (petition) => {
    const downloadUrl = `${config.api.baseURL}/Documents/public/petitions/${petition.id}/download`;
    window.open(downloadUrl, '_blank');
  };

  if (loading) {
    return (
      <Container>
        <LoadingContainer>
          <FaSpinner className="fa-spin" style={{ marginRight: '10px' }} />
          Loading petitions...
        </LoadingContainer>
      </Container>
    );
  }

  return (
    <Container>
      <Header>
        <Title>Petitions Tracker</Title>
        <Subtitle>Track the status of all submitted petitions</Subtitle>
      </Header>

      <SearchContainer>
        <SearchInput
          type="text"
          placeholder="Search petitions..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </SearchContainer>

      {error && <ErrorMessage>{error}</ErrorMessage>}

      <TableContainer>
        <Table>
          <thead>
            <tr>
              <Th style={{ width: '5%' }}>No.</Th>
              <Th style={{ width: '20%' }}>Petitioner</Th>
              <Th style={{ width: '25%' }}>Subject Matter</Th>
              <Th style={{ width: '15%' }}>Petition No.</Th>
              <Th style={{ width: '10%' }}>Date Presented</Th>
              <Th style={{ width: '10%' }}>Date Due</Th>
              <Th style={{ width: '10%' }}>Status</Th>
              <Th style={{ width: '5%' }}>Action</Th>
            </tr>
          </thead>
          <tbody>
            {filteredPetitions.length > 0 ? (
              filteredPetitions.map((petition, index) => (
                <Tr key={petition.id}>
                  <Td style={{ backgroundColor: 'var(--bs-success)', color: 'white' }}>{index + 1}</Td>
                  <Td>
                    {getPetitionerName(petition)}
                    {/* <div style={{ fontSize: '0.8em', color: '#666' }}>
                      ID: {getPetitionerId(petition)}
                    </div> */}
                  </Td>
                  <Td>{petition.description || 'N/A'}</Td>
                  <Td>{petition.documentNumber || petition.name}</Td>
                  <Td>{formatDate(petition.createdDate)}</Td>
                  <Td>{calculateDueDate(petition.createdDate)}</Td>
                  <Td>
                    <StatusBadge status={petition.documentStatus || 'SUBMITTED'}>
                      {getPetitionStatus(petition.documentStatus || 'SUBMITTED')}
                    </StatusBadge>
                  </Td>
                  <Td>
                    <ViewButton onClick={() => handleView(petition)}>
                      <FaEye /> View
                    </ViewButton>
                  </Td>
                </Tr>
              ))
            ) : (
              <Tr>
                <Td colSpan="8" style={{ textAlign: 'center', padding: '30px' }}>
                  {searchTerm ? 'No matching petitions found' : 'No petitions available'}
                </Td>
              </Tr>
            )}
          </tbody>
        </Table>
      </TableContainer>
    </Container>
  );
};

export default PetitionTracker;
