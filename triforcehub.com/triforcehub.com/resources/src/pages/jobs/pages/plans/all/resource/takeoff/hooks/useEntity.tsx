import { useState } from 'react'

export const useEntity = (entity: any) => {
  const [annotation_type, set_annotation_type] = useState<any>(
    entity.annotation_type,
  )

  const [annotation_symbol_id, set_annotation_symbol_id] = useState<any>(
    entity.annotation_symbol_id,
  )

  const [annotation_color, set_annotation_color] = useState<any>(
    entity.annotation_color,
  )

  const [annotation_length, set_annotation_length] = useState<any>(
    entity.annotation_length,
  )

  return {
    annotation_type,
    set_annotation_type,

    annotation_symbol_id,
    set_annotation_symbol_id,

    annotation_color,
    set_annotation_color,

    annotation_length,
    set_annotation_length,
  }
}
