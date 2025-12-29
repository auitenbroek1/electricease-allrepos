import { createElement } from 'react';

import {
  useMatch,
  useResolvedPath,
} from 'react-router-dom'

const RouterLink = ({ children, to, ...props }: any) => {
  let resolved = useResolvedPath(to);
  let match = useMatch(resolved.pathname + '/*');

  // if (match) console.log(resolved.pathname, match)

  let slot = { active: match ? true : false, to }
  let resolvedChildren = (typeof children === 'function' ? children(slot) : children)

  return createElement('div', props, resolvedChildren)
}

export default RouterLink
