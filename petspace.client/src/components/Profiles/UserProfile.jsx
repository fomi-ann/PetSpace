import React, { useState } from 'react';

const UserProfile = ({ data, onSave }) => {

    const [formData, setFormData] = useState({
        if(data) {
            setFormData({
                firstName: data.userFirstName || '',
                lastName: data.userLastName || '',
                phoneNumber: data.phoneNumber || ''
            });
        }
    }, [data]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSave(formData);
    };

    return (
        <div className="container d-flex align-items-center justify-content-center">
            <div className="card p-5" style={{ width: '420px' }}>

                <h2 className="text-center mb-5">Update profile data</h2>

                <form onSubmit={handleSubmit}>

                    <div className="mb-3">
                        <label className="form-label">First Name</label>
                        <input type="text" name="firstName" className="form-control bg-transparent"
                            value={formData.firstName} onChange={handleChange} />
                    </div>

                    <div className="mb-3">
                        <label className="form-label">Last Name</label>
                        <input type="text" name="lastName" className="form-control bg-transparent"
                            value={formData.lastName} onChange={handleChange} />
                    </div>

                    <div className="mb-3">
                        <label className="form-label">Phone Number</label>
                        <input type="text" name="phoneNumber" className="form-control bg-transparent"
                            value={formData.phoneNumber} onChange={handleChange} />
                    </div>

                    <button type="submit" className="btn btn-primary w-100">Save Changes</button>

                </form>
            </div>
        </div>
    );
};

export default UserProfile;