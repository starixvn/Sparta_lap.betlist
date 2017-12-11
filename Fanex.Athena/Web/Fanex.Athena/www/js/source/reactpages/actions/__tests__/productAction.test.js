import * as actions from '../productAction'
import * as types from '../types'

describe('productAction ', () => {
    it('setSelectedProduct - should create correct action', () => {
        const selectedProduct = "SB,BA";
        const expectedAction = {
            type: types.SET_SELECTED_PRODUCT,
            data: selectedProduct
        }

        const result = actions.setSelectedProduct(selectedProduct);

        expect(result).toEqual(expectedAction)
    })

    it('setProductOptions - should create correct action', () => {
        const data = [1,2,3];
        const expectedAction = {
            type: types.SET_PRODUCT_OPTIONS,
            data: data
        }

        const result = actions.setProductOptions(data);

        expect(result).toEqual(expectedAction)
    })
})
