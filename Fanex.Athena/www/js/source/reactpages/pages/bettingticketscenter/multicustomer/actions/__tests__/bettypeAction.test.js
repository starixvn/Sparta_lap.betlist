import thunk from 'redux-thunk'
import configureMockStore from 'redux-mock-store'
import * as actions from '../bettypeAction'
import * as types from '../types'
import DataService from '@jsroot/lib/DataService.js';
import Site from '@jsroot/lib/site'
import Cache from '@jsroot/lib/Cache'

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

describe('bettypeAction ', () => {
    it('setSelectedBettype - should create correct action', () => {
        const item = { data: 123 };
        const expectedAction = {
            type: types.SET_SELECTED_BETTYPE,
            data: item
        }

        const result = actions.setSelectedBettype(item);

        expect(result).toEqual(expectedAction)
    })

    it('setBettypeList - should create correct action', () => {
        const list = [123, 345];
        const expectedAction = {
            type: types.SET_BETTYPE_LIST,
            data: list
        }

        const result = actions.setBettypeList(list);

        expect(result).toEqual(expectedAction)
    })

    it('getBettypeList - xxxx', (done) => {
        const response = { data: [1,2,3] };
        const promise = Promise.resolve(response);
        DataService.getData = jest.fn(() => promise);
        Site.resolveClientUrl = jest.fn((url) => url);
        Cache.get = jest.fn();
        Cache.set = jest.fn();
        
        const productCode = "BG";
        const hasItemAll = true;
        const store = mockStore({ });

        store.dispatch(actions.getBettypeList(productCode, hasItemAll));
        
        promise.then(() => {
            expect(DataService.getData).toHaveBeenCalledWith('/api/CommonDataApi/GetBettypes', { productCode: productCode });
            expect(Cache.set).toHaveBeenCalledWith("BETTYPE_CACHE_KEYBG", response.data);
            done();
        });
    });
})