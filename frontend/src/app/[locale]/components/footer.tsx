import { Button } from './ui/button';

const Footer = () => {
    return (
        <div className="fixed bottom-0 w-full h-50">
            <hr />
            <div className="bg-gray-100 h-full">
                <div>Logo</div>
                <div className="container mx-auto flex items-center justify-between">
                    <Button variant="ghost" className="mr-1">
                        About Us
                    </Button>
                    <Button variant="ghost" className="mr-1">
                        Shipping
                    </Button>
                    <Button variant="ghost">Nutritional values</Button>
                </div>
            </div>
        </div>
    );
};

export default Footer;
