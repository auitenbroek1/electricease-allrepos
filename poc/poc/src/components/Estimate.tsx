"use client";

import { upload } from "@vercel/blob/client";
import { useState, useRef } from "react";

type EstimateStep = {
  prompt: string;
  response: string;
};

type Estimate = {
  sections: {
    [key: string]: EstimateStep[];
  };
};

type AssistantMessage = {
  section_index: number;
  step_index: number;
  section: string;
  step: string;
  message: string;
};

import { estimateWorkflow } from "@/services/estimate";

type StepStatus = {
  completed: boolean;
  loading: boolean;
  error?: string;
  timestamp?: Date;
};

type WorkflowStatus = {
  [sectionIndex: number]: {
    [stepIndex: number]: StepStatus;
  };
};

export const Estimate = () => {
  // const test_url = `https://8sp4u32ekaflalq8.public.blob.vercel-storage.com/05-ELECTRICAL-2023_02-13%20Lot%2010%20Restaurant%20Group%20TI_Permit%20Package%202.14.23-HbFjryuY2RtTr2RTb0hDibQTtRyavb.pdf`;
  const test_url = null;

  const inputFileRef = useRef<HTMLInputElement>(null);
  const [blobUrl, setBlobUrl] = useState<string | null>(test_url);
  const [loading, setLoading] = useState<boolean>(false);
  const [workflowStatus, setWorkflowStatus] = useState<WorkflowStatus>({});

  const handleEstimate = async () => {
    setLoading(true);
    // Reset workflow status
    setWorkflowStatus({});

    if (!blobUrl) return;

    try {
      for (
        let section_index = 0;
        section_index < estimateWorkflow.length;
        section_index++
      ) {
        const section = estimateWorkflow[section_index];
        for (
          let step_index = 0;
          step_index < section.steps.length;
          step_index++
        ) {
          // Set loading state
          setWorkflowStatus((prev) => ({
            ...prev,
            [section_index]: {
              ...prev[section_index],
              [step_index]: { loading: true, completed: false },
            },
          }));

          try {
            await handleProcessStep(section_index, step_index);
            // Set completed state
            setWorkflowStatus((prev) => ({
              ...prev,
              [section_index]: {
                ...prev[section_index],
                [step_index]: {
                  loading: false,
                  completed: true,
                  timestamp: new Date(),
                },
              },
            }));
          } catch (error) {
            // Set error state
            setWorkflowStatus((prev) => ({
              ...prev,
              [section_index]: {
                ...prev[section_index],
                [step_index]: {
                  loading: false,
                  completed: false,
                  error:
                    error instanceof Error
                      ? error.message
                      : "An error occurred",
                },
              },
            }));
          }
          await new Promise((resolve) => setTimeout(resolve, 3 * 1000));
        }
      }
    } catch (error) {
      console.error("Error during estimation:", error);
    } finally {
      setLoading(false);
    }
  };

  const [responses, setResponses] = useState<AssistantMessage[]>([]);

  const handleProcessStep = async (
    section_index: number,
    step_index: number
  ) => {
    const section = section_index + 1;
    const step = step_index + 1;

    const response = await fetch(`/api/estimate/${section}/${step}`, {
      method: "POST",
      body: JSON.stringify({ url: blobUrl }),
    });

    const data: AssistantMessage = await response.json();
    setResponses((prevResponses) => [...prevResponses, data]);
  };

  return (
    <div className="space-y-8">
      <h1 className="text-3xl font-bold">Electric Ease POC</h1>
      {!blobUrl && (
        <form
          className="space-y-4"
          onSubmit={async (event) => {
            event.preventDefault();

            if (!inputFileRef.current?.files) {
              throw new Error("No file selected");
            }

            const file = inputFileRef.current.files[0];

            const newBlob = await upload(file.name, file, {
              access: "public",
              handleUploadUrl: "/api/upload",
            });

            setBlobUrl(newBlob.url);
          }}
        >
          <div className="">
            <input name="file" ref={inputFileRef} type="file" required />
          </div>
          <div>
            <button
              className={`bg-blue-500 text-white py-2 px-4 rounded-md`}
              type="submit"
            >
              Upload
            </button>
          </div>
        </form>
      )}
      {blobUrl && (
        <div>
          <a className="text-blue-500 underline" href={blobUrl}>
            {blobUrl}
          </a>
        </div>
      )}
      {blobUrl && (
        <div>
          <button
            className={`bg-blue-500 text-white py-2 px-4 rounded-md ${
              loading ? "opacity-50 pointer-events-none" : ""
            }`}
            disabled={loading}
            type="button"
            onClick={handleEstimate}
          >
            {loading ? "Estimating..." : "Estimate"}
          </button>
        </div>
      )}

      {blobUrl && (
        <div className="space-y-4">
          <h2 className="text-2xl font-bold">Workflow Progress</h2>
          {estimateWorkflow.map((section, section_index) => (
            <div
              key={section.title}
              className="border rounded-lg p-4 space-y-4"
            >
              <h3 className="text-xl font-bold leading-none">
                {section_index + 1}: {section.title}
              </h3>
              <div className="space-y-4">
                {section.steps.map((step, step_index) => {
                  const status = workflowStatus[section_index]?.[step_index];
                  return (
                    <div key={step.prompt} className="pt-4 border-t space-y-2">
                      <div className="flex items-center space-x-2">
                        <div className="text-sm text-gray-500">
                          {step.prompt}
                          {` `}
                          {!status && (
                            <span className="text-gray-500 whitespace-nowrap">
                              Pending
                            </span>
                          )}
                          {status?.loading && (
                            <span className="text-yellow-500 whitespace-nowrap">
                              Processing...
                            </span>
                          )}
                          {status?.completed && (
                            <span className="text-green-500 whitespace-nowrap">
                              ✓ Complete
                            </span>
                          )}
                          {status?.error && (
                            <span className="text-red-500">
                              ⚠ Error: {status.error}
                            </span>
                          )}
                        </div>
                      </div>
                      <div className="whitespace-pre-wrap">
                        {
                          responses.find(
                            (response) =>
                              response.section_index === section_index &&
                              response.step_index === step_index
                          )?.message
                        }
                      </div>
                    </div>
                  );
                })}
              </div>
            </div>
          ))}
        </div>
      )}

      {false &&
        estimateWorkflow.map((section, section_index) => (
          <div key={section.title}>
            <h2 className="text-2xl font-bold">
              {section_index + 1}: {section.title}
            </h2>
            <ol className="list-decimal list-inside">
              {section.steps.map((step, step_index) => (
                <li className="flex items-center space-x-2" key={step.prompt}>
                  <div>
                    <button
                      onClick={() =>
                        handleProcessStep(section_index, step_index)
                      }
                      type="button"
                    >
                      Process
                    </button>
                  </div>
                  <div>
                    {step_index + 1}: {step.prompt}
                  </div>
                </li>
              ))}
            </ol>
          </div>
        ))}
    </div>
  );
};
