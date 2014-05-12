/*
* Created 20100203@Lee - Left menu page functions
* Revision ?@? - ... 
* 
*/
var __lasAgentId = 60;
function menuItemSelect(id, source) {

    if (id == __lasAgentId) {
        var agentUrl = $('#child-menu-link-' + id).attr("url");
        $.ajax({
            url: agentUrl,
            dataType: 'json',
            async: false,
			cache:false,
            success: function (data) {

                if (data.IsOk) {
                    $('#child-menu-link-' + __lasAgentId).attr("href", data.Url);
                } else {
                    alert("Sorry, an error occurred while processing your request.");
                }

            },
            error: function (jqXHR, exception) {
                if (jqXHR.status === 0) {
                    alert('Not connect.\n Verify Network.');
                } else if (jqXHR.status == 404) {
                    alert('Requested page not found. [404]');
                } else if (jqXHR.status == 500) {
                    alert('Internal Server Error [500].');
                } else if (exception === 'parsererror') {
                    alert('Requested JSON parse failed.');
                } else if (exception === 'timeout') {
                    alert('Time out error.');
                } else if (exception === 'abort') {
                    alert('Ajax request aborted.');
                } else {
                    alert('Uncaught Error.\n' + jqXHR.responseText);
                }
            }
        });
    }
    $('td.active-menu').removeClass('active-menu');
    $('#child-menu-' + id).addClass('active-menu');
    $("#ActiveLinkIdOriginal").val('child-menu-link-' + id);

    var parentTag = $('#child-menu-link-' + id).closest('table');
    var prevTag = parentTag.prev('div');
    var parentMenu = prevTag.text();

    parentMenu = parentMenu.toLowerCase().replace(/\b[a-z]/g, function (letter) {
        return letter.toUpperCase();
    });

    parent.document.title = 'LRF' + ' - ' + parentMenu + ' - ' + $('#child-menu-link-' + id).html();

};

function menuToggle(id) {

    var hideSubMenu = $('#children-menu-' + id).is(':hidden');

    //hide other menu
    $.each($('#menu-container table[id^="children-menu-"]'), function (index, value) {
        if ($(this).attr("id") == 'children-menu-' + id) {
            if (hideSubMenu) {
                $('#children-menu-' + id).slideDown("fast");
            }
            else {
                $('#children-menu-' + id).slideUp("fast");
            }
        }
        else {
            var ie7 = (typeof document.addEventListener != 'function' && window.XMLHttpRequest) ? true : false;
            if (ie7) {
                $(this).hide();
            }
            else {
                $(this).slideUp("fast");
            }

            $('#menu-arrow-' + id).attr("class", "parent-menu-collapse");
        }
    });

    //change arrow icon for menu title
    var arrowClass = hideSubMenu ? 'parent-menu-open' : 'parent-menu-collapse';
    var bgClass = hideSubMenu ? "parent-menu-open2" : "parent-menu-collapse2";
    $('#menu-arrow-' + id).attr('class', arrowClass);
    $("#menu-title-" + id).parent().attr('class', bgClass);

}

function onclickUser(username) {

    var txtUserName = $('#txtUserName');
    if (txtUserName.val() == username) {
        txtUserName.removeClass("search-text");
        txtUserName.addClass("text-focus");
        txtUserName.val('');
    }
};

function onblurUser(username) {

    var txtUserName = $('#txtUserName');
    if (txtUserName.val() == '') {
        txtUserName.removeClass("text-focus");
        txtUserName.addClass("search-text");
        txtUserName.val(username);
    }
};

function KeyPress(e) {

    if (!e) e = window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;
    if (key == 13) {
        searchByUserName();
        return false;
    }
};

function searchByUserName() {

    //ref Core.js
    // BlockContent();

    var userName = $('#txtUserName').val();
    userName = null == userName ? "" : userName;

    var url = "../Customer/CustomerList?userName="+userName;

    //active menu memberinformation
    var tdCustomerList = $('#MemberInfoCustomerList').closest('td');
    if (tdCustomerList.hasClass('active-menu') == false) {
        //remove previous active menu
        $('td').removeClass('active-menu');
        tdCustomerList.addClass('active-menu');
        $('.sub-menu-container').hide();
        $('#aMemberInfo').trigger("mousedown");
    }
    top.main.location.href = url;
}

function activeDefaultLink() {
    var found = false;

    $.each($('div#menu-container a[id^="child-menu-link"]'), function (index, value) {
        if (found) return;

        var link = $(value);
        if (link.prop('id') == "child-menu-link-42") {
            found = true;

            $('td.active-menu').removeClass('active-menu');
            link.closest("td").addClass('active-menu');
            link.closest("table").show();
        }
    });
}

$(window).load(function () {

    // auto suggest username
    var txtUserName = $('#txtUserName');
    var serUrl = "../CustomerAPI/SearchCustomer?licenseeType=&currencyId=0";

    txtUserName.autocomplete({ serviceUrl: serUrl, minChars: 2 });
    $("#search-button").click(function () { return searchByUserName(); });
    $('#child-menu-link-' + __lasAgentId).attr("href", "javascript:void(0);");
    $('#child-menu-link-' + __lasAgentId).attr("target", "_blank");

    activeDefaultLink();
});
