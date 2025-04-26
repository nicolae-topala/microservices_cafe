import { useTranslations } from 'next-intl';

import { Button, ButtonProps } from '../ui/button';

interface TButtonProps extends Omit<ButtonProps, 'children'> {
    namespace: string;
    messageKey: string;
    values?: Record<string, string | number>;
}

const TButton = ({ namespace, messageKey, values, ...props }: TButtonProps) => {
    const t = useTranslations(namespace);

    return <Button {...props}>{t(messageKey, values)}</Button>;
};

export default TButton;
