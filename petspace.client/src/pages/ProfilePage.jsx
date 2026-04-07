import React, { useEffect, useState } from 'react';
import UserIdentityCard from '../components/UserIdentityCard';
import UpdateProfileForm from '../components/UpdateProfileForm';

const ProfilePage = () => {


    const [profile, setProfile] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const token = localStorage.getItem('token');
    const role = localStorage.getItem('userRole');

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const response = await fetch('/api/profile', {
                    headers: { 'Authorization': `Bearer ${token}` }
                });

                if (response.ok) {
                    const data = await response.json();
                    setProfile(data);
                } else {
                    setError('Profile not found.');
                }

            } catch (err) {
                setError('Could not connect to the server.', err);
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

            if (response.ok) {
                alert("Profile updated successfully!");

                setProfile(prev => ({
                    ...prev,
                    userFirstName: updatedData.firstName,
                    userLastName: updatedData.lastName,
                    phoneNumber: updatedData.phoneNumber,

                }));
            } else {
                alert("Failed to update profile.");
            }
        } catch (err) {
            console.error("Update error:", err);
        }
    };

    const getDisplayConfig = (role) => {
        const common = [{ name: 'phoneNumber', label: 'Phone', icon: 'telephone' }];

        switch (role) {
            case 'Vet':
                return [
                    ...common,
                    { name: 'vetSpecialization', label: 'Specialization', icon: 'patch-check' }
                ];
            case 'Clinic':
                return [
                    { name: 'clinicName', label: 'Clinic', icon: 'hospital' },
                    { name: 'clinicAddress', label: 'Address', icon: 'geo-alt' },
                    { name: 'clinicCode', label: 'Clinic Code', icon: 'hash' },
                    ...common
                ];


            default:
                return common;
        }
    };


    const getFieldsConfig = (role) => {
        const common = [
            { name: 'firstName', label: 'First Name', placeholder: 'Enter first name' },
            { name: 'lastName', label: 'Last Name', placeholder: 'Enter last name' },
            { name: 'phoneNumber', label: 'Phone Number', placeholder: 'Enter phone' }
        ];

        switch (role) {

            case 'Vet':
                return [
                    ...common,
                    { name: 'vetSpecialization', label: 'Specialization' }
                ];
            case 'Clinic':
                return [
                    { name: 'clinicName', label: 'Clinic Name' },
                    { name: 'clinicAddress', label: 'Address' },
                    ...common
                ];
            default:
                return common;
        }
    };

    if (loading) return <div className="container mt-5 text-center">Loading...</div>;

    const displayConfig = getDisplayConfig(role);
    const fieldsConfig = getFieldsConfig(role);

    return (
        <div className="container mt-5" style={{ maxWidth: '800px' }}>
            {error && <div className="alert alert-danger">{error}</div>}

            {profile && (
                <div className="row">
                    <div className="col-12">

                        <UserIdentityCard
                            profile={profile}
                            config={displayConfig}
                            title={`${role} Profile`}
                        />


                        <div className="card p-5 mb-4">
                            <h4 className="text-secondary mb-4 border-bottom pb-2">Update Profile</h4>
                            <UpdateProfileForm
                                fields={fieldsConfig}
                                initialData={{
                                    firstName: profile.userFirstName,
                                    lastName: profile.userLastName,
                                    phoneNumber: profile.phoneNumber,
                                    clinicName: profile.clinicName,
                                    clinicAddress: profile.clinicAddress,
                                    vetSpecialization: profile.vetSpecialization
                                }}
                                onSave={handleSave}
                            />
                        </div>


                        {role === 'PetOwner' && (
                            <div className="card p-5 bg-light">
                                <h4 className="text-primary">My Pets</h4>
                                <hr />
                                <p className="text-muted">You don't have any pets registered yet.</p>
                                <button className="btn btn-outline-primary btn-sm w-25">
                                    + Add Pet
                                </button>
                            </div>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

export default ProfilePage;