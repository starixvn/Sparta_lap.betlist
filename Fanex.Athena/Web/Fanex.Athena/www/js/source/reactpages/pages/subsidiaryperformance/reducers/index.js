import { combineReducers } from 'redux';
import * as dataHandlerReducer from './dataHandlerReducer.js'
import * as filterHandlerReducer from './filterHandlerReducer.js'
import * as productReducer from '../../../reducers/product'
import * as baseCurrencyReducer from '../../../reducers/baseCurrency'
import * as fixedTableHeader from '../../../reducers/fixedTableHeader'

export default combineReducers(Object.assign(
    dataHandlerReducer,
    filterHandlerReducer,
    productReducer,
    baseCurrencyReducer,
    fixedTableHeader
));