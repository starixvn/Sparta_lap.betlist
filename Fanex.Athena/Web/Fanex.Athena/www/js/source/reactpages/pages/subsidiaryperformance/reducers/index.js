import { combineReducers } from 'redux';
import * as dataHandlerReducer from './dataHandlerReducer.js'
import * as filterHandlerReducer from './filterHandlerReducer.js'
import * as productReducer from '../../../reducers/productReducer'
import * as baseCurrencyReducer from '../../../reducers/baseCurrency'
import * as loaderReducer from '../../../reducers/loaderReducer'

export default combineReducers(Object.assign(
    dataHandlerReducer,
    filterHandlerReducer,
    productReducer,
    baseCurrencyReducer,
    loaderReducer
));