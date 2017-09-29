import * as productReducer from '../productReducer'
import * as types from '../../actions/types'

const emptyState = {};

const reducerList = [
    { name: "selectedProduct", actionType: types.SET_SELECTED_PRODUCT, expected: "SB,BA" },
    { name: "productOptions", actionType: types.GET_PRODUCT_OPTIONS, expected: [{ Code: "SB", Name: "SB", Status: true }] }
]

describe('productReducer ', () => {
    reducerList.forEach(({ name, actionType, expected }) => {
        it(name, () => {
            const action = {
                type: actionType,
                data: expected
            }

            const result = productReducer[name](emptyState, action);

            expect(result).toEqual(expected);
        })
    })
})