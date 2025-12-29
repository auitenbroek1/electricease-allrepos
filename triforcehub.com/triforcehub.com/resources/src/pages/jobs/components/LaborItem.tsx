import { useJobLabor } from '@/data/JobLabor'

import TableBodyCell from '@/components/Table/TableBodyCell'

import { TrashIcon } from '@/components/Icons'
import { Form } from '@/components'

export const LaborItem = (props: any) => {
  const { data = {} } = props

  const {
    nameRef,
    hoursRef,
    rateRef,
    notesRef,
    enabledRef,

    onDeleteClick,
    save,

    loading,
    errors,
  } = useJobLabor(data)

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
          <div className={`w-60`}>
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
          <div className={`w-32 text-right`}>
            <Form.Text
              defaultValue={data.hours}
              invalid={errors.hours?.length}
              messages={errors.hours}
              name={`hours`}
              onChange={save}
              required
              ref={hoursRef}
              type={`number`}
            />
          </div>
        </TableBodyCell>
        <TableBodyCell>
          <div className={`w-full`}>
            <div className={`w-32 text-right`}>
              <Form.Text
                defaultValue={data.rate}
                invalid={errors.rate?.length}
                messages={errors.rate}
                name={`rate`}
                onChange={save}
                required
                ref={rateRef}
                type={`number`}
              />
            </div>
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
