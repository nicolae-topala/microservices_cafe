import { NextRequest, NextResponse } from 'next/server';
import createMiddleware from 'next-intl/middleware';

import { auth } from '@/auth';
import { SIGN_IN, PUBLIC_ROUTES, DEFAULT_REDIRECT } from '@/lib/routes';

import { routing } from './i18n/routing';

const testPathnameRegex = (
    routes: string[] | string,
    pathName: string
): boolean => {
    const routesArray = Array.isArray(routes) ? routes : [routes];
    return RegExp(
        `^(/(${routing.locales.join('|')}))?(${routesArray.flatMap((route) => (route === '/' ? ['', '/'] : route)).join('|')})/?$`,
        'i'
    ).test(pathName);
};

const handleI18nRouting = createMiddleware(routing);

const authMiddleware = auth((req) => {
    const { nextUrl } = req;
    // Check if the user is authenticated (i.e., if the 'auth' property exists in the request)
    const isAuthenticated = !!req.auth;
    const isPublicRoute = testPathnameRegex(PUBLIC_ROUTES, nextUrl.pathname);
    const isSignInRoute = testPathnameRegex(SIGN_IN, nextUrl.pathname);

    // If the route is sign in and the user is authenticated, redirect them to the root default page
    if (isAuthenticated && isSignInRoute) {
        const newUrl = new URL(DEFAULT_REDIRECT, nextUrl.origin);
        return NextResponse.redirect(newUrl);
    }

    // If the route is not public and the user is not authenticated, redirect them to the login page
    if (!isAuthenticated && !isPublicRoute) {
        const newUrl = new URL(SIGN_IN, nextUrl.origin);
        return NextResponse.redirect(newUrl);
    }

    return handleI18nRouting(req);
});

const middleware = (req: NextRequest) => {
    const { nextUrl } = req;
    const isPublicRoute = testPathnameRegex(PUBLIC_ROUTES, nextUrl.pathname);

    if (isPublicRoute) {
        return handleI18nRouting(req); // Apply internationalization for public pages
    }

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    return (authMiddleware as any)(req); // Apply authentication logic for non-public pages
};

// Read more: https://nextjs.org/docs/app/building-your-application/routing/middleware#matcher
export const config = {
    matcher: ['/((?!api|_next/static|_next/image|favicon.ico).*)'],
};

export default middleware;
