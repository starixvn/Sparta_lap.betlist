import * as types from '@jsroot/actions/types.js'
import DataService from '@jsroot/lib/DataService.js'
import Site from '@jsroot/lib/site'

export function getGridData(url, params) {
    return (dispatch) => {
        DataService.getData(url, params)
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