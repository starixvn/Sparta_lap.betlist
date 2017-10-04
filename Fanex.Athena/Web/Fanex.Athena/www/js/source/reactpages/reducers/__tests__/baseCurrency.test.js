import * as baseCurrencyReducer from '../baseCurrency'
import * as types from '../../actions/types'

const emptyState = {};

const reducerList = [
    { name: "selectedBaseCurrency", actionType: types.SET_SELECTED_BASECURRENCY, expected: "EUR" },
    { name: "baseCurrencyList", actionType: types.GET_BASECURRENCY, expected: "USD" }
]

describe('baseCurrencyReducer ', () => {
    reducerList.forEach(({ name, actionType, expected }) => {
        it(name, () => {
            const action = {
                type: actionType,
                data: expected
            }

            const result = baseCurrencyReducer[name](emptyState, action);

            expect(result).toEqual(expected);
        })
    })
})