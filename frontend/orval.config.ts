import { defineConfig } from "orval";

export default defineConfig({
  civicPlus: {
    output: {
      baseUrl: "https://localhost:7190",
      mode: "tags-split",
      target: "src/api/models.ts",
      schemas: "src/models",
      client: "react-query",
      mock: false,
    },
    input: {
      target: "../CivicPlusChallenge/swagger.json",
    },
  },
});
