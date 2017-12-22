import * as types from './types'

export const setSelectedProduct = (product) => {
    return {
        type: types.SET_SELECTED_PRODUCT,
        data: product
    };
}
export const setProductOptions = (productList) => {
    return {
        type: types.SET_PRODUCT_OPTIONS,
        data: productList
    };
}