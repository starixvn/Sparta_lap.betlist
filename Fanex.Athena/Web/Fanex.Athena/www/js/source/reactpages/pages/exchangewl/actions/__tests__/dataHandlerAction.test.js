import thunk from 'redux-thunk'
import configureMockStore from 'redux-mock-store'

import * as actions from '../dataHandlerAction'
import * as types from '../types'

import Site from '../../../../lib/site'
import axios from 'axios'

jest.mock('axios')
jest.mock('../../../../lib/site')

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

describe('filterHandlerAction', () => {
    const fakeResponse = {
        data: {
            DataList: [],
            TotalDataList: [],
            SummaryTotal: {}
        }
    }

    const params = {
        data: { Level: 1 },
        xsrfToken: "token"
    }

    const expectedHeader = {
        'X-Requested-With': 'XMLHttpRequest',
        '__RequestVerificationToken': params.xsrfToken
    }
    const expectedUrl = "api/ExchangeWinLossReport/Index";
    const expectedData = params.data;

    beforeAll(() => {
        const promise = Promise.resolve(fakeResponse);
        axios.post = jest.fn(() => promise);
        Site.resolveClientUrl = jest.fn((url) => url);
    })

    afterAll(() => {
        jest.resetAllMocks();
    })

    describe('getGridData - should create an correct action', () => {
        it('dispatch correct action', () => {
            const expectedActions = [
                { type: types.SET_GRID_DATA, data: { level: params.data.Level, data: fakeResponse.data } }
            ];
            const store = mockStore({ gridData: Object.assign({},  { level: params.data.Level, data: fakeResponse.data }) });
            store.dispatch(actions.getGridData(params));

            Promise.resolve().then(() => {
                expect(store.getActions()).toEqual(expectedActions);
            })
        })

        it('axios.post should be called with correct parameters', () => {                       
            const store = mockStore({ gridData: Object.assign({}, { level: params.data.Level, data: fakeResponse.data }) });
            store.dispatch(actions.getGridData(params));

            Promise.resolve().then(() => {
                expect(axios.post).toHaveBeenCalledWith(expectedUrl, expectedData, { headers: expectedHeader });
            })
        })
    })

    it('exportExcel - axios.post should be called with correct parameters', () => {
        const store = mockStore({ gridData: Object.assign({}, fakeResponse.data) });
        store.dispatch(actions.exportExcel(params));

        Promise.resolve().then(() => {
            expect(axios.post).toHaveBeenCalledWith(expectedUrl, expectedData, { headers: expectedHeader });
        })
    })

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

    it('setLevelGridData - should create an correct action', () => {
        const data = [{ DataList: [] }, { DataList: [] }];
        const expectedAction = {
            type: types.SET_LEVEL_GRID_DATA,
            data: data
        };

        const result = actions.setLevelGridData(data);

        expect(result).toEqual(expectedAction);
    })
})