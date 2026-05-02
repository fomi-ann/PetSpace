

const PetList = ({ pets, onDelete, onEdit, onShare }) => {

    const getAgeText = (birthDate) => {
        if (!birthDate) return 'N/A';

        const birth = new Date(birthDate);
        const today = new Date();

        let years = today.getFullYear() - birth.getFullYear();
        let months = today.getMonth() - birth.getMonth();

        if (today.getDate() < birth.getDate()) {
            months -= 1;
        }

        if (months < 0) {
            years -= 1;
            months += 12;
        }

        if (years < 0) return 'N/A';

        if (years === 0 && months === 0) {
            return 'Less than 1 month';
        }

        if (years === 0) {
            return `${months} month${months !== 1 ? 's' : ''}`;
        }

        if (months === 0) {
            return `${years} year${years !== 1 ? 's' : ''}`;
        }

        return `${years} year${years !== 1 ? 's' : ''}, ${months} month${months !== 1 ? 's' : ''}`;
    };

    if (!pets.length) {
        return <p className="text-muted">You don't have any pets registered yet.</p>;
    }

    return (
        <div className="mt-3">
            {pets.map(pet => (
                <div key={pet.petId} className="card mb-3 p-3 text-start">
                    <div className="d-flex justify-content-between align-items-start">
                        <div>
                            <h5 className="mb-1">{pet.petName}</h5>

                            <p className="text-muted mb-2">
                                <i>{pet.microchipNr || 'N/A'}</i>
                            </p>
                        </div>

                        <div className="d-flex gap-2">
                            <button
                                type="button"
                                className="btn btn-outline-primary btn-sm"
                                onClick={() => onEdit(pet)}
                            >
                                Edit
                            </button>

                            <button
                                type="button"
                                className="btn btn-outline-danger btn-sm"
                                onClick={() => onDelete(pet.petId)}
                            >
                                Delete
                            </button>


                            {onShare && (
                                <button
                                    type="button"
                                    className="btn btn-outline-secondary btn-sm"
                                    onClick={() => onShare(pet.petId)}
                                >
                                    Share
                                </button>
                            )}

                        </div>
                    </div>

                    <hr />

                    <div className="row">
                        

                        <div className="col-md-6">
                            <h6 className="text-secondary">General</h6>
                            <p className="mb-1">
                                <strong>Species:</strong> {pet.species || 'N/A'}
                            </p>
                            <p className="mb-1">
                                <strong>Breed:</strong> {pet.breed || 'N/A'}
                            </p>
                            <p className="mb-1">
                                <strong>Gender:</strong> {pet.gender || 'N/A'}
                            </p>
                            
                        </div>

                        <div className="col-md-6">
                            <h6 className="text-secondary">Health</h6>
                            <p className="mb-1">
                                <strong>Age:</strong> {getAgeText(pet.birthDate)}
                            </p>
                            <p className="mb-1">
                                <strong>Birth date:</strong> {pet.birthDate ? pet.birthDate.slice(0, 10) : 'N/A'}
                            </p>
                            <p className="mb-1">
                                <strong>Weight:</strong> {pet.weight ? `${pet.weight} kg` : 'N/A'}
                            </p>
                        </div>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default PetList;