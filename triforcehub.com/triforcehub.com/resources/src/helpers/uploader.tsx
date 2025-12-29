import axios from 'axios'

const assetUrl = ``

class Vapor {
  /**
   * Generate the S3 URL to an application asset.
   */
  asset(path: any) {
    return assetUrl + `/` + path
  }

  /**
   * Store a file in S3 and return its UUID, key, and other information.
   */
  async store(file: any, options: any = {}) {
    const response = await axios.post(
      options.signedStorageUrl
        ? options.signedStorageUrl
        : `/vapor/signed-storage-url`,
      {
        bucket: options.bucket || ``,
        content_type: options.contentType || file.type,
        expires: options.expires || ``,
        visibility: options.visibility || ``,
        ...options.data,
      },
      {
        baseURL: options.baseURL || null,
        headers: options.headers || {},
        ...options.options,
      },
    )

    const headers = response.data.headers

    if (`Host` in headers) {
      delete headers.Host
    }

    if (typeof options.progress === `undefined`) {
      options.progress = () => {}
    }

    const cancelToken = options.cancelToken || ``

    await axios
      .put(response.data.url, file, {
        cancelToken: cancelToken,
        headers: headers,
        onUploadProgress: (progressEvent: any) => {
          options.progress(progressEvent.loaded / progressEvent.total)
        },
      })
      .catch((error) => {
        console.log(error)
        console.dir(error.response.data)
      })

    response.data.extension = file.name?.split(`.`).pop() || `pdf`

    return response.data
  }
}

export const uploader = new Vapor()
