import { defineConfig } from 'vite';
import tailwindcss from "@tailwindcss/vite";

export default defineConfig(({ command }) => ({
    appType: 'custom',
    plugins: [
        tailwindcss(),
    ],
    base: '/dist/',
    // base: command === 'serve' ? '/' : '/dist/',
    build: {
        outDir: 'wwwroot/dist',
        emptyOutDir: true,
        manifest: true,
        rollupOptions: {
            input: {
                main: 'Assets/main.ts' // Vite از همین یک فایل شروع میکنه و به بقیه میرسه
            }
        }
    },
    server: {
        port: 5173,
        strictPort: true,
        hmr: {
            clientPort: 5173
        }
    }
}));