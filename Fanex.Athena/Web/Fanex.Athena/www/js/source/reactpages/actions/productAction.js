import { SET_SELECTED_PRODUCT } from './types'

export const setSelectedProduct = (product) => {
    return {
        type: SET_SELECTED_PRODUCT,
        selectedProduct: product
    };
}