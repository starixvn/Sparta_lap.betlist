import { combineReducers } from 'redux';
import * as currencyReducers from '../../../reducers/currencyReducer.js'

export default combineReducers(Object.assign(
 currencyReducers
));