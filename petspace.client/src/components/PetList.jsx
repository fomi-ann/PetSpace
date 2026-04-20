const PetList = ({ pets, onDelete }) => {
    if (!pets.length) {
        return <p className="text-muted">You don't have any pets registered yet.</p>;
    }

    return (
        <div className="mt-3">
            {pets.map(pet => (
                <div key={pet.petId} className="card mb-3 p-3">
                    <div className="d-flex justify-content-between align-items-start">
                        <div className="text-start">
                            <h5 className="mb-1">{pet.petName}</h5>
                            <p className="mb-1"><strong>Species:</strong> {pet.species}</p>
                            <p className="mb-1"><strong>Breed:</strong> {pet.breed || 'N/A'}</p>
                            <p className="mb-0">
                                <strong>Birth date:</strong> {pet.birthDate ? pet.birthDate.slice(0, 10) : 'N/A'}
                            </p>
                        </div>

                        <button
                            type="button"
                            className="btn btn-outline-danger btn-sm"
                            onClick={() => onDelete(pet.petId)}
                        >

                            Delete
                        </button>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default PetList;