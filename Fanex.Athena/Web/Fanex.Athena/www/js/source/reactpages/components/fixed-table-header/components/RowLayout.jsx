import React, { Component } from 'react';
import PropTypes from 'prop-types';

class RowLayout extends Component {
    constructor(props) {
        super(props)
    }

    _getRowLayout() {
        return this.props.columnLayout.map((cellWidth, index) => {
            const width = Number(cellWidth) === NaN ? cellWidth : `${cellWidth}px`;

            return (<td key={`cellLayout${index}`} style={{ width: width }}></td>);
        });
    }

    render() {
        return (
            <tr className="first-row">
                {this._getRowLayout()}
            </tr>
        );
    }
}

RowLayout.propTypes = {
    columnLayout: PropTypes.arrayOf(PropTypes.number, PropTypes.string).isRequired
}

export default RowLayout;