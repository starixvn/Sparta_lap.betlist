const summaryData = [{
        Id: 0,
        Subsidiary: "New York",
        Currency: "USD",

        Details: [{
                Products: "All",
                BetCount: 50,
                Turnover: "100,000.00",
                CustomerWinLoss: "10,000",
                OperatorWinLoss: "-10,000"
            },
            {
                Products: "Sportsbook",
                BetCount: 25,
                Turnover: "50,000.00",
                CustomerWinLoss: "5,000",
                OperatorWinLoss: "4950,000"
            },
            {
                Products: "BA",
                BetCount: 25,
                Turnover: "50,000.00",
                CustomerWinLoss: "5,000",
                OperatorWinLoss: "50,000"
            }
        ]
    },
    {
        Id: 1,
        Subsidiary: "India",
        Currency: "THB",
        Details: [{
            Products: "All",
            BetCount: 50,
            Turnover: "100,000.00",
            CustomerWinLoss: "10,000",
            OperatorWinLoss: "-10,000"
        }]
    },
    {
        Id: 2,
        Subsidiary: "New York",
        Currency: "USD",

        Details: [{
            Products: "All",
            BetCount: 50,
            Turnover: "100,000.00",
            CustomerWinLoss: "10,000",
            OperatorWinLoss: "-10,000"
        }]
    }
];

for (let i = 0; i < 1; i++) {
    summaryData.push({
        Id: 2,
        Subsidiary: "New York",
        Currency: "USD",

        Details: [{
            Products: "All",
            BetCount: 50,
            Turnover: "100,000.00",
            CustomerWinLoss: "10,000",
            OperatorWinLoss: "-10,000"
        }]
    })
}

const totalData = [{
        Subsidiary: "New York",
        Based: "EUR",
        BetCount: 150,
        Turnover: "300,000.00",
        CustomerWinLoss: "30,000",
        OperatorWinLoss: "-15,000"
    },
    {
        Subsidiary: "Total India",
        Based: "EUR",
        BetCount: 100,
        Turnover: "200,000.00",
        CustomerWinLoss: "20,000",
        OperatorWinLoss: "-25,000"
    }
]

const grantTotal = {
    Based: "EUR",
    BetCount: 250,
    Turnover: "500,000.00",
    CustomerWinLoss: "50,000",
    OperatorWinLoss: "-35,000"
}

export default {
    summaryData,
    totalData,
    grantTotal
}