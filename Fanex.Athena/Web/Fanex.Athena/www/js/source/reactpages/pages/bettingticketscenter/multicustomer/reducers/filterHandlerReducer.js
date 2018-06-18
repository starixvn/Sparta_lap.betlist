import createReducer from '../../../../lib/createReducer.js'
import * as types from '../actions/types.js'

export const leagueList = createReducer({}, {
    [types.GET_LEAGUE](state, action) {
        return action.data;
    }
})

export const selectedLeague = createReducer({}, {
    [types.SET_LEAGUE](state, action) {
        return action.data;
    }
})

export const matchList = createReducer({}, {
    [types.GET_MATCH](state, action) {
        return action.data;
    }
})

export const selectedMatch = createReducer({}, {
    [types.SET_MATCH](state, action) {
        return action.data;
    }
})

export const statusList = createReducer({}, {
    [types.GET_STATUS](state, action) {
        return action.data;
    }
})

export const selectedStatus = createReducer({}, {
    [types.SET_STATUS](state, action) {
        return action.data;
    }
})

export const platformList = createReducer({}, {
    [types.GET_PLATFORM](state, action) {
        return action.data;
    }
})

export const selectedPlatform = createReducer({}, {
    [types.SET_PLATFORM](state, action) {
        return action.data;
    }
})

export const liveList = createReducer({}, {
    [types.GET_LIVE](state, action) {
        return action.data;
    }
})

export const selectedLive = createReducer({}, {
    [types.SET_LIVE](state, action) {
        return action.data;
    }
})
