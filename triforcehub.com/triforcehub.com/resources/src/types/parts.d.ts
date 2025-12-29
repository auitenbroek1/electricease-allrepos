interface PartCategory {
  id: number
  name: string
  description: string
}

interface Part {
  id: number
  name: string
  description: string
  categories: PartCategory[]
}

/* new: needs to match laravel resources */

// #region requests

type SavePartRequest = {
  categories?: string[] | undefined
  cost: number | undefined
  description?: string | undefined
  labor: number | undefined
  name: string | undefined
  tags?: string[] | undefined
}

type SavePartArguments = {
  id: number | undefined
  params: SavePartRequest
}

// #endregion

// #region resources

type PartResource = {
  assemblies?: AssembyResource[]
  categories?: PartCategoryResource[]
  cost: number
  description?: string
  id: number
  labor: number
  name: string
  tags?: PartTagResource[]
}

// category

type PartCategoryPlaceholder = {
  description: string | undefined
  id: number | undefined
  name: string | undefined
}

type PartCategoryResource = {
  description?: string
  id: number
  name: string
}

// tag

type PartTagResource = {
  description?: string
  id: number
  name: string
}

type PartTagPlaceholder = {
  description: string | undefined
  id: number | undefined
  name: string | undefined
}

// #endregion
