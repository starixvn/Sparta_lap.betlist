import React, { Component } from 'react';

class RowLayout extends Component {
    constructor(props) {
        super(props)
    }

    _getRowLayout() {
        return this.props.columnLayout.map((cellWidth, index) => {
            return (<td key={`cellLayout${index}`} style={{ width: cellWidth + 'px' }}></td>);
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

export default RowLayout;