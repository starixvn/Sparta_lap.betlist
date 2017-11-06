import * as dataHandlerReducer from './dataHandlerReducer.js'
import * as filterHandlerReducer from './filterHandlerReducer.js'

export default Object.assign({}, {...dataHandlerReducer, ...filterHandlerReducer })