import { Facebook, Instagram, Youtube } from 'lucide-react';
import Image from 'next/image';

import TButton from './i18n/TButton';
import { Separator } from './ui/separator';

const Footer = () => {
    return (
        <div className="bg-inherit w-full h-auto">
            <Separator />
            <div>
                <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-6 p-6 container mx-auto">
                    <div className="flex flex-col items-start">
                        <div className="flex items-center gap-3 mb-4">
                            <Image
                                src="/logo.svg"
                                alt="Logo"
                                width={32}
                                height={24}
                            />
                            <h3 className="font-bold text-lg">MEOWCCHIATO</h3>
                        </div>
                    </div>

                    <div className="flex flex-col">
                        <TButton
                            namespace="footer"
                            messageKey="about"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                        <TButton
                            namespace="footer"
                            messageKey="locations"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                        <TButton
                            namespace="footer"
                            messageKey="products"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                        <TButton
                            namespace="footer"
                            messageKey="coffee"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                    </div>

                    <div className="flex flex-col">
                        <TButton
                            namespace="footer"
                            messageKey="shipping"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                        <TButton
                            namespace="footer"
                            messageKey="gdpr"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                        <TButton
                            namespace="footer"
                            messageKey="careers"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                        <TButton
                            namespace="footer"
                            messageKey="faq"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                    </div>

                    <div className="flex flex-col">
                        <TButton
                            namespace="footer"
                            messageKey="nutritional_values"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                        <TButton
                            namespace="footer"
                            messageKey="privacy_policy"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                        <TButton
                            namespace="footer"
                            messageKey="cookies_policy"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                        <TButton
                            namespace="footer"
                            messageKey="terms_conditions"
                            variant="link"
                            className="justify-start text-foreground"
                        />
                    </div>

                    <div className="flex flex-col items-start">
                        <div className="flex space-x-4">
                            <Facebook />
                            <Instagram />
                            <Youtube />
                        </div>
                    </div>
                </div>

                <Separator className="w-5/6 mx-auto" />
                <div className="flex justify-center p-4">
                    <p className="text-sm ">
                        ©Copyright 2025 - Meowcchiato all rights reserved.
                    </p>
                </div>
            </div>
        </div>
    );
};

export default Footer;
