import React, { Component } from 'react';
import { connect } from 'react-redux';



class FormFilter extends Component {
    constructor(props) {
        super(props)
        this.state = { }
    }
  
    render() {
        return (
<div>Hello Form</div>
        );
        }

    function mapStateToProps(state) {
        return {
             currencyList: state.currencyList
        };
    };

export default connect(mapStateToProps)(FormFilter);