import { hasLocale } from 'next-intl';
import { getRequestConfig } from 'next-intl/server';

import { routing } from './routing';

export default getRequestConfig(async ({ requestLocale }) => {
    const requested = await requestLocale;
    const locale = hasLocale(routing.locales, requested)
        ? requested
        : routing.defaultLocale;

    const [common, navigation, forms, dashboard, auth, filters] =
        await Promise.all([
            import(`../../messages/${locale}/common.json`),
            import(`../../messages/${locale}/navigation.json`),
            import(`../../messages/${locale}/forms.json`),
            import(`../../messages/${locale}/dashboard.json`),
            import(`../../messages/${locale}/auth.json`),
            import(`../../messages/${locale}/filters.json`),
        ]);

    return {
        locale,
        messages: {
            ...common.default,
            ...navigation.default,
            ...forms.default,
            ...dashboard.default,
            ...auth.default,
            ...filters.default,
        },
    };
});
