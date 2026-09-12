// 1. معرفی تایپ‌های اختصاصی Vite به TypeScript (حل مشکل glob)
/// <reference types="vite/client" />

// 2. معرفی Alpine به شیء Window (حل مشکل window.Alpine)
import type { Alpine as AlpineType } from 'alpinejs';

declare global {
    interface Window {
        Alpine: AlpineType;
    }
}

declare module '@alpinejs/morph';