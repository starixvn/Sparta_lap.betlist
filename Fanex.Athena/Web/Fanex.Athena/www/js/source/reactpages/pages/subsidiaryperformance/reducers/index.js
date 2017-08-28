import { combineReducers } from 'redux';
import * as dataHandlerReducer from './dataHandlerReducer.js'
import * as productReducer from '../../../reducers/product'
import * as baseCurrencyReducer from '../../../reducers/baseCurrency'

export default combineReducers(Object.assign(
    dataHandlerReducer,
    productReducer,
    baseCurrencyReducer
));