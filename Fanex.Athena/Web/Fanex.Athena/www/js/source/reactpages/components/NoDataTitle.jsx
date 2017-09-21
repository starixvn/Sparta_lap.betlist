import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import langHelper from '../lib/langHelper';

export default class NoDataTitle extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <p className="no-info-row center-align">{langHelper.getSingleResource("NoInfo")}</p>
        )
    }
}