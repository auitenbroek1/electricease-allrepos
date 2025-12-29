import { CardProvider } from './Context'

import { Container } from './Container'

export const Root = (props: any) => {
  const { children, ...initial } = props

  return (
    <CardProvider {...initial}>
      <Container>{children}</Container>
    </CardProvider>
  )
}
