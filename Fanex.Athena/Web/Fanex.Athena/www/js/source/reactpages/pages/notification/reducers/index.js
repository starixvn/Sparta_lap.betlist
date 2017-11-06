import { combineReducers } from 'redux'

import notificationReducer from './notification'
import resultAmendmentReducer from '../components/resultAmendment/reducers'

import * as loaderReducer from '../../../reducers/loaderReducer'
import * as xsrfTokenReducer from '../../../reducers/xsrfTokenReducer'

const notification = combineReducers(notificationReducer);
const resultAmendment = combineReducers(resultAmendmentReducer);

export default combineReducers(Object.assign({}, {
    notification,
    resultAmendment
}, loaderReducer, xsrfTokenReducer));