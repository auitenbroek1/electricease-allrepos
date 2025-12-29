export const sleep = async (ms: any = 1000) => {
  return new Promise((resolve: any) => {
    setTimeout(() => {
      resolve()
    }, ms)
  })
}
