// console.log(import.meta.env.VITE_API_BASE_URL);  // Debugging line
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || "http://localhost:9000";

const config = {
    API_BASE_URL: API_BASE_URL
};

export default config;