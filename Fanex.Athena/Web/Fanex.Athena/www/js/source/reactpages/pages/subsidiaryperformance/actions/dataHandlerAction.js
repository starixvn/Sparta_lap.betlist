import * as types from './types.js'
import axios from 'axios'

import Site from '../../../lib/site'

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
    return (dispatch) => {
        fetchData(params).then(response => {
            dispatch(setGridData(response.data));
        });
    };
}

export function exportExcel(params) {
    return () => fetchData(params);
}

export function setGridData(data) {
    return {
        type: types.SET_GRID_DATA,
        data: data
    };
}