import * as types from './types.js'

export const setRefno = (value) => {
    return {
        type: types.SET_REFNO,
        data: value
    }
}

export const setUsername = (value) => {
    return {
        type: types.SET_USERNAME,
        data: value
    }
}