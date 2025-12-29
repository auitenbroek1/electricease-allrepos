const getRange = (start: number, end: number) => {
  return Array(end - start + 1)
    .fill(null)
    .map((v, i) => i + start)
}

const generatePaginationRange = (current: number, total: number, siblings: number = 3) => {
  let padding = 2 // dots plus first
  let threshold = 1 + 1 + siblings + 1 + siblings + 1 + 1

  if (total <= threshold) {
    return getRange(1, total)
  }

  //

  let start = current - siblings - padding
  let startDelta = 0
  if (start < 1) {
    startDelta = 1 - start
    start = 1
  }

  let end = current + siblings + padding + startDelta
  let endDelta = 0
  if (end > total) {
    endDelta = end - total
    end = total
    start = start - endDelta
  }

  //

  // console.log(start, end)
  const range:any[] = getRange(start, end)

  // show dots only if more than 1 slot is being replaced
  // i.e
  // 1 ... 4 5 6 7 ( 7 is current)
  if (current >= siblings + 3) {
    range[0] = 1
    range[1] = '...'
  }

  if (current <= total - siblings - 3) {
    range[range.length - 1] = total
    range[range.length - 2] = '...'
  }

  return range
}

export {
  generatePaginationRange,
}
