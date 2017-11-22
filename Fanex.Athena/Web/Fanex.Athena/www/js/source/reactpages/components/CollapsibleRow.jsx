import React from 'react';
import ReactDOM from 'react-dom';
import PropTypes from 'prop-types';

export default class CollapsibleRow extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isCollapse : !this.props.isExpand
        };
    };

    static propTypes = {
        isExpand: PropTypes.bool,
        isHtml: PropTypes.bool,
        subject: PropTypes.string,
        content: PropTypes.string,
        onExpand: PropTypes.func
    }

    componentWillReceiveProps(nextProps) {
        if( this.props.isExpand !== nextProps.isExpand ) {
            this.setState({ isCollapse : !nextProps.isExpand });
        }
    }

    onClick = () => {
        const newState = !this.state.isCollapse;

        this.setState({ isCollapse : newState });

        if (!newState) {
            this.props.onExpand && this.props.onExpand();
        }
    }

    render() {
        return <div className={this.state.isCollapse ? "collapse-item collapse" : "collapse-item"}>
                    <div className="subject-item"><span>{this.props.subject}</span><i className="icon-arrow" onClick={this.onClick}></i></div>
                    {
                        this.props.isHtml ? 
                            <div className="content-item" dangerouslySetInnerHTML={{ __html: this.props.content }} /> :
                            <div className="content-item">{this.props.content}</div>                    
                    }
               </div>
    }
};