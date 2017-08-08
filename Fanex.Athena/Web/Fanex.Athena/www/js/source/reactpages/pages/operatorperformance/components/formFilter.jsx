import React, { Component } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import Select from 'react-select';
import { Calendar,DateRange  } from 'react-date-range';
class FormFilter extends Component {
    constructor(props) {
        super(props)
        this.state = {
            value: []
        }
    }

    render() {

        var options = [
            { value: 'one', label: 'One' },
            { value: 'two', label: 'Two' }
        ];

        function logChange(val) {
            console.log("Selected: " + JSON.stringify(val));
        }

        function handleSelect(date){
                console.log(date); // Momentjs object
            }
        return (
            <div>
                Hello Form
             <Select key="1"
                    name="form-field-name"
                    value="one"
                    options={options}
                    onChange={logChange}
                />
                <Select key="2" multi simpleValue value={this.state.value} placeholder="Select your favourite(s)"
                    options={options} onChange={(value) => {
                     
                        this.setState({value:value})
                }} />

                <div>
				<Calendar
					onInit={handleSelect}
					onChange={handleSelect}
				/>
            <DateRange
					onInit={handleSelect}
					onChange={handleSelect}
				/>
          />
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