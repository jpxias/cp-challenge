import { defineConfig } from "orval";
const API_BASE_URL = "http://localhost:7190/api";
const API_BASE_AWS = "";
export default defineConfig({
  civicPlus: {
    output: {
      baseUrl: API_BASE_URL,
      mode: "tags-split",
      target: "src/api/models.ts",
      schemas: "src/models",
      client: "react-query",
      mock: false,
    },
    input: {
      target: "../backend/CivicPlusChallenge/swagger.json",
    },
  },
});
