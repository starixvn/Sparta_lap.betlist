import React, { Component } from 'react';
import PropTypes from 'prop-types';

class TableSection extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        const { tableClass, ...rest } = this.props;
        return (
            <div {...rest}>
                <table className={tableClass}>
                    {this.props.children}
                </table>
            </div>
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