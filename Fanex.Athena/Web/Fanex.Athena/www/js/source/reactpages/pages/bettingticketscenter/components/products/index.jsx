import React, { Component } from 'react'
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'

import AppContainer from './AppContainer.jsx'
import reducer from '../../reducers/products/index.js'
import Store from '../../../../lib/store'

const store = Store.createStore(reducer);

export default class App extends Component {
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