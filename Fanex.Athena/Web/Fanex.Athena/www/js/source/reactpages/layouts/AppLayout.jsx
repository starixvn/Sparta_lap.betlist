import React, { Component, PropTypes } from 'react';

export default class AppLayout extends Component {

  constructor(props) {
    super(props);
    
  }

  render() {
   return (
        <div id="container" className="container">
            {this.props.children}
        </div>
    );
  }
}

