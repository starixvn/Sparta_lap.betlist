import { SET_SELECTED_BASECURRENCY } from '../actions/types'
import createReducer from '../lib/createReducer.js'

export const selectedBaseCurrency = createReducer({}, {
    [SET_SELECTED_BASECURRENCY](state, action) {
        return action.selectedBaseCurrency
    }
});

export const baseCurrencyList = createReducer({}, {});