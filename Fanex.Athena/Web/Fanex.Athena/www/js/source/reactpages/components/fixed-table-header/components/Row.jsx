import React, { Component } from 'react';

class Row extends Component {
    constructor(props) {
        super(props)
    }

    render() {
        return (
            <tr>{this.props.children}</tr>
        );
    }
}

export default Row;