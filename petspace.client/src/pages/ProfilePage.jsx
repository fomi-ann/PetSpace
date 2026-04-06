import React, { useEffect, useState } from 'react';
// import { useNavigate } from 'react-router';
import UserProfile from '../components/Profiles/UserProfile';


const ProfilePage = () => {
    const [profile, setProfile] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const role = localStorage.getItem('userRole');
    const token = localStorage.getItem('token');

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const response = await fetch('/api/profile', {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                });

                if (response.ok) {
                    const data = await response.json();
                    setProfile(data);
                } else {
                    setError('Profile not found.');
                }
            } catch (err) {
                console.error("Error:", err);
                setError('Could not connect to the server.');
            } finally {
                setLoading(false);
            }
        };

        if (token) fetchProfile();
    }, [token]);

    const handleSave = async (updatedData) => {
        try {
            const response = await fetch('/api/profile', {
                method: 'PUT',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(updatedData)
            });

            if (!response.ok) {
                throw new Error('Failed to update profile');
            }

            alert("Profile updated!");
        } catch (err) {
            console.error(err);
            alert("Error updating profile");
        }
    };

    if (loading) return <div className="container mt-5 text-center">Loading...</div>;

    return (
        <div className="container mt-5" style={{ maxWidth: '600px' }}>
            {error && <div className="alert alert-danger">{error}</div>}

            {role === 'PetOwner' && (
                <UserProfile data={profile} onSave={handleSave} />
            )}
        </div>
    );
};

//const VetProfileForm = () => <div><h3>Hello, Vet!</h3><input placeholder="Input" className="form-control" /></div>;
//const ClinicProfileForm = () => <div><h3>Hello, Clinic</h3><input placeholder="Input" className="form-control" /></div>;
//const UserProfileForm = () => <div><h3>Hello User!</h3><input placeholder="Input" className="form-control" /></div>;

export default ProfilePage;