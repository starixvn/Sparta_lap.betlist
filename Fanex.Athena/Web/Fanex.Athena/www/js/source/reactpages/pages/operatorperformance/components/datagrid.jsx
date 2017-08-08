import React, { Component } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js'


class DataGrid extends Component {
    constructor(props) {
        super(props)
       
    }
  
    render() {
        return (
            <div>Hello GRid</div>
        );
        }
}
function mapDispatchToProps(dispatch) {
  return bindActionCreators(ActionCreators, dispatch);
}
    function mapStateToProps(state) {
        return {
            
        };
    };

export default connect(mapStateToProps,mapDispatchToProps)(DataGrid);