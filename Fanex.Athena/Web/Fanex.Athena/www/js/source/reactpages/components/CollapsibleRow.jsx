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
        const { subject, content, isHtml } = this.props;

        return <div className={this.state.isCollapse ? "collapse-item collapse" : "collapse-item"}>
                    <div title={subject} className="subject-item"><span>{subject}</span><i className="icon-arrow" onClick={this.onClick}></i></div>
                    {
                        isHtml ? 
                            <div className="content-item" dangerouslySetInnerHTML={{ __html: content }} /> :
                            <div className="content-item">{content}</div>                    
                    }
               </div>
    }
};