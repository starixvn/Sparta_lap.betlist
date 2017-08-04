import React, { Component, PropTypes } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'
import { ActionCreators } from './actions'

class AppContainer extends Component {

  constructor(props: any, context: any) {
    super(props, context);
    
  }

  render() {
   return (
     <div>
         Hello
     </div>
    );
  }

function mapDispatchToProps(dispatch) {
  return bindActionCreators(ActionCreators, dispatch);
}

function mapStateToProps(state) {
  return {
   
  };
}


export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);