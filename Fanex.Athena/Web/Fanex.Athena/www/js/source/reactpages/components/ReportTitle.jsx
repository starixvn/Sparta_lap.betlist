import React, { Component } from 'react';
import ReactDOM from 'react-dom';

export default class ReportTitle extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="report-title-container">
                <p className="report-title">{this.props.content}</p>
            </div>
        )
    }
}