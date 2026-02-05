export interface OpenAiMsgResponse {
  id?: string;
  aiResponse?: string;
  title: string;
  promptInput: string;
  language: string;
  maximumLength: string;
  creativity: string;
  toneOfVoice: string;
  selectedModel: string;
}
