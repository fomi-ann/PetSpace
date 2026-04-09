import { Link } from 'react-router-dom';

const AppSidebar = ({ isOpen, onClose, links }) => {
    return (
        <>
            {isOpen && (
                <div
                    className="position-fixed top-0 start-0 w-100 h-100"
                    style={{
                        zIndex: 1040,
                        backgroundColor: 'rgba(0, 0, 0, 0.35)'
                    }}
                    onClick={onClose}
                />
            )}

            <aside
                className="position-fixed top-0 start-0 h-100 bg-white"
                style={{
                    width: '260px',
                    zIndex: 1050,
                    transform: isOpen ? 'translateX(0)' : 'translateX(-100%)',
                    transition: 'transform 0.3s ease-in-out'
                }}
            >
                <div className="d-flex align-items-center justify-content-between p-3 border-bottom">
                    <div className="d-flex align-items-center">
                        <i className="bi bi-stars me-2 text-primary"></i>
                        <span className="fw-bold fs-5">PetSpace</span>
                    </div>

                    <button
                        type="button"
                        className="btn btn-sm btn-danger"
                        onClick={onClose}
                        aria-label="Close navigation"
                    >
                        <i className="bi bi-x-lg"></i>
                    </button>
                </div>

                <nav className="p-3">
                    <ul className="list-unstyled m-0">
                        {links.map(link => (
                            <li key={link.path} className="mb-2">
                                <Link
                                    to={link.path}
                                    className="d-flex align-items-center gap-2 p-2 rounded text-dark"
                                    onClick={onClose}
                                >
                                    <i className={`bi bi-${link.icon}`}></i>
                                    <span>{link.label}</span>
                                </Link>
                            </li>
                        ))}
                    </ul>
                </nav>
            </aside>
        </>
    );
};

export default AppSidebar;