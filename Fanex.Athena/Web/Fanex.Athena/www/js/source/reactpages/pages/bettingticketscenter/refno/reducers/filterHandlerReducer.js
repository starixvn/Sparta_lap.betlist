import createReducer from '@jsroot/lib/createReducer.js'
import * as types from '../actions/types.js'

export const refno = createReducer({}, {
    [types.SET_REFNO](state, action) {
        return action.data;
    }
})

export const username = createReducer({}, {
    [types.SET_USERNAME](state, action) {
        return action.data;
    }
})