'use client';

import { useParams } from 'next/navigation';
import { Locale } from 'next-intl';
import { MouseEvent, startTransition } from 'react';

import { useRouter, usePathname } from '@/i18n/navigation';
import { routing } from '@/i18n/routing';

import { Button } from './ui/button';

const LocaleSwitcher = () => {
    const router = useRouter();
    const pathname = usePathname();
    const params = useParams();

    const onSelectChange = (event: MouseEvent<HTMLButtonElement>) => {
        const nextLocale = event.currentTarget.value as Locale;
        startTransition(() => {
            router.replace(
                // @ts-expect-error -- TypeScript will validate that only known `params`
                // are used in combination with a given `pathname`. Since the two will
                // always match for the current route, we can skip runtime checks.
                { pathname, params },
                { locale: nextLocale }
            );
        });
    };

    return (
        <div>
            <p>Locale switcher:</p>
            <ul>
                {routing.locales.map((locale) => (
                    <Button
                        key={locale}
                        onClick={onSelectChange}
                        value={locale}
                    >
                        {locale}
                    </Button>
                ))}
            </ul>
        </div>
    );
};

export default LocaleSwitcher;
