import { useJobQuote } from '@/data/JobQuote'

import TableBodyCell from '@/components/Table/TableBodyCell'

import { TrashIcon } from '@/components/Icons'
import { Form } from '@/components'

export const Quote = (props: any) => {
  const { data = {} } = props

  const {
    costRef,
    enabledRef,
    nameRef,
    notesRef,

    onDeleteClick,
    save,

    loading,
    errors,
  } = useJobQuote(data)

  return (
    <>
      <tr>
        <TableBodyCell>
          <input
            defaultChecked={data.id ? data.enabled : true}
            onChange={save}
            ref={enabledRef}
            type={`checkbox`}
          />
        </TableBodyCell>
        <TableBodyCell>
          <div className={`w-96`}>
            <Form.Text
              defaultValue={data.name}
              invalid={errors.name?.length}
              messages={errors.name}
              name={`name`}
              onChange={save}
              ref={nameRef}
              required
              type={`text`}
            />
          </div>
        </TableBodyCell>
        <TableBodyCell>
          <div className={`w-36 text-right`}>
            <Form.Text
              defaultValue={data.cost}
              invalid={errors.cost?.length}
              messages={errors.cost}
              name={`cost`}
              onChange={save}
              required
              ref={costRef}
              type={`number`}
            />
          </div>
        </TableBodyCell>
        <TableBodyCell>
          <div className={`w-full`}>
            <Form.Text
              defaultValue={data.notes}
              invalid={errors.notes?.length}
              messages={errors.notes}
              name={`notes`}
              onChange={save}
              ref={notesRef}
              type={`text`}
            />
          </div>
        </TableBodyCell>
        <TableBodyCell>
          {data.id ? (
            <button
              onClick={onDeleteClick}
              type={`button`}
            >
              <div className={`h-5 w-5`}>
                <TrashIcon />
              </div>
            </button>
          ) : (
            <button
              onClick={save}
              type="button"
            >
              Save
            </button>
          )}
        </TableBodyCell>
      </tr>
      {false && (
        <tr>
          <td></td>
          <td colSpan={999}>
            loading: {loading ? `loading...` : `done`}, {JSON.stringify(errors)}
          </td>
        </tr>
      )}
    </>
  )
}
