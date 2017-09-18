import * as types from './types.js'
import axios from 'axios'
import ReportExcel from './reportExcel'

import Site from 'lrflib/site'

export function fetchData(params) {
    return (dispatch, getState) => {
        const url = Site.resolveClientUrl('api/SubsidiaryPerformanceReport/Index');
        axios.post(url, params.data, {
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    '__RequestVerificationToken': params.xsrfToken
                }
            })
            .then(response => {
                if (params.isExportExcel) {
                    const excel = new ReportExcel(response.data);
                    excel.export();
                } else {
                    dispatch(setGridData(response.data));
                }
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