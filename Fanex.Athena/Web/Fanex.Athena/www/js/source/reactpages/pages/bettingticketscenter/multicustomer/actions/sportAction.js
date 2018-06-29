import DataService from '@jsroot/lib/DataService.js'
import Site from '@jsroot/lib/site'
import LangHelper from '@jsroot/lib/langHelper'
import * as types from './types'

const Url = Site.resolveClientUrl('api/CommonApi/GetSports');
export const setSelectedSport = (item) => {
    return {
        type: types.SET_SELECTED_SPORT,
        data: item
    };
}

export const setSportList = (list) => {
    return {
        type: types.SET_SPORT_LIST,
        data: list
    };
}

export function getSportList(productCode) {
    return (dispatch) => {
        DataService.getData(Url, { productCode: productCode})
            .then(response => {
                var data = response.data;
                var itemAll = { value: 0, label: LangHelper.getSingleResource('All') };
                data.unshift(itemAll);
                dispatch(setSportList(data));
                dispatch(setSelectedSport(itemAll));
            });
    };
}