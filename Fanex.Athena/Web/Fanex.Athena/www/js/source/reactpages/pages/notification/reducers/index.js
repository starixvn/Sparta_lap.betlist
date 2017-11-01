import { combineReducers } from 'redux';
import * as notificationDataReducer from '../notificationTab/reducers/dataHandlerReducer.js'
import * as notificationFilterReducer from '../notificationTab/reducers/filterHandlerReducer.js'
import * as resultAmendmentDataReducer from '../resultAmendmentTab/reducers/dataHandlerReducer.js'
import * as resultAmendmentFilterReducer from '../resultAmendmentTab/reducers/filterHandlerReducer.js'
import * as loaderReducer from '../../../reducers/loaderReducer'

export default combineReducers(Object.assign(
    notificationDataReducer,
    notificationFilterReducer,
    resultAmendmentDataReducer,
    resultAmendmentFilterReducer,
    loaderReducer
));