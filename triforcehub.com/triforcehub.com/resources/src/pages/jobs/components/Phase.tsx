import { PageSectionItem } from '@/components'

import Assemblies from './Assemblies'
import Parts from './Parts'

import PhaseContext from '../contexts/Phase'
import PhaseActions from './PhaseActions'

const Phase = (props: any) => {
  const { item, withActions = false } = props

  let name: any = []
  name.push(item.name)
  if (item?.area?.length) name.push(item.area)
  if (item?.cost_code?.length) name.push(item.cost_code)
  name = name.join(` / `)

  return (
    <PhaseContext.Provider value={item}>
      <PageSectionItem
        actions={<PhaseActions />}
        heading={name}
        openByDefault={!withActions}
      >
        <div className="space-y-4">
          <PageSectionItem
            heading="Assemblies"
            openByDefault={true}
          >
            <Assemblies data={item.assemblies} />
          </PageSectionItem>
          <PageSectionItem
            heading="Additional Material"
            // openByDefault={item.parts.length}
            openByDefault={true}
          >
            <Parts items={item.parts} />
          </PageSectionItem>
        </div>
      </PageSectionItem>
    </PhaseContext.Provider>
  )
}

export default Phase
