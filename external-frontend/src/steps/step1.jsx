import React, { useEffect } from 'react';
import { toast } from 'react-toastify';
import countyConstituencies from '../Data/countyConstituencyMap';

export default function Step1({ formData, updateFormData,onNext }) {
  const handlePetitionerTypeChange = (e) => {
    updateFormData({ ...formData, petitionerType: e.target.value });
  };

  useEffect(() => {
    // Reset fields when type changes
    updateFormData((prev) => ({
      ...prev,
      ...(prev.petitionerType === 'individual'
        ? { orgName: '', registrationNumber: '', representativeName: '' }
        : { firstName: '', lastName: '', idNumber: '' }),
    }));
  }, [formData.petitionerType]);

  //Form validation
  const validateAndProceed = () => {
  const errors = [];

  if (!formData.petitionerType) errors.push("Petitioner type is required");

 if (formData.petitionerType === 'individual') {
    if (!formData.firstName) errors.push("First Name is required");
    if (!formData.lastName) errors.push("Last Name is required");
    
    if (!formData.idNumber) {
      errors.push("ID/Passport Number is required");
    } else  {
      const idRegex =/^[1-9][0-9]{6,7}$/;
      const passportRegex = /^[A-Z]{1}[0-9]{8}$/;
      if (!idRegex.test(formData.idNumber) && !passportRegex.test(formData.idNumber)) {
        errors.push("Invalis ID or Passport Number ");
      }
    }
  }

  if (formData.petitionerType === 'organization') {
    if (!formData.orgName) errors.push("Organization Name is required");
    if (!formData.registrationNumber) errors.push("Registration Number is required");
    if (!formData.representativeName) errors.push("Representative's Name is required");
  }

  if (!formData.email) {
    errors.push("Email Address is required");
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
    errors.push("Email Address is invalid");
  }

  if (!formData.phone) {
    errors.push("Phone Number is required");
  } else if (!/^[17]\d{8}$/.test(formData.phone)) {
    errors.push("Phone Number must be 9 digits (after +254) and start with 1 or 7");
  }
  if (!formData.address) errors.push("Physical Address is required");
  if (!formData.county) errors.push("County is required");
  if (!formData.constituency) errors.push("Constituency is required");

  if (errors.length > 0) {
    errors.forEach(err => toast.error(err, { position: "top-right" }));
    return;
  }

  onNext();
};


  return (
    <div>
      <h3 className="mb-4">1. Petitioner Information</h3>

      <div className="mb-3">
        <label className="form-label">You are submitting as:</label>
        <select
          className="form-select"
          value={formData.petitionerType}
          onChange={handlePetitionerTypeChange}
          required
        >
          <option value="" selected disabled>Select petitioner type</option>
          <option value="individual">Individual</option>
          <option value="organization">Organization</option>
          <option value="group">Group of Individuals</option>
        </select>
      </div>

      {/* Individual Fields */}
      {formData.petitionerType === 'individual' && (
        <>
          <div className="row">
            <div className="col-md-6 mb-3">
              <label className="form-label">First Name <span className="text-danger">*</span></label>
              <input
                type="text"
                className="form-control"
                value={formData.firstName}
                onChange={(e) => updateFormData({ ...formData, firstName: e.target.value })}
              />
            </div>
            <div className="col-md-6 mb-3">
              <label className="form-label">Last Name <span className="text-danger">*</span></label>
              <input
                type="text"
                className="form-control"
                value={formData.lastName}
                onChange={(e) => updateFormData({ ...formData, lastName: e.target.value })}
              />
            </div>
          </div>
          <div className="mb-3">
            <label className="form-label">ID/Passport Number <span className="text-danger">*</span></label>
            <input
              type="text"
              className="form-control"
              value={formData.idNumber}
              onChange={(e) => updateFormData({ ...formData, idNumber: e.target.value })}
            />
          </div>
        </>
      )}

      {/* Organization Fields */}
      {formData.petitionerType === 'organization' && (
        <>
          <div className="mb-3">
            <label className="form-label">Organization Name <span className="text-danger">*</span></label>
            <input
              type="text"
              className="form-control"
              value={formData.orgName}
              onChange={(e) => updateFormData({ ...formData, orgName: e.target.value })}
            />
          </div>
          <div className="mb-3">
            <label className="form-label">Registration Number <span className="text-danger">*</span></label>
            <input
              type="text"
              className="form-control"
              value={formData.registrationNumber}
              onChange={(e) => updateFormData({ ...formData, registrationNumber: e.target.value })}
            />
          </div>
          <div className="mb-3">
            <label className="form-label">Representative's Name <span className="text-danger">*</span></label>
            <input
              type="text"
              className="form-control"
              value={formData.representativeName}
              onChange={(e) => updateFormData({ ...formData, representativeName: e.target.value })}
            />
          </div>
        </>
      )}

      {/* Common Fields */}
      <h5 className="mt-4 mb-3">Contact Information</h5>

      <div className="row">
        <div className="col-md-6 mb-3">
          <label className="form-label">Email Address <span className="text-danger">*</span></label>
          <input
            type="email"
            className="form-control"
            value={formData.email}
            onChange={(e) => updateFormData({ ...formData, email: e.target.value })}
          />
        </div>
        <div className="col-md-6 mb-3">
          <label className="form-label">Phone Number <span className="text-danger">*</span></label>
          <div className="input-group">
            <span className="input-group-text">+254</span>
            <input
              type="tel"
              className="form-control"
              value={formData.phone}
              onChange={(e) => updateFormData({ ...formData, phone: e.target.value })}
            />
          </div>
        </div>
      </div>

      <div className="mb-3">
        <label className="form-label">Physical Address <span className="text-danger">*</span></label>
        <textarea
          className="form-control"
          rows="2"
          value={formData.address}
          onChange={(e) => updateFormData({ ...formData, address: e.target.value })}
        />
      </div>

      <div className="row">
        <div className="col-md-6 mb-3">
          <label className="form-label">County <span className="text-danger">*</span></label>
          <select
            className="form-select"
            value={formData.county}
            onChange={(e) => {
              const selectedCounty = e.target.value;
              updateFormData({ ...formData, county: selectedCounty , constituency: '' 

              });
            }}
          >
            <option value="" selected disabled>Select county</option>
            {Object.keys(countyConstituencies).map((county) => (
              <option key={county} value={county}>
                {county}
              </option>
            ))}
          </select>
        </div>

        <div className="col-md-6 mb-3">
          <label className="form-label">Constituency <span className="text-danger">*</span></label>
          <select
            className="form-select"
            value={formData.constituency}
            onChange={(e) => updateFormData({ ...formData, constituency: e.target.value })}
            disabled={!formData.county}
          >
            <option value="" selected disabled>Select constituency</option>
            {formData.county &&
              countyConstituencies[formData.county]?.map((constituency) => (
                <option key={constituency} value={constituency}>
                  {constituency}
                </option>
              ))}
          </select>
        </div>
      </div>

      <div className="d-flex justify-content-end mt-4">
  <button type="button" className="btn btn-success" onClick={validateAndProceed}>
    Next:Petition Details
  </button>
</div>

    </div>
    
  );
}
