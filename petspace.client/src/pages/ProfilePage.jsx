import React, { useEffect, useState } from 'react';
import UserIdentityCard from '../components/UserIdentityCard';
import UpdateProfileForm from '../components/UpdateProfileForm';



const ProfilePage = () => {


    const [profile, setProfile] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [pets, setPets] = useState([]);

    const token = localStorage.getItem('token');
    const role = localStorage.getItem('userRole');

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

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const response = await fetch('/api/profile', {
                    headers: { Authorization: `Bearer ${token}` }
                });

                if (response.ok) {
                    const data = await response.json();
                    console.log('PROFILE DATA:', data);
                    setProfile(data);
                    localStorage.setItem('userFirstName', data.userFirstName || '');
                    window.dispatchEvent(new Event('userNameUpdated'));
                } else {
                    setError('Profile not found.');
                
                }
            } catch (err) {
                console.error('Profile fetch error:', err);
                setError('Could not connect to the server.');
            } finally {
                setLoading(false);
            }
        };

        if (!token) {
            setError('No token found.');
            setLoading(false);
            return;
        }

        const loadData = async () => {
            await fetchProfile();
            await fetchPets();
        };

        loadData();

    }, [token]);


    const handleSave = async (updatedData) => {
        try {
            const response = await fetch('/api/profile', {
                method: 'PUT',
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(updatedData)
            });

            if (response.ok) {
                alert('Profile updated successfully!');

                setProfile(prev => ({
                    ...prev,
                    userFirstName: updatedData.firstName,
                    userLastName: updatedData.lastName,
                    phoneNumber: updatedData.phoneNumber,
                    specialization: updatedData.specialization,
                    licence: updatedData.licence,
                    clinicName: updatedData.clinicName,
                    address: updatedData.address,
                    phone: updatedData.phone
                }));

                localStorage.setItem('userFirstName', updatedData.firstName || '');
                window.dispatchEvent(new Event('userNameUpdated'));

            } else {
                alert('Failed to update profile.');
            }
        } catch (err) {
            console.error('Update error:', err);
        }
    };

    const getDisplayConfig = (role) => {
        const common = [{ name: 'phoneNumber', label: 'Phone', icon: 'telephone' }];

        switch (role) {
            case 'Vet':
                return [
                    ...common,
                    { name: 'specialization', label: 'Specialization', icon: 'patch-check' },
                    { name: 'licence', label: 'Licence', icon: 'card-text' }
                ];

            case 'Clinic':
                return [
                    { name: 'clinicName', label: 'Clinic', icon: 'hospital' },
                    { name: 'address', label: 'Address', icon: 'geo-alt' },
                    { name: 'phone', label: 'Clinic Phone', icon: 'telephone' },
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
                    { name: 'specialization', label: 'Specialization', placeholder: 'Enter specialization' },
                    { name: 'licence', label: 'Licence', placeholder: 'Enter licence' }
                ];

            case 'Clinic':
                return [
                    ...common,
                    { name: 'clinicName', label: 'Clinic Name', placeholder: 'Enter clinic name' },
                    { name: 'address', label: 'Address', placeholder: 'Enter address' },
                    { name: 'phone', label: 'Clinic Phone', placeholder: 'Enter clinic phone' }
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
                                    firstName: profile.userFirstName || '',
                                    lastName: profile.userLastName || '',
                                    phoneNumber: profile.phoneNumber || '',
                                    clinicName: profile.clinicName || '',
                                    address: profile.address || '',
                                    phone: profile.phone || '',
                                    specialization: profile.specialization || '',
                                    licence: profile.licence || ''
                                }}
                                onSave={handleSave}
                            />
                        </div>


                        {role === 'PetOwner' && (
                            <div className="card p-5 bg-light">


                                <div className="d-flex justify-content-between align-items-center mb-3">
                                    <h4 className="text-primary">My Pets</h4>
                                    <a href="/pets" className="btn btn-outline-primary btn-sm">
                                        Manage pets
                                    </a>
                                </div>


                                <hr />

                                {!pets.length && (
                                    <p className="text-muted">You don't have any pets registered yet.</p>
                                )}


                                {pets.slice(0, 3).map(pet => (

                                    <div key={pet.petId} className="border rounded p-3 mb-2 bg-white text-start">

                                        <h5 className="mb-1">{pet.petName}</h5>

                                        <p className="mb-1">
                                            <strong>Species:</strong> {pet.species || 'N/A'}
                                        </p>

                                        <p className="mb-0">
                                            <strong>Weight:</strong> {pet.weight ? `${pet.weight} kg` : 'N/A'}
                                        </p>


                                    </div>
                                ))}


                            </div>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

export default ProfilePage;