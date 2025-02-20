import { auth } from '@/auth';

const Home = async () => {
    const session = await auth();

    return (
        <div>
            <div>
                Current seesion: {session?.user?.email}
                {session?.user?.id}
            </div>
        </div>
    );
};

export default Home;
