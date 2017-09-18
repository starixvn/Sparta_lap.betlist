export const headerCellStyle = {
    font: { bold: true, color: { rgb: "FFFFFF" } },
    alignment: { horizontal: "center" },
    fill: {
        patternType: "solid",
        fgColor: { rgb: "4C6375" }
    },
    border: { left: { style: "thin", color: { rgb: "D6D6D6" } } }
}

export const bodyCellStyle = {
    rowspan: { alignment: { vertical: "center" } },
    center: { alignment: { vertical: "center", horizontal: "center" } },
    number: { numFmt: "#,##0" },
    decimal: { numFmt: "#,##0.00" }
}

export const totalCellStyle = {
    default: { font: { bold: true } },
    number: { font: { bold: true }, numFmt: "#,##0" },
    decimal: { font: { bold: true }, numFmt: "#,##0.00" }
}

export const grandTotalCellStyle = {
    default: {
        font: { bold: true },
        fill: {
            patternType: "solid",
            fgColor: { rgb: "B8C3C9" }
        }
    },
    number: {
        font: { bold: true },
        numFmt: "#,##0",
        fill: {
            patternType: "solid",
            fgColor: { rgb: "B8C3C9" }
        }
    },
    decimal: {
        font: { bold: true },
        numFmt: "#,##0.00",
        fill: {
            patternType: "solid",
            fgColor: { rgb: "B8C3C9" }
        }
    }
}