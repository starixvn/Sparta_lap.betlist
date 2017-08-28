import * as types from './types'
export const setSelectedProduct = (product) => {
    let item = {
        type: types.SET_SELECTED_PRODUCT,
        data: product
    };
    return item;
}