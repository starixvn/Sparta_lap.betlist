import React from 'react'
import ReactDOM from 'react-dom';
import AppContainer from './AppContainer.jsx'
import { Provider } from 'react-redux'
import { createLogger } from 'redux-logger'
import { createStore, applyMiddleware, combineReducers, compose } from 'redux'
import thunkMiddleware from 'redux-thunk'
import reducer from '../reducers/index.js'
import { debuggingConfig } from '../../../constants/config'

const isDev = process.env.NODE_ENV === "development";
// middleware that logs actions
const loggerMiddleware = createLogger({
  predicate: (getState, action) => isDev ||
    (window.__lrfConfig__ &&
    window.__lrfConfig__.env === debuggingConfig.env &&
    (!window.__lrfConfig__.actionFilter || window.__lrfConfig__.actionFilter === action.type))
});

const defaultState = Object.assign({}, window.Lrf);

function configureStore(initialState) {
  const enhancer = compose(
    applyMiddleware(
      thunkMiddleware, // lets us dispatch() functions
      loggerMiddleware,
    ),
  );

  return createStore(reducer, initialState, enhancer);
}

const store = configureStore(defaultState);

export default class App extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <Provider store={store}>
        <AppContainer />
      </Provider>
    );
  }
}

ReactDOM.render(<App />, document.getElementById("app-container"));