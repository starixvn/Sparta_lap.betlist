jest.mock('react-redux')
jest.mock('redux')
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { ActionCreators } from '../../actions/index.js'

describe('connect ', () => {
    let mapStateToProps, mapDispatchToProps;
    const connectMock = jest.fn();

    beforeAll(() => {
        connect.mockImplementationOnce((mapStateToPropsFunc, mapDispatchToPropsFunc) => {
            mapStateToProps = mapStateToPropsFunc;
            mapDispatchToProps = mapDispatchToPropsFunc;
            return connectMock;
        });
        require('../FormFilter').default;
    });

    it('props should be mapped correctly', () => {
        const expectedResult = {
            dateRange: { fromDate: "1/1/2018", toDate: "1/2/2018" },
            selectedBaseCurrency: "EUR",
            isLoading: false
        }

        const state = {
            gridData: [],
            dateRange: { fromDate: "1/1/2018", toDate: "1/2/2018" },
            selectedBaseCurrency: "EUR",
            isLoading: false
        }

        const result = mapStateToProps(state);

        expect(result).toEqual(expectedResult);
    })

    it('bindActionCreators should be called with correct parameter', () => {
        const mockBindAction = jest.fn();
        bindActionCreators.mockImplementationOnce(mockBindAction);

        const dispatch = {};
        const expectedResult = [ActionCreators, dispatch];

        mapDispatchToProps(dispatch);

        expect(mockBindAction.mock.calls[0]).toEqual(expectedResult);
    })

    it('returned function of connect should be called with correct parameter', () => {
        const FormFilter = require('../FormFilter').FormFilter;
        const expectedResult = [FormFilter];

        expect(connectMock.mock.calls[0]).toEqual(expectedResult);
    })
})