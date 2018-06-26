import { combineReducers } from 'redux';
import * as gridDataReducer from '@jsroot/reducers/gridDataReducer.js'
import * as filterHandlerReducer from './filterHandlerReducer.js'
import * as baseCurrencyReducer from '@jsroot/reducers/baseCurrencyReducer'
import * as loaderReducer from '@jsroot/reducers/loaderReducer'
import * as convertAllReducer from '@jsroot/reducers/convertAllReducer'

export default combineReducers(Object.assign(
    gridDataReducer,
    filterHandlerReducer,
    baseCurrencyReducer,
    loaderReducer,
    convertAllReducer
));