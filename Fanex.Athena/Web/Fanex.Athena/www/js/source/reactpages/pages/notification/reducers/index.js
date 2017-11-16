import { combineReducers } from 'redux'

import generalReducer from './general'
import resultAmendmentReducer from './resultAmendment'

import * as loaderReducer from '../../../reducers/loaderReducer'
import * as xsrfTokenReducer from '../../../reducers/xsrfTokenReducer'

const general = combineReducers(generalReducer);
const resultAmendment = combineReducers(resultAmendmentReducer);

export default combineReducers(Object.assign({}, {
    general,
    resultAmendment
}, loaderReducer, xsrfTokenReducer));