import * as XLSX from 'xlsx-style';

const EXCEL_TYPE = "xlsx";

export default class Excel {
    static export(worksheet, fileName) {
        const option = { bookType: EXCEL_TYPE, bookSST: false, type: 'binary' };
        const workbook = { SheetNames: [fileName], Sheets: {}, Props: {} };
        workbook.Sheets[fileName] = worksheet;

        var workbookOutput = XLSX.write(workbook, option);
        this.download(workbookOutput, fileName);
    }

    static indexToColumnName(index) {
        var columnName = '',
            a = 1,
            b = 26;

        index += 1; //we assume index is start from 0

        while ((index -= a) >= 0) {
            columnName = String.fromCharCode(parseInt((index % b) / a) + 65) + columnName;
            a = b;
            b *= 26;
        }
        return columnName;
    }

    static getCellAddress(cellObject) {
        return this.indexToColumnName(cellObject.c) + (cellObject.r + 1).toString();
    }

    static download(workbookOut, fileName) {
        const blob = new Blob([this.sheetToArrayBuffer(workbookOut)], {
            type: "application/octet-stream"
        });

        const fileNameDownload = `${fileName}.${EXCEL_TYPE}`;

        // IE 10+
        if (typeof navigator !== "undefined" && navigator.msSaveOrOpenBlob) {
            navigator.msSaveOrOpenBlob(blob, fileNameDownload);
        }
        else {
            const element = window.document.createElement('a');
            element.href = window.URL.createObjectURL(blob);
            element.download = fileNameDownload;
            document.body.appendChild(element);
            element.click();
            document.body.removeChild(element);
        }
    }

    static sheetToArrayBuffer(s) {
        const buffer = new ArrayBuffer(s.length);
        const view = new Uint8Array(buffer);
        for (let i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
        return buffer;
    }
}