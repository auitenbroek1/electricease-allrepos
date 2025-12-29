export type MessageContent = {
  type: string;
  text?: string;
  source?: {
    type: string;
    media_type: string;
    data: string;
    cache_control?: { type: string };
  };
  cache_control?: { type: string };
};

export type Message = {
  role: "user" | "assistant";
  content: MessageContent[];
};
