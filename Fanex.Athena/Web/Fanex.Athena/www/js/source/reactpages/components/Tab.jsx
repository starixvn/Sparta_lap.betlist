import React from 'react';
import ReactDOM from 'react-dom';

export default class Tab extends React.Component {
    constructor(props) {
        super(props);
    };

    _renderTab() {
        const tabs = this.props.tabList;

        return tabs.map(function(tab, i){
            return <li key={`tab${i}`} className="tab">
                    <a className={tab.isActive ? "active" : ""} href={tab.url}>{tab.name}</a>
                </li>            
            });
    }

    render() {
        return <div className="header-tab">
            <div className="content-tab">
                <ul className="tabs">
                    {this._renderTab()}
                </ul>
            </div>
        </div>
        }
};