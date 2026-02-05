import React from 'react';

const steps = [
  { number: 1, label: 'Petitioner' },
  { number: 2, label: 'Petition Details' },
  { number: 3, label: 'Supporting Documents' },
  { number: 4, label: 'Review & Submit' }
];

export default function ProgressSteps({ currentStep }) {
  return (
    <div className="d-flex justify-content-between align-items-center mb-4">
      {steps.map((step, index) => (
        <React.Fragment key={step.number}>
          <div className="text-center">
            <div
              className={`rounded-circle text-white d-inline-flex align-items-center justify-content-center ${
                step.number <= currentStep ? 'bg-success' : 'bg-secondary'
              }`}
              style={{ width: '40px', height: '40px' }}
            >
              {step.number}
            </div>
            <div className="mt-1 small">{step.label}</div>
          </div>

          {index < steps.length - 1 && (
            <div className="flex-grow-1 border-top border-2 mx-3" />
          )}
        </React.Fragment>
      ))}
    </div>
  );
}
