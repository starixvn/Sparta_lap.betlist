import React, { Component } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'

class FormFilter extends Component {
    constructor(props) {
        super(props)
       
    }
  
    render() {
        return (
<div>Hello Form</div>
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

export default connect(mapStateToProps,mapDispatchToProps)(FormFilter);