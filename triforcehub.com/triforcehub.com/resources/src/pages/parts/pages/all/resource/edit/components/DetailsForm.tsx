import { Controller } from 'react-hook-form'
import { ArrowDownTrayIcon } from '@/components/Icons'
import { MultiSelect } from '@mantine/core'

import { Card, Form, Grid } from '@/components'

import { useDetailsForm } from '../hooks/useDetailsForm'

interface Props {
  part: Partial<PartResource>
  categories: PartCategoryResource[]
  tags: PartTagResource[]
}

interface Taxonomy {
  label: string
  value: string
}

export const DetailsForm = (props: Props) => {
  const { part, categories: categoriesData, tags: tagsData } = props

  const { cancel, control, errors, register, save } = useDetailsForm({ part })

  const categories: Taxonomy[] = categoriesData.map((item) => {
    return {
      label: item.name,
      value: item.id?.toString(),
    }
  })

  const tags: Taxonomy[] = tagsData.map((item) => {
    return {
      label: item.name,
      value: item.id?.toString(),
    }
  })

  console.log(`debug.api.form`, { part, categories, tags })

  return (
    <Card.Root>
      <Card.Main>
        <Grid.Root cols={2}>
          <Grid.Column>
            <Grid.Root cols={2}>
              <Grid.Column span={2}>
                <Form.Control
                  error={errors.name}
                  field={
                    <Form.Input
                      {...register(`name`)}
                      type={`text`}
                    />
                  }
                  label={`Name`}
                  required={true}
                />
              </Grid.Column>
              {/* <Grid.Column span={2}>
                <Form.Control
                  error={errors.description}
                  field={
                    <Form.Input
                      {...register(`description`)}
                      type={`text`}
                    />
                  }
                  label={`Description`}
                />
              </Grid.Column> */}
              <Grid.Column>
                <Form.Control
                  error={errors.cost}
                  field={
                    <Form.Input
                      {...register(`cost`)}
                      type={`number`}
                    />
                  }
                  label={`Cost`}
                  required={true}
                />
              </Grid.Column>
              <Grid.Column>
                <Form.Control
                  error={errors.labor}
                  field={
                    <Form.Input
                      {...register(`labor`)}
                      type={`number`}
                    />
                  }
                  label={`Labor Unit`}
                  required={true}
                />
              </Grid.Column>
            </Grid.Root>
          </Grid.Column>
          <Grid.Column>
            <Grid.Root cols={2}>
              <Grid.Column span={2}>
                <Form.Control
                  field={
                    <Controller
                      control={control}
                      name="categories"
                      render={({ field }) => (
                        <MultiSelect
                          {...field}
                          clearable
                          data={categories}
                          searchable
                          styles={{ input: { lineHeight: 1 } }}
                        />
                      )}
                    />
                  }
                  label={`Categories`}
                />
              </Grid.Column>
              <Grid.Column span={2}>
                <Form.Control
                  field={
                    <Controller
                      control={control}
                      name="tags"
                      render={({ field }) => (
                        <MultiSelect
                          {...field}
                          clearable
                          data={tags}
                          searchable
                          styles={{ input: { lineHeight: 1 } }}
                        />
                      )}
                    />
                  }
                  label={`Tags`}
                />
              </Grid.Column>
            </Grid.Root>
          </Grid.Column>
        </Grid.Root>
        <hr />
        <Grid.Root>
          <Grid.Column>
            <div className="flex justify-center">
              <Form.Button
                onClick={cancel}
                type={`secondary`}
              >
                Cancel
              </Form.Button>
              <div className="w-8"></div>
              <Form.Button onClick={save}>
                <span className="h-5 w-5">
                  <ArrowDownTrayIcon />
                </span>
                <span>Save</span>
              </Form.Button>
            </div>
          </Grid.Column>
        </Grid.Root>
      </Card.Main>
    </Card.Root>
  )
}
