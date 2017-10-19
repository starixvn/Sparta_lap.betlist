import createReducer from '../../../../lib/createReducer.js'
import * as types from '../actions/types.js'

export const dateRange = createReducer({}, {
    [types.SET_DATE_RANGE](state, action) {
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

export const notificationTypes = createReducer({}, {
    [types.GET_NOTIFICATION_TYPE](state, action) {
        return action.data;
    }
})

export const selectedNotificationType = createReducer({}, {
    [types.SET_NOTIFICATION_TYPE](state, action) {
        return action.data;
    }
})