import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import moment from 'moment'
import { ToastContainer, toast, Flip } from 'react-toastify';

import Select from 'react-select'
import MultipleSelect from '@9softstudio/react-multiselect'
import { DateRange } from '@jsroot/components/date-range/index'
import AutoComplete from '@jsroot/components/auto-complete'
import CheckBox from '@jsroot/components/CheckBox'

import { ActionCreators } from '../actions/index.js'
import LangHelper from '@jsroot/lib/langHelper'
import Site from '@jsroot/lib/site'
import AppData from '@jsroot/lib/AppData'
import DataService from '@jsroot/lib/DataService.js'
import Service from '../../common/lib/Service'
import ApiUrl from '../../common/constants/apiUrl'

const ToastMsg = () => (
  <div>
    <div>{LangHelper.getSingleResource("BTCExportExcelMsg1")}</div>
    <div>{LangHelper.getSingleResource("BTCExportExcelMsg2")}</div>
  </div>
)

export class FormFilter extends Component {
    constructor(props) {
        super(props);
        this.baseCurrencyList = AppData.baseCurrencyList;
        this.defaultBaseCurrency = props.selectedBaseCurrency;
    }

    componentDidMount() {
        this._onChangeProduct(this.props.selectedProduct);
    }

    _showExportWarning = () => {
        toast.success(<ToastMsg />);
    }

    _isEnableLeagueMatch = () => {
        const productCode = this.props.selectedProduct;
        return (productCode === 'SB' || productCode === 'BA') && this.isSingleDate();
    }

    get isValidModel() {
        return !!this.props.selectedProduct;
    }

    isSingleDate = (dateRange) => {
        const { fromDate, toDate } = dateRange || this.props.dateRange;
        return moment(fromDate, "MM/DD/YYYY", true).isSame(moment(toDate, "MM/DD/YYYY", true), 'day');
    }

    submit = (event) => {
        event.preventDefault();
        const params = this._getParams();
        if (!params) {
            return;
        }

        toast.dismiss();
        const title = Service.getTitle(params.FromDate, params.ToDate, params.CurrencyName, params.BaseCurrency);
        this.props.setReportTitle(title);
        this.props.setRequestParam(params);
        this.props.toggleLoader(true);
        this.props.getGridData(ApiUrl.MULTICUSTOMER_DATA_URL, params);
    }

    _getParams() {
        if (!this.isValidModel) {
            return null;
        }

        const { fromDate, toDate } = this.props.dateRange;
        const { PageSize, SortBy, SortOrder } = this.props.requestParam;

        return {
            PageSize: PageSize,
            PageIndex: 1,
            FromDate: Site.convertDateToString(fromDate),
            ToDate: Site.convertDateToString(toDate),
            ProductCodeList: this.props.selectedProduct,
            CurrencyId: this.props.selectedCurrency.value,
            CurrencyName: this.props.selectedCurrency.label,
            BaseCurrency: this.props.selectedBaseCurrency,
            StatusList: this.props.selectedStatus,
            LiveStatus: this.props.selectedLiveStatus,
            SportId: this.props.selectedSport,
            BetTypeId: this.props.selectedBettype,
            LeagueId: this.props.selectedLeague,
            MatchId: this.props.selectedMatch,
            SortBy: SortBy,
            SortOrder: SortOrder
        }
    }

    exportExcel = (event) => {
        event.preventDefault();
        
        const params = this._getParams();
        if (!params) {
            return;
        }

        params.IsExport = true;
        this.props.toggleLoader(true);
        DataService.getFile(ApiUrl.MULTICUSTOMER_DATA_URL, params).then((response) => {
            toast.dismiss();
            const contentType = response.headers["content-type"];
            if(contentType && contentType.indexOf('application/json') > -1)
            {
                const RowsExceedLimitErrorCode = 1001;
                Site.readAsJson(response.data, (result) => {
                    if(result.ErrorCode === RowsExceedLimitErrorCode){
                        this._showExportWarning();
                    }
                    else
                    {
                        const msg = result.ErrorMsg || LangHelper.getSingleResource("ErrorProcessingMsg");
                        alert(msg);
                    }
                });
            } else {
                const NoContentStatusCode = 204;
                if(response.status === NoContentStatusCode){
                    alert(LangHelper.getSingleResource("NoInfo"));
                }
                else {
                    const fileName = "BettingTicketsCenter_MultiCustomer." +
                        (response.headers["content-type"] === "application/zip" ? "zip" : "xls");
                
                    Site.downloadFile(response.data, fileName);
                }
            }
            
            this.props.toggleLoader(false);
        }).catch(error => {
            alert(LangHelper.getSingleResource("ErrorProcessingMsg"));
            this.props.toggleLoader(false);
        });
    }

    _renderDateRange() {
        const dateRange = this.props.dateRange;
        const dateConstraint = AppData.dateConstraint;

        return (
            <DateRange startDate={dateRange.fromDate} endDate={dateRange.toDate}
                minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
                today={dateRange.toDate} limitedMonthRange={dateConstraint.limitedMonthRange}
                onChange={this._onChangeDateRange} />
        )
    }

    _onChangeDateRange = (date, isValidMonthRange = true) => {
        if (isValidMonthRange) {
            this.props.setDateRange(date);
            this.props.getBettypeList(this.props.selectedProduct, this.isSingleDate({fromDate: date.startDate, toDate: date.endDate}));
            if(this._isEnableLeagueMatch()){
                this.props.getLeagueList(this.props.selectedSport.value, date.startDate);
            }
        }
        else {
            const message = LangHelper.getSingleResource("DateRangeExceedMsg").replace("{0}", AppData.dateConstraint.limitedMonthRange);
            alert(message);
        }
    }

    _renderProductSelectList() {
        const productList = AppData.productList;

        return productList && productList.length && (
           <Select instanceId="ProductList"
                value={this.props.selectedProduct}
                options={productList}
                onChange={(item) => this._onChangeProduct(item.value)} />
        );
    }
    
    _onChangeProduct = (productCode) => {
        this.props.setSelectedProduct(productCode);
        this.props.getSportList(productCode, this.isSingleDate() && this.props.dateRange.fromDate);
        this.props.getBettypeList(productCode, this.isSingleDate());
    }

   _renderSportSelectList() {
       const sportList = this.props.sportList;
       
        return sportList && sportList.length && (
            <Select instanceId="SportList"
            value={this.props.selectedSport}
            options={sportList}
            onChange={(item) => this._onChangeSport(item.value)} />
        );
    }

    _onChangeSport = (sportId) => {
        this.props.setSelectedSport(sportId);
        if(this._isEnableLeagueMatch()){
            this.props.getLeagueList(sportId, this.props.dateRange.fromDate);
        }
    }

    _renderBettypeSelectList() {
        const bettypeList = this.props.bettypeList;
       
        return bettypeList && bettypeList.length && (
            <Select instanceId="BettypeList"
                value={this.props.selectedBettype}
                options={bettypeList}
                onChange={(item) => this.props.setSelectedBettype(item.value)} />
        );
    }

      _renderLeagueSelectList() {
         const leagueList = this.props.leagueList;
       
        return leagueList && leagueList.length && (
            <Select disabled={!this._isEnableLeagueMatch()} instanceId="LeagueList"
            value={this.props.selectedLeague}
            options={leagueList}
            onChange={(item) => this._onChangeLeague(item.value)} />
         );
      }

    _onChangeLeague = (leagueId) => {
        this.props.setSelectedLeague(leagueId);
        if(this.isSingleDate()){
            this.props.getMatchList(this.props.selectedSport.value, leagueId, this.props.dateRange.fromDate);
        }
    }

    _renderMatchSelectList() {
        const matchList = this.props.matchList;
       
        return matchList && matchList.length && (
                <Select disabled={!this._isEnableLeagueMatch()} instanceId="MatchList"
            value={this.props.selectedMatch}
            options={matchList}
            onChange={(item) => this.props.setSelectedMatch(item.value)} />
        );
     }
    
    _renderCurrencySelectList() {
        return <Select instanceId="ddlCurrency"
        value={this.props.selectedCurrency.value}
        options={AppData.currencyList}
        onChange={this._onChangeCurrency} />
    }

    _onChangeCurrency = (currency) => {
        const currencyName = currency.label;
        let baseCurrency = currencyName;

        if (currency.value == 0) {
            this.baseCurrencyList = AppData.baseCurrencyList;
            baseCurrency = this.baseCurrencyList.find(x => x.label === this.props.selectedBaseCurrency) ?
                            this.props.selectedBaseCurrency :
                            this.defaultBaseCurrency;
        }
        else if(!this.baseCurrencyList.find(x => x.label === currencyName)){
            this.baseCurrencyList = AppData.baseCurrencyList.slice(0);
            this.baseCurrencyList.push({value: currencyName, label: currencyName});      
        }

        this.props.setSelectedCurrency(currency)
        this.props.setSelectedBaseCurrency(baseCurrency);
    }

    _renderBaseCurrencySelectList() {
        const baseCurrencyList = this.baseCurrencyList;

        return baseCurrencyList && baseCurrencyList.length && (
            <Select instanceId="ddlBased"
                value={this.props.selectedBaseCurrency}
                options={baseCurrencyList}
                onChange={(item) => this.props.setSelectedBaseCurrency(item.value)} />
        );
    }

    _renderStatusSelectList() {
        const statusList = AppData.statusList;

        return statusList && statusList.length && (
                <MultipleSelect id="StatusList"
                    dataSource={statusList}
                    keyField={"Id"}
                    valueField={"Value"}
                    statusField={"Status"}
                    optionAllLabel={LangHelper.getSingleResource("All")}
                    noneSelectedLabel={LangHelper.getSingleResource("SelectOptions")}
                    onChange={(currentItem, selectedItemsKey) => this.props.setSelectedStatus(selectedItemsKey)} />);
    }

      _renderLiveStatusSelectList() {
          const liveStatusList = AppData.liveStatusList;

          return liveStatusList && liveStatusList.length && (
            <Select instanceId="LiveStatusList"
                value={this.props.selectedLiveStatus}
                options={liveStatusList}
                onChange={(item) => this.props.setSelectedLiveStatus(item.value)} />
        );
    }

    render() {
        return (
            <div className="page-filter">
                <div className="row">
                    <ToastContainer 
                            autoClose={10000} 
                            pauseOnFocusLoss={false} 
                            style={{width:"500px"}}
                            transition={Flip}/>
                     <div className="col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Date")}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Product")}</label>
                            {this._renderProductSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Sport")}</label>
                        {this._renderSportSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("BetType")}</label>
                        {this._renderBettypeSelectList()}
                        </div>
                    </div>

                     <div className="col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("League")}</label>
                        {this._renderLeagueSelectList()}
                        </div>
                    </div>

                     <div className="col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Match")}</label>
                            {this._renderMatchSelectList()}
                        </div>
                    </div>
                    
                    </div>
                    <div className="row">
                     <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Currency")}</label>
                        {this._renderCurrencySelectList()}
                        </div>
                    </div>
                    
                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("BaseCurrency")}</label>
                        {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>

                     <div className="col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Status")}</label>
                                {this._renderStatusSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Live")}</label>
                        {this._renderLiveStatusSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-1">
                        <div className="form-group">
                            <div className="button-group">
                            <button type="button" disabled={!this.isValidModel} className="btn btn-submit" onClick={this.submit}>{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>
                </div>
                 <div className="row">
                    <div className="caret-block">
                        <i className="icon-excel right" onClick={this.exportExcel} title={LangHelper.getSingleResource("ExportToExcel")}></i>
                    </div>
               </div>
               <div className="row">
                   <div className="col-1" style={{flex: "0 0 4%"}}>
                       {LangHelper.getSingleResource('Note')}:
                    </div>
                    <div className="col-9">
                        * {LangHelper.getSingleResource('BettingTicketCenter_MultiCustomersTabNote1')}
                        <br />
                        * {LangHelper.getSingleResource('BettingTicketCenter_MultiCustomersTabNote2').replace("{0}", AppData.dateConstraint.minDate)}
                    </div>
               </div>
            </div>
        )
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { requestParam, dateRange, selectedProduct, sportList, selectedSport, selectedBettype, bettypeList, selectedLeague, leagueList, selectedMatch, matchList,
        selectedCurrency, baseCurrencyList, selectedBaseCurrency, selectedStatus, selectedLiveStatus } = state;
        return Object.assign({}, { requestParam, dateRange, selectedProduct, sportList, selectedSport, selectedBettype, bettypeList, selectedLeague, leagueList, selectedMatch, matchList,
            selectedCurrency, baseCurrencyList, selectedBaseCurrency, selectedStatus, selectedLiveStatus });
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);