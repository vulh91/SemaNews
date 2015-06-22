
// index giữ giá trị thẻ div đang được trỏ đến
// editMode giữ giá trị thực hiện enum quản lý tiến trình
var editMode = "";
var xpath = "";
// current select element ([Chọn Element] button clicked) - article define
var current_select_btn;
//thực hiện sự kiện khi trang web đã load thành công

//----------------------------------------------
function getElementXPath(element) {
    if (element && element.id)
        return '//*[@@id="' + element.id + '"]';
    else
        return this.getElementTreeXPath(element);
}
function getElementTreeXPath(element) {
    var paths = [];

    // Use nodeName (instead of localName) so namespace prefix is included (if any).
    for (; element && element.nodeType == 1; element = element.parentNode) {
        var index = 0;
        for (var sibling = element.previousSibling; sibling; sibling = sibling.previousSibling) {
            // Ignore document type declaration.
            if (sibling.nodeType == Node.DOCUMENT_TYPE_NODE)
                continue;

            if (sibling.nodeName == element.nodeName)
                ++index;
        }

        var tagName = element.nodeName.toLowerCase();
        var pathIndex = (index ? "[" + (index + 1) + "]" : "");
        paths.splice(0, 0, tagName + pathIndex);
    }

    return paths.length ? "/" + paths.join("/") : null;
}

//----------------------------------------------
//function đọc custom xpath 
function getElementXPath_custom(element) {
    return this.getElementTreeXPath_custom(element);
}
function getElementTreeXPath_custom(element) {
    var paths = [];

    // Use nodeName (instead of localName) so namespace prefix is included (if any).
    for (; element && element.nodeType == 1; element = element.parentNode) {
        var index = 0;
        for (var sibling = element.previousSibling; sibling; sibling = sibling.previousSibling) {
            // Ignore document type declaration.
            if (sibling.nodeType == Node.DOCUMENT_TYPE_NODE)
                continue;

            if (sibling.nodeName == element.nodeName)
                ++index;
        }

        var nodes = [], values = [];
        for (var attr, i = 0, attrs = element.attributes, l = attrs.length; i < l; i++) {
            attr = attrs.item(i)
            nodes.push(attr.nodeName);
            values.push(attr.nodeValue);
        }

        var attrs = "";

        for (var i = 0; i < nodes.length; i++) {
            attrs += "{" + nodes[i] + "=" + values[i] + "}";
        }

        var tagName = element.nodeName.toLowerCase();
        var pathIndex = (index ? "[" + (index + 1) + "]" : "");

        if (tagName != "html" && tagName != "body")
            pathIndex = pathIndex + attrs;



        paths.splice(0, 0, tagName + pathIndex);
    }


    return paths.length ? "/" + paths.join("/") : null;
}



$(document).ready(function () {


    $("#WMDefineForm").hide();

    $("#WMController").offset({ left: 0, top: 0 });

    var groupOptCount = $("select#WMGroup option").length;
    if (groupOptCount > 1) {
        $("#WMCreateForm").hide();
        editMode = "group";
    }
    else {
        $("#WMGroupForm").hide();
        editMode = "create";
    }

    $("select#WMGroup").change(function () {
        $("*").removeClass("hover");
        $("*").removeClass("child_hover");
        $("*").removeClass("welist_hover");
        $("*").removeClass("wepage_hover");
        var selected = $("select#WMGroup option:selected").val();
        if (selected >= 1) {
            var add = ".xpath" + selected;
            var highlightSet = $(add);
            var hightlightType = $(".notation" + selected);
            $("#GroupSelectNumber").val(selected);
            for (var i = 0; i < highlightSet.length; i++) {
                var xpath = $(highlightSet[i]).text();
                var element = document.evaluate(xpath.toString(), document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
                $(element).removeAttr("background");
                $(element).children().addClass("child_hover");
                $(element).addClass("hover");
                if ($(hightlightType[i]).text() == "1") {
                    $(element).addClass("welist_hover");
                }
                else if ($(hightlightType[i]).text() == "2") {
                    $(element).addClass("wepage_hover");
                }
            }
        }
    });

    // Select a xpath - btn "Select" clicked event
    $("#WMbtnSelect").click(function (e) {
        var weType = $("#WMWeType option:selected").val();
        var textvalue = $("#WMtxtXpath").val();

        if (weType == "LIST") {
            // clone template and set values
            var itemTemplate = $("#HE-item-template").clone();
            itemTemplate.removeAttr("id");
            itemTemplate.addClass("HE-item");
            itemTemplate.removeClass("hidden");
            var xpathInp = itemTemplate.find(".he-list").first();
            xpathInp.val(textvalue);
            // add to table
            //$("#containerHE-List").append(itemTemplate);
            itemTemplate.insertBefore('.pe-ele-area');

            // register delete button event
            // Delete item button click
            $(".delete-we-item").click(function () {
                $($(this).parents(".HE-item")).remove();
            });

            e.preventDefault();
        }
        else if (weType == "PAGINATION") {
            $("#pe-ele").val(textvalue)
        }
        $("#WMConfirmInputValue").show(400);
        setTimeout(function () {
            $("#WMConfirmInputValue").fadeOut(400);
        }, 3000);

    });

    // prevent hover active when click in menu item
    $(".DWM_outer").click(function (e) {
        editMode = "stop";
    });

    // save click - field define
    $("#WMSubmit").click(function () {
        var heList = $(".HE-item .he-list");
        var peEle = $("#pe-ele");
        // save values
        var heVal = "", peVal = peEle.val();
        var url = $(this).data().url;
        var fieldId = $("#fieldId").val();
        // ----------------------------
        for (var i = 0; i < heList.length; i++) {
            heVal += $(heList[i]).val() + "###";
        }

        $.ajax({
            type: "POST",
            url: url,
            data: { heList: heVal, peEle: peVal, fieldId: fieldId },
            success: function (data) {
                if (data.status == "success") {
                    SuccessRedirect(data.msg);
                } else {
                    alert(data.msg);
                }
            }
        });

    });

    // [chọn Element] btn click - article define
    $(".we-select-btn").click(function () {
        current_select_btn = $(this);
        ActiveHover();
    });

    // Add Element btn clicked
    $("#article-add-element-btn").click(function () {
        $($(current_select_btn.parents("tr")).find("input[type='text']")).val($("#WMtxtXpath").val());
        $(".WMDefineForm").show();
    });

    $(".go-back-btn").click(function () {
        window.location.replace(document.referrer);
    });

});

// save struct success function
function SuccessRedirect(success_msg) {
    $("#WMConfirmInputValue").html(success_msg);
    $("#WMConfirmInputValue").show(400);
    setTimeout(function () {
        window.location.replace(document.referrer);
    }, 3000);
}

//--------------
function ActiveHover() {
    CloseDefineForm();
    editMode = "create";
}
//--------------
function ParentSelect() {
    if (lastTarget.parentNode)
        lastTarget = lastTarget.parentNode;
    else
        return;
    SetWMSelector();

}
//--------------

function NextSiblingSelect() {
    if (lastTarget.nextSibling) {
        lastTarget = lastTarget.nextSibling;
        while (lastTarget.nodeName == "#text") {
            if (lastTarget.nextSibling)
                lastTarget = lastTarget.nextSibling;
            else
                return;

        }
    }
    else
        return;
    SetWMSelector();

}

function PreviousSiblingSelect() {
    if (lastTarget.previousSibling) {
        lastTarget = lastTarget.previousSibling;
        while (lastTarget.nodeName == "#text") {
            if (lastTarget.previousSibling)
                lastTarget = lastTarget.previousSibling;
            else
                return;
        }
    }
    else
        return;
    SetWMSelector();

}

function FirstChildSelect() {
    if (lastTarget.firstChild) {
        lastTarget = lastTarget.firstChild;
        while (lastTarget.nodeName == "#text") {
            if (lastTarget.nextSibling)
                lastTarget = lastTarget.nextSibling;
            else
                return;

        }
    }
    else
        return;
    SetWMSelector();
}

//------------

function ReviewElementByXpath() {
    var xpath = $("#WMtxtXpath").val();
    var element = document.evaluate(xpath.toString(), document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
    if (element != null) {
        selectEle = element;
        SetWMSelector();
    }
}

//------------

function ActiveDefineForm() {
    editMode = 1;
    $(".WMDefineForm").show();
}

function CloseDefineForm() {
    $(".WMDefineForm").hide();
    editMode = "create";
}

function FormDeleteRow(ele) {
    var test = $(ele).attr("name");
    var xpathrow = "#ListWE_" + test + "__Address";
    $(xpathrow).removeAttr("value");
    var rowid = "#WMWeRow" + test;
    $(rowid).hide();
}

//------------


//------------


var ActiveCreateForm = function () {
    $("*").removeClass("hover");
    $("*").removeClass("child_hover");
    $("*").removeClass("welist_hover");
    $("*").removeClass("wepage_hover");
    $("#WMCreateForm").show();
    $("#WMGroupForm").hide();
    editMode = "create";
}

var DeleteGroup = function () {
    $("#GroupStatus").val("delete");
}

//------------
var SetWMSelector = function () {

    var offset, el = lastTarget;
    var now = +new Date;
    if (now - last < 25)
        return;
    last = now;
    if (el.className === "DWM_outer") {
        box.hide();
        el = document.elementFromPoint(e.clientX, e.clientY);
    }
    box.show();


    el = $(el);
    offset = el.offset();
    box.css({
        width: el.outerWidth() - 1,
        height: el.outerHeight() - 1,
        left: offset.left,
        top: offset.top
    });




    var fromHead = $(lastTarget).index();
    var fromTail = $(lastTarget.parentNode).children().length - fromHead;


    xpath = getElementXPath_custom(lastTarget) + "{fromHead=" + fromHead + "}{fromTail=" + fromTail + "}";

    $("#WMtxtXpath").val(xpath);
}


// hover area functions

var box = $("<div class='DWM_outer' />").css({
    display: "none", position: "absolute",
    zIndex: 65000, background: "rgba(255, 0, 0, .3)"
}).appendTo("body");

var lastTarget, last = +new Date;
$("body").mousemove(function (e) {
    if (editMode == "create") {
        var offset, el = e.target;
        var now = +new Date;
        var el_id = $(el).attr("id");
        if (typeof el_id == "string") {
            var checkid = el_id.substring(0, 2);
            if (checkid == "WM") {
                box.hide();
                return;
            }
        }

        if (now - last < 25)
            return;
        last = now;
        if (el === document.body || el_id == "WM") {
            box.hide();
            return;
        } else if (el.className === "DWM_outer") {
            box.hide();
            el = document.elementFromPoint(e.clientX, e.clientY);
        }
        box.show();

        if (el === lastTarget)
            return;
        lastTarget = el;
        el = $(el);
        offset = el.offset();
        box.css({
            width: el.outerWidth() - 1,
            height: el.outerHeight() - 1,
            left: offset.left,
            top: offset.top
        });


        var fromHead = $(lastTarget).index();
        var fromTail = $(lastTarget.parentNode).children().length - fromHead;


        xpath = getElementXPath_custom(lastTarget) + "{fromHead=" + fromHead + "}{fromTail=" + fromTail + "}";
        $("#WMtxtXpath").val(xpath);

    }
});