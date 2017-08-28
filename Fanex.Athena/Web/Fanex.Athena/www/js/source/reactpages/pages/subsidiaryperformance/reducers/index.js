import { combineReducers } from 'redux';
import * as dataHandlerReducer from './dataHandlerReducer.js'
import * as productReducer from '../../../reducers/product'
export default combineReducers(Object.assign(
    dataHandlerReducer,
    productReducer
));