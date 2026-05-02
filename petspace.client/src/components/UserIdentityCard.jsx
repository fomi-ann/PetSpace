import React from 'react';

const UserIdentityCard = ({ profile, config, title, onEdit, role }) => {

    if (!profile) return null;
    const displayFields = config || [];

    return (
        <div className="card mb-4 overflow-hidden position-relative">

            <div className="bg-primary p-3">
                <h5 className="text-white m-0 text-center text-uppercase fs-6 fw-bold">
                    {title || "Profile Overview"}
                </h5>
            </div>

            <div className="card-body p-4">
                <div className="row align-items-start">

                    <div className="col-auto">
                        <div
                            className="rounded-circle bg-light d-flex align-items-center justify-content-center"
                            style={{ width: '90px', height: '90px', border: '3px solid #f0f0f0' }}
                        >
                            <span className="h1 mb-0 text-secondary fw-lighter text-uppercase">
                                {profile.userFirstName?.[0]}
                                {profile.userLastName?.[0]}
                            </span>
                        </div>
                    </div>

                    <div className="col text-start">
                        <div className="d-flex align-items-center gap-2 mb-2">
                            <h2 className="mb-0 fw-bold text-dark">
                                {profile.userFirstName} {profile.userLastName}
                            </h2>

                            {role === 'Vet' && profile.isVerified && (
                                <span className="badge bg-success fw-normal" style={{ fontSize: '0.75rem' }}>
                                    Verified
                                </span>
                            )}

                            {role === 'Vet' && !profile.isVerified && profile.clinicName && (
                                <span className="badge bg-warning text-dark fw-normal" style={{ fontSize: '0.75rem' }}>
                                    Pending
                                </span>
                            )}
                        </div>

                        <div className="text-muted mb-2 d-flex align-items-center">
                            <i className="bi bi-envelope me-2 text-primary"></i>
                            <span>{profile.email || 'N/A'}</span>
                        </div>

                        {displayFields.map((field, index) => (
                            <div key={index} className="text-muted mb-1 d-flex align-items-center">
                                {field.icon && (
                                    <i className={`bi bi-${field.icon} me-2 text-primary`}></i>
                                )}
                                <strong className="small text-uppercase me-2">
                                    {field.label}:
                                </strong>
                                <span>{profile[field.name] || 'N/A'}</span>
                            </div>
                        ))}
                    </div>

                </div>

                {onEdit && (
                    <div className="d-flex justify-content-end mt-3">
                        <button
                            className="btn btn-outline-primary btn-sm"
                            onClick={() => onEdit(profile)}
                        >
                            Edit Profile
                        </button>
                    </div>
                )}
            </div>
        </div>
    );
};

export default UserIdentityCard;