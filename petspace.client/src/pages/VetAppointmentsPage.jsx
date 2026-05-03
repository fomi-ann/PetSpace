import { useEffect, useState } from 'react';

const VetAppointmentsPage = () => {
    const token = localStorage.getItem('token');

    const [appointments, setAppointments] = useState([]);
    const [error, setError] = useState('');

    const [selectedAppointment, setSelectedAppointment] = useState(null);
    const [historyPetId, setHistoryPetId] = useState(null);
    const [medicalRecords, setMedicalRecords] = useState([]);

    const [diagnosis, setDiagnosis] = useState('');
    const [treatmentPlan, setTreatmentPlan] = useState('');
    const [petWeight, setPetWeight] = useState('');
    const [comment, setComment] = useState('');

    const fetchAppointments = async () => {
        try {
            const response = await fetch('/api/appointments/vet', {
                headers: { Authorization: `Bearer ${token}` }
            });

            if (response.ok) {
                const data = await response.json();
                setAppointments(data);
            } else {
                setError('Failed to load appointments.');
            }
        } catch (err) {
            console.error('Vet appointments fetch error:', err);
            setError('Could not connect to the server.');
        }
    };

    useEffect(() => {
        const loadAppointments = async () => {
            await fetchAppointments();
        };

        loadAppointments();
    }, [token]);

    const updateStatus = async (appointmentId, appStatusCodeId) => {

        try {

            const response = await fetch(`/api/appointments/${appointmentId}/status`, {
                method: 'PUT',
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ appStatusCodeId })
            });

            if (response.ok) {
                await fetchAppointments();
            } else {
                alert('Failed to update status.');
            }

        } catch (err) {
            console.error('Status update error:', err);
        }
    };

    const handleCreateMedicalRecord = async (e) => {
        e.preventDefault();

        if (!selectedAppointment) return;

        const dto = {
            appId: selectedAppointment.appId,
            diagnosis,
            treatmentPlan,
            petWeight: petWeight ? parseFloat(petWeight) : 0,
            comment
        };

        try {
            const response = await fetch('/api/medical-records', {
                method: 'POST',
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(dto)
            });

            if (response.ok) {
                alert('Medical record created successfully.');

                setSelectedAppointment(null);
                setDiagnosis('');
                setTreatmentPlan('');
                setPetWeight('');
                setComment('');
            } else {
                alert('Failed to create medical record.');
            }
        } catch (err) {
            console.error('Medical record create error:', err);
        }
    };


    const getStatusClass = (status) => {
        switch (status) {
            case 'Pending':
                return 'badge bg-warning text-dark';
            case 'Approved':
                return 'badge bg-success';
            case 'Rejected':
                return 'badge bg-danger';
            case 'Completed':
                return 'badge bg-secondary';
            default:
                return 'badge bg-light text-dark border';
        }
    };

    const handleViewHistory = async (petId) => {
        if (historyPetId === petId) {
            setHistoryPetId(null);
            setMedicalRecords([]);
            return;
        }

        try {
            const response = await fetch(`/api/vet/pets/${petId}/medical-records`, {
                headers: { Authorization: `Bearer ${token}` }
            });

            if (response.ok) {
                const data = await response.json();
                setMedicalRecords(data);
                setHistoryPetId(petId);
            } else {
                alert('Failed to load medical history.');
            }
        } catch (err) {
            console.error('Medical history fetch error:', err);
        }
    };

    return (
        <div className="container mt-5" style={{ maxWidth: '900px' }}>

            <div className="card p-5">

                <h3 className="mb-4">Vet Appointments</h3>

                {error && <div className="alert alert-danger">{error}</div>}

                {!appointments.length && !error && (
                    <p className="text-muted">No appointments yet.</p>
                )}

                {appointments.map(app => (

                    <div key={app.appId} className="card mb-3 p-3 text-start">

                        <div className="d-flex justify-content-between align-items-start">

                            <div>
                                <h5 className="mb-2">{app.petName || 'Unknown pet'}</h5>

                                <p className="mb-1">
                                    <strong>Date:</strong>{' '}
                                    {app.appDateTime
                                        ? new Date(app.appDateTime).toLocaleString()
                                        : 'N/A'}
                                </p>

                                <p className="mb-1">
                                    <strong>Clinic:</strong> {app.clinicName || 'N/A'}
                                </p>

                                <p className="mb-1">
                                    <strong>Reason:</strong> {app.appReason || 'N/A'}
                                </p>
                            </div>

                            <span className={getStatusClass(app.status)}>
                                {app.status || 'N/A'}
                            </span>

                        </div>
                        <div className="d-flex gap-2 mt-3">

                            {app.status === 'Pending' && (
                                <>
                                    <button
                                        className="btn btn-success btn-sm"
                                        onClick={() => updateStatus(app.appId, 2)}
                                    >
                                        Approve
                                    </button>

                                    <button
                                        className="btn btn-danger btn-sm"
                                        onClick={() => updateStatus(app.appId, 3)}
                                    >
                                        Reject
                                    </button>
                                </>
                            )}

                            {app.status === 'Approved' && (
                                <button
                                    className="btn btn-outline-secondary btn-sm"
                                    onClick={() => updateStatus(app.appId, 4)}
                                >
                                    Complete
                                </button>
                            )}

                            {app.status === 'Completed' && (
                                <button
                                    className="btn btn-outline-primary btn-sm"
                                    onClick={() => setSelectedAppointment(app)}
                                >
                                    Add Medical Record
                                </button>
                            )}

                            <button
                                className="btn btn-outline-secondary btn-sm"
                                onClick={() => handleViewHistory(app.petId)}
                            >
                                View History
                            </button>

                        </div>

                        {historyPetId === app.petId && (
                            <div className="card p-4 mt-4 bg-light text-start">
                                <h4 className="mb-3">Health History</h4>

                                {!medicalRecords.length && (
                                    <p className="text-muted mb-0">No medical records found.</p>
                                )}

                                {medicalRecords.map(record => (
                                    <div key={record.medicalRecordId} className="border rounded p-3 mb-2 bg-white">
                                        <h6 className="mb-2">{record.diagnosis || 'Medical Record'}</h6>

                                        <p className="mb-1">
                                            <strong>Date:</strong>{' '}
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

                                        <p className="mb-1">
                                            <strong>Treatment:</strong> {record.treatmentPlan || 'N/A'}
                                        </p>

                                        <p className="mb-1">
                                            <strong>Weight:</strong>{' '}
                                            {record.petWeight ? `${record.petWeight} kg` : 'N/A'}
                                        </p>

                                        <p className="mb-0">
                                            <strong>Comment:</strong> {record.comment || 'N/A'}
                                        </p>
                                    </div>
                                ))}
                            </div>
                        )}

                        {selectedAppointment?.appId === app.appId && (
                            <div className="card p-4 mt-4 bg-light text-start">

                                <h4 className="mb-3">Add Medical Record</h4>

                                <p className="text-muted small">
                                    Pet: <strong>{app.petName}</strong>
                                </p>

                                <form onSubmit={handleCreateMedicalRecord}>
                                    <div className="mb-3">
                                        <label className="form-label">Diagnosis</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            value={diagnosis}
                                            onChange={(e) => setDiagnosis(e.target.value)}
                                            required
                                        />
                                    </div>

                                    <div className="mb-3">
                                        <label className="form-label">Treatment Plan</label>
                                        <textarea
                                            className="form-control"
                                            rows="3"
                                            value={treatmentPlan}
                                            onChange={(e) => setTreatmentPlan(e.target.value)}
                                            required
                                        />
                                    </div>

                                    <div className="mb-3">
                                        <label className="form-label">Pet Weight (kg)</label>
                                        <input
                                            type="number"
                                            step="0.1"
                                            className="form-control"
                                            value={petWeight}
                                            onChange={(e) => setPetWeight(e.target.value)}
                                        />
                                    </div>

                                    <div className="mb-3">
                                        <label className="form-label">Comment</label>
                                        <textarea
                                            className="form-control"
                                            rows="3"
                                            value={comment}
                                            onChange={(e) => setComment(e.target.value)}
                                        />
                                    </div>

                                    <div className="d-flex gap-2">
                                        <button type="submit" className="btn btn-primary">
                                            Save Record
                                        </button>

                                        <button
                                            type="button"
                                            className="btn btn-outline-secondary"
                                            onClick={() => setSelectedAppointment(null)}
                                        >
                                            Cancel
                                        </button>
                                    </div>
                                </form>
                            </div>
                        )}

                    </div>
                ))}

            </div>
        </div>
    );
};

export default VetAppointmentsPage;