import * as types from './types.js'
import axios from 'axios'

import Site from '../../../../lib/site'

const fetchData = (params) => {
    const url = Site.resolveClientUrl('api/NotificationReport/ResultAmendment');
    return axios.post(url, params.data, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            '__RequestVerificationToken': params.xsrfToken
        }
    });
}

const markNotification = (id, type) => {
    const url = Site.resolveClientUrl('api/NotificationReport/MarkNotificationAsRead');
    return axios.post(url, { MessageId: id, MessageType: type });
}

export function getGridData(params) {
    return (dispatch) => {
        fetchData(params).then(response => {
            dispatch(setGridData({
                Data: response.data.DataList,
                Total: response.data.TotalRows
            }));
        });
    };
}

export function setGridData(data) {
    return {
        type: types.SET_GRID_DATA,
        data: data
    };
}

export function markItemAsRead(params) {   
    return (dispatch) => {
        markNotification(params.Id, params.Type).then(response => {
            if(response.data.IsSuccessful)
            {
                params.Data[params.Index].IsRead = true;

                dispatch(setGridData({
                    Data: params.Data,
                    Total: params.Total
                }));
            }
        });
    };
}