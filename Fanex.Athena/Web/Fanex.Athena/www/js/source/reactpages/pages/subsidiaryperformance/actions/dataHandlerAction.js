import * as types from './types.js'
import axios from 'axios'

export function fetchData(params, xsrfToken) {
    return (dispatch, getState) => {
        axios.post('/api/SubsidiaryPerformanceReport/Index', params, {
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    '__RequestVerificationToken': xsrfToken
                }
            })
            .then(response => {
                dispatch(setGridData(response.data.Data));
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

export const setSubsidiary = (subsidiary) => {
    return {
        type: types.SET_SUBSIDIARY,
        data: subsidiary
    }
}

export const setDateRange = (dateRange) => {
    const range = {
        fromDate: dateRange.startDate.format("MM/DD/YYYY"),
        toDate: dateRange.endDate.format("MM/DD/YYYY")
    }

    return {
        type: types.SET_DATE_RANGE,
        data: range
    }
}

export const changeConvertAll = (checked) => {
    return {
        type: types.CHANGE_CONVERT_ALL,
        data: checked
    }
}