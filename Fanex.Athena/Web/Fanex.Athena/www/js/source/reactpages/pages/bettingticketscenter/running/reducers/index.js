import { combineReducers } from 'redux';
import * as gridDataReducer from '@jsroot/reducers/gridDataReducer.js'
import * as usernameReducer from '@jsroot/reducers/usernameReducer'
import * as productReducer from '@jsroot/reducers/productReducer'
import * as currencyReducer from '@jsroot/reducers/currencyReducer'
import * as baseCurrencyReducer from '@jsroot/reducers/baseCurrencyReducer'
import * as loaderReducer from '@jsroot/reducers/loaderReducer'
import * as dateRangeReducer from '@jsroot/reducers/dateRangeReducer'

export default combineReducers(Object.assign(
    gridDataReducer,
    usernameReducer,
    productReducer,
    currencyReducer,
    baseCurrencyReducer,
    loaderReducer,
    dateRangeReducer
));