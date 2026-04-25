import { useEffect, useState } from 'react';

const VetAppointmentsPage = () => {
    const token = localStorage.getItem('token');

    const [appointments, setAppointments] = useState([]);
    const [error, setError] = useState('');

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
                        <h5 className="mb-2">{app.petName || 'Unknown pet'}</h5>

                        <p className="mb-1">
                            <strong>Date:</strong>{' '}
                            {app.appDateTime ? new Date(app.appDateTime).toLocaleString() : 'N/A'}
                        </p>

                        <p className="mb-1">
                            <strong>Clinic:</strong> {app.clinicName || 'N/A'}
                        </p>

                        <p className="mb-1">
                            <strong>Reason:</strong> {app.appReason || 'N/A'}
                        </p>

                        <p className="mb-2">
                            <strong>Status:</strong> {app.status || 'N/A'}
                        </p>


                        <div className="d-flex gap-2">

                            <button
                                type="button"
                                className="btn btn-success btn-sm"
                                onClick={() => updateStatus(app.appId, 2)}
                            >
                                Approve
                            </button>

                            <button
                                type="button"
                                className="btn btn-danger btn-sm"
                                onClick={() => updateStatus(app.appId, 3)}
                            >
                                Reject
                            </button>

                            <button
                                type="button"
                                className="btn btn-outline-secondary btn-sm"
                                onClick={() => updateStatus(app.appId, 4)}
                            >
                                Complete
                            </button>

                        </div>

                    </div>

                ))}
            </div>


        </div>
    );
};

export default VetAppointmentsPage;