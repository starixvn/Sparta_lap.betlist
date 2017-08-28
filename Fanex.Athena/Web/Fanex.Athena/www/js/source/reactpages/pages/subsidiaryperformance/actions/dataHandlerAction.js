import * as types from './types.js'

export function fetchData(inputValue) {
  
  var result ={
    type: types.FETCH_DATA,
    data: [{ a: "alex", b: "vok" }],
  };
    
  return result;
    
}