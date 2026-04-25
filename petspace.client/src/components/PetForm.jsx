import { useState, useEffect } from 'react';

const PetForm = ({ onSubmit, initialData = null, buttonText = 'Add Pet' }) => {


    const [formData, setFormData] = useState({
        petName: '',
        petSpecies: '',
        petBreed: '',
        petGender: '',
        petBirthDate: '',
        petMicrochipNr: '',
        petWeight: ''
    });


    useEffect(() => {
        if (!initialData) {
            // ? problem
            setFormData({
                petName: '',
                petSpecies: '',
                petBreed: '',
                petGender: '',
                petBirthDate: '',
                petMicrochipNr: '',
                petWeight: ''
            });
            return;
        }


       
        setFormData({
            petName: initialData.petName || '',
            petSpecies: initialData.species || '',
            petBreed: initialData.breed || '',
            petGender: initialData.gender || '',
            petBirthDate: initialData.birthDate
                ? initialData.birthDate.slice(0, 10)
                : '',
            petMicrochipNr: initialData.microchipNr || '',
            petWeight: initialData.weight || ''
        });
    }, [initialData]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        const preparedData = {
            ...formData,
            petWeight: formData.petWeight ? parseFloat(formData.petWeight) : null,
            petBirthDate: formData.petBirthDate || null
        }

        onSubmit(preparedData);

    };

    return (
        <form onSubmit={handleSubmit} className="mt-4">


            <div className="row">
                <div className="col-md-6 mb-3">
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

                <div className="col-md-6 mb-3">
                    <label className="form-label">Species</label>
                    <input
                        type="text"
                        name="petSpecies"
                        className="form-control"
                        value={formData.petSpecies}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="col-md-6 mb-3">
                    <label className="form-label">Breed</label>
                    <input
                        type="text"
                        name="petBreed"
                        className="form-control"
                        value={formData.petBreed}
                        onChange={handleChange}
                    />
                </div>

                <div className="col-md-6 mb-4 d-flex align-items-end ">
                    <div>
                        <div className="form-check form-check-inline">
                            <input
                                className="form-check-input"
                                type="radio"
                                name="petGender"
                                value="Male"
                                checked={formData.petGender === 'Male'}
                                onChange={handleChange}
                            />
                            <label className="form-check-label">Male</label>
                        </div>

                        <div className="form-check form-check-inline">
                            <input
                                className="form-check-input"
                                type="radio"
                                name="petGender"
                                value="Female"
                                checked={formData.petGender === 'Female'}
                                onChange={handleChange}
                            />
                            <label className="form-check-label">Female</label>
                        </div>
                    </div>
                </div>

                <div className="col-md-6 mb-3">
                    <label className="form-label">Birth Date</label>
                    <input
                        type="date"
                        name="petBirthDate"
                        className="form-control"
                        value={formData.petBirthDate}
                        onChange={handleChange}
                    />
                </div>

                <div className="col-md-6 mb-3">
                    <label className="form-label">Weight (kg)</label>
                    <input
                        type="number"
                        step="0.1"
                        name="petWeight"
                        className="form-control"
                        value={formData.petWeight}
                        onChange={handleChange}
                    />
                </div>

                <div className="col-12 mb-3">
                    <label className="form-label">Microchip Number</label>
                    <input
                        type="text"
                        name="petMicrochipNr"
                        className="form-control"
                        value={formData.petMicrochipNr}
                        onChange={handleChange}
                    />
                </div>

                <div className="col-12">
                    <button type="submit" className="btn btn-primary">
                        { buttonText }
                    </button>
                </div>

            </div>


        </form>
    );
};

export default PetForm;