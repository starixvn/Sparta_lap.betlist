import { SET_SELECTED_BASECURRENCY } from './types'

export const setSelectedBaseCurrency = (baseCurrency) => {
    return {
        type: SET_SELECTED_BASECURRENCY,
        selectedBaseCurrency: baseCurrency
    }
}