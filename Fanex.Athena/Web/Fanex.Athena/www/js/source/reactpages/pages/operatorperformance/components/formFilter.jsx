import React, { Component } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import Select from 'react-select';
import { Calendar, DateRange } from 'react-date-range';

const defaultRanges = {
    'Today': {
        startDate: (now) => {
            return now;
        },
        endDate: (now) => {
            return now;
        }
    },

    'Yesterday': {
        startDate: (now) => {
            return now.add(-1, 'days');
        },
        endDate: (now) => {
            return now.add(-1, 'days');
        }
    },

    'Last 7 Days': {
        startDate: (now) => {
            return now.add(-7, 'days');
        },
        endDate: (now) => {
            return now;
        }
    },

    'Last 30 Days': {
        startDate: (now) => {
            return now.add(-30, 'days');
        },
        endDate: (now) => {
            return now;
        }
    }
}
class FormFilter extends Component {
    constructor(props) {
        super(props)
        this.state = {
            value: []
        }
    }

    _submit() {
        var inputValue = { SelectedCurrency: this.props.SelectedCurrency }
        this.props.fetchData();
    }
    render() {
        var options = [
            { value: 'one', label: 'One' },
            { value: 'two', label: 'Two' }
        ];

        function logChange(val) {
            console.log("Selected: " + JSON.stringify(val));
        }

        function handleSelect(date) {
            console.log(date); // Momentjs object
        }
        return (
            <div className="row">
                <div className="col-lg-2 col-md-2 col-sm-2 col-xs-12">
                    <div className="form-input">
                        <label className="label-text">Operator</label>
                        <Select id="ddlOperator" key="1"
                            name="form-field-name"
                            value="one"
                            options={options}
                            onChange={logChange} />
                    </div>
                </div>

                <div className="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                    <div className="form-input">
                        <label className="label-text">Date</label>
                        <DateRange
                            linkedCalendars={true}
                            ranges={defaultRanges}
                            onInit={handleSelect}
                            onChange={handleSelect}
                            theme={{
                                DateRange: { position: "absolute" },
                                PredefinedRanges: { marginLeft: 10, marginTop: 10 }
                            }}
                        />
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