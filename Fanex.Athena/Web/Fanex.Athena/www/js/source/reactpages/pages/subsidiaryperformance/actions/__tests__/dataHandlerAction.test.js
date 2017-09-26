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

const fakeResponse = {
    data: {
        Data: [],
        Total: [],
        GrandTotal: {}
    }
}

const params = {
    data: { Subsidiary: "1,2,3", ProductList: "SB,BA" },
    xsrfToken: "token"
}

const expectedUrl = "api/SubsidiaryPerformanceReport/Index";
const expectedData = params.data;
const expectedHeader = {
    'X-Requested-With': 'XMLHttpRequest',
    '__RequestVerificationToken': params.xsrfToken
}

const fakeModules = () => {
    const promise = Promise.resolve(fakeResponse);
    axios.post = jest.fn(() => promise);
    Site.resolveClientUrl = jest.fn((url) => url);
}

describe('dataHandlerAction', () => {
    beforeAll(() => {
        fakeModules();
    })

    afterAll(() => {
        jest.resetAllMocks();
    })

    describe('getGridData - ', () => {
        it('dispatch correct action', () => {
            const expectedActions = [
                { type: types.SET_GRID_DATA, data: fakeResponse.data }
            ];
            const store = mockStore({ gridData: Object.assign({}, fakeResponse.data) });
            store.dispatch(actions.getGridData({}));

            Promise.resolve().then(() => {
                expect(store.getActions()).toEqual(expectedActions);
            })
        })

        it('axios.post should be called with correct parameters', () => {
            const store = mockStore({ gridData: Object.assign({}, fakeResponse.data) });
            store.dispatch(actions.getGridData(params));

            Promise.resolve().then(() => {
                expect(axios.post).toHaveBeenCalledWith(expectedUrl, expectedData, { headers: expectedHeader });
            })
        })
    })

    describe('exportExcel - ', () => {
        it('axios.post should be called with correct parameters', () => {
            const store = mockStore({ gridData: Object.assign({}, fakeResponse.data) });
            store.dispatch(actions.exportExcel(params));

            Promise.resolve().then(() => {
                expect(axios.post).toHaveBeenCalledWith(expectedUrl, expectedData, { headers: expectedHeader });
            })
        })
    })

    describe('setGridData - ', () => {
        it('create an correct action', () => {
            const data = fakeResponse.data;
            const expectedAction = {
                type: types.SET_GRID_DATA,
                data: data
            }

            const result = actions.setGridData(data);

            expect(result).toEqual(expectedAction);
        })
    })
})