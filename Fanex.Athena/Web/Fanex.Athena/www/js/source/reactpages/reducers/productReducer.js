import * as types from '../actions/types'
import createReducer from '../lib/createReducer.js'

export const selectedProduct = createReducer({}, {
    [types.SET_SELECTED_PRODUCT](state, action) {
        return action.data;
    }
});

export const productOptions = createReducer({}, {
    [types.SET_PRODUCT_OPTIONS](state, action) {
        return action.data;
    }
});