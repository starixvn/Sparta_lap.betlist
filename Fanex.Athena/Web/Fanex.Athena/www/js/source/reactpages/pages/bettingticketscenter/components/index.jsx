import React from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { createLogger } from 'redux-logger'
import { createStore, applyMiddleware, combineReducers, compose } from 'redux'
import thunkMiddleware from 'redux-thunk'

import Store from '../../../lib/store'
import reducer from '../reducers'
import AppContainer from './AppContainer'

const store = Store.createStore(reducer, {});

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