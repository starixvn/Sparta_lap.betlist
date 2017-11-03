import React, { Component } from 'react'
import ReactDOM from 'react-dom'
import PropTypes from 'prop-types'

import axios from 'axios'

import SuggestionList from './SuggestionList'

export default class AutoComplete extends Component {
    constructor(props) {
        super(props);

        this.state = {
            suggestions: null,
            selectedIndex: -1
        }

        this.input = null;
        this.wrapper = null;
        this.suggestionsWidth = 0;
        this.cacheData = {};
    }

    static propTypes = {
        url: PropTypes.string,
        limitToList: PropTypes.bool,
        maxItemsShown: PropTypes.number,
        onSelect: PropTypes.func,
        onInput: PropTypes.func,
        responseDataField: PropTypes.string,
        cache: PropTypes.bool
    }

    static defaultProps = {
        url: null,
        limitToList: true,
        maxItemsShown: 10,
        responseDataField: "suggestions",
        cache: true
    }

    componentDidMount() {
        this.suggestionsWidth = this.input.clientWidth;

        document.addEventListener('click', this._handleDocumentClick)
    }

    componentWillUnmount() {
        document.removeEventListener('click', this._handleDocumentClick)
    }

    get showItems() {
        return this.state.suggestions && this.state.suggestions.length > 0;
    }

    _handleDocumentClick = (event) => {
        const clickOutside = this.wrapper && !this.wrapper.contains(event.target);

        if (clickOutside && this.showItems) {
            this._clearState();
        }
    }

    _onKeyDown = (event) => {
        if (!this.showItems) {
            return;
        }

        const selectedIndex = this.state.selectedIndex;
        switch (event.keyCode) {
            case 38: //up
                if (selectedIndex > 0) {
                    this.setState({ selectedIndex: selectedIndex - 1 });
                }
                break;
            case 40: //down
                if (selectedIndex < this.state.suggestions.length - 1) {
                    this.setState({ selectedIndex: selectedIndex + 1 });
                }
                break;
            case 13: //enter
                if (selectedIndex > -1) {
                    const selectedItem = this.state.suggestions[selectedIndex];
                    this._onSelect(selectedItem);
                }
                break;
        }
    }

    _setStateWithCaching(value, suggestionList) {
        if (this.props.cache) {
            this.cacheData[value] = suggestionList;
        }

        this.setState({
            suggestions: suggestionList
        });
    }

    _clearState() {
        this.setState({ suggestions: null, selectedIndex: -1 });
    }

    _onSelect = (selectedItem) => {
        const { onSelect } = this.props;
        onSelect && onSelect(selectedItem);
        this.input.value = typeof (selectedItem) === "object" ? selectedItem.value : selectedItem;

        this._clearState();
    }

    _getDataByUrl(url, value) {
        const { responseDataField } = this.props;
        const apiUrl = `${url}?query=${value}`;

        axios.get(apiUrl, {
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        }).then((response) => {
            var data = responseDataField ? response.data[responseDataField] : response.data;
            this._setStateWithCaching(value, data);
        });
    }

    _onChange = (event) => {
        const value = event.target.value;
        if (value === "") {
            this._clearState();
            return;
        }

        const cachedData = this.cacheData[value];
        if (cachedData) {
            this.setState({ suggestions: cachedData });
            return;
        }

        const { url, onInput } = this.props;
        if (url) {
            this._getDataByUrl(url, value);
        }
        else if (onInput) {
            onInput(value, this._setStateWithCaching.bind(this));
        }
    }

    render() {
        return (
            <div className="ac-container" ref={(element) => this.wrapper = element}>
                <input type="text" className="input-text" placeholder={this.props.placeholder}
                    onChange={this._onChange} onKeyDown={this._onKeyDown}
                    ref={(element) => this.input = element} />

                {this.showItems ?
                    <SuggestionList selectedIndex={this.state.selectedIndex}
                        width={this.suggestionsWidth}
                        dataList={this.state.suggestions}
                        onSelect={this._onSelect} />
                    : null}
            </div>
        );
    }
}