import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import RowLayout from './RowLayout';
import PropTypes from 'prop-types';
import createTableSection from './tableSection';
import { MAX_WIDTH, DEFAULT_MILLISECOND_FOR_WAITING, DEFAULT_COLUMN_WIDTH, BODY_WIDTH } from '../constants';

const headerContainerProps = { className: "header-content", isHeader: true };
const Header = createTableSection(headerContainerProps);

const bodyContainerProps = { className: "body-content" };
const Body = createTableSection(bodyContainerProps);

const footerContainerProps = { className: "footer-content" };
const Footer = createTableSection(footerContainerProps);

function debounce(func, wait) {
    let timeout;
    return function () {
        const context = this,
            args = arguments;

        const later = function () {
            func.apply(context, args);
        };

        clearTimeout(timeout);
        timeout = setTimeout(later, wait || DEFAULT_MILLISECOND_FOR_WAITING);
    };
}

class Table extends Component {
    constructor(props) {
        super(props);

        this.diffWidth = BODY_WIDTH - props.maxWidth;
        this.columnsWidth = this._getColumnsWidth();

        this.state = {
            maxWidth: props.maxWidth
        }
    }

    static propTypes = {
        width: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
        maxWidth: PropTypes.number,
        minWidth: PropTypes.number,
        autoWidth: PropTypes.bool,
        bodyHeight: PropTypes.number,
        header: PropTypes.oneOfType([PropTypes.object, PropTypes.arrayOf(PropTypes.object)]).isRequired,
        body: PropTypes.arrayOf(PropTypes.object),
        footer: PropTypes.arrayOf(PropTypes.object)
    }

    static defaultProps = {
        width: MAX_WIDTH,
        maxWidth: MAX_WIDTH,
        autoWidth: true
    }

    _getColumnsWidth() {
        const headerRows = this.props.header;
        const headerIsArray = Array.isArray(headerRows);
        if (headerIsArray && headerRows.length === 0) {
            return null;
        }

        const columnsWidth = [];
        const currentNextCellIndexByRow = {};

        const getWidthByCells = (cells, currentRowIndex) => {
            for (let i = 0; i < cells.length; i++) {
                const cell = cells[i];
                const cellProps = cell.props;
                const colspan = Number(cellProps.colSpan);

                if (colspan && colspan > 1) {
                    const nextRowIndex = currentRowIndex + 1;
                    if (currentNextCellIndexByRow[nextRowIndex] === undefined) {
                        currentNextCellIndexByRow[nextRowIndex] = 0;
                    }
                    const fromNextCellIndex = currentNextCellIndexByRow[nextRowIndex];
                    const toNextCellIndex = fromNextCellIndex + colspan - 1;
                    const nextCells = headerRows[nextRowIndex].props.children;

                    const nextColspanCells = this._getNextColspanCells(nextCells, fromNextCellIndex, toNextCellIndex);
                    currentNextCellIndexByRow[nextRowIndex] += nextColspanCells.length;
                    getWidthByCells(nextColspanCells, nextRowIndex);
                }
                else {
                    const cellWidth = cellProps.colWidth ? cellProps.colWidth : DEFAULT_COLUMN_WIDTH;
                    columnsWidth.push(cellWidth);
                }
            }
        }

        const currentRowIndex = 0;
        const cellList = headerIsArray ? headerRows[currentRowIndex].props.children : headerRows.props.children;
        getWidthByCells(cellList, currentRowIndex);

        return columnsWidth;
    }

    _getNextColspanCells(cells, fromCellIndex, toCellIndex) {
        const colspanCells = [];
        for (let i = fromCellIndex; i <= toCellIndex; i++) {
            const cell = cells[i];
            if (cell.props.colSpan) {
                toCellIndex -= 1;
            }
            colspanCells.push(cell);
        }

        return colspanCells;
    }

    get columnWidthSum() {
        return !this.columnsWidth || !this.columnsWidth.length || this.columnsWidth.find(x => typeof x !== "number") ?
            null : this.columnsWidth.reduce((prev, current) => prev + current, 0);
    }

    componentDidMount() {
        this._handleResize();
        window.addEventListener('resize', debounce(this._handleResize))
    }

    componentWillUnmount() {
        window.removeEventListener('resize', debounce(this._handleResize))
    }

    _handleResize = (event) => {
        event && event.preventDefault();
        const maxWidth = document.body.clientWidth - this.diffWidth;

        if (this.state.maxWidth !== maxWidth) {
            this.setState({ maxWidth });
        }
    }

    _getUpdatedColumnLayout() {
        const { width, autoWidth } = this.props;
        const sumOfColumnWidth = autoWidth ? this.state.maxWidth : width;

        const newColumnsWidth = this.columnWidthSum && this.columnsWidth.map(cellWidth => {
            return sumOfColumnWidth / this.columnWidthSum * cellWidth
        });

        return newColumnsWidth;
    }

    render() {
        const { width, autoWidth, minWidth, bodyHeight, header, body, footer } = this.props;
        const maxWidth = this.state.maxWidth;

        const newColumnLayout = this.columnWidthSum && this.columnWidthSum !== width ?
            this._getUpdatedColumnLayout() :
            this.columnsWidth;

        if (!newColumnLayout) {
            return null;
        }

        const sectionProps = { width, autoWidth, minWidth, maxWidth };
        const rowLayout = <RowLayout columnLayout={newColumnLayout} />;

        return (
            <div className="table-container" style={{ maxWidth }}>
                <Header {...sectionProps}>
                    {rowLayout}
                    {header}
                </Header>

                {
                    body &&
                    <Body {...sectionProps} maxHeight={bodyHeight}>
                        {rowLayout}
                        {body}
                    </Body>
                }

                {
                    footer &&
                    <Footer {...sectionProps}>
                        {rowLayout}
                        {footer}
                    </Footer>
                }
            </div>
        )
    }
}

export default Table;