jest.mock('react-redux')
jest.mock('redux')
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { ActionCreators } from '../../../actions/general/index.js'

describe('connect ', () => {
    let mapStateToProps, mapDispatchToProps;
    const connectMock = jest.fn();

    beforeAll(() => {
        connect.mockImplementationOnce((mapStateToPropsFunc, mapDispatchToPropsFunc) => {
            mapStateToProps = mapStateToPropsFunc;
            mapDispatchToProps = mapDispatchToPropsFunc;
            return connectMock;
        });
        require('../datagrid').default;
    });

    it('props should be mapped correctly', () => {
        const expectedResult = {
            gridData: [],
            dateRange: {},
            param: {},
            xsrfToken: "token"
        }

        const state = {
            general: {
                gridData: [],
                dateRange: {},
                param: {},
                anotherProp: "Not included"
            },
            xsrfToken: "token",
            anotherProp: "Not included"
        }

        const result = mapStateToProps(state);

        expect(result).toEqual(expectedResult);
    })

    it('bindActionCreators should be called with correct parameter', () => {
        const mockBindAction = jest.fn();
        bindActionCreators.mockImplementationOnce(mockBindAction);

        const dispatch = {};
        mapDispatchToProps(dispatch);

        expect(mockBindAction).toHaveBeenCalledWith(ActionCreators, dispatch);
    })

    it('returned function of connect should be called with correct parameter', () => {
        const DataGrid = require('../datagrid').DataGrid;

        expect(connectMock).toHaveBeenCalledWith(DataGrid);
    })
})