import { combineReducers } from 'redux';
import * as gridDataReducer from '../../../../reducers/gridDataReducer.js'
import * as filterHandlerReducer from './filterHandlerReducer.js'
import * as baseCurrencyReducer from '../../../../reducers/baseCurrencyReducer'
import * as loaderReducer from '../../../../reducers/loaderReducer'

export default combineReducers(Object.assign(
    gridDataReducer,
    filterHandlerReducer,
    baseCurrencyReducer,
    loaderReducer
));