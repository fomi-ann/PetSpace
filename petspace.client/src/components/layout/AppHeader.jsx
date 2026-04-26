





const AppHeader = ({
    onMenuClick,
    userName,
    onLogout,
    showMenu = true,
    showGreeting = true
}) => {
    const token = localStorage.getItem('token');

    return (
        <header
            className="bg-white border-bottom position-relative"
            style={{
                height: '56px',
                width: '100%',
                display: 'flex',
                alignItems: 'center'
            }}
        >

            <div
                className="d-flex align-items-center justify-content-start px-3"
                style={{ width: '220px', zIndex: 2 }}
            >
                {showMenu && (
                    <button
                        type="button"
                        className="btn btn-primary btn-sm"
                        onClick={onMenuClick}
                        aria-label="Open navigation menu"
                    >
                        <i className="bi bi-list fs-5"></i>
                    </button>
                )}
            </div>

            <div
                className="position-absolute d-flex align-items-center"
                style={{
                    left: '50vw',
                    transform: 'translateX(-50%)',
                    zIndex: 1
                }}
            >
                <i className="bi bi-stars me-2 text-primary"></i>
                <span className="fw-bold fs-5 text-dark">PetSpace</span>
            </div>

            <div
                className="d-flex align-items-center justify-content-end gap-2 px-3 ms-auto"
                style={{
                    width: '220px',
                    zIndex: 2,
                    overflow: 'hidden'
                }}
            >
                {showGreeting && token && userName && (
                    <span
                        className="text-muted small mb-0 text-truncate"
                        style={{ maxWidth: '110px' }}
                    >
                        Hello, {userName}
                    </span>
                )}

                {token && onLogout && (
                    <button
                        type="button"
                        className="btn btn-outline-danger btn-sm flex-shrink-0"
                        onClick={onLogout}
                    >
                        Logout
                    </button>
                )}
            </div>
        </header>
    )
}

export default AppHeader;