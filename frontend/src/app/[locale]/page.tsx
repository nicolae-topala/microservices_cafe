import { auth } from '@/auth';
import TButton from '@/components/common/i18n/TButton';
import LocaleSwitcher from '@/components/common/locale-switcher';

const Home = async () => {
    const session = await auth();

    return (
        <div>
            <h1>Test</h1>
            <h2>Test</h2>
            <h3>Test</h3>
            <h4>Test</h4>
            <p>Test</p>
            <div>
                Current seesion: {session?.user?.email}
                {session?.user?.id}
            </div>

            <br />
            <div>
                <LocaleSwitcher />
            </div>
            <TButton namespace="locale" messageKey="en" />
            <br />
        </div>
    );
};

export default Home;
