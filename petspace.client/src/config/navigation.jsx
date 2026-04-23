export const navigationLinks = [
    {
        label: 'Profile',
        path: '/profile',
        icon: 'person-circle',
        roles: ['PetOwner', 'Vet', 'Clinic']
    },
    {
        label: 'My Pets',
        path: '/pets',
        icon: 'heart',
        roles: ['PetOwner']
    },
    {
        label: 'Appointments',
        path: '/appointments',
        icon: 'calendar-check',
        roles: ['PetOwner', 'Vet', 'Clinic']
    },
    {
        label: 'Book Appointment',
        path: '/appointments/create',
        icon: 'calendar-plus',
        roles: ['PetOwner']
    }
];