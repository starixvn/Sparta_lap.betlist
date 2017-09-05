import React, { Component, Children } from 'react';
import ReactDOM from 'react-dom';
import RowLayout from './RowLayout';
import PropTypes from 'prop-types';
import Body from './Body';
import Footer from './Footer';

const MAX_WIDTH = window.innerWidth - 30;

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
        width: MAX_WIDTH,
        maxWidth: MAX_WIDTH,
        autoWidth: true
    }

    get columnWidthSum() {
        const columnLayout = this.props.columnLayout;
        return columnLayout.find(x => typeof x !== "number") ?
            null : columnLayout.reduce((prev, next) => prev + next, 0);
    }

    _getUpdatedColumnLayout() {
        const { width, columnLayout, maxWidth, autoWidth } = this.props;
        const sumOfColumnWidth = autoWidth ? maxWidth : width;

        const newColumnLayout = this.columnWidthSum && columnLayout.map(cellWidth => {
            return sumOfColumnWidth / this.columnWidthSum * cellWidth
        });

        return newColumnLayout;
    }

    _buildContent() {
        const { width, columnLayout, autoWidth, maxWidth } = this.props;

        const newColumnLayout = this.columnWidthSum && this.columnWidthSum !== width ?
            this._getUpdatedColumnLayout() :
            columnLayout;

        return React.Children.map(this.props.children, (child, index) => {
            const rows = React.Children.toArray(child.props.children);
            const layoutRow = <RowLayout key={`rowLayout${index}`} columnLayout={newColumnLayout} />;

            const newChild = {
                props: {
                    ...child.props,
                    autoWidth,
                    width,
                    maxWidth,
                    children: [layoutRow, ...rows]
                }
            };

            return Object.assign({}, child, newChild);
        });
    }

    render() {
        return (
            <div className="table-container" style={{ maxWidth: this.props.maxWidth }}>
                {this._buildContent()}
            </div>
        )
    }
}

export default Table;