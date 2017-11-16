import React from 'react';
import ReactDOM from 'react-dom';

export default class CollapseRow extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isCollapse : !this.props.isExpand
        };
    };

    onClick = () => {
        const newState = !this.state.isCollapse;

        this.setState({ isCollapse : newState });

        if (!newState) {
            this.props.onExpand && this.props.onExpand();
        }
    }

    render() {
        return <div className={this.state.isCollapse ? "collapse-item collapse" : "collapse-item"}>
                    <div className="subject-item">{this.props.subject}<i className="icon-arrow" onClick={this.onClick}></i></div>
                    <div className="content-item">{this.props.content}</div>
               </div>
    }
};