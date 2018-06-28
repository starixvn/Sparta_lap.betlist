import * as types from './types'

export const setSelectedSport = (item) => {
    return {
        type: types.SET_SELECTED_SPORT,
        data: item
    };
}

export const setSportList = (list) => {
    return {
        type: types.SET_SPORT_LIST,
        data: list
    };
}