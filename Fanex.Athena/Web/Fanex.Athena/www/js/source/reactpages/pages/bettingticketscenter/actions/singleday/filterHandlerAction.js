import * as types from './types.js'

export const setSelectedLeague = (value) => {
    return {
        type: types.SET_LEAGUE,
        data: value
    }
}

export const setSelectedMatch = (value) => {
    return {
        type: types.SET_MATCH,
        data: value
    }
}

export const setDateRange = (dateRange) => {
    const range = {
        fromDate: dateRange.startDate,
        toDate: dateRange.endDate
    }

    return {
        type: types.SET_DATE_RANGE,
        data: range
    }
}


export const setStatus = (value) => {
    return {
        type: types.SET_STATUS,
        data: value
    }
}

export const setPlatform = (value) => {
    return {
        type: types.SET_PLATFORM,
        data: value
    }
}

export const setLive = (value) => {
    return {
        type: types.SET_LIVE,
        data: value
    }
}