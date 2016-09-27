var commonLoaders = [
    {
        test: /\.(png|jpe?g|gif|svg)(\?.*)?$/,
        loader: 'url',
        query: {
            limit: 10000
        }
    },
    {
        test: /\.(woff2?|eot|ttf|otf)(\?.*)?$/,
        loader: 'url',
        query: {
            limit: 10000
        }
    }
]

module.exports = {
    commonLoaders: commonLoaders
}