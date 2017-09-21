import React, { Component } from 'react';
import PropTypes from 'prop-types';

const SCROLLBAR_WIDTH = 19;
const GUTTER = 15;
const MAX_WIDTH = window.innerWidth - (GUTTER * 2);

class TableSection extends Component {
    constructor(props) {
        super(props);
    }

    static propTypes = {
        tableClass: PropTypes.string,
        width: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
        maxWidth: PropTypes.number,
        minWidth: PropTypes.number,
        autoWidth: PropTypes.bool,
    };

    static defaultProps = {
        tableClass: "table",
        width: MAX_WIDTH,
        maxWidth: MAX_WIDTH,
        autoWidth: true
    };

    get containerWidth() {
        const { autoWidth, maxWidth, width } = this.props;
        return autoWidth ? maxWidth : width;
    }

    get containerStyle() {
        const { minWidth } = this.props;

        return {
            ...this.props.style,
            width: this.containerWidth,
            minWidth
        };
    }

    get tableStyle() {
        const { minWidth } = this.props;
        const tableWidth = Math.max(this.containerWidth, minWidth) - SCROLLBAR_WIDTH;

        return {
            width: tableWidth
        }
    }

    render() {
        const { tableClass, width, autoWidth, maxWidth, minWidth, ...rest } = this.props;

        return (
            <div {...rest} style={this.containerStyle}>
                <div className="table-wrapper">
                    <table style={this.tableStyle} className={tableClass}>
                        {this.props.children}
                    </table>
                </div>
            </div >
        );
    }
}

export default TableSection;