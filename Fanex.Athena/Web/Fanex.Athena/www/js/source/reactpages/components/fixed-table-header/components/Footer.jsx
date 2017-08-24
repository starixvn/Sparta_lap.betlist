import React, { Component } from 'react';
import TableSection from './TableSection';

class Body extends Component {
    constructor(props) {
        super(props);
    }


    render() {
        return (
            <TableSection className="footer-content" {...this.props}>
                <tbody>
                    {this.props.children}
                </tbody>
            </TableSection>
        );
    }
}

export default Body;