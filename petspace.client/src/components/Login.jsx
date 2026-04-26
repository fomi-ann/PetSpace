import { useState } from 'react';
import { useNavigate } from 'react-router';

const Login = () => {
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        try {
            const response = await fetch('/api/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password })
            });

            if (response.ok) {
                const data = await response.json();

                localStorage.setItem('token', data.token);
                localStorage.setItem('userRole', data.role);
                localStorage.setItem('userFirstName', data.userFirstName);
                localStorage.removeItem('userFirstName');
                // console.log("TOKEEEEEN", data.token);
                navigate('/profile');
            } else {
                setError("Invalid email or password");
            }

        } catch (err) {
            console.error("Login error:", err);
            setError("Server connection error");
        }
    };

    return (
        <div className="container d-flex align-items-center justify-content-center mt-5">

            <div className="card p-5" style={{ width: '420px' }}>
                <h2 className="text-center mb-5">Login</h2>

                {error && <div className="alert alert-danger p-2 small text-center">{error}</div>}

                <form onSubmit={handleSubmit}>

                    <div className="input-group mb-4">
                        <input
                            type="email"
                            className="form-control bg-transparent border-end-0"
                            placeholder="Email"
                            required
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                        <span className="input-group-text bg-transparent border-start-0 text-muted">
                            <i className="bi bi-person-fill"></i>
                        </span>
                    </div>


                    <div className="input-group mb-3">
                        <input
                            type="password"
                            className="form-control bg-transparent border-end-0"
                            placeholder="Password"
                            required
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                        <span className="input-group-text bg-transparent border-start-0 text-muted">
                            <i className="bi bi-lock-fill"></i>
                        </span>
                    </div>

                    <div className="d-flex justify-content-between align-items-top mb-4 small text-muted">
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" id="rememberMe" />
                            <label className="form-check-label" htmlFor="rememberMe">
                                Remember me
                            </label>
                        </div>
                        
                        <a href="#" className="text-decoration-none text-muted">Forgot Password?</a>
                    </div>

                    <div className="d-grid mb-4">
                        <button type="submit" className="btn btn-primary py-2">Login</button>
                    </div>


                    <div className="text-center small text-muted">
                        Don't have an account?{' '}
                        <button
                            type="button"
                            className="btn btn-link p-0 small fw-bold text-decoration-none"
                            onClick={() => navigate('/register')}
                        >
                            Register
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};


export default Login;