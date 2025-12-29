const fs = require(`fs-extra`)

const copyFiles = async () => {
  try {
    await fs.copy(
      `./node_modules/@pdftron/webviewer/public`,
      `./public/webviewer/lib`,
    )
    console.log(`WebViewer files copied over successfully`)

    await fs.copy(
      `./node_modules/pdfjs-dist/build/pdf.worker.min.mjs`,
      `./public/pdf.worker.min.mjs`,
    )
  } catch (err) {
    console.error(err)
  }
}

copyFiles()
