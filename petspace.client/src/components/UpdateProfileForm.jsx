import { useState } from 'react';

const emptyForm = {
    firstName: '',
    lastName: '',
    phoneNumber: '',
    clinicName: '',
    address: '',
    phone: '',
    specialization: '',
    licence: ''
};

const UpdateProfileForm = ({ fields, initialData = null, onSave, onCancel }) => {
    const [formData, setFormData] = useState(() => ({
        ...emptyForm,
        firstName: initialData?.userFirstName || initialData?.firstName || '',
        lastName: initialData?.userLastName || initialData?.lastName || '',
        phoneNumber: initialData?.phoneNumber || '',
        clinicName: initialData?.clinicName || '',
        address: initialData?.address || '',
        phone: initialData?.phone || '',
        specialization: initialData?.specialization || '',
        licence: initialData?.licence || ''
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
        setFormData(emptyForm);
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

            <div className="d-flex gap-2">
                <button type="submit" className="btn btn-primary">
                    Save Changes
                </button>

                {onCancel && (
                    <button
                        type="button"
                        className="btn btn-outline-secondary"
                        onClick={() => {
                            setFormData({
                                firstName: '',
                                lastName: '',
                                phoneNumber: '',
                                clinicName: '',
                                address: '',
                                phone: '',
                                specialization: '',
                                licence: ''
                            });
                            onCancel();
                        }}
                    >
                        Cancel
                    </button>
                )}
            </div>
        </form>
    );
};

export default UpdateProfileForm;