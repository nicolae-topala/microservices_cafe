//next-auth.js.org/getting-started/typescript#module-augmentation
Ref: https: import { DefaultSession, DefaultUser } from 'next-auth';
import { DefaultJWT } from 'next-auth/jwt';

declare module 'next-auth' {
    interface Session {
        user: {
            id: string;
            username: string;
            name: string;
            email: string;
        } & DefaultSession;
    }

    interface User extends DefaultUser {
        username: string;
    }
}

declare module 'next-auth/jwt' {
    interface JWT extends DefaultJWT {
        username: string;
    }
}
