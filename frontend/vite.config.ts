import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

export default defineConfig({
    plugins: [plugin()],
    server: {
        port: 11840,
        proxy: {
            '/api': {
                target: 'http://localhost:5241',
                changeOrigin: true,
            }
        }
    }
})
