import { useParams } from 'react-router-dom'

export const JobDocuments = () => {
  const params = useParams()

  console.log(params)

  return (
    <>
      <div>documents</div>
    </>
  )
}
