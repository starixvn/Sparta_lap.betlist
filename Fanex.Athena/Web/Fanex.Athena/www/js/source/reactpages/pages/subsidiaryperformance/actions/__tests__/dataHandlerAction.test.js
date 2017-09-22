import thunk from 'redux-thunk'
import axios from 'axios'
import configureMockStore from 'redux-mock-store'

import * as actions from '../dataHandlerAction'
import * as types from '../types'
import Site from '../../../../lib/site'

jest.mock('axios')
jest.mock('../../../../lib/site')

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

describe('dataHandlerAction', () => {
    afterEach(() => {
        jest.resetAllMocks();
    })

    it('getGridData - dispatch correct action', () => {
        const fakeResponse = {
            data: {
                Data: [],
                Total: [],
                GrandTotal: {}
            }
        }
        const promise = Promise.resolve(fakeResponse);
        axios.post = jest.fn(() => promise);

        const expectedActions = [
            { type: types.SET_GRID_DATA, data: fakeResponse.data }
        ];
        const store = mockStore({ gridData: Object.assign({}, fakeResponse.data) });

        let dispatcher = store.dispatch(actions.getGridData({ url: "", data: {} }));

        Promise.resolve(dispatcher).then(() => {
            expect(store.getActions()).toEqual(expectedActions);
        });
    })
})