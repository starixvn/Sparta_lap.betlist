import * as types from '../../../../actions/types.js'
import axios from 'axios'

import Site from '../../../../lib/site'

const fetchData = (params) => {
    const url = Site.resolveClientUrl('api/DailyBetList/index?currencyid=0&currencyname=ALL&basecurrency=eur&productcode=SB&status=won&date=05/30/2018&_search=false&nd=1527667610711&rows=50&page=1&sidx=&sord=asc&_=1527667521796');
    return axios.get(url, params.data, {
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