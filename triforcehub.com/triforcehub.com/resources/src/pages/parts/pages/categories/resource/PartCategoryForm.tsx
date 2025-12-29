import { usePartCategory } from '@/hooks/usePartCategory'

import { Card, Form } from '@/components'
import FormButton from '@/components/Form/FormButton'
import { ArrowDownTrayIcon } from '@/components/Icons'

export const PartCategoryForm = () => {
  const {
    data,
    errors,

    nameRef,
    descriptionRef,

    onCancelClick,
    onSaveClick,
  } = usePartCategory()

  //

  return (
    <Card.Root>
      <Card.Main>
        <Card.Columns number={2}>
          <Card.Column>
            <Card.Group>
              <Card.Header title={`Category Details`}></Card.Header>
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
                      defaultValue={data.description ?? ``}
                    />
                  }
                  errors={errors?.description}
                /> */}
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
