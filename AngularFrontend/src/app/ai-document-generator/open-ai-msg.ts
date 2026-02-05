export interface OpenAiMsg {
  id?: string;
  title: string;
  createdDate?: Date;
  promptInput: string;
  language: string;
  maximumLength: string;
  creativity: string;
  toneOfVoice: string;
  selectedModel: string;
}
