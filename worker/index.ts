import { Container, getContainer } from "@cloudflare/containers";

export class TestApiContainer extends Container {
  defaultPort = 8080;   // port the ASP.NET app listens on
  sleepAfter = "10m";   // stop container after 10 min idle
}

interface Env {
  TEST_API: DurableObjectNamespace<TestApiContainer>;
}

export default {
  async fetch(request: Request, env: Env): Promise<Response> {
    // single shared instance for all requests
    return getContainer(env.TEST_API).fetch(request);
  },
};
