import { useTranslations } from 'next-intl';
import { HTMLAttributes } from 'react';

interface TTypographyProps
    extends Omit<HTMLAttributes<HTMLElement>, 'children'> {
    component: 'h1' | 'h2' | 'h3' | 'h4' | 'h5' | 'h6' | 'p' | 'span';
    namespace: string;
    messageKey: string;
    values?: Record<string, string | number>;
}

const TTypography = ({
    namespace,
    messageKey,
    values,
    component,
    ...props
}: TTypographyProps) => {
    const t = useTranslations(namespace);
    const Comp = component;

    return <Comp {...props}>{t(messageKey, values)}</Comp>;
};

export default TTypography;
