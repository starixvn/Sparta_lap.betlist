import React from 'react';
import axios from 'axios'

import { PageLayout } from '../PageLayout';
import BetList from '../../lib/BetList';

const DefaultProps = { 
};
describe('PageLayout methods ', () => {
    
    it('componentDidMount - Assign functions to window', () => { 
        const pageLayout = new PageLayout(DefaultProps);
            
        pageLayout.componentDidMount();

        expect(typeof(window.ViewResult)).toBe("function");
        expect(typeof(window.showNewCombMP)).toBe("function");
        expect(typeof(window.showCombMPDetail)).toBe("function");
    });

    it('componentWillUnmount - Remove assigned window functions', () => { 
        const pageLayout = new PageLayout(DefaultProps);
            
        pageLayout.componentDidMount(); // assign 
        pageLayout.componentWillUnmount();

        expect(window.ViewResult).toBeUndefined();
        expect(window.showNewCombMP).toBeUndefined();
        expect(window.showCombMPDetail).toBeUndefined();
    });

    it('_closePopup  - set state isShowPopup = false', () => { 
        const pageLayout = new PageLayout();
        pageLayout.setState = jest.fn();

        pageLayout._closePopup();

        expect(pageLayout.setState).toHaveBeenCalledWith({ isShowPopup: false});
    });

    it('window.ViewResult() - Call Betlist and setState correctly',() =>{
        const expectedResultUrl = "www.facebook.com";
        const pageLayout = new PageLayout(DefaultProps);
        pageLayout.setState = jest.fn();
        BetList.getResultUrl = jest.fn(() => expectedResultUrl);
        pageLayout.componentDidMount();

        window.ViewResult(1,2,3,4);

        expect(BetList.getResultUrl).toHaveBeenCalledWith(1,2,3,4);
        expect(pageLayout.setState).toHaveBeenCalledWith({ isShowPopup: true, popupUrl: expectedResultUrl, popupTitle: "Result"});
    });

    it('window.showNewCombMP() - Call _showNewParlayDetail() correctly',() =>{
        const pageLayout = new PageLayout(DefaultProps);
        pageLayout._showNewParlayDetail = jest.fn();
        
        pageLayout.componentDidMount();

        window.showNewCombMP(1,2,3,4);

        expect(pageLayout._showNewParlayDetail).toHaveBeenCalledWith(1,2,3,4);
    });

    it('window.showCombMPDetail() - Call Betlist and setState correctly',() =>{
        const expectedResultUrl = "www.facebook.com";
        const expectedPopupTitle = "System ABC";
        const pageLayout = new PageLayout(DefaultProps);
        BetList.getParlayDetailUrl = jest.fn(() => expectedResultUrl);
        pageLayout.setState = jest.fn();

        pageLayout.componentDidMount();

        window.showCombMPDetail(1,2,3,4,expectedPopupTitle,6);

        expect(BetList.getParlayDetailUrl).toHaveBeenCalledWith(1,2,3,4,expectedPopupTitle,6);
        expect(pageLayout.setState).toHaveBeenCalledWith({ isShowPopup: true, popupUrl: expectedResultUrl, popupTitle: expectedPopupTitle});
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

        const pageLayout = new PageLayout({toggleLoader: jest.fn()});
        

        it('Assign class .mx-parlay to divEvent',() =>{
            pageLayout._showNewParlayDetail(transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam);

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

            pageLayout._showNewParlayDetail(transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam);

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

            pageLayout._showNewParlayDetail(transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam);

            expect(divEvent.innerHTML).toBe("");
            expect(divEvent.style.display).toBe('none');
            expect(nameDiv.style.display).toBe('block');
        });
    });
});