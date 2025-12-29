import { Card, PageSectionIntro, PageSectionItems } from '@/components'

import { Collection } from './components/Collection'

export const CollectionPage = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Material Vault`}>
        {/* description */}
      </PageSectionIntro>
      <Card.Root>
        <Card.Main>
          <Collection />
        </Card.Main>
      </Card.Root>
    </PageSectionItems>
  )
}
