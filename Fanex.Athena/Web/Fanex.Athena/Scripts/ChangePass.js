/* File Created: April 28, 2014 */

// Prototype
IbcMsg = function () {
    this.spmsgerr = 'spmsgerr';
    this.diverrmsg = 'diverrmsg';
}

IbcMsg.prototype.Show = function (msg, success) {

    if (success) {
        $('#spmsgerr').addClass('msgscc');
        document.getElementById('1_1').className = "succ_1_1";
        document.getElementById('1_2').className = "succ_1_2";
        document.getElementById('1_3').className = "succ_1_3";
        document.getElementById('2_1').className = "succ_2_1";
        document.getElementById('2_2').className = "succ_2_2";
        document.getElementById('3_1').className = "succ_3_1";
        document.getElementById('3_2').className = "succ_3_2";
        document.getElementById('3_3').className = "succ_3_3";
    }
    else {
        document.getElementById('spmsgerr').className = 'msgerr';
        document.getElementById('1_1').className = "emsg_1_1";
        document.getElementById('1_2').className = "emsg_1_2";
        document.getElementById('1_3').className = "emsg_1_3";
        document.getElementById('2_1').className = "emsg_2_1";
        document.getElementById('2_2').className = "emsg_2_2";
        document.getElementById('3_1').className = "emsg_3_1";
        document.getElementById('3_2').className = "emsg_3_2";
        document.getElementById('3_3').className = "emsg_3_3";
    }

    $('#' + this.spmsgerr).html(msg);
    document.getElementById(this.diverrmsg).style.display = IE ? 'block' : 'table';
}

IbcMsg.prototype.Hide = function () {
    $('#' + this.diverrmsg).hide();
}

ibcMsg = new IbcMsg();
var errMsg = null;
function ChangePassword() {
    // Validate password
    var txtOldPwd = $('#txtOldPwd').val();
    var txtNewPwd = $('#txtNewPwd').val();
    var txtConfirmPwd = $('#txtConfirmPwd').val();

    if (txtOldPwd.length == 0) {
        errMsg = _page.alertInputOldPwd;

        $('#txtOldPwd').focus();
    }
    else if (txtNewPwd != txtConfirmPwd) {
        errMsg = _page.alertErrorConfirm;
        $('#txtNewPwd').focus();
    }
    else {
        errMsg = ValidatePassword(txtNewPwd);
        $('#txtNewPwd').focus();
    }

    if (null != errMsg) {
        ibcMsg.Show(errMsg);
        return;
    }
    Site.blockUI();



    $.ajax({
        url: Site.resolveClientUrl("Utility/ChangePass"),
        data: ({ txtNewPwd: $('#txtNewPwd').val(),
            txtOldPwd: $('#txtOldPwd').val(),
            txtConfirmPwd: $('#txtConfirmPwd').val()
        }),
        dataType: "json",
        type: "POST",
        async: false,
        success: function (result) {
            Site.unBlockUI();
            // Post with return result in JSON format
            var errCode = result.errCode;
            if (errCode == 0) {
                ibcMsg.Show(_page.alertPasswordSucc, true);
                if (_page.firstLogged) {
                    setTimeout(refresh(), 2000);
                }
            }
            else {
                $('#txtOldPwd').focus();
                ibcMsg.Show(result.errMsg);
            }
        }
    });
}

function Reset() {
    $('#txtOldPwd').val("");
    $('#txtNewPwd').val("");
    $('#txtConfirmPwd').val("");
}
function refresh() {
    var wnd = window.parent;
    while (wnd != wnd.parent) // Jump out
    {
        wnd = wnd.parent;
    }

    wnd.location = wnd.location.href;
}

// Check pass call in keyup event
function CheckPass(pwdElement, iconElementId) {
    var isPwd = IsPassWord(pwdElement.value);
    var iconElement = $(iconElementId);

    if (!iconElement) return false;

    if (isPwd) {
        iconElement.className = 'PwdSuccess';
    }
    else {
        iconElement.className = 'PwdError';
    }

    return isPwd;
}

function ValidatePassword(str) {
    if (str.length == 0) return _page.alertPasswordEmpty;
    else if (!IsPassWord(str)) return _page.alertPasswordRequire;

    return null;
}

$(window).load(function () {
    if ($("#header-title")) {//Set title page
        parent.document.title = document.title;
    }
});