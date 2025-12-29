import { useAssemblyTag } from '@/hooks/useAssemblyTag'

import { Card, Form } from '@/components'
import FormButton from '@/components/Form/FormButton'
import { ArrowDownTrayIcon } from '@/components/Icons'

export const AssemblyTagForm = () => {
  const {
    data,
    errors,

    nameRef,
    descriptionRef,
    colorRef,

    debouncedAutoSave,

    onCancelClick,
    onSaveClick,
  } = useAssemblyTag()

  //

  return (
    <Card.Root key={data?.id}>
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
                      defaultValue={data?.name}
                      onChange={debouncedAutoSave}
                      ref={nameRef}
                    />
                  }
                  errors={errors?.name}
                />
                {/* <Form.Control
                  label={`Description`}
                  field={
                    <Form.Input
                      defaultValue={data?.description}
                      onChange={debouncedAutoSave}
                      ref={descriptionRef}
                    />
                  }
                  errors={errors?.description}
                /> */}
                <Form.Control
                  label={`Color`}
                  field={
                    <input
                      defaultValue={data?.color ?? `#ffffff`}
                      onChange={debouncedAutoSave}
                      ref={colorRef}
                      type={`color`}
                    />
                  }
                  errors={errors?.color}
                />
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
