import { combineReducers } from 'redux'

import * as loaderReducer from '../../../reducers/loaderReducer'
import * as xsrfTokenReducer from '../../../reducers/xsrfTokenReducer'

import productReducer from './product'

const products = combineReducers(productReducer);

export default combineReducers(Object.assign({}, {}, loaderReducer, xsrfTokenReducer));