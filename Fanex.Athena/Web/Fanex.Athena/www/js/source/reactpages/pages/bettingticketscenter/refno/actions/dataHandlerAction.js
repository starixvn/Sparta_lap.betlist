import * as types from '../../../../actions/types.js'
import axios from 'axios'

import Site from '../../../../lib/site'

const fetchData = (params) => {
    const url = Site.resolveClientUrl('api/BettingTicketsCenter/GetByRefNo');
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
    return () => {
        axios({
            url: Site.resolveClientUrl('BettingTicketsCenter/ExportRefNo'),
            method: 'GET',
            responseType: 'blob', // important
        }).then((response) => {
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', 'BettingTicketsCenter_RefNo.xls');
            document.body.appendChild(link);
            link.click();
            link.parentNode.removeChild(link);
        });
    };
}

export function setGridData(data) {
    return {
        type: types.SET_GRID_DATA,
        data: data
    };
}