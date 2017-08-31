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

    static propTypes = {
        width: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
        maxWidth: PropTypes.number,
        autoWidth: PropTypes.bool,
        columnLayout: PropTypes.arrayOf(PropTypes.oneOfType([PropTypes.number, PropTypes.string])).isRequired
    }

    static defaultProps = {
        width: window.innerWidth - 30,
        maxWidth: window.innerWidth - 30,
        autoWidth: true
    }

    get columnWidthSum() {
        const columnLayout = this.props.columnLayout;
        return columnLayout.every(x => typeof x !== "string") ?
            columnLayout.reduce((prev, next) => prev + next, 0) : null;
    }

    _buildContent() {
        const { width, columnLayout, autoWidth, maxWidth } = this.props;
        const updatedWidth = this.columnWidthSum && this.columnWidthSum > width ? this.columnWidthSum : width;

        return React.Children.map(this.props.children, (child, index) => {
            const rows = React.Children.toArray(child.props.children);
            const layoutRow = <RowLayout key={`rowLayout${index}`}
                width={updatedWidth}
                columnWidthSum={this.columnWidthSum}
                columnLayout={columnLayout} />;

            const newChild = {
                props: {
                    ...child.props,
                    autoWidth,
                    width: updatedWidth,
                    maxWidth,
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