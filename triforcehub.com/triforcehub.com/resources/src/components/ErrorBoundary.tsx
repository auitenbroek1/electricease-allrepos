/* eslint-disable */

import { Component } from 'react'

export class ErrorBoundary extends Component<any, any> {
  constructor(props: any) {
    super(props)

    this.state = { error: null, errorInfo: null }
  }

  componentDidCatch(error: any, errorInfo: any) {
    console.log(error, errorInfo)

    this.setState({
      error: error,
      errorInfo: errorInfo,
    })
  }

  render() {
    if (this.state.errorInfo) {
      // Error path
      return (
        <div>
          <h2>Something went wrong.</h2>
          <details style={{ whiteSpace: 'pre-wrap' }}>
            {this.state.error && this.state.error.toString()}
            <br />
            {this.state.errorInfo.componentStack}
          </details>
        </div>
      )
    }
    // Normally, just render children
    return this.props.children
  }
}
