import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import Select from 'react-select'
import { DateRange } from '../../../components/date-range/index'
import LangHelper from '../../../lib/langHelper'
import Site from '../../../lib/site'

const RANGE_MONTH_CONSTRAINT = 3;

export class FormFilter extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="page-filter">         
            </div>
        );
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
};

function mapStateToProps(state) {
    const { gridData, ...rest } = state;
    return Object.assign({}, rest);
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);