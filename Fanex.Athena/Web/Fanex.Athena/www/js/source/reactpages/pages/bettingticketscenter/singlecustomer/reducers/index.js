import { combineReducers } from 'redux';
import * as gridDataReducer from '@jsroot/reducers/gridDataReducer.js'
import * as filterHandlerReducer from './filterHandlerReducer.js'
import * as productReducer from '@jsroot/reducers/productReducer'
import * as baseCurrencyReducer from '@jsroot/reducers/baseCurrencyReducer'
import * as statusReducer from '@jsroot/reducers/statusReducer'
import * as platformReducer from '@jsroot/reducers/platformReducer'
import * as liveStatusReducer from '@jsroot/reducers/liveStatusReducer'
import * as loaderReducer from '@jsroot/reducers/loaderReducer'
import * as dateRangeReducer from '@jsroot/reducers/dateRangeReducer'

export default combineReducers(Object.assign(
    gridDataReducer,
    filterHandlerReducer,
    productReducer,
    baseCurrencyReducer,
    statusReducer,
    platformReducer,
    liveStatusReducer,
    loaderReducer,
    dateRangeReducer
));