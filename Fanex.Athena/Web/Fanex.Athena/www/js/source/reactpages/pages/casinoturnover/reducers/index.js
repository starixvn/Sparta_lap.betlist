import { combineReducers } from 'redux';
import * as dateRangeReducer from '@jsroot/reducers/dateRangeReducer'
import * as currencyReducer from '@jsroot/reducers/currencyReducer'
import * as baseCurrencyReducer from '@jsroot/reducers/baseCurrencyReducer'
import * as convertAllReducer from '@jsroot/reducers/convertAllReducer'
import * as productReducer from '@jsroot/reducers/productReducer'
import * as usernameReducer from '@jsroot/reducers/usernameReducer'
import * as statusReducer from '@jsroot/reducers/statusReducer'
import * as reportTitleReducer from '@jsroot/reducers/reportTitleReducer'
import * as loaderReducer from '@jsroot/reducers/loaderReducer'
import * as filterHandlerReducer from './filterHandlerReducer'

export default combineReducers(Object.assign(
    dateRangeReducer,
    currencyReducer,
    baseCurrencyReducer,
    convertAllReducer,
    productReducer,
    usernameReducer,
    statusReducer,
    reportTitleReducer,
    loaderReducer,
    filterHandlerReducer
));