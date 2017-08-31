import * as types from './types.js'
import axios from 'axios'

import * as site from '../../../lib/site'

export function fetchData(params) {
    return (dispatch, getState) => {
        const url = site.resolveClientUrl('api/SubsidiaryPerformanceReport/Index');
        axios.post(url, params.data, {
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    '__RequestVerificationToken': params.xsrfToken
                }
            })
            .then(response => {
                dispatch(setGridData(response.data));
            })
            .catch(function(error) {
                console.log("error:", error);
            });
    };
}

export function setGridData(data) {
    var result = {
        type: types.SET_GRID_DATA,
        data: data
    };

    return result;
}