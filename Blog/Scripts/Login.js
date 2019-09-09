
function getPasswordHash(passwordElement , nonceElement , hashElement)
{
    var password = $("#" + passwordElement).val();
    var nonace = $(nonceElement).attr('value');
    $(hashElement).attr('value', $.sha256(password + nonace));
    $(passwordElement).val('');
    debugger;
}