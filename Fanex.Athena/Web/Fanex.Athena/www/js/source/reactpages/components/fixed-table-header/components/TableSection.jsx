import React, { Component } from 'react';
import PropTypes from 'prop-types';

const SCROLLBAR_WIDTH = 19;
const MAX_WIDTH = window.innerWidth - 30;

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

    get containerStyle(){
        const { autoWidth, maxWidth, width, minWidth } = this.props;
        const containerWidth = autoWidth ? maxWidth : width;

        return { 
            ...this.props.style, 
            width: containerWidth, 
            minWidth };
    }

    get tableStyle(){
        const { autoWidth, maxWidth, width, minWidth } = this.props;       
        let tableWidth = autoWidth ? maxWidth : width;
        tableWidth = Math.max(tableWidth, minWidth) - SCROLLBAR_WIDTH;

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