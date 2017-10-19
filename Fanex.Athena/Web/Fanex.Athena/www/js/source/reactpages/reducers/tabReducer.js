import * as types from '../actions/types'
import createReducer from '../lib/createReducer.js'

export const tabList = createReducer({}, {
    [types.GET_TABLIST](state, action) {
        return action.data;
    }
})