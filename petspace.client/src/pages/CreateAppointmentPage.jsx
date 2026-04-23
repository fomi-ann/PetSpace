import { useEffect, useState } from 'react';

const CreateAppointmentPage = () => {
    const token = localStorage.getItem('token');

    const [pets, setPets] = useState([]);
    const [vets, setVets] = useState([]);
    const [clinics, setClinics] = useState([]);

    const [formData, setFormData] = useState({
        petId: '',
        vetId: '',
        clinicId: '',
        appDateTime: '',
        appReason: ''
    });

    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchPets = async () => {
            try {
                const response = await fetch('/api/pets', {
                    headers: { Authorization: `Bearer ${token}` }
                });

                if (response.ok) {
                    const data = await response.json();
                    setPets(data);
                }
            } catch (err) {
                console.error('Pets fetch error:', err);
            }
        };

        const fetchVets = async () => {
            try {
                const response = await fetch('/api/vets', {
                    headers: { Authorization: `Bearer ${token}` }
                });

                if (response.ok) {
                    const data = await response.json();
                    setVets(data);
                }
            } catch (err) {
                console.error('Vets fetch error:', err);
            }
        };

        const fetchClinics = async () => {
            try {
                const response = await fetch('/api/clinics', {
                    headers: { Authorization: `Bearer ${token}` }
                });

                if (response.ok) {
                    const data = await response.json();
                    setClinics(data);
                }
            } catch (err) {
                console.error('Clinics fetch error:', err);
            }
        };

        fetchPets();
        fetchVets();
        fetchClinics();
    }, [token]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage('');
        setError('');

        try {
            const response = await fetch('/api/appointments', {
                method: 'POST',

                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },


                body: JSON.stringify(formData)
            });

            if (response.ok) {
                setMessage('Appointment request submitted successfully.');

                setFormData({
                    petId: '',
                    vetId: '',
                    clinicId: '',
                    appDateTime: '',
                    appReason: ''
                });
            } else {
                setError('Failed to create appointment.');
            }
        } catch (err) {
            console.error('Create appointment error:', err);

            setError('Could not connect to the server.');
        }
    };

    return (
        <div className="container mt-5" style={{ maxWidth: '800px' }}>
            <div className="card p-5">
                <h3 className="mb-4">Book Appointment</h3>

                {message && <div className="alert alert-success">{message}</div>}

                {error && <div className="alert alert-danger">{error}</div>}

                <form onSubmit={handleSubmit}>
                    <div className="row">
                        <div className="col-md-6 mb-3">

                            <label className="form-label">Pet</label>
                            <select
                                name="petId"
                                className="form-control"
                                value={formData.petId}
                                onChange={handleChange}
                                required>

                                <option value="">Select pet</option>
                                {pets.map(pet => (
                                    <option key={pet.petId} value={pet.petId}>
                                        {pet.petName}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div className="col-md-6 mb-3">


                            <label className="form-label">Clinic</label>
                            <select
                                name="clinicId"
                                className="form-control"
                                value={formData.clinicId}
                                onChange={handleChange}
                                required>

                                <option value="">Select clinic</option>
                                {clinics.map(clinic => (
                                    <option key={clinic.clinicId} value={clinic.clinicId}>
                                        {clinic.clinicName}
                                    </option>
                                ))}
                            </select>

                        </div>

                        <div className="col-md-6 mb-3">

                            <label className="form-label">Veterinarian</label>
                            <select
                                name="vetId"
                                className="form-control"
                                value={formData.vetId}
                                onChange={handleChange}
                                required>     
                                
                                
                                <option value="">Select veterinarian</option>
                                {vets.map(vet => (
                                    <option key={vet.vetId} value={vet.vetId}>
                                        {vet.vetName}
                                    </option>
                                ))}
                            </select>

                        </div>

                        <div className="col-md-6 mb-3">
                            <label className="form-label">Appointment Date</label>
                            <input
                                type="datetime-local"
                                name="appDateTime"
                                className="form-control"
                                value={formData.appDateTime}
                                onChange={handleChange}
                                required
                            />
                        </div>

                        <div className="col-12 mb-3">
                            <label className="form-label">Reason for Visit</label>
                            <textarea
                                name="appReason"
                                className="form-control"
                                rows="4"
                                value={formData.appReason}
                                onChange={handleChange}
                                required
                            ></textarea>
                        </div>

                        <div className="col-12">

                            <button type="submit" className="btn btn-primary">
                                Submit Appointment Request
                            </button>
                        </div>

                    </div>




                </form>

            </div>
        </div>
    );
};

export default CreateAppointmentPage;