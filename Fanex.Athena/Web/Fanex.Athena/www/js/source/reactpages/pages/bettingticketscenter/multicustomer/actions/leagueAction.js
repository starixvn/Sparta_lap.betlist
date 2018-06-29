import DataService from '@jsroot/lib/DataService.js'
import Site from '@jsroot/lib/site'
import LangHelper from '@jsroot/lib/langHelper'
import Cache from '@jsroot/lib/Cache'
import * as types from './types'
import { getMatchList } from './matchAction'

const Url = Site.resolveClientUrl('api/CommonApi/GetLeagues');
const LEAGUE_CACHE_KEY = "LEAGUE_CACHE_KEY";

function dispatchLeagueList(dispatch, data, date){
    dispatch(setLeagueList(data));
    if(data.length > 0)
    {
        dispatch(setSelectedLeague(data[0]));
        dispatch(getMatchList(data[0].value, date))
    }
}

export const setSelectedLeague = (item) => {
    return {
        type: types.SET_SELECTED_LEAGUE,
        data: item
    };
}

export const setLeagueList = (list) => {
    return {
        type: types.SET_LEAGUE_LIST,
        data: list
    };
}

export function getLeagueList(sportId, date) {
    const cacheKey = LEAGUE_CACHE_KEY + sportId + "_" + date;
    return (dispatch) => {
        var cacheData = Cache.get(cacheKey);
        if(cacheData)
        {
            dispatchLeagueList(dispatch, cacheData, date);
            return;
        }

        DataService.getData(Url, { sportId: sportId, date: date})
            .then(response => {
                var data = response.data;
                Cache.set(cacheKey, data);
                dispatchLeagueList(dispatch, data, date);
            });
    };
}