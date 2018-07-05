import Site from '@jsroot/lib/site'
const ApiUrl = {
    CUSTOMER_SEARCH_URL: Site.resolveClientUrl("SearchCustomerAPI/SearchCustomer"),
    RUNNING_DATA_URL: Site.resolveClientUrl('api/BettingTicketCenter/GetByRunning')
}
export default ApiUrl;
