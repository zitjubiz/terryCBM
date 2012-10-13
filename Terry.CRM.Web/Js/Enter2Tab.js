	    $(document).bind("keydown",
	    function(event){
	        var src = event.srcElement?event.srcElement:event.target;

            if(event.keyCode==13&&    
                src.type != 'button' &&    
                src.type != 'submit' &&    
                src.type != 'reset' &&    
                src.type != 'textarea' &&    
                src.type != '')   
            {
                //
                var ele = document.forms[0].elements;   
                for (var i = 0; i < ele.length; i++) {   
                    var q;   
                    q = (i == ele.length - 1) ? 0 : i + 1;   
                    if (src == ele[i]) {   
                        ele[q].focus();   
                        break;   
                    }   
                }   
                event.preventDefault();  

            }
        });
