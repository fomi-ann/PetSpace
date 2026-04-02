import './App.css'
import { BrowserRouter as Router, Routes, Route } from 'react-router';
import Login from './components/Login';

function App() {
  
  return (
      <Router>
          <h1>Hello</h1>
          <Routes>
              
              <Route path="/" element={<Login />} />
          </Routes>
    </Router>
  )
}

export default App
