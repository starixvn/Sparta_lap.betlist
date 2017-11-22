import React, { Component } from 'react';
import PropTypes from 'prop-types';

export default class Page extends Component {
    constructor(props) {
        super(props);

        this.state = {
            index: this.props.pageOption.PageIndex,
            isShow: this.props.pageOption.TotalItem > this.props.pageOption.PageList[0],
            totalPage: Math.ceil(this.props.pageOption.TotalItem/this.props.pageOption.PageSize),
            pageSize: this.props.pageOption.PageSize
        };

        this.inputValue = this.props.pageOption.PageIndex;
    }

    static propTypes = {
        pageOption: PropTypes.object,
        onPaging: PropTypes.func
    };

    static defaultProps = {
        pageOption: {
            PageIndex: 1,
            PageSize: 50,
            PageList: [50, 100, 200],
            TotalItem: 0
        }
    };

    componentWillReceiveProps(nextProps) {
        if (this.props.pageOption !== nextProps.pageOption) {
            this._updateState(
                nextProps.pageOption.PageIndex,
                nextProps.pageOption.TotalItem > nextProps.pageOption.PageList[0],
                Math.ceil(nextProps.pageOption.TotalItem/nextProps.pageOption.PageSize),
                nextProps.pageOption.PageSize
            );
        }
    }

    goToPage(index, pageSize) {
        this._updateState(index, null, this.props.pageOption.TotalItem/pageSize, pageSize);
        this.props.onPaging && this.props.onPaging(index, pageSize);
    }

    onKeyUpHandle = (e) => {
        const code =  e.keyCode ? e.keyCode : e.which;
        if (code === 13) {
            var value = parseInt(this.state.index);
            if (value > 0 && value <= this.state.totalPage) {
                this.goToPage(value, this.state.pageSize);
            }
        }
    }

    onClickHandle(isDisable, handleFunc) {
        if(!isDisable) {
            handleFunc();
        }
    }


    _updateState(index, isShow, totalPage, pageSize) {
        this.setState({
            index: index !== null ? index : this.state.index,
            isShow: isShow !== null ? isShow : this.state.isShow,
            totalPage: totalPage ? totalPage : this.state.totalPage,
            pageSize: pageSize ? pageSize : this.state.pageSize
        });
    }

    _renderPageSizeSelection(isDisable, pageIndex, pageSize) {
        return (
            <select className="pagesize-select" disabled={isDisable} value={pageSize} onChange={(e) => this.goToPage(1, e.target.value)}>
                {this._renderPageSizeOption(this.props.pageOption.PageList)}
	        </select>
        );
    }

    _renderPageSizeOption(pageList) {
        return pageList.map((item, index) => {
            return (<option key={`pagesize${index}`} value={item}>{item}</option>);
        });
    }

    _renderPageIcon(iconClass, isDisable, onClickFunc) {
        return (<a href="javascript:;" className={`icon ${iconClass} ${isDisable ? "disable" : ""}`} onClick={() => this.onClickHandle(isDisable, onClickFunc)}></a>);
    }

    _renderPageInput(index) {
        return (
            <div className="page-info"><span>Page </span><input type="text" className="page-form" value={index} maxLength="4" size="2"
                        onChange={(e) => this._updateState(e.target.value, null, null, null) }
                        onKeyUp={this.onKeyUpHandle} />
            <span>{` of ${this.state.totalPage}`} </span>
            </div>);
    }

    render() {
        const { index, totalPage, pageSize } = this.state;
        const isDisable = isNaN(parseInt(index));

        return (
            <div className="page-container center-align" style={{ display: `${this.state.isShow ? 'block' : 'none'}` }}>
                {this._renderPageIcon("page-first", index === 1, () => this.goToPage(1, pageSize))}
                {this._renderPageIcon("page-prev", isDisable || index <= 1 || index > totalPage, () => this.goToPage(index - 1, pageSize))}
                {this._renderPageInput(index)}
                {this._renderPageIcon("page-next", isDisable || index >= totalPage || index < 1, () => this.goToPage(index + 1, pageSize))}
                {this._renderPageIcon("page-last", index === totalPage, () => this.goToPage(totalPage, pageSize))}
                {this._renderPageSizeSelection(isDisable, index, pageSize)}
            </div>);
    }
}