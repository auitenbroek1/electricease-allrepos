export const maxDuration = 300;
export const dynamic = "force-dynamic"; // always run dynamically

import { NextResponse } from "next/server";
import { callAnthropicWithRetry } from "@/services/api";
import { Message } from "@/services/api.types";
import { estimateWorkflow } from "@/services/estimate";
import { createHash } from "crypto";
import { getCachedResponse, cacheResponse } from "@/services/api";

export async function POST(
  req: Request,
  { params }: { params: Promise<{ section: string; step: string }> }
) {
  const { section, step } = await params;

  const section_index = parseInt(section) - 1;
  const step_index = parseInt(step) - 1;

  //

  const { url } = await req.json();

  const url_hash = createHash("sha256").update(url).digest("hex");

  const buffer = await fetch(url).then((res) => res.arrayBuffer());
  const data = Buffer.from(buffer).toString("base64");

  const messages: Message[] = [
    {
      role: "user",
      content: [
        {
          type: "text",
          text: "You are an expert electrical estimator. Analyze these blueprints thoroughly. We will proceed through multiple analysis steps to generate a complete estimate.",
        },
        {
          type: "document",
          source: {
            type: "base64",
            media_type: "application/pdf",
            data: data,
          },
          cache_control: { type: "ephemeral" },
        },
      ],
    },
  ];

  const upto_section_index = section_index;
  console.log("upto_section_index", upto_section_index);

  for (let i = 0; i <= upto_section_index; i++) {
    const upto_step_index =
      i === upto_section_index
        ? step_index
        : estimateWorkflow[i].steps.length - 1;
    console.log("upto_step_index", upto_step_index);
    for (let j = 0; j <= upto_step_index; j++) {
      // console.log("getting prompt", i + 1, j + 1);
      const prompt = estimateWorkflow[i].steps[j].prompt;
      messages.push({
        role: "user",
        content: [{ type: "text", text: prompt }],
      });

      // get the prompt hash
      const prompt_hash = createHash("sha256").update(prompt).digest("hex");
      // console.log("prompt_hash", prompt_hash);
      const cachedResponse = await getCachedResponse(url_hash + prompt_hash);
      if (cachedResponse) {
        messages.push({
          role: "assistant",
          content: [
            {
              type: "text",
              text: cachedResponse,
              // text: `cached response for section ${i + 1} step ${
              //   j + 1
              // }, as a uuid`,
            },
          ],
        });
      }
    }
  }

  // check all messages, and if it ends with a user prompt, call the anthropic api
  if (messages[messages.length - 1].role === "user") {
    const response = await callAnthropicWithRetry(messages);
    const responseText =
      response.content[0].type === "text" ? response.content[0].text : "";

    const prompt_text = messages[messages.length - 1].content[0].text;
    if (prompt_text) {
      const prompt_hash = createHash("sha256")
        .update(prompt_text)
        .digest("hex");
      await cacheResponse(url_hash + prompt_hash, responseText);

      messages.push({
        role: "assistant",
        content: [{ type: "text", text: responseText }],
      });
    }
  }

  const section_title = estimateWorkflow[section_index].title;
  const step_prompt = estimateWorkflow[section_index].steps[step_index].prompt;

  // find last assistant message
  const last_assistant_message = messages.findLast(
    (message) => message.role === "assistant"
  );

  return NextResponse.json({
    section_index,
    step_index,
    section: section_title,
    step: step_prompt,
    message: last_assistant_message?.content[0].text,
  });
}
