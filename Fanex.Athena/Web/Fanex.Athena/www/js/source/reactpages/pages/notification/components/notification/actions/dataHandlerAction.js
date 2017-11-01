import * as types from './types.js'
import axios from 'axios'

import Site from '../../../../../lib/site'

const fetchData = (params) => {
    const url = Site.resolveClientUrl('api/NotificationReport/Index');
    return axios.post(url, params.data, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            '__RequestVerificationToken': params.xsrfToken
        }
    });
}

export function getNotificationGridData(params) {
    return (dispatch) => {
        fetchData(params).then(response => {
            dispatch(setNotificationGridData(response.data));
        });
    };
}

export function setNotificationGridData(data) {
    return {
        type: types.SET_GRID_DATA,
        data: data
    };
}