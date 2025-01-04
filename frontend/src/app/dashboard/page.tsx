import { auth } from '@/auth';
import { Header } from '@/components/header';

const DashboardPage = async () => {
    const session = await auth();

    return (
        <div>
            <Header />
            <div>
                <h1>Dashboard</h1>
                <p>User session:</p>
                <p>{JSON.stringify(session?.user)}</p>
            </div>
        </div>
    );
};

export default DashboardPage;
