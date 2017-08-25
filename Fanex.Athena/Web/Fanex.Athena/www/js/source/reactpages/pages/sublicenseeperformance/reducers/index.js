import { combineReducers } from 'redux';
import * as dataHandlerReducer from './dataHandlerReducer.js'
export default combineReducers(Object.assign(
    dataHandlerReducer
));