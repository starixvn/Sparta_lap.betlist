import { combineReducers } from 'redux'

import notificationReducer from '../components/notification/reducers'
import resultAmendmentReducer from '../components/resultAmendment/reducers'

import * as loaderReducer from '../../../reducers/loaderReducer'

const notification = combineReducers(notificationReducer);
const resultAmendment = combineReducers(resultAmendmentReducer);

export default combineReducers(Object.assign({}, {
    notification,
    resultAmendment
}, loaderReducer));