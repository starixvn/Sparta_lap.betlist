import createReducer from '../../../lib/createReducer.js'
import * as types from '../actions/types.js'

export const reportByOption = createReducer({}, {
    [types.GET_REPORTBY](state, action) {
        return action.data;
    }
})

export const selectedReportBy = createReducer({}, {
    [types.SET_REPORTBY](state, action) {
        return action.data;
    }
})

export const excludeTestDataOption = createReducer({}, {
    [types.GET_EXCLUDE_TESTDATA](state, action) {
        return action.data;
    }
})

export const selectedExcludeTestData = createReducer({}, {
    [types.SET_EXCLUDE_TESTDATA](state, action) {
        return action.data;
    }
})

export const dateRange = createReducer({}, {
    [types.SET_DATE_RANGE](state, action) {
        return action.data;
    }
})

export const dateConstraint = createReducer({}, {
    [types.GET_DATE_CONSTRAINT](state, action) {
        return action.data;
    }
})

export const xsrfToken = createReducer({}, {
    [types.GET_XSRF_TOKEN](state, action) {
        return action.data;
    }
})

export const params = createReducer({}, {
    [types.SET_PARAM](state, action) {
        return action.data;
    }
})