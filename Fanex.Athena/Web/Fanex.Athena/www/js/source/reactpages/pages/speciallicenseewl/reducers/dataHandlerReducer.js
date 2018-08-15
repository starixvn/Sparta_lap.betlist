import createReducer from '../../../lib/createReducer.js'
import * as types from '../actions/types.js'

export const gridData = createReducer({}, {
    [types.SET_GRID_DATA](state, action) {
        return action.data;
    }
});

export const setting = createReducer({}, {
    [types.GET_SETTING](state, action) {
        return action.data;
    }
})