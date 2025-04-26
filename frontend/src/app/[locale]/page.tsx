import { getTranslations } from 'next-intl/server';

import { auth } from '@/auth';

import TButton from './components/i18n/TButton';
import LocaleSwitcher from './components/localeSwitcher';

const Home = async () => {
    const session = await auth();
    const t = await getTranslations('dashboard');

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
            <TButton namespace="dashboard" messageKey="welcome" />
            <br />
            {t('welcome')}
        </div>
    );
};

export default Home;
