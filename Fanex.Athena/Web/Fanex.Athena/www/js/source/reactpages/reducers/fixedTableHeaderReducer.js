import * as types from '../actions/types'
import createReducer from '../lib/createReducer.js'

export const containerWidth = createReducer({}, {
    [types.GET_CONTAINER_WIDTH](state, action) {
        return action.data;
    }
})