import { useEffect, useState } from 'react';

import PetForm from '../components/PetForm';
import PetList from '../components/PetList';

const MyPetsPage = () => {
    const token = localStorage.getItem('token');

    const [pets, setPets] = useState([]);
    const [error, setError] = useState('');

    const [editingPet, setEditingPet] = useState(null);

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

    useEffect(() => {
        const loadPets = async () => {
            await fetchPets();
        };

        loadPets();
    }, [token]);


    const handleAddPet = async (petData) => {
        try {
            const response = await fetch('/api/pets', {
                method: 'POST',
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(petData)
            });

            if (response.ok) {
                await fetchPets();
            } else {
                alert('Failed to add pet.');
            }
        } catch (err) {
            console.error('Add pet error:', err);
        }
    };

    const handleUpdatePet = async (updatedData) => {
        try {
            const response = await fetch(`/api/pets/${editingPet.petId}`, {
                method: 'PUT',
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(updatedData)
            });

            if (response.ok) {
                setEditingPet(null);
                await fetchPets();
            } else {
                alert('Failed to update pet.');
            }
        } catch (err) {
            console.error('Update pet error:', err);
        }
    };

    const handleDeletePet = async (petId) => {
        try {
            const response = await fetch(`/api/pets/${petId}`, {
                method: 'DELETE',
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            if (response.ok) {
                setPets(prev => prev.filter(pet => pet.petId !== petId));
            } else {
                alert('Failed to delete pet.');
            }
        } catch (err) {
            console.error('Delete pet error:', err);
        }
    };

    return (
        <div className="container mt-5" style={{ maxWidth: '1100px' }}>
            <div className="row">
                <div className="col-md-7">
                    <div className="card p-4">
                        <h3 className="text-primary mb-4">My Pets</h3>

                        {error && <div className="alert alert-danger">{error}</div>}

                        <PetList
                            pets={pets}
                            onDelete={handleDeletePet}
                            onEdit={setEditingPet}
                        />
                    </div>
                </div>

                <div className="col-md-5">
                    <div className="card p-4">
                        <h4 className="text-secondary mb-3">
                            {editingPet ? 'Edit Pet' : 'Add New Pet'}
                        </h4>

                        <PetForm
                            key={editingPet ? editingPet.petId : 'add-pet'}
                            initialData={editingPet}
                            onSubmit={editingPet ? handleUpdatePet : handleAddPet}
                            buttonText={editingPet ? 'Save Changes' : 'Add Pet'}
                            onCancel={editingPet ? () => setEditingPet(null) : null}
                        />

                        
                    </div>
                </div>
            </div>
        </div>
    );
};

export default MyPetsPage;