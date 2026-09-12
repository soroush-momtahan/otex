import Alpine from 'alpinejs';
import morph from '@alpinejs/morph';


Alpine.plugin(morph);

window.Alpine = Alpine;

// 1. Auto-import
import './main.css';
import.meta.glob('../Features/**/*.cshtml.css', { eager: true });
import.meta.glob('../Core/**/*.cshtml.css', { eager: true });
import.meta.glob('../Pages/**/*.cshtml.css', { eager: true });

const featureScripts = import.meta.glob('../Features/**/*.cshtml.ts', { eager: true });
const coreScripts = import.meta.glob('../Core/**/*.cshtml.ts', { eager: true });
const pageScripts = import.meta.glob('../Pages/**/*.cshtml.ts', { eager: true });

const allScripts = [
    ...Object.values(featureScripts),
    ...Object.values(coreScripts),
    ...Object.values(pageScripts)
];

// یک اینترفیس برای ماژول‌هایی که تابعی برای رجیستر کردن دارند
interface AlpineModule {
    register?: (alpine: typeof Alpine) => void;
}

allScripts.forEach((module: unknown) => {
    const alpineModule = module as AlpineModule;
    if (typeof alpineModule.register === 'function') {
        alpineModule.register(Alpine);
    }
});

// Start Alpine
// window.Alpine = Alpine; // الان دیگه این خط ارور نمیده!
// وقتی مطمئن شدیم DOM کاملاً لود شده، Alpine رو استارت می‌کنیم
// document.addEventListener('DOMContentLoaded', () => {
//     Alpine.start();
// });
Alpine.start();