export const HEADER_STYLE = {
    font: { bold: true, color: { rgb: "FFFFFF" } },
    alignment: { horizontal: "center" },
    fill: {
        patternType: "solid",
        fgColor: { rgb: "4C6375" }
    },
    border: { left: { style: "thin", color: { rgb: "D6D6D6" } } }
};

export const ROW_SPAN_CELL_STYLE = { alignment: { vertical: "center" } };
export const ROW_SPAN_CENTER_CELL_STYLE = { alignment: { vertical: "center", horizontal: "center" } };
export const NUMBER_CELL_STYLE = { numFmt: "#,##0" };
export const DECIMAL_CELL_STYLE = { numFmt: "#,##0.00" };

export const TOTAL_CELL_STYLE = { font: { bold: true } };
export const TOTAL_NUMBER_CELL_STYLE = { font: { bold: true }, numFmt: "#,##0" };
export const TOTAL_DECIMAL_CELL_STYLE = { font: { bold: true }, numFmt: "#,##0.00" };

export const GRAND_TOTAL_CELL_STYLE = {
    font: { bold: true },
    fill: {
        patternType: "solid",
        fgColor: { rgb: "B8C3C9" }
    }
};
export const GRAND_TOTAL_NUMBER_CELL_STYLE = {
    font: { bold: true },
    numFmt: "#,##0",
    fill: {
        patternType: "solid",
        fgColor: { rgb: "B8C3C9" }
    }
};
export const GRAND_TOTAL_DECIMAL_CELL_STYLE = {
    font: { bold: true },
    numFmt: "#,##0.00",
    fill: {
        patternType: "solid",
        fgColor: { rgb: "B8C3C9" }
    }
};