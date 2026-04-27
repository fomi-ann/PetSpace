import { useEffect, useState } from 'react';

const HealthHistoryPage = () => {
    const token = localStorage.getItem('token');

    const [pets, setPets] = useState([]);
    const [selectedPetId, setSelectedPetId] = useState('');
    const [records, setRecords] = useState([]);
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
                } else {
                    setError('Failed to load pets.');
                }
            } catch (err) {
                console.error('Pets fetch error:', err);
                setError('Could not connect to the server.');
            }
        };

        fetchPets();
    }, [token]);

    const fetchMedicalRecords = async (petId) => {
        setError('');
        setRecords([]);

        if (!petId) return;

        try {
            const response = await fetch(`/api/pets/${petId}/medical-records`, {
                headers: { Authorization: `Bearer ${token}` }
            });

            if (response.ok) {
                const data = await response.json();
                setRecords(data);
            } else {
                setError('Failed to load medical records.');
            }
        } catch (err) {
            console.error('Medical records fetch error:', err);
            setError('Could not connect to the server.');
        }
    };

    const handlePetChange = async (e) => {
        const petId = e.target.value;
        setSelectedPetId(petId);
        await fetchMedicalRecords(petId);
    };

    return (
        <div className="container mt-5" style={{ maxWidth: '900px' }}>
            <div className="card p-5">
                <h3 className="text-primary mb-4">Health History</h3>

                {error && <div className="alert alert-danger">{error}</div>}

                <div className="mb-4">
                    <label className="form-label">Select Pet</label>
                    <select
                        className="form-control"
                        value={selectedPetId}
                        onChange={handlePetChange}
                    >
                        <option value="">Choose pet</option>
                        {pets.map(pet => (
                            <option key={pet.petId} value={pet.petId}>
                                {pet.petName}
                            </option>
                        ))}
                    </select>
                </div>

                {!selectedPetId && (
                    <p className="text-muted">Please select a pet to view health history.</p>
                )}

                {selectedPetId && !records.length && (
                    <p className="text-muted">No medical records found for this pet.</p>
                )}

                {records.map(record => (
                    <div key={record.medicalRecordId} className="card mb-3 p-3 text-start">
                        <div className="d-flex justify-content-between align-items-start">
                            <div>
                                <h5 className="mb-2">{record.diagnosis || 'Medical Record'}</h5>

                                <p className="mb-1">
                                    <strong>Appointment date:</strong>{' '}
                                    {record.appointmentDate
                                        ? new Date(record.appointmentDate).toLocaleString()
                                        : 'N/A'}
                                </p>

                                <p className="mb-1">
                                    <strong>Clinic:</strong> {record.clinicName || 'N/A'}
                                </p>

                                <p className="mb-1">
                                    <strong>Veterinarian:</strong> {record.vetName || 'N/A'}
                                </p>
                            </div>

                            <span className="badge bg-primary">
                                {record.createdAt
                                    ? new Date(record.createdAt).toLocaleDateString()
                                    : 'Record'}
                            </span>
                        </div>

                        <hr />

                        <div className="row">
                            <div className="col-md-6">
                                <h6 className="text-secondary">Treatment</h6>
                                <p className="mb-1">
                                    <strong>Treatment plan:</strong> {record.treatmentPlan || 'N/A'}
                                </p>
                                <p className="mb-1">
                                    <strong>Weight:</strong>{' '}
                                    {record.petWeight ? `${record.petWeight} kg` : 'N/A'}
                                </p>
                            </div>

                            <div className="col-md-6">
                                <h6 className="text-secondary">Comment</h6>
                                <p className="mb-1">{record.comment || 'N/A'}</p>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default HealthHistoryPage;