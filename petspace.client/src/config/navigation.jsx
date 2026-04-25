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
    //{
    //    label: 'Appointments',
    //    path: '/appointments',
    //    icon: 'calendar-check',
    //    roles: ['PetOwner', 'Vet', 'Clinic']
    //},

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
    }
    //

];