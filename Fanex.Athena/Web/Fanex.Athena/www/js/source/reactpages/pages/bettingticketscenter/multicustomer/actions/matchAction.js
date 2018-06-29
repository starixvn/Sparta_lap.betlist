import DataService from '@jsroot/lib/DataService.js'
import Site from '@jsroot/lib/site'
import LangHelper from '@jsroot/lib/langHelper'
import Cache from '@jsroot/lib/Cache'
import * as types from './types'

const Url = Site.resolveClientUrl('api/CommonApi/GetMatches');
const MATCH_CACHE_KEY = "MATCH_CACHE_KEY";

function dispatchMatchList(dispatch, data){
    dispatch(setMatchList(data));
    if(data.length > 0)
    {
        dispatch(setSelectedMatch(data[0]));
    }
}

export const setSelectedMatch = (item) => {
    return {
        type: types.SET_SELECTED_MATCH,
        data: item
    };
}

export const setMatchList = (list) => {
    return {
        type: types.SET_MATCH_LIST,
        data: list
    };
}

export function getMatchList(leagueId, date) {
    const cacheKey = MATCH_CACHE_KEY + leagueId + "_" + date;
    return (dispatch) => {
        var cacheData = Cache.get(cacheKey);
        if(cacheData)
        {
            dispatchMatchList(dispatch, cacheData);
            return;
        }

        DataService.getData(Url, { leagueId: leagueId, date: date})
            .then(response => {
                var data = response.data;
                Cache.set(cacheKey, data);
                dispatchMatchList(dispatch, data);
            });
    };
}