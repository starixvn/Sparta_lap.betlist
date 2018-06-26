import createReducer from '@jsroot/lib/createReducer.js'
import * as types from '../actions/types.js'

export const userName = createReducer({}, {
    [types.SET_USERNAME](state, action) {
        return action.data;
    }
})