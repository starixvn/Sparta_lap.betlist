import { combineReducers } from 'redux';
import * as gridDataReducer from '../../../../reducers/gridDataReducer.js'
import * as filterHandlerReducer from './filterHandlerReducer.js'
import * as dateRangeReducer from '../../../../reducers/dateRangeReducer'

export default combineReducers(Object.assign(
    gridDataReducer,
    filterHandlerReducer,
    dateRangeReducer
));