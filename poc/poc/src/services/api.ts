/* eslint-disable @typescript-eslint/no-explicit-any */
import Anthropic from "@anthropic-ai/sdk";
import { put, list } from "@vercel/blob";

// Mock flag - set to true to return mock responses
const USE_MOCK = process.env.USE_MOCK === "true";

// Mock response that will be returned when USE_MOCK is true
const MOCK_RESPONSE = {
  id: "mock_msg_123",
  type: "message",
  role: "assistant",
  content: [
    {
      type: "text",
      text: "This is a mock response that simulates the analysis of electrical blueprints. The mock includes standard sections like power distribution, lighting systems, and labor requirements.",
    },
  ],
  model: "claude-3-5-sonnet-latest",
};

// Retry configuration
const RETRY_DELAYS = [1000, 2000, 4000]; // Delays in ms between retries
const MAX_RETRIES = 3;

import { logEvent } from "./log";

import { Message } from "./api.types";

const anthropic = new Anthropic({
  apiKey: process.env.ANTHROPIC_API_KEY,
});

// Helper function to handle Anthropic API calls with retry logic
export async function callAnthropicWithRetry(
  messages: Message[],
  retryCount = 0
): Promise<Anthropic.Message> {
  if (USE_MOCK) {
    // Wait 3 seconds before returning mock response
    await new Promise((resolve) => setTimeout(resolve, 3000));
    return MOCK_RESPONSE as any;
  }

  try {
    const response = await anthropic.messages.create(
      {
        model: "claude-3-5-sonnet-latest",
        max_tokens: 4096,
        messages: messages as any,
        stream: false,
      },
      {
        headers: {
          "anthropic-beta": "pdfs-2024-09-25,prompt-caching-2024-07-31",
        },
      }
    );
    return response;
  } catch (error) {
    const isRateLimit =
      error instanceof Error &&
      "status" in error &&
      (error as any).status === 429;

    if (isRateLimit && retryCount < MAX_RETRIES) {
      const delay = RETRY_DELAYS[retryCount];
      logEvent("anthropic_rate_limit_retry", {
        retryCount,
        delay,
        error: error instanceof Error ? error.message : "Unknown error",
      });

      await new Promise((resolve) => setTimeout(resolve, delay));
      return callAnthropicWithRetry(messages, retryCount + 1);
    }

    throw error;
  }
}

export async function getCachedResponse(prompt_hash: string) {
  try {
    const { blobs } = await list({
      prefix: `cache/${prompt_hash}.txt`,
    });

    if (blobs.length > 0) {
      const response = await fetch(blobs[0].url).then((r) => r.text());
      return response;
    }
    return null;
  } catch (error) {
    logEvent("cache_get_error", {
      prompt_hash,
      error: error instanceof Error ? error.message : "Unknown error",
    });
    return null;
  }
}

export async function cacheResponse(prompt_hash: string, response: string) {
  try {
    await put(`cache/${prompt_hash}.txt`, response, {
      access: "public",
      addRandomSuffix: false,
    });
  } catch (error) {
    logEvent("cache_put_error", {
      prompt_hash,
      error: error instanceof Error ? error.message : "Unknown error",
    });
  }
}
