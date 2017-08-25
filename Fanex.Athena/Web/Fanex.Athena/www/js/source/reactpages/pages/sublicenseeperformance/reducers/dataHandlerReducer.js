import createReducer from '../../../lib/createReducer.js'
import * as types from '../actions/types.js'


export const gridData = createReducer({}, {
    [types.FETCH_DATA](state, action) {
        return Object.assign({}, action.data);
    }
});