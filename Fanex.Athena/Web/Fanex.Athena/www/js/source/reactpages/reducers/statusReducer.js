import * as types from '../actions/types'
import createReducer from '../lib/createReducer.js'

export const selectedStatus = createReducer({}, {
    [types.SET_SELECTED_STATUS](state, action) {
        return action.data
    }
});

export const statusList = createReducer({}, {
    [types.GET_STATUS](state, action) {
        return action.data
    }
});