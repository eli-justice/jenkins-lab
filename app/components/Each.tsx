/* eslint-disable @typescript-eslint/no-empty-object-type */
/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { Children } from 'react'

const Each = ({ render, of }: { render: (item: any, index?: number) => {}; of: any }) => {
  return <>{Children.toArray(of?.map((item:any, index:number) => render(item, index)))}</>
}

export default Each
