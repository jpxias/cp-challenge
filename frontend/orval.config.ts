import { defineConfig } from "orval";

const API_BASE_URL = "https://localhost:7190/api";

export default defineConfig({
  civicPlus: {
    output: {
      baseUrl: process.env.API_URL ?? API_BASE_URL,
      mode: "tags-split",
      target: "src/api/models.ts",
      schemas: "src/models",
      client: "react-query",
      mock: false,
    },
    input: {
      target: "./swagger.json",
    },
  },
});
