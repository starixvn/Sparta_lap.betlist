// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX
"use strict";
var react_redux_1 = require('react-redux');
var TodoItem = function (_a) {
    var todo = _a.todo, destroyTodo = _a.destroyTodo;
    return (<div>
            {todo.text}
            <span onClick={destroyTodo}> x </span>
        </div>);
};
var mapStateToProps = function (state) {
    return {
        todo: state.todos[0]
    };
};
var mapDispatchToProps = function (dispatch) {
    return {
        destroyTodo: function () { return dispatch({
            type: 'DESTROY_TODO'
        }); }
    };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.default = react_redux_1.connect(mapStateToProps, mapDispatchToProps)(TodoItem);
