import React, { Component } from 'react';
import PropTypes from 'prop-types';

class TableSection extends Component {
    constructor(props) {
        super(props);
        this.containerStyle = {
            width: props.autoWidth ? "100%" : props.width + 19
        }

        this.tableStyle = {
            width: props.autoWidth ? "100%" : props.width
        }
    }

    render() {
        const { tableClass, width, autoWidth, ...rest } = this.props;
        return (
            <div style={this.containerStyle} {...rest}>
                <div className="table-wrapper">
                    <table style={this.tableStyle} className={tableClass}>
                        {this.props.children}
                    </table>
                </div>
            </div >
        );
    }
}

TableSection.propTypes = {
    tableClass: PropTypes.string
}

TableSection.defaultProps = {
    tableClass: "table"
}

export default TableSection;