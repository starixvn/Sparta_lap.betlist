import React, { Component } from 'react';

class RowLayout extends Component {
    constructor(props) {
        super(props)
    }

    render() {
        const cells = this.props.layoutConfig.map((cellWidth, index) => {
            return (<td key={`cellLayout${index}`} style={{ width: cellWidth + 'px' }}></td>);
        });

        return (
            <tr className="first-row">
                {cells}
            </tr>
        );
    }
}

export default RowLayout;