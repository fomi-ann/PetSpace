import { useState } from 'react';

const AddPetForm = ({ onAdd }) => {
    const [formData, setFormData] = useState({
        petName: '',
        species: '',
        breed: '',
        birthDate: ''
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onAdd(formData);
        setFormData({
            petName: '',
            species: '',
            breed: '',
            birthDate: ''

        });
    };

    return (
        <form onSubmit={handleSubmit} className="mt-4">
            <div className="mb-3">
                <label className="form-label">Pet Name</label>
                <input
                    type="text"
                    name="petName"
                    className="form-control"
                    value={formData.petName}
                    onChange={handleChange}
                    required
                />
            </div>

            <div className="mb-3">
                <label className="form-label">Species</label>
                <input
                    type="text"
                    name="species"
                    className="form-control"
                    value={formData.species}
                    onChange={handleChange}
                    required

                />
            </div>

            <div className="mb-3">
                <label className="form-label">Breed</label>
                <input
                    type="text"
                    name="breed"
                    className="form-control"
                    value={formData.breed}
                    onChange={handleChange}
                />
            </div>


            <div className="mb-3">
                <label className="form-label">Birth Date</label>
                <input
                    type="date"
                    name="birthDate"
                    className="form-control"
                    value={formData.birthDate}
                    onChange={handleChange}
                />

            </div>


            <button type="submit" className="btn btn-primary">
                Add Pet

            </button>


        </form>
    );
};

export default AddPetForm;