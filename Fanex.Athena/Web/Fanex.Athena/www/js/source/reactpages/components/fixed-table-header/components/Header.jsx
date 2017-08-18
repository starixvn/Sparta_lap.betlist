import React, { Component } from 'react';

class Header extends Component {
    constructor(props) {
        super(props)
    }

    render() {
        return (
            <th>{this.props.children}</th>
        );
    }
}

export default Header;