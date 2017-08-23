import React, { Component } from 'react';

class Cell extends Component {
    constructor(props) {
        super(props)
    }

    render() {
        return (
            <td {...this.props}>{this.props.children}</td>
        );
    }
}

export default Cell;