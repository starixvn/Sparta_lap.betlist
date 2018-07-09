import DataService from '@jsroot/lib/DataService.js'
import Site from '@jsroot/lib/site'
import LangHelper from '@jsroot/lib/langHelper'
import * as types from './types'
import { getLeagueList } from './leagueAction'

const Url = Site.resolveClientUrl('api/CommonDataApi/GetSports');
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

export function getSportList(productCode, date) {
    return (dispatch) => {
        DataService.getData(Url, { productCode: productCode})
            .then(response => {
                let data = response.data;
                let idList = data.map((item) => item.value).join(",");
                
                let itemAll = { value: idList, label: LangHelper.getSingleResource('All') };
                data.unshift(itemAll);
                dispatch(setSportList(data));
                dispatch(setSelectedSport(itemAll));
                if(date){
                    dispatch(getLeagueList(idList, date));
                }
            });
    };
}