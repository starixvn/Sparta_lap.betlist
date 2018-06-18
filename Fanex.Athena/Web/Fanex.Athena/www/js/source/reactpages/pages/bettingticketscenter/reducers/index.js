import { combineReducers } from 'redux';
import * as gridDataReducer from '../../../reducers/gridDataReducer.js'
import * as filterHandlerReducer from './filterHandlerReducer.js'

export default combineReducers(Object.assign(
    gridDataReducer,
    filterHandlerReducer
));