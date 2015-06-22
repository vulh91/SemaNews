
function notifyMessage(msg, type) {
    $.bootstrapGrowl(msg, {
        ele: 'body', // which element to append to
        type: type, // (null, 'info', 'error', 'success')
        offset: { from: 'bottom', amount: 20 }, // 'top', or 'bottom'
        align: 'right', // ('left', 'right', or 'center')
        delay: 4000, // Time while the message will be displayed. It's not equivalent to the *demo* timeOut!
        allow_dismiss: true, // If true then will display a cross to close the popup.
        stackup_spacing: 10 // spacing between consecutively stacked growls.
    });
}

$.fn.clickToggle = function (e, t) { return this.each(function () { var o = !1; $(this).bind("click", function () { return o ? (o = !1, t.apply(this, arguments)) : (o = !0, e.apply(this, arguments)) }) }) };
