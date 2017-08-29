import * as types from './types.js'

export function fetchData(inputValue) {

  return (dispatch, getState) => {
      axios.fetch(inputValue).then(rs=>dispatch(setGridData(rs)));
  };

}

export function setGridData(data) {

  var result = {
    type: types.SET_GRID_DATA,
    data: data
  };

  return result;

}