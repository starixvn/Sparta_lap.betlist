import createReducer from '../../../../lib/createReducer.js'
import * as types from '../../actions/notification/types.js'

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

export const notificationTypes = createReducer({}, {
    [types.GET_NOTIFICATION_TYPE](state, action) {
        return action.data;
    }
})

export const selectedType = createReducer({}, {
    [types.SET_NOTIFICATION_TYPE](state, action) {
        return action.data;
    }
})

export const param = createReducer({}, {
    [types.SET_PARAM](state, action) {
        return action.data;
    }
})