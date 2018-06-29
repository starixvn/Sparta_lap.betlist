import DataService from '@jsroot/lib/DataService.js'
import Site from '@jsroot/lib/site'
import LangHelper from '@jsroot/lib/langHelper'
import Cache from '@jsroot/lib/Cache'
import * as types from './types'

const Url = Site.resolveClientUrl('api/CommonApi/GetBettypes');
const BETTYPE_CACHE_KEY = "BETTYPE_CACHE_KEY";

function dispatchBettypeList(dispatch, data, hasItemAll){
    // Remove Item All
    data.length && data[0].value == 0 && data.shift();

    if(hasItemAll){
        var itemAll = { value: 0, label: LangHelper.getSingleResource('All') };
        data.unshift(itemAll);
    }

    dispatch(setBettypeList(data));
    if(data.length > 0)
    {
        dispatch(setSelectedBettype(data[0]));
    }
}

export const setSelectedBettype = (item) => {
    return {
        type: types.SET_SELECTED_BETTYPE,
        data: item
    };
}

export const setBettypeList = (list) => {
    return {
        type: types.SET_BETTYPE_LIST,
        data: list
    };
}

export function getBettypeList(productCode, hasItemAll = false) {
    return (dispatch) => {
        var cacheData = Cache.get(BETTYPE_CACHE_KEY + productCode);
        if(cacheData)
        {
            dispatchBettypeList(dispatch, cacheData, hasItemAll);
            return;
        }

        DataService.getData(Url, { productCode: productCode})
            .then(response => {
                var data = response.data;
                Cache.set(BETTYPE_CACHE_KEY + productCode, data);
                dispatchBettypeList(dispatch, data, hasItemAll);
            });
    };
}