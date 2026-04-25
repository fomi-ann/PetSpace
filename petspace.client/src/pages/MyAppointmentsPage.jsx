import { useEffect, useState } from 'react';


const MyAppointmentsPage = () => {


    const token = localStorage.getItem('token');

    const [appointments, setAppointments] = useState([]);
    const [error, setError] = useState('');

    useEffect(() => {

        const fetchAppointments = async () => {

            try {
                const response = await fetch('/api/appointments/my', {
                    headers: { Authorization: `Bearer ${token}` }
                });

                if (response.ok) {
                    const data = await response.json();
                    setAppointments(data);
                } else {
                    setError('Failed to load appointments.');
                }

            } catch (err) {
                console.error('Appointments fetch error:', err);
                setError('Could not connect to the server.');
            }
        };

        fetchAppointments();

    }, [token]);

    const getStatusClass = (status) => {

        switch (status) {

            case 'Approved':
                return 'badge bg-success';

            case 'Rejected':
                return 'badge bg-danger';

            case 'Completed':
                return 'badge bg-secondary';

            default:
                return 'badge bg-warning text-dark';
        }
    };

    return (
        <div className="container mt-5" style={{ maxWidth: '900px' }}>

            <div className="card p-5">

                <h3 className="mb-4">My Appointments</h3>

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
                                    <strong>Veterinarian:</strong> {app.vetName || 'N/A'}
                                </p>

                                <p className="mb-1">
                                    <strong>Reason:</strong> {app.appReason || 'N/A'}
                                </p>
                            </div>

                            <span className={getStatusClass(app.status)}>
                                {app.status || 'Pending'}
                            </span>


                        </div>

                    </div>

                ))}

            </div>
        </div>
    );
};

export default MyAppointmentsPage;