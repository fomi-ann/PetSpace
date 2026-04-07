import React, { useState } from 'react';

const UserUpdateForm = ({ initialData, onSave }) => {
    const [formData, setFormData] = useState({
        firstName: initialData.userFirstName || '',
        lastName: initialData.userLastName || '',
        phoneNumber: initialData.phoneNumber || ''
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    return (


        <form onSubmit={(e) => { e.preventDefault(); onSave(formData); }}>
            <div className="mb-3">
                <label className="form-label">First Name</label>
                <input
                    name="firstName"
                    className="form-control"
                    value={formData.firstName}
                    onChange={handleChange}
                />
            </div>
            <div className="mb-3">
                <label className="form-label">Last Name</label>
                <input
                    name="lastName"
                    className="form-control"
                    value={formData.lastName}
                    onChange={handleChange}
                />
            </div>
            <div className="mb-3">
                <label className="form-label">Phone Number</label>
                <input
                    name="phoneNumber"
                    className="form-control"
                    value={formData.phoneNumber}
                    onChange={handleChange}
                />
            </div>
            <button type="submit" className="btn btn-primary w-100">Save Changes</button>
            </form>

    );
};

export default UserUpdateForm;