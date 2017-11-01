import { combineReducers } from 'redux';
import * as notificationDataReducer from '../components/notification/reducers/dataHandlerReducer.js'
import * as notificationFilterReducer from '../components/notification/reducers/filterHandlerReducer.js'
import * as resultAmendmentDataReducer from '../components/resultAmendment/reducers/dataHandlerReducer.js'
import * as resultAmendmentFilterReducer from '../components/resultAmendment/reducers/filterHandlerReducer.js'
import * as loaderReducer from '../../../reducers/loaderReducer'

export default combineReducers(Object.assign(
    notificationDataReducer,
    notificationFilterReducer,
    resultAmendmentDataReducer,
    resultAmendmentFilterReducer,
    loaderReducer
));