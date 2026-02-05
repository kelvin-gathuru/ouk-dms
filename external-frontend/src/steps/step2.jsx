import React from 'react';
import { toast } from 'react-toastify';

export default function Step2({ formData, updateFormData,onBack, onNext }) {
  //Form validation
  const validateAndProceed = () => {
    const errors = [];

    if (!formData.petitionTitle) errors.push("Petition Title is required");
    if (!formData.grievanceStatement) errors.push("Grievance Statement is required");
    if (!formData.detailedDescription) errors.push("Detailed Description is required");
    if (!formData.prayer) errors.push("Prayer is required");
    if (!formData.petitionCategory) errors.push("Petition Category is required");
    if (!formData.targetInstitution) errors.push("Target Institution is required");

    if (errors.length > 0) {
      errors.forEach(err => toast.error(err, { position: "top-right" }));
      return;
    }

    onNext();
  };

  return (
    <div>
      <h3 className="mb-4">2. Petition Details</h3>
      <div className="row g-3">
        <div className="col-12">
          <label className="form-label">Title of Petition <span className="text-danger">*</span></label>
          <input
            type="text"
            className="form-control"
            maxLength="200"
            value={formData.petitionTitle}
            onChange={(e) => updateFormData({ ...formData, petitionTitle: e.target.value })}
          />
          <div className="form-text">A clear and concise title (max 200 characters)</div>
        </div>

        <div className="col-12">
          <label className="form-label">Grievance Statement <span className="text-danger">*</span></label>
          <textarea
            className="form-control"
            maxLength="500"
            rows="3"
            value={formData.grievanceStatement}
            onChange={(e) => updateFormData({ ...formData, grievanceStatement: e.target.value })}
          />
          <div className="form-text">Briefly state the grievance (max 500 characters)</div>
        </div>

        <div className="col-12">
          <label className="form-label">Detailed Description <span className="text-danger">*</span></label>
          <textarea
            className="form-control"
            rows="5"
            value={formData.detailedDescription}
            onChange={(e) => updateFormData({ ...formData, detailedDescription: e.target.value })}
          />
          <div className="form-text">Detailed explanation, facts, and context</div>
        </div>

        <div className="col-12">
          <label className="form-label">Efforts Made So Far</label>
          <textarea
            className="form-control"
            rows="3"
            value={formData.effortsMade}
            onChange={(e) => updateFormData({ ...formData, effortsMade: e.target.value })}
          />
          <div className="form-text">Describe prior attempts to resolve the issue</div>
        </div>

        <div className="col-12">
          <label className="form-label">Prayer (What You're Asking For) <span className="text-danger">*</span></label>
          <textarea
            className="form-control"
            rows="3"
            value={formData.prayer}
            onChange={(e) => updateFormData({ ...formData, prayer: e.target.value })}
          />
          <div className="form-text">Clearly state what action you want Parliament to take</div>
        </div>

        <div className="col-md-6">
          <label className="form-label">Petition Category <span className="text-danger">*</span></label>
          <select
            className="form-select"
            value={formData.petitionCategory}
            onChange={(e) => updateFormData({ ...formData, petitionCategory: e.target.value })}
          >
            <option value="" selected disabled>Select a category</option>
            <option value="governance">Governance</option>
            <option value="health">Health</option>
            <option value="education">Education</option>
            <option value="infrastructure">Infrastructure</option>
            <option value="environment">Environment</option>
            <option value="justice">Justice</option>
            <option value="economy">Economy</option>
            <option value="other">Other</option>
          </select>
        </div>

        <div className="col-md-6">
          <label className="form-label">Target Institution <span className="text-danger">*</span></label>
          <input
            type="text"
            className="form-control"
            value={formData.targetInstitution}
            onChange={(e) => updateFormData({ ...formData, targetInstitution: e.target.value })}
          />
          <div className="form-text">e.g., Ministry, Department, or Agency</div>
        </div>
      </div>
      {/* Navigation Buttons */}
<div className="d-flex justify-content-between mt-4">
  <button type="button" className="btn btn-secondary" onClick={onBack}>
    Back
  </button>
  <button type="button" className="btn btn-success" onClick={validateAndProceed}>
    Next:Petition Details
  </button>
</div>

    </div>
  );
}
