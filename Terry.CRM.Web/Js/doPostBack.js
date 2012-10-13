    //利用_dopostBack函数实现页面对服务器端事件的调用
    function __doPostBack_Ex(eventTarget,eventArgument) 
    {
        var theform;
        if (window.navigator.appName.toLowerCase().indexOf("netscape") > -1) {
            theform = document.forms[0];
        }
        else {
            theform = document.forms[0];
        }

        if(!theform.__EVENTTARGET)
        {            
            theform.appendChild(document.createElement("<input type='hidden' name='__EVENTTARGET'>"));
        }
        
        if(!theform.__EVENTARGUMENT)
        {            
            theform.appendChild(document.createElement("<input type='hidden' name='__EVENTARGUMENT'>"));                        
        }
        
        theform.__EVENTTARGET.value = eventTarget.split("$").join(":");
        theform.__EVENTARGUMENT.value = eventArgument;
        if ((typeof(theform.onsubmit) == "function")) 
        {
            if(theform.onsubmit()!=false)
            {
                theform.submit();    
            }
        }
        else
        {            
            theform.submit();    
        }
    }
    //----
    function __doPostBack(eventTarget, eventArgument)
    {
        __doPostBack_Ex(eventTarget, eventArgument);
    }
