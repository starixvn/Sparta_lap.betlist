import * as types from '@jsroot/actions/types.js'
import DataService from '@jsroot/lib/DataService.js'
import Site from '@jsroot/lib/site'

const DataUrl = Site.resolveClientUrl('api/BettingTicketCenter/GetByRunning');

export function getGridData(params) {
    return (dispatch) => {
        DataService.getData(DataUrl, params)
            .then(response => {
                dispatch(setGridData(response.data));
            });
    };
}

export function setGridData(data) {
    return {
        type: types.SET_GRID_DATA,
        data: data
    };
}