import * as actions from '../baseCurrencyAction'
import * as types from '../types'

describe('baseCurrencyAction ', () => {
    it('setSelectedBaseCurrency - should create correct action', () => {
        const baseCurrency = "EUR";
        const expectedAction = {
            type: types.SET_SELECTED_BASECURRENCY,
            data: baseCurrency
        }

        const result = actions.setSelectedBaseCurrency(baseCurrency);

        expect(result).toEqual(expectedAction)
    })
})