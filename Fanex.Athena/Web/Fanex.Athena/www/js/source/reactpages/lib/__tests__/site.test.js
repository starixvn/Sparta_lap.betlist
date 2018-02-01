import moment from 'moment'
import Site from '../site'

describe('Site ', () => {
    it('resolveClientUrl - return correct value', () => {
        const expectedResult = "/api/Action";

        const result = Site.resolveClientUrl("api/Action");

        expect(result).toBe(expectedResult);
    })

    describe('convertDateToString', () => {
        it('type of date param is string - return date', () => {
            const date = "10/01/2017";

            const result = Site.convertDateToString(date);

            expect(result).toBe(date);
        })

        it('type of date param is moment object - return formatted date', () => {
            const dateString = "10/01/2017";
            const date = moment(dateString, "MM/DD/YYYY");

            const result = Site.convertDateToString(date);

            expect(result).toBe(dateString);
        })
    })

    describe("asNull", () => {
        it("text is null - return itself", () => {
            const text = null;
            const result = Site.asNull(text);

            expect(result).toBe(text);
        })

        it("text is empty - return null", () => {
            const text = "";
            const result = Site.asNull(text);

            expect(result).toBe(null);
        })

        it("text is white space - return null", () => {
            const text = " ";
            const result = Site.asNull(text);

            expect(result).toBe(null);
        })

        it("text has value - return itself with trimming", () => {
            const text = "value ";
            const expectedResult = "value";

            const result = Site.asNull(text);

            expect(result).toBe(expectedResult);
        })
    })
})