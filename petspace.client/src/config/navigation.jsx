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
        label: 'Health History',
        path: '/health-history',
        icon: 'clipboard2-pulse',
        roles: ['PetOwner']
    },

    //PetOwner App
    {
        label: 'Book Appointment',
        path: '/appointments/create',
        icon: 'calendar-plus',
        roles: ['PetOwner']
    },
    {
        label: 'My Appointments',
        path: '/appointments/my',
        icon: 'calendar-check',
        roles: ['PetOwner']
    },
    //

    //Vet App
    {
        label: 'Appointments',
        path: '/appointments/vet',
        icon: 'calendar-week',
        roles: ['Vet']
    },
    //

    {
        label: 'Vet Requests',
        path: '/clinic/vet-requests',
        icon: 'person-check',
        roles: ['Clinic']
    }
];