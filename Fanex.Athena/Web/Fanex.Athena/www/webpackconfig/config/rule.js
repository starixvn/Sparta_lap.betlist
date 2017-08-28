var commonRules = [
   {
       test: require.resolve("jquery"),
       use: [{ loader: "expose-loader?$!expose-loader?jQuery" }]
   }
]

module.exports = {
    commonRules: commonRules
}