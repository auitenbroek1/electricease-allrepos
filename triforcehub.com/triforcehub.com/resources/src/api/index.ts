import { PartCategoryController } from "./PartCategoryController"
import { PartController } from "./PartController"
import { PartTagController } from "./PartTagController"

export const api = {
  parts: {
    ...PartController,
    categories: PartCategoryController,
    tags: PartTagController,
  },
}
