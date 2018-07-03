import React from 'react';
import axios from 'axios'

import BaseDataGrid from '../BaseDataGrid';
import renderer from 'react-test-renderer';
import LangHelper from '@jsroot/lib/langHelper.js';
import BetList from '../../lib/BetList';

describe('BaseDataGrid methods ', () => {
    
    it('componentDidMount - Assign functions to window', () => { 
        const dataGrid = new BaseDataGrid();
            
        dataGrid.componentDidMount();

        expect(typeof(window.ViewResult)).toBe("function");
        expect(typeof(window.showNewCombMP)).toBe("function");
        expect(typeof(window.showCombMPDetail)).toBe("function");
    });

    it('componentWillUnmount - Remove assigned window functions', () => { 
        const dataGrid = new BaseDataGrid();
            
        dataGrid.componentDidMount(); // assign 
        dataGrid.componentWillUnmount();

        expect(window.ViewResult).toBeUndefined();
        expect(window.showNewCombMP).toBeUndefined();
        expect(window.showCombMPDetail).toBeUndefined();
    });

    it('_closeResultPopup  - set state isShowPopup = false', () => { 
        const dataGrid = new BaseDataGrid();
        dataGrid.setState = jest.fn();

        dataGrid._closeResultPopup();

        expect(dataGrid.setState).toHaveBeenCalledWith({ isShowPopup: false});
    });

    it('window.ViewResult() - Call Betlist and setState correctly',() =>{
        const expectedResultUrl = "www.facebook.com";
        const dataGrid = new BaseDataGrid();
        dataGrid.setState = jest.fn();
        BetList.getResultUrl = jest.fn(() => expectedResultUrl);
        dataGrid.componentDidMount();

        window.ViewResult(1,2,3,4);

        expect(BetList.getResultUrl).toHaveBeenCalledWith(1,2,3,4);
        expect(dataGrid.setState).toHaveBeenCalledWith({ isShowPopup: true, popupUrl: expectedResultUrl, popupTitle: "Result"});
    });

    it('window.showNewCombMP() - Call _showNewParlayDetail() correctly',() =>{
        const dataGrid = new BaseDataGrid();
        dataGrid._showNewParlayDetail = jest.fn();
        
        dataGrid.componentDidMount();

        window.showNewCombMP(1,2,3,4);

        expect(dataGrid._showNewParlayDetail).toHaveBeenCalledWith(1,2,3,4);
    });

    it('window.showCombMPDetail() - Call Betlist and setState correctly',() =>{
        const expectedResultUrl = "www.facebook.com";
        const expectedPopupTitle = "System ABC";
        const dataGrid = new BaseDataGrid();
        BetList.getParlayDetailUrl = jest.fn(() => expectedResultUrl);
        dataGrid.setState = jest.fn();

        dataGrid.componentDidMount();

        window.showCombMPDetail(1,2,3,4,expectedPopupTitle,6);

        expect(BetList.getParlayDetailUrl).toHaveBeenCalledWith(1,2,3,4,expectedPopupTitle,6);
        expect(dataGrid.setState).toHaveBeenCalledWith({ isShowPopup: true, popupUrl: expectedResultUrl, popupTitle: expectedPopupTitle});
    });
    describe('_showNewParlayDetail()', () => {
        const transId = "abc";
        const transIndex = 999;
        const refNo = "bccc";
        const handicap1 = 222;
        const handicap2 = 333;
        const betcheck = 444;
        const betteam = 555;
        document.body.innerHTML = `<div id='divEvent_${transId}${transIndex}'></div><div id='combinationOfferName_${transId}${transIndex}'></div>`;
        let divEvent = document.querySelector('#divEvent_' + transId + "" + transIndex);
        let nameDiv = document.querySelector('#combinationOfferName_' + transId + transIndex);

        const dataGrid = new BaseDataGrid({toggleLoader: jest.fn()});
        

        it('Assign class .mx-parlay to divEvent',() =>{
            dataGrid._showNewParlayDetail(transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam);

            expect(divEvent.classList.toString()).toBe('mx-parlay');
        });

        it('divEvent is hidden - get choice from server and append to divEvent',(done) =>{
            const expectedParlayDetailUrl = "www.alskdlf.com";
            divEvent.style.display = "none";

            BetList.getChoiceParlayDetailUrl = jest.fn(() => expectedParlayDetailUrl);
            const promise = Promise.resolve({
                data: '<div>KimJongUn</div>'
            });
            axios.get = jest.fn((url, option) => promise);
            BetList.buildChoiceParlayDetail = jest.fn(() => "abc");

            dataGrid._showNewParlayDetail(transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam);

            expect(BetList.getChoiceParlayDetailUrl).toHaveBeenCalledWith(transId, refNo, handicap1, handicap2, betcheck, betteam);
            expect(axios.get).toHaveBeenCalledWith(expectedParlayDetailUrl);
            promise.then((result) => {
                expect(BetList.buildChoiceParlayDetail).toHaveBeenCalledWith('<div>KimJongUn</div>');
                expect(divEvent.innerHTML).toBe("abc");
                expect(divEvent.style.display).toBe('block');
                expect(nameDiv.style.display).toBe('none');
                done();
            });
        });

        it('divEvent is show - hide divEvent and show nameDiv',() =>{
            divEvent.style.display = "block";

            dataGrid._showNewParlayDetail(transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam);

            expect(divEvent.innerHTML).toBe("");
            expect(divEvent.style.display).toBe('none');
            expect(nameDiv.style.display).toBe('block');
        });
    });
});