import React, { Component } from 'react';
import TableSection from './TableSection';

class Header extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <TableSection className="header-content" {...this.props}>
                <thead>
                    {this.props.children}
                </thead>
            </TableSection>
        );
    }
}

export default Header;