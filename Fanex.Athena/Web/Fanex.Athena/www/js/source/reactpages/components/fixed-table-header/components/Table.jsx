import React, { Component, Children } from 'react';
import RowLayout from './RowLayout';

class Table extends Component {
    constructor(props) {
        super(props);
    }

    _buildContent() {
        return React.Children.map(this.props.children, (child, index) => {
            let rows = React.Children.toArray(child.props.children);
            const layoutRow = <RowLayout key={`rowLayout${index}`} columnLayout={this.props.columnLayout} />;

            const newChild = {
                props: {
                    children: [layoutRow, ...rows]
                }
            };

            return Object.assign({}, child, newChild);
        });
    }

    render() {
        return (
            <div className="table-container">
                {this._buildContent()}
            </div>
        )
    }
}

export default Table;