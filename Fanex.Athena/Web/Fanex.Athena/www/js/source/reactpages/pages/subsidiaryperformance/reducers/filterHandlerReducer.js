import createReducer from '../../../lib/createReducer.js'
import * as types from '../actions/types.js'

export const subsidiaryList = createReducer({}, {
    [types.GET_SUBSIDIARY](state, action) {
        return action.data;
    }
})

export const selectedSubsidiary = createReducer({}, {
    [types.SET_SUBSIDIARY](state, action) {
        return action.data;
    }
})

export const dateRange = createReducer({}, {
    [types.SET_DATE_RANGE](state, action) {
        return action.data;
    }
})

export const convertAll = createReducer({}, {
    [types.CHANGE_CONVERT_ALL](state, action) {
        return action.data;
    }
})

export const xsrfToken = createReducer({}, {
    [types.GET_XSRF_TOKEN](state, action) {
        return action.data;
    }
})

export const dateConstraint = createReducer({}, {
    [types.GET_DATE_CONSTRAINT](state, action) {
        return action.data;
    }
})

export const isValidDateRange = createReducer({}, {
    [types.SET_VALID_DATE_RANGE](state, action) {
        return action.data
    }
})