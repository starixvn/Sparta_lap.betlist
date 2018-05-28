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
        subject: PropTypes.shape({
            isHtml: PropTypes.bool,
            text: PropTypes.string
        }),
        content: PropTypes.shape({
            isHtml: PropTypes.bool,
            text: PropTypes.string
        }),    
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
        const { subject, content } = this.props;

        return <div className={this.state.isCollapse ? "collapse-item collapse" : "collapse-item"}>
                    <div className="subject-item">
                        { subject.isHtml ? <span dangerouslySetInnerHTML={{ __html: subject.text }} /> : <span>{subject.text}</span> }
                        <i className="icon-arrow" onClick={this.onClick}></i>
                    </div>
                    {
                        content.isHtml ? 
                            <div className="content-item" dangerouslySetInnerHTML={{ __html: content.text }} /> :
                            <div className="content-item">{content.text}</div>                    
                    }
               </div>
    }
};