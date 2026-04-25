import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import AppLayout from './components/layout/AppLayout';

import ProfilePage from './pages/ProfilePage';
import MyPetsPage from './pages/MyPetsPage';

import CreateAppointmentPage from './pages/CreateAppointmentPage';
import MyAppointmentsPage from './pages/MyAppointmentsPage';

import VetAppointmentsPage from './pages/VetAppointmentsPage';

function App() {
    return (
        <Router>
            <Routes>

                <Route path="/" element={<Login />} />

                <Route path="/register" element={<Register />} />

                <Route
                    path="/profile"
                    element={
                        <AppLayout>
                            <ProfilePage />
                            
                        </AppLayout>
                    }
                />

                <Route
                    path="/pets"
                    element={
                        <AppLayout>
                            <MyPetsPage />
                        </AppLayout>
                    }
                />

                <Route
                    path="/appointments/create"
                    element={
                        <AppLayout>
                            <CreateAppointmentPage />
                        </AppLayout>
                    }
                />

                <Route
                    path="/appointments/my"
                    element={
                        <AppLayout>
                            <MyAppointmentsPage />
                        </AppLayout>
                    }
                />

                <Route
                    path="/appointments/vet"
                    element={
                        <AppLayout>
                            <VetAppointmentsPage />
                        </AppLayout>
                    }
                />



            </Routes>

            

        </Router>
    );
}

export default App;