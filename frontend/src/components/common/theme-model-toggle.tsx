'use client';

import { Moon, Sun } from 'lucide-react';
import { useTheme } from 'next-themes';
import * as React from 'react';

import { Button } from './ui/button';
import { Tooltip, TooltipContent, TooltipTrigger } from './ui/tooltip';

export function ModeToggle() {
    const { setTheme, theme } = useTheme();
    const isLightMode = theme === 'light';

    const handleToggle = () => setTheme(isLightMode ? 'dark' : 'light');

    return (
        <Tooltip>
            <TooltipTrigger asChild>
                <Button variant="outline" size="icon" onClick={handleToggle}>
                    <Sun className="h-[1.2rem] w-[1.2rem] scale-100 rotate-0 transition-all dark:scale-0 dark:-rotate-90" />
                    <Moon className="absolute h-[1.2rem] w-[1.2rem] scale-0 rotate-90 transition-all dark:scale-100 dark:rotate-0" />
                </Button>
            </TooltipTrigger>
            <TooltipContent>
                <p>Toggle theme</p>
            </TooltipContent>
        </Tooltip>
    );
}
