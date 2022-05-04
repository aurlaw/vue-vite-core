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
        entryFileNames:"[name].js",
        assetFileNames: "assets/[name].[ext]"
      }
    }
  }
})
