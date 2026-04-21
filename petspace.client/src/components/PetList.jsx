const PetList = ({ pets, onDelete }) => {

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
                <div key={pet.petId} className="card mb-3 p-3">
                    <div className="d-flex justify-content-between align-items-start">
                        <div className="text-start">
                            <h5 className="mb-2">{pet.petName}</h5>

                            <p className="mb-1">
                                <strong>Species:</strong> {pet.species || 'N/A'}
                            </p>

                            <p className="mb-1">
                                <strong>Breed:</strong> {pet.breed || 'N/A'}
                            </p>

                            <p className="mb-1">
                                <strong>Gender:</strong> {pet.gender || 'N/A'}
                            </p>

                            <p className="mb-1">
                                <strong>Birth date:</strong> {pet.birthDate ? pet.birthDate.slice(0, 10) : 'N/A'}
                            </p>

                            <p className="mb-1">
                                <strong>Age:</strong> {getAgeText(pet.birthDate)}
                            </p>

                            <p className="mb-1">
                                <strong>Microchip:</strong> {pet.microchipNr || 'N/A'}
                            </p>

                            <p className="mb-0">
                                <strong>Weight:</strong> {pet.weight ? `${pet.weight} kg` : 'N/A'}
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