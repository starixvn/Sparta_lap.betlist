import { combineReducers } from 'redux';
import * as dataHandlerReducer from './dataHandlerReducer.js'
import * as filterHandlerReducer from './filterHandlerReducer.js'
import * as loaderReducer from '../../../../reducers/loaderReducer'
import * as tabReducer from '../../../../reducers/tabReducer'

export default combineReducers(Object.assign(
    dataHandlerReducer,
    filterHandlerReducer,
    loaderReducer,
    tabReducer
));