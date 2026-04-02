import { defineConfig } from "orval";

export default defineConfig({
  civicPlus: {
    output: {
      baseUrl: "http://localhost:7190",
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
