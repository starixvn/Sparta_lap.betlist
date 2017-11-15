import * as types from './types'

export const setSelectedCurrency = (currency) => {
    return {
        type: types.SET_SELECTED_CURRENCY,
        data: currency
    }
}