const AppHeader = ({ onMenuClick, userName, onLogout }) => {
    return (
        <header className="navbar bg-white border-bottom py-2">
            <div className="container-fluid d-flex align-items-center justify-content-between">
                <div className="d-flex align-items-center" style={{ width: '220px' }}>
                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={onMenuClick}
                        aria-label="Open navigation menu"
                    >
                        <i className="bi bi-list fs-4"></i>
                    </button>
                </div>

                <div className="d-flex align-items-center justify-content-center flex-grow-1">
                    <i className="bi bi-stars me-2 text-primary"></i>
                    <span className="fw-bold fs-4 text-dark">PetSpace</span>
                </div>

                <div
                    className="d-flex align-items-center justify-content-end gap-3"
                    style={{ width: '220px' }}
                >
                    <span className="text-muted small mb-0">
                        {userName ? `Hello,  ${userName}` : 'Please update your profile'}
                    </span>

                    <button
                        type="button"
                        className="btn btn-outline-danger btn-sm"
                        onClick={onLogout}
                    >
                        Logout
                    </button>
                </div>
            </div>
        </header>
    );
};

export default AppHeader;