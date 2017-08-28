import React, { Component } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import Select from 'react-select';
import { Calendar, DateRange, defaultOptions } from '../../../components/date-range/index';


class FormFilter extends Component {
    constructor(props) {
        super(props)
        this.state = {
            value: []
        }
    }

    handleSelect(date) {
        console.log(`Start: ${date.startDate.format("MM/DD/YYYY")}`);
        console.log(`End: ${date.endDate.format("MM/DD/YYYY")}`); // Momentjs object
    }

    _submit() {
        var inputValue = { SelectedCurrency: this.props.SelectedCurrency }
        this.props.fetchData();
    }
    render() {
        var options = [
            { value: '1', label: 'New York', className: "input-select-option" },
            { value: '2', label: 'India', className: "input-select-option" }
        ];

        function logChange(val) {
            console.log("Selected: " + JSON.stringify(val));
        }

        return (
            <div className="page-filter">
                <div className="row">
                    <div className="col-lg-2 col-md-2 col-sm-2 col-xs-12">
                        <div className="form-group">
                            <label className="label-text">Operator</label>
                            <Select id="ddlOperator" className="input-select"
                                name="form-field-name"
                                value="2"
                                options={options}
                                onChange={logChange} />
                        </div>
                    </div>

                    <div className="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                        <div className="form-group">
                            <label className="label-text">Date</label>

                            <DateRange minDate='01-02-2017' onlyClasses={true}
                                ranges={defaultOptions.ranges}
                                onClickApplyButton={(date) => this.handleSelect(date)} />

                        </div>
                    </div>

                    <div className="col-lg-2 col-md-2 col-sm-2 col-xs-12">
                        <div className="form-group">
                            <label className="label-text">Product</label>
                            <Select id="ddlProduct"
                                name="form-field-name"
                                value="0"
                                options={[{ value: '0', label: 'Sportsbook' }, { value: '1', label: 'BA' }]}
                                onChange={logChange} />
                        </div>
                    </div>

                    <div className="col-lg-2 col-md-2 col-sm-2 col-xs-12">
                        <div className="form-group">
                            <label className="label-text">Based</label>
                            <Select id="ddlBased"
                                name="form-field-name"
                                value="6"
                                options={[{ value: '0', label: 'All' }, { value: '6', label: 'EUR' }]}
                                onChange={logChange} />
                        </div>
                    </div>
                </div>

                <div className="row">
                    <div className="col-lg-2 col-md-2 col-sm-2 col-xs-12">
                        <div className="form-group">
                            <div className="checkbox-group">
                                <input type="checkbox" id="IsConvertAll" />
                                <label htmlFor="IsConvertAll">Convert All</label>
                            </div>

                            <div className="button-group">
                                <button type="button" className="btn btn-submit">Submit</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    return {
        currencyList: state.currencyList
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);