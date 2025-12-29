import { usePartTag } from '@/hooks/usePartTag'

import { Card, Form } from '@/components'
import FormButton from '@/components/Form/FormButton'
import { ArrowDownTrayIcon } from '@/components/Icons'

export const PartTagForm = () => {
  const {
    data,
    errors,

    nameRef,
    descriptionRef,
    colorRef,

    onCancelClick,
    onSaveClick,
  } = usePartTag()

  //

  return (
    <Card.Root>
      <Card.Main>
        <Card.Columns number={2}>
          <Card.Column>
            <Card.Group>
              <Card.Header title={`Tag Details`}></Card.Header>
              <hr />
              <Form.Controls cols={1}>
                <Form.Control
                  label={`Name`}
                  field={
                    <Form.Input
                      ref={nameRef}
                      defaultValue={data.name ?? ``}
                    />
                  }
                  errors={errors?.name}
                />
                {/* <Form.Control
                  label={`Description`}
                  field={
                    <Form.Input
                      ref={descriptionRef}
                      defaultValue={data.description ?? ''}
                    />
                  }
                  errors={errors?.description}
                /> */}
                <Form.Control>
                  <Form.Label>Color</Form.Label>
                  {data?.color ? (
                    <input
                      name={`color`}
                      ref={colorRef}
                      type={`color`}
                      defaultValue={data.color}
                    />
                  ) : (
                    <input
                      name={`color`}
                      ref={colorRef}
                      type={`color`}
                    />
                  )}
                </Form.Control>
              </Form.Controls>
            </Card.Group>
          </Card.Column>
          <Card.Column></Card.Column>
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
  )
}
