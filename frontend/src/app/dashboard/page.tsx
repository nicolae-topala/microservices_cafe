import { auth } from '@/auth';

const DashboardPage = async () => {
    const session = await auth();

    return (
        <div>
            <div>
                <h1>Dashboard</h1>
                <p>User session:</p>
                <p>{JSON.stringify(session?.user)}</p>
            </div>
        </div>
    );
};

export default DashboardPage;
