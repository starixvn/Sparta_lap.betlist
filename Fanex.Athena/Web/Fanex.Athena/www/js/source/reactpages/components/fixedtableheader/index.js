import React from 'react';
import ReactDOM from 'react-dom';
import FixedTableHeader from './FixedTableHeader';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware, combineReducers, compose } from 'redux';
import thunkMiddleware from 'redux-thunk';
import { createLogger } from 'redux-logger';
import reducer from './reducers/index.js';

// middleware that logs actions
const loggerMiddleware = createLogger({ predicate: (getState, action) => __DEV__ });

function configureStore(initialState) {
    const enhancer = compose(
      applyMiddleware(
        thunkMiddleware, // lets us dispatch() functions
        loggerMiddleware,
      ),
    );
    return createStore(reducer, initialState, enhancer);
}

const store = configureStore({});

export default class App extends React.Component {
    constructor(props) {
        super(props);
        console.log("init");
    }
    componentDidMount() {
        console.log("hello");
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