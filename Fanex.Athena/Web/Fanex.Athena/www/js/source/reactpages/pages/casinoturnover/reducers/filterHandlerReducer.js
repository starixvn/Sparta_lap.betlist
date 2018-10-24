import createReducer from '@jsroot/lib/createReducer.js'
import * as type from '../actions/type.js'

export const params = createReducer({}, {
    [type.SET_PARAMS](state, action) {
        return action.data;
    }
})