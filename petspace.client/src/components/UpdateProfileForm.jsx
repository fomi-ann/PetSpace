import { useState } from 'react';

const UpdateProfileForm = ({ fields, profile, onSave }) => {
    const [formData, setFormData] = useState(() => ({
        firstName: profile?.userFirstName || '',
        lastName: profile?.userLastName || '',
        phoneNumber: profile?.phoneNumber || '',
        clinicName: profile?.clinicName || '',
        clinicAddress: profile?.address || '',
        clinicPhone: profile?.phone || '',
        vetSpecialization: profile?.specialization || '',
        vetLicence: profile?.licence || ''
    }));

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSave(formData);
    };

    return (
        <form onSubmit={handleSubmit}>
            {fields.map(field => (
                <div className="mb-3" key={field.name}>
                    <label className="form-label">{field.label}</label>
                    <input
                        type="text"
                        name={field.name}
                        className="form-control"
                        placeholder={field.placeholder || ''}
                        value={formData[field.name] || ''}
                        onChange={handleChange}
                    />
                </div>
            ))}

            <button type="submit" className="btn btn-primary w-100">
                Save Changes
            </button>
        </form>
    );
};

export default UpdateProfileForm;