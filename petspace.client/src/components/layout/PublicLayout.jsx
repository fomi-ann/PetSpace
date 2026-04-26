import AppHeader from './AppHeader';



const PublicLayout = ({ children }) => {
    return (
        <>
            <AppHeader showMenu={false} showGreeting={false} />
            <main>
                {children}
            </main>
        </>
    );
};

export default PublicLayout;