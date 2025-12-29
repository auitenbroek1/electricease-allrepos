// Define types for better type safety
export type AnalysisStep = {
  prompt: string;
};

export type EstimateSection = {
  enabled: boolean;
  title: string;
  steps: AnalysisStep[];
};
