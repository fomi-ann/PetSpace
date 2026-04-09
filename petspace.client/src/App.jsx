import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import ProfilePage from './pages/ProfilePage';
import AppLayout from './components/layout/AppLayout';

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
            </Routes>
        </Router>
    );
}

export default App;