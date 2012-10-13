var beforeRow = null;

function openReportFrm(url, win_name) {
    var width = screen.availWidth - 10;
    var height = screen.availHeight - 10;
    var my_obj_win = window.open(url, win_name, "height=" + height + "px, width=" + width + "px, top=0px, left=0px, menubar=0, resizable=1, scrollbars=1, directories=0 ");

    try {
        my_obj_win.focus();
    } catch (e) {
    }

    return my_obj_win;
}


function openDetailFrm(url, frm_name) {
    var width = screen.availWidth - 10;
    var height = screen.availHeight - 10;
    var my_obj_win = window.open(url, frm_name, "");
    window.opener = null;
    window.open('', '_self', '');
    window.close();
    try {
        my_obj_win.focus();
    } catch (e) {
    }

    return my_obj_win;
}

function gvRowonclick(row) {//GridView
    if (beforeRow != null) {
        beforeRow.style.backgroundColor = '';
    }
    row.style.backgroundColor = 'PeachPuff';
    beforeRow = row;
}
function gvRowonmouseover(row) {////当鼠标停留时更改背景色

    if (row != beforeRow) {
        row.backup_backgroundColor = row.style.backgroundColor
        row.style.backgroundColor = '#D2E7F4'
        row.style.cursor = 'pointer';
    }
}
function gvRowonmouseout(row) {////当鼠标移开时还原背景色 
    if (row != beforeRow) {   //alert(row.backup_backgroundColor)
        //if( row.backup_backgroundColor )
        row.style.backgroundColor = row.backup_backgroundColor
    }
}

function HTMLEncode(html) {
    var temp = document.createElement("div");
    (temp.textContent != null) ? (temp.textContent = html) : (temp.innerText = html);
    var output = temp.innerHTML;
    temp = null;
    return output;
}

function HTMLDecode(text) {
    var temp = document.createElement("div");
    temp.innerHTML = text;
    var output = temp.innerText || temp.textContent;
    temp = null;
    return output;
}



/**
* X-browser event handler attachment and detachment
* TH: Switched first true to false per http://www.onlinetools.org/articles/unobtrusivejavascript/chapter4.html
*
* @argument obj - the object to attach event to
* @argument evType - name of the event - DONT ADD "on", pass only "mouseover", etc
* @argument fn - function to call
*/
function addEvent(obj, evType, fn) {
    if (obj.addEventListener) {
        obj.addEventListener(evType, fn, false);
        return true;
    } else if (obj.attachEvent) {
        var r = obj.attachEvent("on" + evType, fn);
        return r;
    } else {
        return false;
    }
}
function removeEvent(obj, evType, fn, useCapture) {
    if (obj.removeEventListener) {
        obj.removeEventListener(evType, fn, useCapture);
        return true;
    } else if (obj.detachEvent) {
        var r = obj.detachEvent("on" + evType, fn);
        return r;
    } else {
        alert("Handler could not be removed");
    }
}

/**
* Gets the full width/height because it's different for most browsers.
*/
function getViewportHeight() {
    if (window.innerHeight != window.undefined) return window.innerHeight;
    if (document.compatMode == 'CSS1Compat') return document.documentElement.clientHeight;
    if (document.body) return document.body.clientHeight;
    return window.undefined;
}
function getViewportWidth() {
    if (window.innerWidth != window.undefined) return window.innerWidth;
    if (document.compatMode == 'CSS1Compat') return document.documentElement.clientWidth;
    if (document.body) return document.body.clientWidth;
    return window.undefined;
}

/**
* Gets the real scroll top
*/
function getScrollTop() {
    if (self.pageYOffset) // all except Explorer
    {
        return self.pageYOffset;
    }
    else if (document.documentElement && document.documentElement.scrollTop)
    // Explorer 6 Strict
    {
        return document.documentElement.scrollTop;
    }
    else if (document.body) // all other Explorers
    {
        return document.body.scrollTop;
    }
}
function getScrollLeft() {
    if (self.pageXOffset) // all except Explorer
    {
        return self.pageXOffset;
    }
    else if (document.documentElement && document.documentElement.scrollLeft)
    // Explorer 6 Strict
    {
        return document.documentElement.scrollLeft;
    }
    else if (document.body) // all other Explorers
    {
        return document.body.scrollLeft;
    }
}

/*----说明有字符是不是数字---*/
function fucCheckNUM(NUM) {
    var i, j, strTemp;
    strTemp = "0123456789";
    if (NUM.length == 0)
        return false
    for (i = 0; i < NUM.length; i++) {
        j = strTemp.indexOf(NUM.charAt(i));
        if (j == -1) {
            //说明有字符不是数字     
            return false;
        }
    }
    //说明是数字     
    return true;
}

/*----说明有字符是不是正整数，不包括0---*/
function fucCheckQty(NUM) {
    if (NUM == "") return false;
    var re = /^[0-9]*[1-9][0-9]*$/;
    return re.test(NUM);
}


/*----说明有字符是不是Number Or Float---*/
function fucCheckPrice(NUM) {
    if (NUM == "") return false;
    var re = /^\d{0,}[.]?\d{1,}$/;
    return re.test(NUM);
}

/*----检查值是否为空---*/
function isEmpty(str) {
    if ((str == null) || (str.length == 0)) return true;
    else return (false);
}
// Is any object an array
function isArray(obj) {
    return obj && !(obj.propertyIsEnumerable('length')) && typeof obj === 'object' && typeof obj.length === 'number';
};


/*----返回错误的信息---*/
function getErrorMsg(str_in) {
    var str_out = "";
    if (str_in == "undefined" || str_in == null)
        return str_out;

    if (str_in.length > 0)
        str_out = "<li>" + str_in + "</li>";

    return str_out;

}


//"^\\d+$"　　//非负整数（正整数   +   0）   
//  "^[0-9]*[1-9][0-9]*$"　　//正整数   
//  "^((-\\d+)|(0+))$"　　//非正整数（负整数   +   0）   
//  "^-[0-9]*[1-9][0-9]*$"　　//负整数   
//  "^-?\\d+$"　　　　//整数   
//  "^\\d+(\\.\\d+)?$"　　//非负浮点数（正浮点数   +   0）   
//  "^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$"　　//正浮点数   
//  "^((-\\d+(\\.\\d+)?)|(0+(\\.0+)?))$"　　//非正浮点数（负浮点数   +   0）   
//  "^(-(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*)))$"　　//负浮点数   
//  "^(-?\\d+)(\\.\\d+)?$"　　//浮点数   
//  "^[A-Za-z]+$"　　//由26个英文字母组成的字符串   
//  "^[A-Z]+$"　　//由26个英文字母的大写组成的字符串   
//  "^[a-z]+$"　　//由26个英文字母的小写组成的字符串   
//  "^[A-Za-z0-9]+$"　　//由数字和26个英文字母组成的字符串   
//  "^\\w+$"　　//由数字、26个英文字母或者下划线组成的字符串   
//  "^[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+$"　　　　//email地址   
//  "^[a-zA-z]+://(\\w+(-\\w+)*)(\\.(\\w+(-\\w+)*))*(\\?\\S*)?$"　　//url   

/*-----------GridView JavaScript Control-----------------------*/

function onDel(btnDel) {
    var MsgConfirm = document.getElementById("ctl00_CPH1_hidDelMsg").value;
    document.getElementById("ctl00_CPH1_hidBtnDel").value = "" + btnDel;
    jConfirm(MsgConfirm, "请确认", isDel);
}

function isDel(result) {
    if (true == result) {
        var btnDel = document.getElementById("ctl00_CPH1_hidBtnDel").value;
        __doPostBack(btnDel, '');

    }
}

function onEdit(sId, url,newwindow) {
    var NewUrl = "";

    if (url.indexOf("?") > 0)
        NewUrl = url + "&id=" + sId; //+ "&fixCache=" + new Date().getTime();
    else
        NewUrl = url + "?id=" + sId; //+ "&fixCache=" + new Date().getTime();

    if (newwindow) {
        window.open(NewUrl);
    }
    else {
        window.location.href = NewUrl;
    }
    return false;
}


function onEdit2(url) {
    var NewUrl = "";

    if (url.indexOf("?") > 0)
        NewUrl = url + "&fixCache=" + new Date().getTime();
    else
        NewUrl = url + "&fixCache=" + new Date().getTime();

    window.location.href = NewUrl;
    return false;
}

function setSelItem(result, ddlID) {
    var ddl = document.getElementById(ddlID);
    for (var i = 0; i < ddl.length; i++) {
        if (String(ddl.options[i].text).trim() == String(result).trim()) {
            ddl.selectedIndex = i;
        }
    }
}
function Trim(d) {
    if (typeof d != "string")
    { return d }
    var c = d;
    var b = "";
    b = c.substring(0, 1);
    while (b == " ") {
        c = c.substring(1, c.length);
        b = c.substring(0, 1)
    }
    b = c.substring(c.length - 1, c.length);
    while (b == " ") {
        c = c.substring(0, c.length - 1);
        b = c.substring(c.length - 1, c.length)
    } return c
}
function GetCookie(key) {
    var c = "TerryCRM_" + key;
    var b = document.cookie; 
    b = b.split(";");
    var d = 0; 
    while (d < b.length) {
        var e = b[d];
        e = e.split("=");
        if (Trim(e[0]) == c) 
            { return (e[1]) }
        d++;
    }
       return -1
}

function SetCookie(key, value) {
    var b = "TerryCRM_" + key; 
    var c = new Date();
    c.setTime(c.getTime() + (365 * 24 * 60 * 60 * 1000));
    document.cookie = b + "=" + value + "; expires=" + c.toUTCString() + ";"
}

var g_collapse_clear = 1;


function ToggleDiv(b) {
    t_open_div = document.getElementById(b + "_open");
    t_closed_div = document.getElementById(b + "_closed");
    t_cookie = GetCookie("collapse_settings");
    if (1 == g_collapse_clear) {
        t_cookie = ""; 
        g_collapse_clear = 0
    } 
    if (t_open_div.className == "hidden") {
        t_open_div.className = "";
        t_closed_div.className = "hidden";
        t_cookie = t_cookie + "|" + b + ",1"
    }
    else {
        t_closed_div.className = "";
        t_open_div.className = "hidden";
        t_cookie = t_cookie + "|" + b + ",0"
    }
    SetCookie("collapse_settings", t_cookie)
}

