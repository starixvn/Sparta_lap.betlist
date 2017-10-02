import * as XLSX from 'xlsx-style';
import Excel from '../excel';

jest.mock('xlsx-style');

const originalDownloadFunction = Excel.download;

describe('Excel ', () => {
    describe('export', () => {
        const worksheetContent = "ws content";
        const worksheet = {};
        const filename = "filename";

        beforeAll(() => {
            XLSX.write.mockImplementation(() => worksheetContent);
            Excel.download = jest.fn();
        })

        afterAll(() => {
            XLSX.write.mockReset();
            Excel.download.mockReset();
        })

        it('XLSX.write should be called with correct parameter', () => {
            const option = { bookType: "xlsx", bookSST: false, type: 'binary' };
            const workbook = { SheetNames: ["filename"], Sheets: {}, Props: {} };
            workbook.Sheets["filename"] = worksheet;

            Excel.export(worksheet, filename);

            expect(XLSX.write).toHaveBeenCalledWith(workbook, option);
        })

        it('download function should be called with correct parameter', () => {
            Excel.export(worksheet, filename);

            expect(Excel.download).toHaveBeenCalledWith(worksheetContent, filename);
        })
    })

    it('getCellAddress - XLSX.utils.encode_cell should be called with correct parameter', () => {
        const cellObject = { c: 0, r: 0 };

        Excel.getCellAddress(cellObject);

        expect(XLSX.utils.encode_cell).toHaveBeenCalledWith(cellObject);
    })

    it('sheetToArrayBuffer - return correct buffer content', () => {
        const sheetContent = "AH";
        const buffer = new ArrayBuffer(sheetContent.length);
        const expectedResult = new Uint8Array(buffer);
        expectedResult[0] = 65;
        expectedResult[1] = 72;

        const result = Excel.sheetToArrayBuffer(sheetContent);

        expect(result).toEqual(buffer);
    })
})