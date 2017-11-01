import * as types from './types.js'
import axios from 'axios'

import Site from '../../../../../lib/site'

const fetchData = (params) => {
    const url = Site.resolveClientUrl('api/NotificationReport/ResultAmendment');
    return axios.post(url, params.data, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            '__RequestVerificationToken': params.xsrfToken
        }
    });
}

export function getResultAmendmentGridData(params) {
    return (dispatch) => {
        fetchData(params).then(response => {
            dispatch(settResultAmendmentGridData(response.data));
        });
    };
}

export function settResultAmendmentGridData(data) {
    return {
        type: types.SET_GRID_DATA,
        data: data
    };
}