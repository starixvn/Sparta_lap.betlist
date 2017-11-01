import createReducer from '../../../../lib/createReducer.js'
import * as types from '../actions/types.js'

export const resultAmendmentDateRange = createReducer({}, {
    [types.SET_DATE_RANGE](state, action) {
        return action.data;
    }
})

export const resultAmendmentXsrfToken = createReducer({}, {
    [types.GET_XSRF_TOKEN](state, action) {
        return action.data;
    }
})

export const resultAmendmentDateConstraint = createReducer({}, {
    [types.GET_DATE_CONSTRAINT](state, action) {
        return action.data;
    }
})

export const resultAmendmentParam = createReducer({}, {
    [types.SET_PARAM](state, action) {
        return action.data;
    }
})