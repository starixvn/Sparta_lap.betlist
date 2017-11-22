import thunk from 'redux-thunk'
import configureMockStore from 'redux-mock-store'

import * as actions from '../dataHandlerAction'
import * as types from '../types'

import Site from '../../../../../lib/site'
import axios from 'axios'

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

describe('dataHandlerAction', () => {
    beforeAll(() => {
        Site.resolveClientUrl = jest.fn((url) => url);

        jest.mock('axios');
    });

    afterAll(() => {
        jest.resetAllMocks();
    })

    describe('getGridData', () => {
        const expectedUrl = "api/NotificationReport/Index";
        const fakeDataResponse = {
            data: {
                DataList: [],
                TotalRows: 0
            }
        }
        const params = {
            data: { FromDate: "01/01/2017", ToDate: "01/10/2017" },
            xsrfToken: "token"
        }
        const expectedHeader = {
            'X-Requested-With': 'XMLHttpRequest',
            '__RequestVerificationToken': params.xsrfToken
        }

        beforeAll(() => {
            const promise = Promise.resolve(fakeDataResponse);
            axios.post = jest.fn(() => promise);
        })

        it('dispatch correct action', () => {
            const expectedActions = [
                { type: types.SET_GRID_DATA, data: { Data: fakeDataResponse.data.DataList, Total: fakeDataResponse.data.TotalRows } }
            ];
            const store = mockStore({ gridData: Object.assign({}, { Data: fakeDataResponse.data.DataList, Total: fakeDataResponse.data.TotalRows }) });
            store.dispatch(actions.getGridData({}));

            Promise.resolve().then(() => {
                expect(store.getActions()).toEqual(expectedActions);
            })
        })

        it('axios.post should be called with correct parameters', () => {
            const store = mockStore({ gridData: Object.assign({}, fakeDataResponse.data) });
            store.dispatch(actions.getGridData(params));

            Promise.resolve().then(() => {
                expect(axios.post).toHaveBeenCalledWith(expectedUrl, params.data, { headers: expectedHeader });
            })
        })
    })

    describe('setGridData', () => {
        it('create an correct action', () => {
            const data = {
                Data: [],
                Total: 0
            };
            const expectedAction = {
                type: types.SET_GRID_DATA,
                data: data
            }

            const result = actions.setGridData(data);

            expect(result).toEqual(expectedAction);
        })
    })
})