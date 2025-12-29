import { useContext, useEffect, useMemo, useRef, useState } from 'react'
import debounce from 'lodash/debounce'

import { Collection } from './Context'
import { Input } from '../Form/Input'
import FormSelect from '../Form/FormSelect'
import axios from 'axios'

import { StarOnIcon, StarOffIcon } from '@/components/Icons'

export const Filters = (props: any) => {
  const {
    children,
    showAssemblyFavorites = false,
    showAssemblyCategories = false,
    showMaterialFavorites = false,
    showMaterialCategories = false,
  } = props

  const { q, setQ, categories, setCategories, favorites, setFavorites }: any =
    useContext(Collection)

  const qRef = useRef<any>(null)

  const onChange = (value: any) => {
    setQ(value)
  }

  const onChangeDebounced = useMemo(
    () => debounce((value) => onChange(value), 1000),
    [],
  )

  //

  const categoriesRef = useRef<any>(null)
  const [assemblyCategories, setAssemblyCategories] = useState<any>([])

  useEffect(() => {
    if (!showAssemblyCategories) return

    const initial = [{ value: `Select a Category...`, key: `` }]

    const fetchData = async () => {
      try {
        // TODO: must be able to return all categories, without having to mess with the size
        const response = await axios.get(`/api/assemblies/categories?size=999`)
        const data = response.data.data
        console.log(`debug.filters`, data.length)

        const categories = data.map((item: any) => {
          return {
            key: item.id,
            value: item.name,
          }
        })

        setAssemblyCategories([...initial, ...categories])
      } catch (error) {
        console.log(`debug.filters`, error)
      }
    }

    console.log(`debug.filters`, `assemblyCategories`)
    fetchData()

    setAssemblyCategories(initial)
  }, [])

  const [materialCategories, setMaterialCategories] = useState<any>([])

  useEffect(() => {
    if (!showMaterialCategories) return

    const initial = [{ value: `Select a Category...`, key: `` }]

    const fetchData = async () => {
      try {
        // TODO: must be able to return all categories, without having to mess with the size
        const response = await axios.get(`/api/parts/categories?size=999`)
        const data = response.data.data
        console.log(`debug.filters`, data.length)

        const categories = data.map((item: any) => {
          return {
            key: item.id,
            value: item.name,
          }
        })

        setMaterialCategories([...initial, ...categories])
      } catch (error) {
        console.log(`debug.filters`, error)
      }
    }

    console.log(`debug.filters`, `assemblyCategories`)
    fetchData()

    setAssemblyCategories(initial)
  }, [])

  const onCategoriesChange = (value: any) => {
    console.log(`debug.filters`, `onCategoriesChange`, value)
    setCategories(value)
  }

  //

  const showFavorites = showAssemblyFavorites || showMaterialFavorites

  const toggleFavorites = () => {
    setQ(``)
    if (qRef?.current) {
      qRef.current.value = ``
    }

    setCategories(``)
    if (categoriesRef?.current) {
      categoriesRef.current.value = ``
    }

    setFavorites(!favorites)
  }

  //

  return (
    <div className="w-1/2">
      <div className="flex">
        {showFavorites && (
          <div className="flex items-center justify-center px-2">
            <div className="px-2">
              <button
                className="h-5 w-5 cursor-pointer text-brand-gradient-light"
                onClick={toggleFavorites}
              >
                {favorites ? <StarOnIcon /> : <StarOffIcon />}
              </button>
            </div>
          </div>
        )}
        <div
          className={`
            grid
            w-full
            grid-cols-2
            gap-4
          `}
        >
          {showAssemblyCategories && (
            <div className={``}>
              <FormSelect
                disabled={favorites ? true : false}
                onChange={(event: any) =>
                  onCategoriesChange(event.target.value)
                }
                options={assemblyCategories}
                ref={categoriesRef}
              />
            </div>
          )}
          {showMaterialCategories && (
            <div className={``}>
              <FormSelect
                disabled={favorites ? true : false}
                onChange={(event: any) =>
                  onCategoriesChange(event.target.value)
                }
                options={materialCategories}
                ref={categoriesRef}
              />
            </div>
          )}
          <div className={``}>
            <Input
              defaultValue={q}
              disabled={favorites ? true : false}
              onChange={(event: any) => onChangeDebounced(event.target.value)}
              placeholder={`Search...`}
              ref={qRef}
              type={`search`}
            />
          </div>
        </div>
      </div>
    </div>
  )
}
