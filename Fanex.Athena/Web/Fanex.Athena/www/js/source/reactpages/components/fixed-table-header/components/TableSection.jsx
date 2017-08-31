import React, { Component } from 'react';
import PropTypes from 'prop-types';

const scrollbarWidth = 19;
class TableSection extends Component {
    constructor(props) {
        super(props);
        
        const containerWidth = props.autoWidth ? props.maxWidth : props.width + scrollbarWidth;
        this.containerStyle = props.style ? { ...props.style, width: containerWidth } :
                                            { width: containerWidth };

        this.tableStyle = {
            width: props.autoWidth ? props.maxWidth - scrollbarWidth : props.width
        }
    }

    static propTypes = {
        tableClass: PropTypes.string,
        width: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
        maxWidth: PropTypes.number,
        autoWidth: PropTypes.bool,
    };

    static defaultProps = {
        tableClass: "table",
        width: window.innerWidth - 30,
        maxWidth: window.innerWidth - 30,
        autoWidth: true
    };

    render() {
        const { tableClass, width, autoWidth, maxWidth, ...rest } = this.props;

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