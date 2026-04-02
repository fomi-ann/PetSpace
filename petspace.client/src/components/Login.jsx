import { useNavigate } from 'react-router';

const Login = () => {
    const navigate = useNavigate();

    return (
        <div className="container d-flex align-items-center justify-content-center vh-100">
            <div className="card p-4 shadow" style={{ width: '400px' }}>
                <h2 className="text-center">PetSpace</h2>
                <button
                    className="btn btn-outline-primary mt-3"
                    onClick={() => navigate('/register')}
                >Sign Up
                </button>
            </div>
        </div>
    );
};

export default Login;