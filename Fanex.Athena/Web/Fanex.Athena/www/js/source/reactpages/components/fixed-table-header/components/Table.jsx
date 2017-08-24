import React, { Component, Children } from 'react';
import ReactDOM from 'react-dom';
import RowLayout from './RowLayout';
import PropTypes from 'prop-types';
import Body from './Body';
import Footer from './Footer';

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
                    ...child.props,
                    autoWidth: this.props.autoWidth,
                    width: this.props.width,
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

Table.propTypes = {
    width: PropTypes.number,
    autoWidth: PropTypes.bool,
    columnLayout: PropTypes.arrayOf(PropTypes.number, PropTypes.string).isRequired
}

Table.defaultProps = {
    width: window.innerWidth - 15,
    autoWidth: true
}

export default Table;