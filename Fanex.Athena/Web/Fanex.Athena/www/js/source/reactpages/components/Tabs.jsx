import React, { Component } from 'react'

import Tab from './Tab'

export default class Tabs extends Component {
    constructor(props) {
        super(props);

        const tabComponents = this._getTabComponents();
        const tabState = tabComponents.map((item) => {
            return item.props.isActive;
        });

        this.state = {
            tabState: tabState
        }
    }

    _renderActiveTabContent() {
        const tabComponents = this._getTabComponents();
        const activeTabIndex = this.state.tabState.findIndex(x => x);

        return tabComponents[activeTabIndex].props.children;
    }

    onTabChange(tabIndex) {
        this.setState({
            tabState : this.state.tabState.map((item, index) => {
                if(index === tabIndex) {
                    return true;
                }
                else {
                    return false;
                }
            })
        });
    }

    _renderTabComponents() {
        const tabComponents = this._getTabComponents();
        const that = this;

        return tabComponents.map((item, index) => {
            return (
                <Tab key={`tab${index}`} isActive={that.state.tabState[index]} label={item.props.label} onClick={() => that.onTabChange(index)} />
            );
        });
        }

   _getTabComponents() {
        return React.Children.toArray(this.props.children);
        }

    render() {
        return (
            <div className="page-filter-tab">
                <div className="header-tab">
                    <div className="content-tab">
                        <ul className="tabs">{this._renderTabComponents()}</ul>
                    </div>
                </div>
                <div className="tab-panel">{this._renderActiveTabContent()}</div>
            </div>
        );
    }
}