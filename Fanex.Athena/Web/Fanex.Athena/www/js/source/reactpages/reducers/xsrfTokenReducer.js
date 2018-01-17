import * as types from '../actions/types'
import createReducer from '../lib/createReducer.js'

export const xsrfToken = createReducer({}, {
    [types.GET_XSRF_TOKEN](state, action) {
        return action.data;
    }
})