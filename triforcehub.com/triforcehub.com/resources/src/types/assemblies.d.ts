interface AssemblyCategory {
  id: number;
  name: string;
  description: string;
}

interface Assembly {
  id: number;
  name: string;
  description: string;
  categories: AssemblyCategory[];
}

/* new: needs to match laravel resources */

interface AssembyResource {
  id: number | null
  name: string | null
  description?: string | null
}
