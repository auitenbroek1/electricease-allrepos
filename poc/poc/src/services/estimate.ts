import { EstimateSection } from "./estimate.types";

// Define the analysis workflow
export const estimateWorkflow: EstimateSection[] = [
  {
    enabled: true,
    title: "Power Distribution",
    steps: [
      {
        prompt:
          "Analyze all electrical panels, transformers, and distribution equipment. List their locations, ratings, and specifications.",
      },
      {
        prompt:
          "Calculate total amperage and determine service entrance requirements.",
      },
    ],
  },
  {
    enabled: true,
    title: "Lighting Systems",
    steps: [
      {
        prompt:
          "Identify and count all lighting fixtures. Group by type, location, and mounting method.",
      },
      {
        prompt: "Calculate total lighting load and required circuits.",
      },
    ],
  },
  {
    enabled: true,
    title: "Branch Circuits",
    steps: [
      {
        prompt:
          "List all receptacles, equipment connections, and special circuits.",
      },
      {
        prompt: "Calculate conduit requirements and wire lengths.",
      },
    ],
  },
  {
    enabled: true,
    title: "Labor Requirements",
    steps: [
      {
        prompt:
          "Based on the electrical components identified, list all required labor roles (e.g., master electrician, journeyman, apprentice) and their specific responsibilities.",
      },
      {
        prompt:
          "Calculate labor hours for each major task: panel installation, conduit runs, wire pulling, device installation, and testing. Break down by skill level required.",
      },
      {
        prompt:
          "Identify any specialized certifications or qualifications needed for specific tasks, and estimate additional labor costs for specialized work.",
      },
      {
        prompt:
          "Calculate total labor hours by trade level, including time for coordination, safety meetings, and quality control inspections.",
      },
    ],
  },
  {
    enabled: true,
    title: "References and Resources",
    steps: [
      {
        prompt:
          "List all electrical codes, standards, and regulations referenced in this estimate (e.g., NEC, local codes, IEEE standards).",
      },
      {
        prompt:
          "Document all pricing sources used for materials and labor rates (e.g., distributor price lists, industry databases, local wage rates).",
      },
      {
        prompt:
          "List any manufacturer specifications, installation guides, or technical documentation referenced for specialized equipment.",
      },
      {
        prompt:
          "Document any industry-standard labor unit rates or productivity factors used in calculating labor hours.",
      },
      {
        prompt:
          "Summarize any assumptions made during the estimation process and their sources or justifications.",
      },
    ],
  },
  {
    enabled: true,
    title: "Summary",
    steps: [
      {
        prompt:
          "Based on all the previous analysis, generate a complete electrical estimate including material costs, labor hours, and total bid amount. Include any areas of uncertainty or items requiring verification.",
      },
    ],
  },
];
