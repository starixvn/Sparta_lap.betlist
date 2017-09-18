import * as types from './types.js'
import axios from 'axios'

import Site from 'lrflib/site'


export const fetchData = (params) => {
    const url = Site.resolveClientUrl('api/SubsidiaryPerformanceReport/Index');
    return axios.post(url, params.data, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            '__RequestVerificationToken': params.xsrfToken
        }
    });
}

export function getGridData(params) {
    return (dispatch, getState) => {

        fetchData(params).then(response => {
            dispatch(setGridData(response.data));
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