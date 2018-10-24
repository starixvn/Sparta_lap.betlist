import * as type from './type.js'

export const setParams = (params) => {
    return {
        type: type.SET_PARAMS,
        data: params
    }
}