import axios from 'axios'
import { useQuery } from '@tanstack/react-query'
import TutorialModal from '@/components/Tutorial/TutorialModal'
import { useAuth } from '@/contexts/AuthContext'
import { useState } from 'react'

const Stats = (props: any) => {
  const { children, title } = props
  return (
    <div>
      <h3 className="text-lg font-medium leading-6 text-gray-900">{title}</h3>
      <div className="mt-5 grid grid-cols-4 gap-4">{children}</div>
    </div>
  )
}

const Stat = (props: any) => {
  const { name, number, red = false } = props

  return (
    <div className="overflow-hidden rounded-lg bg-white px-4 py-5 shadow sm:p-6">
      <div className="truncate text-sm font-medium text-gray-500">{name}</div>
      <div
        className={`mt-1 text-3xl font-semibold ${red ? `text-red-500` : ``}`}
      >
        {number}
      </div>
    </div>
  )
}

export const HomeIndex = () => {
  const { data } = useQuery([`dashboard.data`], async () => {
    const response = await axios.get(`/api/dashboard`)
    console.log(response.data)
    return response.data
  })

  const { user } = useAuth()
  console.log(`debug.home`, user)

  //

  const [muted, set_muted] = useState(true)

  const toggle_mute = () => {
    set_muted(!muted)
  }

  //

  return (
    <div>
      {user?.member.show_get_started ? (
        <>
          <div className="text-lg font-medium leading-6 text-gray-900 flex space-x-4 items-center">
            <div>Get Started</div>
            <div>
              <button
                className="text-brand-gradient-light flex space-x-2"
                type="button"
                onClick={toggle_mute}
              >
                {muted ? (
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    strokeWidth={1.5}
                    stroke="currentColor"
                    className="w-6 h-6"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="M17.25 9.75 19.5 12m0 0 2.25 2.25M19.5 12l2.25-2.25M19.5 12l-2.25 2.25m-10.5-6 4.72-4.72a.75.75 0 0 1 1.28.53v15.88a.75.75 0 0 1-1.28.53l-4.72-4.72H4.51c-.88 0-1.704-.507-1.938-1.354A9.009 9.009 0 0 1 2.25 12c0-.83.112-1.633.322-2.396C2.806 8.756 3.63 8.25 4.51 8.25H6.75Z"
                    />
                  </svg>
                ) : (
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    strokeWidth={1.5}
                    stroke="currentColor"
                    className="w-6 h-6"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="M19.114 5.636a9 9 0 0 1 0 12.728M16.463 8.288a5.25 5.25 0 0 1 0 7.424M6.75 8.25l4.72-4.72a.75.75 0 0 1 1.28.53v15.88a.75.75 0 0 1-1.28.53l-4.72-4.72H4.51c-.88 0-1.704-.507-1.938-1.354A9.009 9.009 0 0 1 2.25 12c0-.83.112-1.633.322-2.396C2.806 8.756 3.63 8.25 4.51 8.25H6.75Z"
                    />
                  </svg>
                )}
                <span>{muted ? `Unmute` : `Mute`}</span>
              </button>
            </div>
          </div>
          <div className="w-full max-w-3xl mt-4">
            <video
              autoPlay
              className="block w-full h-auto"
              controls
              muted={muted}
              src={`https://s3.us-east-2.amazonaws.com/media.triforcehub.com/red/00.mp4`}
            />
          </div>
          <hr className={`my-8`} />
        </>
      ) : (
        ``
      )}
      <Stats title={`Upcoming Due Bids`}>
        <Stat
          name={`Less than 7 days`}
          number={data?.upcoming?.within7days ?? 0}
          red={false}
        />
        <Stat
          name={`Less than 30 days`}
          number={data?.upcoming?.within30days ?? 0}
        />
      </Stats>
      <hr className={`my-8`} />
      <Stats title={`Bids Completed`}>
        <Stat
          name={`Completed`}
          number={data?.completed?.all ?? 0}
        />
      </Stats>
      <hr className={`my-8`} />
      <Stats title={`Bids Won/Lost`}>
        <Stat
          name={`Won`}
          number={data?.won?.all ?? 0}
        />
        <Stat
          name={`Lost`}
          number={data?.lost?.all ?? 0}
        />
      </Stats>
    </div>
  )
}
