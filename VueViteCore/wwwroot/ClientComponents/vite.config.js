import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  // server: {
  //   hmr:false
  // },
  build: {
    manifest:true,
    // outDir:'../dist',
    rollupOptions: {
      input:'./src/main.js',
      output: {
        chunkFileNames:"dist/assets/[name].[hash].js",
        entryFileNames:"dist/[name].js",
        assetFileNames: "dist/assets/[name].[ext]"
      }
    }
  }
})
