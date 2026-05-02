import { useEffect, useState } from 'react';

const ClinicVetRequestsPage = () => {
    const token = localStorage.getItem('token');
    const [requests, setRequests] = useState([]);

    const fetchRequests = async () => {
        const response = await fetch('/api/clinic/vet-requests', {
            headers: { Authorization: `Bearer ${token}` }
        });

        if (response.ok) {
            const data = await response.json();
            setRequests(data);
        }
    };

    useEffect(() => {
        fetchRequests();
    }, []);

    const handleUpdate = async (vetId, approve) => {
        await fetch(`/api/clinic/vet-requests/${vetId}?approve=${approve}`, {
            method: 'PUT',
            headers: { Authorization: `Bearer ${token}` }
        });

        fetchRequests();
    };

    return (
        <div className="container mt-5">
            <div className="card p-4">
                <h4 className="mb-3">Vet Requests</h4>

                {!requests.length && <p>No requests</p>}

                {requests.map(r => (
                    <div key={r.vetId} className="border p-3 mb-2">
                        <h6>{r.vetName}</h6>
                        <p className="mb-1">{r.email}</p>
                        <p className="mb-1">{r.specialization}</p>
                        <p className="mb-2">Licence: {r.licence}</p>

                        <div className="d-flex gap-2">
                            <button
                                className="btn btn-success btn-sm"
                                onClick={() => handleUpdate(r.vetId, true)}
                            >
                                Approve
                            </button>

                            <button
                                className="btn btn-danger btn-sm"
                                onClick={() => handleUpdate(r.vetId, false)}
                            >
                                Reject
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ClinicVetRequestsPage;