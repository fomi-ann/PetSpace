import './App.css'
import { BrowserRouter as Router, Routes, Route } from 'react-router';
import Login from './components/Login';
import Register from './components/Register';
import ProfilePage from './pages/ProfilePage';

function App() {
  
  return (
      <Router>
          <h1>Hello</h1>
          <Routes>
              
              <Route path="/" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/profile" element={<ProfilePage />} />
          </Routes>
    </Router>
  )
}

export default App
