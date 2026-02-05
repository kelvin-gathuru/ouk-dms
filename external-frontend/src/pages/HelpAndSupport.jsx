import React from 'react';
import { Container, Card, Accordion } from 'react-bootstrap';

export default function HelpAndSupport() {
  return (
    <Container className="mt-4">
      <Card className="shadow">
        <Card.Body>
          <h2 className="mb-4">Help & Support: Filing a Petition in Kenya</h2>
          
          <Card className="mb-4">
            <Card.Header className="bg-light">
              <h5>Understanding Petitions in Kenya</h5>
            </Card.Header>
            <Card.Body>
              <p>
                A petition is a formal written request, typically signed by many people, appealing to authority in respect of a particular cause. 
                In Kenya, petitions are an important tool for citizens to raise concerns and seek redress from the government or other authorities.
              </p>
            </Card.Body>
          </Card>

          <Accordion defaultActiveKey="0" className="mb-4">
            <Accordion.Item eventKey="0">
              <Accordion.Header>Step 1: Prepare Your Petition</Accordion.Header>
              <Accordion.Body>
                <ul>
                  <li>Clearly state the subject of your petition</li>
                  <li>Provide a brief background of the issue</li>
                  <li>State the specific grievances or concerns</li>
                  <li>Include any supporting documents or evidence</li>
                  <li>Be clear and concise in your writing</li>
                </ul>
              </Accordion.Body>
            </Accordion.Item>

            <Accordion.Item eventKey="1">
              <Accordion.Header>Step 2: Gather Signatures</Accordion.Header>
              <Accordion.Body>
                <ul>
                  <li>Collect signatures from concerned parties</li>
                  <li>Each signature should include full name, ID number, and contact information</li>
                  <li>Ensure all signatories understand the petition's purpose</li>
                  <li>Keep a digital and physical copy of all signatures</li>
                </ul>
              </Accordion.Body>
            </Accordion.Item>

            <Accordion.Item eventKey="2">
              <Accordion.Header>Step 3: Submit Your Petition</Accordion.Header>
              <Accordion.Body>
                <ul>
                  <li>Submit the petition to the relevant authority or office</li>
                  <li>Keep a copy of the submission receipt</li>
                  <li>Follow up on the progress of your petition</li>
                  <li>Be prepared to attend any scheduled hearings or meetings</li>
                </ul>
              </Accordion.Body>
            </Accordion.Item>
          </Accordion>

          <Card className="mb-4">
            <Card.Header className="bg-light">
              <h5>Important Notes</h5>
            </Card.Header>
            <Card.Body>
              <ul>
                <li>Ensure your petition is respectful and constructive</li>
                <li>Be truthful and accurate in your statements</li>
                <li>Keep records of all communications regarding your petition</li>
                <li>Be patient as the process may take time</li>
                <li>Seek legal advice if needed</li>
              </ul>
            </Card.Body>
          </Card>

          <Card>
            <Card.Header className="bg-light">
              <h5>Contact Information</h5>
            </Card.Header>
            <Card.Body>
              <p>For further assistance, please contact:</p>
              <ul>
                <li><strong>Email:</strong> cna@parliament.go.ke </li>
                <li><strong>Phone:</strong> 254 2 2221291 or 2848000 </li>
              </ul>
            </Card.Body>
          </Card>
        </Card.Body>
      </Card>
    </Container>
  );
}
