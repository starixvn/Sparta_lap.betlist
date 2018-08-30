import * as actions from '../dataHandlerAction'
import * as types from '../types'

describe('dataHandlerAction', () => {
    it('setGridData - should create an correct action', () => {
        const data = {
            DataList: []
        };

        const expectedAction = {
            type: types.SET_GRID_DATA,
            data: data
        };

        expect(actions.setGridData(data)).toEqual(expectedAction);
    })
})