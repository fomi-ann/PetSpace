import { useState } from 'react'
import { useNavigate } from 'react-router';

const Register = () => {
    const navigate = useNavigate();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState('');


    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        if (password !== confirmPassword) {
            setError("Passwords do not match!");
            return;
        }

        try {
            const response = await fetch('/api/register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password })
            });

            if (response.ok) {
                alert("Account created successfully!");
                navigate('/');
            } else {
                const data = await response.json();
                setError(data[0]?.description || "Registration failed");
            }
        } catch (err) {
            console.error("Registration error:", err);
            setError("Server connection error");
        }
    };

    return (
        <div className="container d-flex align-items-center justify-content-center">
            <div className="card p-5" style={{ width: '420px' }}>
                <h2 className="text-center mb-5">Register</h2>

                {error && <div className="alert alert-danger p-2 small">{error}</div>}

                <form onSubmit={handleSubmit}>
                    <div className="input-group mb-4">
                        <input
                            type="email"
                            className="form-control bg-transparent border-end-0"
                            placeholder="Email Address"
                            required
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                        <span className="input-group-text bg-transparent border-start-0 text-muted">
                            <i className="bi bi-envelope-fill"></i>
                        </span>
                    </div>

                    <div className="input-group mb-4">
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

                    <div className="input-group mb-4">
                        <input
                            type="password"
                            className="form-control bg-transparent border-end-0"
                            placeholder="Confirm Password"
                            required
                            onChange={(e) => setConfirmPassword(e.target.value)}
                        />
                        <span className="input-group-text bg-transparent border-start-0 text-muted">
                            <i className="bi bi-shield-lock-fill"></i>
                        </span>
                    </div>


                    <div className="d-grid mb-4">
                        <button type="submit" className="btn btn-primary py-2">Create Account</button>
                    </div>

                    <div className="text-center small text-muted">
                        Already have an account?{' '}
                        <button
                            type="button"
                            className="btn btn-link p-0 small fw-bold text-decoration-none"
                            onClick={() => navigate('/')}
                        >
                            Login
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default Register;