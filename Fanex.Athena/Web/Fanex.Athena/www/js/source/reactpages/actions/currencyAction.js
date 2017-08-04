import * as types from './types'
export function getCurrencyList() {
    return {
        type: types.GET_CurrencyList,
        data:[{Name:"USD",ID:"2"}]
    }
}