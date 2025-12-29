import { useAssembly } from '@/hooks/useAssembly'

import {
  Card,
  Form,
  Table,
  TableHeader,
  TableHeaderRow,
  TableHeaderCell,
  TableBody,
  TableBodyRow,
  TableBodyCell,
} from '@/components'
import FormButton from '@/components/Form/FormButton'
import { ArrowDownTrayIcon } from '@/components/Icons'

import { MultiSelect } from '@mantine/core'

import { CollectionModal as PartModal } from '@/pages/parts/pages/all/collection/modal'
import { useState } from 'react'
import { decimal } from '@/utilities/format'

export const Details = () => {
  const {
    data,
    errors,

    nameRef,
    descriptionRef,
    categoriesRef,
    tagsRef,

    debouncedAutoSave,

    onCancelClick,
    onSaveClick,

    //

    categoriesData,
    handleCategoriesChange,

    tagsData,
    handleTagsChange,

    handlePartsChange,
  } = useAssembly()

  //

  const [isModalOpen, setIsModalOpen] = useState(false)

  const addParts = async () => {
    setIsModalOpen(true)
  }

  const setPartsData = async (parts: any) => {
    console.log(`add`, parts)

    const diff = [...data.parts]

    for (const id of parts) {
      diff.push({ id, quantity: 1 })
    }

    handlePartsChange(diff)
  }

  const handleQuantityChange = (id: any, quantity: any) => {
    console.log(`change`, id, quantity)

    const diff = data.parts.map((part: any) => {
      if (part.id === id) {
        part.quantity = quantity
      }
      return part
    })

    handlePartsChange(diff)
  }

  const handleRemove = (id: any) => {
    console.log(`remove`, id)

    const diff = data.parts.filter((part: any) => part.id !== id)

    handlePartsChange(diff)
  }

  //

  return (
    <>
      <Card.Root key={data?.id}>
        <Card.Main>
          <Card.Columns number={2}>
            <Card.Column>
              <Card.Group>
                <Form.Controls cols={2}>
                  <Form.Control
                    cols={2}
                    errors={errors?.name}
                    field={
                      <Form.Input
                        defaultValue={data?.name}
                        onChange={debouncedAutoSave}
                        ref={nameRef}
                      />
                    }
                    label={`Name`}
                    required={true}
                  />
                  {/* <Form.Control
                    cols={2}
                    errors={errors?.description}
                    field={
                      <Form.Input
                        defaultValue={data?.description}
                        onChange={debouncedAutoSave}
                        ref={descriptionRef}
                      />
                    }
                    label={`Description`}
                  /> */}
                </Form.Controls>
              </Card.Group>
            </Card.Column>
            <Card.Column>
              <Card.Group>
                <Form.Controls cols={2}>
                  <Form.Control
                    cols={2}
                    field={
                      <MultiSelect
                        clearable
                        data={categoriesData}
                        defaultValue={categoriesRef.current}
                        onChange={handleCategoriesChange}
                        searchable
                        styles={{ input: { lineHeight: 1 } }}
                      />
                    }
                    key={`categories-${categoriesData?.length ?? 0}`}
                    label={`Categories`}
                  />
                  <Form.Control
                    cols={2}
                    field={
                      <MultiSelect
                        clearable
                        data={tagsData}
                        defaultValue={tagsRef.current}
                        onChange={handleTagsChange}
                        searchable
                        styles={{ input: { lineHeight: 1 } }}
                      />
                    }
                    key={`tags-${tagsData?.length ?? 0}`}
                    label={`Tags`}
                  />
                </Form.Controls>
              </Card.Group>
            </Card.Column>
          </Card.Columns>

          <hr />

          <Card.Columns number={1}>
            <Card.Column number={1}>
              <Card.Group>
                <Card.Header title={`Material`}></Card.Header>
                <hr />
                <Table>
                  <colgroup>
                    <col width={`*`} />
                    <col width={1} />
                    <col width={1} />
                    <col width={1} />
                    <col width={1} />
                  </colgroup>
                  <TableHeader>
                    <TableHeaderRow>
                      <TableHeaderCell>Name</TableHeaderCell>
                      <TableHeaderCell>
                        <div className={`text-center`}>Cost</div>
                      </TableHeaderCell>
                      <TableHeaderCell>
                        <div className={`text-center`}>Labor</div>
                      </TableHeaderCell>
                      <TableHeaderCell>
                        <div className={`text-center`}>Quantity</div>
                      </TableHeaderCell>
                      <TableHeaderCell></TableHeaderCell>
                    </TableHeaderRow>
                  </TableHeader>
                  <TableBody>
                    {data?.parts?.map((item: any, index: number) => (
                      <TableBodyRow key={item.id}>
                        <TableBodyCell>{item.name}</TableBodyCell>
                        <TableBodyCell>
                          <div className={`w-36 text-right`}>
                            <Form.Input
                              value={decimal(item.cost)}
                              disabled={true}
                              prefix={`$`}
                              type={`number`}
                            />
                          </div>
                        </TableBodyCell>
                        <TableBodyCell>
                          <div className={`w-36 text-right`}>
                            <Form.Input
                              value={decimal(item.labor)}
                              disabled={true}
                              type={`number`}
                            />
                          </div>
                        </TableBodyCell>
                        <TableBodyCell>
                          <div className={`w-36 text-right`}>
                            <Form.Input
                              className={`text-right`}
                              onChange={(event: any) =>
                                handleQuantityChange(
                                  item.id,
                                  event.target.value,
                                )
                              }
                              defaultValue={
                                item.quantity ? decimal(item.quantity) : ``
                              }
                              step={0.01}
                              type={`number`}
                            />
                          </div>
                        </TableBodyCell>
                        <TableBodyCell>
                          <button
                            onClick={() => handleRemove(item.id)}
                            type={`button`}
                          >
                            Remove
                          </button>
                        </TableBodyCell>
                      </TableBodyRow>
                    ))}
                  </TableBody>
                </Table>
                <div className="mt-4 text-center">
                  <button
                    className="relative inline-flex items-center rounded-md border border-transparent bg-blue-600 px-4 py-2 text-sm font-medium text-white shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-600 focus:ring-offset-2"
                    onClick={addParts}
                    type="button"
                  >
                    Add Material
                  </button>
                </div>
              </Card.Group>
            </Card.Column>
          </Card.Columns>

          <hr />

          <Form.Buttons>
            <FormButton
              onClick={onCancelClick}
              type={`secondary`}
            >
              Cancel
            </FormButton>
            <FormButton onClick={onSaveClick}>
              <span className="h-5 w-5">
                <ArrowDownTrayIcon />
              </span>
              <span>Save</span>
            </FormButton>
          </Form.Buttons>
        </Card.Main>
      </Card.Root>

      <PartModal
        open={isModalOpen}
        setOpen={setIsModalOpen}
        setData={setPartsData}
      />
    </>
  )
}
