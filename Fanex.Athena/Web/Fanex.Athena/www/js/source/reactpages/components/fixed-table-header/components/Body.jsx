import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import PropTypes from 'prop-types';

class Body extends Component {
    constructor(props) {
        super(props);
        this.containerStyle = {
            width: props.autoWidth ? "100%" : props.width + 19,
            maxHeight: props.maxHeight
        }

        this.tableStyle = {
            width: props.autoWidth ? "100%" : props.width,
            position: "fixed"
        }
    }

    componentDidMount() {
    }

    render() {
        const { tableClass, width, maxHeight, autoWidth, ...rest } = this.props;
        return (
            <div className="body-content" style={this.containerStyle} {...rest}>
                <div className="table-wrapper">
                    <table style={this.tableStyle} className={tableClass}>
                        <tbody>
                            {this.props.children}
                        </tbody>
                    </table>
                </div>
            </div>
        );
    }
}

Body.propTypes = {
    tableClass: PropTypes.string
}

Body.defaultProps = {
    tableClass: "table"
}

export default Body;