import './App.css'
import { BrowserRouter as Router, Routes, Route } from 'react-router';
import Login from './components/Login';
import Register from './components/Register';

function App() {
  
  return (
      <Router>
          <h1>Hello</h1>
          <Routes>
              
              <Route path="/" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/profile" element={<div>Profile page placeholder</div>} />
          </Routes>
    </Router>
  )
}

export default App
