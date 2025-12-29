describe(`Authentication`, () => {

  it(`shows the login form`, () => {
    cy
      .visit(`/auth/login`)

    cy
      .contains(`Sign in to your account`)
  })

  it(`logs in`, () => {
    cy.visit(`/auth/login`)

    cy
      .get(`input[name="email"]`)
      .type(`demo@electric-ease.com`)

    cy
      .get(`input[name="password"]`)
      .type(`Triforce2022!`)

    cy.get(`button[type="submit"]`)
      .click()

    cy.url()
      .should(`include`, `/app/home`)

    cy.contains(`Dashboard`)
  })
})
