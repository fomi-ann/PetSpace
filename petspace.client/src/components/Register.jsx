import { useNavigate } from 'react-router';

const Register = () => {
    const navigate = useNavigate();

    return (
        <div className="container d-flex align-items-center justify-content-center">
            <div className="card p-5" style={{ width: '420px' }}>
                <h2 className="text-center mb-5">Register</h2>

                <form>
                    <div className="input-group mb-4">
                        <input
                            type="email"
                            className="form-control bg-transparent border-end-0"
                            placeholder="Email Address"
                            required
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