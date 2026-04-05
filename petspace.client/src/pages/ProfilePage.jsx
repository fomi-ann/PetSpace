import { useState } from 'react'
import { useNavigate } from 'react-router';

const Profile = () => {
    const [profileData, setProfileData] = useState(null);
    const [isEditing, setIsEditing] = useState(false);


    useEffect(() => {

    }, []);

    return (
        <div>
            {isEditing ? (
                <ClinicForm data={profileData} />
            ) : (
                <ClinicView data={profileData} onEdit={() => setIsEditing(true)} />
            )}
        </div>
    );
}