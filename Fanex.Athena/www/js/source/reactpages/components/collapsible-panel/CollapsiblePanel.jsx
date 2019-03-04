import React from 'react';
import PropTypes from 'prop-types';

export default class CollapsiblePanel extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isCollapsed : false
        };
    };

    static propTypes = {
        isShowHandler: PropTypes.bool
    }

    onClick = () => {
        const newState = !this.state.isCollapsed;

        this.setState({ isCollapsed : newState }, () => {
            window.dispatchEvent(new Event('resize'));
        });
    }

    render() {
        const { isShowHandler } = this.props;
        let handler = null;
        if (isShowHandler) {
            const arrowStyle = { 
                transform: this.state.isCollapsed ? null : 'rotate(180deg)',
                color:'#fff', 
                textAlign: 'center', 
                marginTop: '10px', 
                background: '#4c6274', 
                height: "20px",
                width: "25px", 
                float: 'left'};

            handler = (<div className="filter-handle" style={{height: "20px",width: "25px", margin: 'auto', cursor: 'pointer'}}>
                    <i className="icon-arrow" onClick={this.onClick} style={arrowStyle}></i>
                    </div>);
        }

         return (<div className={`${ this.props.className }`}>
            { !this.state.isCollapsed && this.props.children }
                    {handler}
           </div>)
    }
}