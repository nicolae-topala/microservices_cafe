import { getTranslations } from 'next-intl/server';

import { auth } from '@/auth';
import TButton from '@/components/common/i18n/TButton';
import LocaleSwitcher from '@/components/common/locale-switcher';

const DashboardPage = async () => {
    const session = await auth();
    const t = await getTranslations('dashboard');

    return (
        <div>
            <div>
                <h1>Dashboard</h1>
                <p>User session:</p>
                <p>{JSON.stringify(session?.user)}</p>
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

export default DashboardPage;
