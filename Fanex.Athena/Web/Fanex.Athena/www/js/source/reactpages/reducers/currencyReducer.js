import * as types from '../actions/types'
import createReducer from '../lib/createReducer.js'

export const selectedCurrency = createReducer({}, {
    [types.SET_SELECTED_CURRENCY](state, action) {
        return action.data
    }
});

export const currencyList = createReducer({}, {
    [types.GET_CURRENCY](state, action) {
        return action.data
    }
});