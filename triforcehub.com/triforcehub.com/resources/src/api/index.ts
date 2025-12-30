import { CopilotController } from "./copilot"
import { PartCategoryController } from "./PartCategoryController"
import { PartController } from "./PartController"
import { PartTagController } from "./PartTagController"

export const api = {
  copilot: CopilotController,
  parts: {
    ...PartController,
    categories: PartCategoryController,
    tags: PartTagController,
  },
}
