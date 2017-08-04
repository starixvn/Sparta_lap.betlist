import createReducer from '../lib/createReducer'
import * as types from '../actions/types'

export const getCurrencyList = createReducer({}, {
  [types.GET_CurrencyList](state, action) {
      let newState = Object.assign({},action.data);
      return newState;
  },

});
