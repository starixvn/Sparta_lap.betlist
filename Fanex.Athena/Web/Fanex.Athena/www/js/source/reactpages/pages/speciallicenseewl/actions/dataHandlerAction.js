import * as types from './types.js'

export function setGridData(data) {
    return {
        type: types.SET_GRID_DATA,
        data: data
    };
}