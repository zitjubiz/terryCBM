////////////////////////////////////////////////////////////////////////////////
/*
 *--------------- 客户端表单通用验证checkForm(oForm) -----------------

 *	1.对非ie的支持
 *	2.增加了内置表达式和内置提示
 *  3.增加了显示方式（弹出式和页面显示式）
 *	4.增加了显示一条和显示全部
 *	5.进行了封装（CLASS_CHECK）
 *	6.支持外接函数或表达式（应用在密码一致）
 *	7.简化了调用方式，所有操作只需要<script language='javascript' src='checkform.js'>,
	  然后在HTML里定义各标签验证格式
 *	8.对IE增加了对键盘输入的限制（如：定义usage='int'时，输入框只能输入数字
 *	9.增加了对disabled的不验证
 * 10.自定义报警方式（重写showMessageEx方法）
 * 11.能对多FORM验证,还有针对普通按钮(非submit)下的验证

			只需要对需要验证的标签设置三个属性：usage,exp,tip

			usage	:	内置格式或表达式或函数
			exp		:	正则表达式（注意如果指定了usage则忽略exp)
			tip		:	出错提示（如果是内置格式可以不要此属性，有缺省提示）

			调用时只需要引用<script language='javascript' src='checkform.js'>，然后为每个标记
			增加以上3个属性（不一定需要全部）
			
   * 12.增加"^"符号，则表示可为空。 例如：   usage='^int'

   * 13.增加多表单验证（不限表单数目）
                     (在一个页面出现多表单验证时，实现如下：如
                           在第二个表单TextBox上加上：usage2="notempty"，
                           在第二个表单提交按钮上加上： MultiForms="2"，
                           显示错误的DIV为：<div id="divErrorMessage2"> </div> 即可。其它第三，第四。。。如此类推
                       )
                       
   * 14.此keycheck函数暂时最多只支持3个表单，如需要，增加sUsage即可！

示例：

    账号:<INPUT name=id usage="notempty">不能为空<BR>
	整数:<INPUT usage="int" >46<BR>
	正整数:<INPUT usage="int+" >13试试能不能输入非数字<BR>
	负整数:<INPUT usage="int-" >-45<BR>
	浮点数:<INPUT usage="float" >56.4<BR>
	正浮点数:<INPUT usage="float+" >1.0<BR>
    使用正规表达式 <asp:TextBox id="billingFirstName" maxlength="50" runat="server"  Exp=".{2,}$" tip="<%$ Resources:Resource, strBillFirstName %>"></asp:TextBox>

	<INPUT type=submit value=提交查询内容 check=true>
	
	
	------------------------------------------
    第2个表单示例：
	
	姓名:<INPUT name=user Tip="第2个表单---姓名不能为空" Exp2="[^ ]+" disabled=true>为Disabled时不验证<BR><BR>
	
	账号:<INPUT name=id Tip="第2个表单---账号不能为空" Exp2="[^ ]+">不能为空<BR><BR>
	
    密码<INPUT name="nn" id="password2" usage2="notempty" tip="第2个表单---不为空">不为空<br><BR>
    
    重复密码<INPUT name="nn" id="rpassword2" usage2="test2()" tip="第2个表单---密码要一致">密码要一致<br><BR>
    
	<INPUT type=submit value="提交FORM2内容"   check="true"  MultiForms="2"><BR>
	
	<div id="divErrorMessage2"> </div>
	

    //调用外部JS函数
    function test2()
    {
        return document.getElementById('password2').value==document.getElementById('rpassword2').value;
    }
 */
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function CLASS_CHECK(){
	this.pass		= true;		//是否通过验证
	this.showAll	= true;		//是否显示所有的验证错误
	this.alert		= 0;		//报警方式（默认alert报警） 0:auto;1:alert;2:span;
	this.message	= "";		//错误内容
	this.first		= null;		//在显示全部验证错误时的第一个错误控件（用于回到焦点）
	this.cancel		= false;
	this.display	= null;
	this.MultiForms	= "";      //多表单, "": 第一个表单 ; 2:第二个表单 ;  3:第三个表单.....

	//定义内置格式
	var aUsage = {
		"int":"^([+-]?)\\d+$",										//整数
		"int+":"^([+]?)\\d+$",										//正整数
		"int-":"^-\\d+$",											//负整数
		"num":"^([+-]?)\\d*\\.?\\d+$",								//数字
		"num+":"^([+]?)\\d*\\.?\\d+$",								//正数
		"num-":"^-\\d*\\.?\\d+$",									//负数
		"float":"^([+-]?)\\d*\\.\\d+$",								//浮点数
		"float+":"^([+]?)\\d*\\.\\d+$",								//正浮点数
		"float-":"^-\\d*\\.\\d+$",									//负浮点数
																	//邮件
		"email":"^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$",	
		"color":"^#[a-fA-F0-9]{6}",									//颜色
		"url":"^http[s]?:\\/\\/([\\w-]+\\.)+[\\w-]+([\\w-./?%&=]*)?$",	//联接
		"chinese":"^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$",			//仅中文
		"ascii":"^[\\x00-\\xFF]+$",									//仅ACSII字符
		"zipcode":"^\\d{6}$",										//邮编
		"mobile":"^0{0,1}13[0-9]{9}$",								//手机
		"ip4":"^\(([0-1]\\d{0,2})|(2[0-5]{0,2}))\\.(([0-1]\\d{0,2})|(2[0-5]{0,2}))\\.(([0-1]\\d{0,2})|(2[0-5]{0,2}))\\.(([0-1]\\d{0,2})|(2[0-5]{0,2}))$",				//ip地址
		"notempty":"[^ ]+",											//非空
		"picture":"(.*)\\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$",	//图片
		"rar":"(.*)\\.(rar|zip|7zip|tgz)$",							//压缩文件
		"date":"^\\d{2,4}[\\/\\-]?((((0?[13578])|(1[02]))[\\/|\\-]?((0?[1-9]|[0-2][0-9])|(3[01])))|(((0?[469])|(11))[\\/|\\-]?((0?[1-9]|[0-2][0-9])|(30)))|(0?[2][\\/\\-]?(0?[1-9]|[0-2][0-9])))$",								//日期
		"time":"^(20|21|22|23|[01]\\d|\\d)(([:.][0-5]\\d){1,2})$",	//时间
		
		"chardigit":"^[A-Za-z0-9]+$",								//字符和数字
		"int>0":  "^[0-9]*[1-9][0-9]*$" ,                           //正整数,不包括0 ,可用于Qty
		"intfloat+": "^(([+]?)\\d+)|(([+]?)\\d*\\.\\d+)$"         //正的整数或者浮点数, ,可用于Price

	};

	//缺省消息
	var aMessage = {
	    "int": "请输入整数", 									//整数
	    "int+": "请输入正整数", 								//正整数
	    "int-": "请输入负整数", 								//负整数
	    "num": "请输入数字", 									//数字
	    "num+": "请输入正数", 									//正数
	    "num-": "请输入负整数", 								//负数
	    "float": "请输入浮点数", 								//浮点数
	    "float+": "请输入正浮点数", 								//正浮点数
	    "float-": "请输入负浮点数", 								//负浮点数
	    "email": "请输入正确的邮箱地址", 						//邮件
	    "color": "请输入正确的颜色", 							//颜色
	    "url": "请输入正确的连接地址", 						//联接
	    "chinese": "请输入中文", 									//中文
	    "ascii": "请输入ascii字符", 								//仅ACSII字符
	    "zipcode": "请输入正确的邮政编码", 						//邮编
	    "mobile": "请输入正确的手机号码", 						//手机
	    "ip4": "请输入正确的IP地址", 							//ip地址
	    "notempty": "不能为空", 									//非空
	    "picture": "请选择图片", 									//图片
	    "rar": "请输入压缩文件", 								//压缩文件
	    "date": "请输入正确的日期", 							//日期
	    "time": "请输入正确的时间", 							//时间
	    "chardigit": "请输入英文字符和数字", 					//字符和数字
	    "int>0": "请输入正整数,不包括0",                          //正整数,不包括0
	    "intfloat+": "请输入正的整数或者浮点数"                   //正的整数或者浮点数

	};

	var me = this;
		//定义单独验证函数(用于普通button验证)
		//me.checkForm = function(e){checkForm(e);}

	me.checkForm =function(oForm){
		me.pass		= true;
		me.message	= "";
		me.first	= null;
		
		if(me.cancel==true){
			return true;
		}
		
		var els = oForm.elements;

		var errIndex = 1;
		//遍历所有表元素
		for(var i=0;i<els.length;i++){
			//取得格式
			//var sUsage= getAttribute(els[i],"usage"+ this.MultiForms);			//.getAttribute("Usage");
			var sUsage= getAttribute(els[i],"usage"+ this.MultiForms);			//.getAttribute("Usage");	// 多表单

			
			var sReg	= "";
			var bEmpty	= false;
			
			//如果设置Usage，则使用内置正则表达式，忽略Exp
			if(typeof(sUsage)!="undefined"&&sUsage!=null){
				if(sUsage.substring(0,1)=="^"){					
					if(aUsage[sUsage.substring(1,sUsage.length)]!=null){
						sUsage = sUsage.substring(1,sUsage.length);
						bEmpty = true;				
					}
				}
			
				//如果Usage在表达室里找到，则使用内置表达式，无则认为是表达式；表达式可以是函数；
				if(aUsage[sUsage]!=null){			
					sReg = aUsage[sUsage];
				} else {
					try{
						if(eval(sUsage)==false){	
							me.pass		= false;
							if(me.first==null){
								me.first	= els[i];
							}							
							addMessage(getMessage(els[i]),errIndex);	
							errIndex++;						
							if(me.showAll==false){
								setFocus(els[i]);
								break;
							}
						}
					} catch(e){ 
						alert("表达式[" + sUsage +"]错误:" + e.description)
						return false;
					}
				}
			} else {			
			    
			        // sReg = getAttribute(els[i],"exp");//.getAttribute("Exp");
			        sReg = getAttribute(els[i],"exp" + this.MultiForms);//.getAttribute("Exp"); 多表单

			}

			if(typeof(sReg)!="undefined"&&sReg!=null){
				//对于失效状态不验证
				if(isDisabled(els[i])==true){
					continue;
				}

				//取得表单的值,用通用取值函数
				var sVal = getValue(els[i]);	
				
				if(bEmpty){
					if(sVal==""){
						continue;
					}
				}		

				//字符串->正则表达式,不区分大小写
				var reg = new RegExp(sReg,"i");
				if(!reg.test(sVal)){
					me.pass		= false;
					if(me.first==null){
						me.first	= els[i];
					}

					//alert(reg);
					//验证不通过,弹出提示warning
					var sTip = getMessage(els[i]);
					if(sTip.length==0&&typeof(sUsage)!="undefined"&&sUsage!=null&&aMessage[sUsage]!=null){
						sTip = aMessage[sUsage];
					}
					addMessage(sTip,errIndex);
					errIndex++;

					if(me.showAll==false){
						//该表单元素取得焦点,用通用返回函数
						setFocus(els[i]);
						break;
					}
				}
			}        
		}

		if(me.pass==false){
			showMessage(this.MultiForms);

			if(me.first!=null&&me.showAll==true){
				setFocus(me.first);
			}
		}

		return me.pass;
	}

	function getAttribute(o,att){
		if(o.tagName =="INPUT"&&typeof(o.id)!="undefined"&&o.id.indexOf("_0")>0){
			var id = o.id.substring(0,o.id.indexOf("_0"));
			var oo  = document.getElementById(id);

			if(oo!=null&&oo.tagName=="TABLE"){
				return oo.getAttribute(att);
			} else {
				return o.getAttribute(att);
			}

		} else {
			return o.getAttribute(att);
		}
	}


	/*
	 *	添加错误信息
	 */
	function addMessage(msg,index){
//如果是<li>
	   if(me.alert==1){
			me.message += "" + msg + "\n";
		} else {
			me.message += "<li> " + msg + "<br>";
		}
//如果是数字
//		if(me.alert==1){
//			me.message += index + "." + msg + "\n";
//		} else {
//			me.message += index + "." + msg + "<br>";
//		}
	}

	/*
	 *	显示错误
	 */
	function getMessage(els) {
		var sTip = getAttribute(els,"tip");
		if(typeof(sTip)!="undefined"&&sTip!=null){
			return sTip;
		} else {
			return "";
		}
	}


	/*
	 *	显示错误
	 */
	function showMessage(_multiForms){	
		//外接显示错误函数
		if(typeof(me.showMessageEx)=="function"){
			return me.showMessageEx(me.message);
		}
		
		var timeout			= null;				
		var loaded			= false;
		var obj				= document.createElement("span");

		switch(me.alert){
			case 0:
				return show(me.message.replace(/\n/g,"<br>"));
				break;
			case 1:
			    //为了和系统整合,把alert改成jAlert
				jAlert(me.message,"警告");
				break;
			case 2:
				var divTip ; 
				var divTipContent ;
				//var divTip = me.display;  这个会使页面的内容隐藏掉
				try	{
					if(typeof(divTip)=="undefined"||divTip==null){
					
						divTip = document.getElementById("divErrorMessage" + _multiForms); //多表单Error

						if(typeof(divTip)=="undefined"||divTip==null){
							divTip = document.createElement("div");
							divTip.id			= "divErrorMessage"  + this.MultiForms;
							divTip.name			= "divErrorMessage"  + this.MultiForms;
							divTip.style.color	= "blue";
							divTip.style.display = 'none';
							document.body.appendChild(divTip);
						}
					}
					divTip.innerHTML = me.message;
				}catch(e){}
				break;
			default:
				break;
		}

		function show(str,autoHide){
			if(loaded==false){
				obj.style.border		= "#999999 1px solid";				
				obj.style.padding		= "10px";
				obj.style.fontSize		= "9pt";
				obj.style.width			= "250px";
				obj.style.fontFamily	= "宋体"; 
				obj.style.height		= str.split("<br>").length*14;
				obj.style.backgroundColor = "#fff4d4";
				obj.style.textAlign		= "left";
				obj.onclick				= function(){this.style.display="none";}
				obj.title				= "click me to hide";
				obj.zIndex				= "9999";

				if(typeof(document.body.firstChild)!="undefined"&&document.body.firstChild!=null){
					document.body.insertBefore(obj,document.body.firstChild);
				} else {
					document.body.appendChild(obj);
				}
				loaded = true;
			}

			obj.style.display	= "";
			obj.style.position	= "absolute";
			obj.style.left 		= (document.body.clientWidth	- obj.offsetWidth)	/ 2;
			obj.style.top		= (document.body.clientHeight	- obj.offsetHeight)/ 2;
			obj.innerHTML		= str;

			if(autoHide != false){
				timeout	= setTimeout(	
												function(){
													obj.style.display	= "none";	
													clearTimeout(timeout);
												}
												,2500
											 );
			}
		}
	}

	/*
	 *	获得元素是否失效（失效的元素不做判断）
	 */
	function isDisabled(el){
		//对于radio,checkbox元素，只要其中有一个非失效元素就验证
		if(el.type=="radio"||el.type=="checkbox"){
			//取得第一个元素的name,搜索这个元素组
			var tmpels = document.getElementsByName(el.name);
			for(var i=0;i<tmpels.length;i++){
				if(tmpels[i].disabled==false){
					return false;
				}
			}
			return true;
		} else {
			return el.disabled;
		}
	}


	/*
	 *	取得对象的值（对于单选多选框把其选择的个数作为需要验证的值）
	 */
	function getValue(el){
		//取得表单元素的类型
		var sType = el.type;
		switch(sType){
			//文本输入框,直接取值el.value
			case "text":
			case "hidden":
			case "password":
			case "file":
			case "textarea": 
				return el.value;
			//单多下拉菜单,遍历所有选项取得被选中的个数返回结果"0"表示选中一个，"00"表示选中两个
			case "checkbox":
			case "radio": 
				return getRadioValue(el);
			case "select-one":
			case "select-multiple": 				
				return getSelectValue(el);
		}
		//取得radio,checkbox的选中数,用"0"来表示选中的个数,我们写正则的时候就可以通过0{1,}来表示选中个数
		function getRadioValue(el){
			var sValue = "";

			if(el.tagName =="INPUT"&&typeof(el.id)!="undefined"&&el.id.indexOf("_0")>0){
				var id = el.id.substring(0,el.id.indexOf("_0"));
				var oo  = document.getElementById(id);

				if(oo!=null&&oo.tagName=="TABLE"){
					var index = 0;

					for(var i=0;i<oo.rows.length;i++){
						var tr = oo.rows[i];
						for(var j=0;j<tr.cells.length;j++){							
							var cell = document.getElementById(id + "_" + index);
							if(cell!=null&&cell.checked){
								sValue +=0;
							}
							index++;
						}
					}
					return sValue;
				}
			}
		
			//取得第一个元素的name,搜索这个元素组
			var tmpels = document.getElementsByName(el.name);
			for(var i=0;i<tmpels.length;i++){
				if(tmpels[i].checked){
					sValue += "0";
				}
			}
	
			return sValue;
		}
		//取得select的选中数,用"0"来表示选中的个数,我们写正则的时候就可以通过0{1,}来表示选中个数
		function getSelectValue(el){
			var sValue = "";
			for(var i=0;i<el.options.length;i++){
				//单选下拉框提示选项设置为value=""
				if(el.options[i].selected && el.options[i].value!=""){
					sValue += "0";
				}
			}
			return sValue;
		}
	}

	/*
	 *	对没有通过验证的元素设置焦点
	 */
	function setFocus(el){
		//取得表单元素的类型
		var sType = el.type;
		switch(sType){
			//文本输入框,光标定位在文本输入框的末尾
			case "text":
			case "hidden":
			case "password":
			case "file":
			case "textarea": 
				try{el.focus();var rng = el.createTextRange(); rng.collapse(false); rng.select();}catch(e){};
				break;
			
			//单多选,第一选项非失效控件取得焦点
			case "checkbox":
			case "radio": 
				var els = document.getElementsByName(el.name);
				for(var i=0;i<els.length;i++){
					if(els[i].disabled == false){
						els[i].focus();
						break;
					}
				}
				break;
			case "select-one":
			case "select-multiple":
				el.focus();
				break;
		}
	}

	

	//避免内存漏洞的addEvent方法
	function addEvent(obj, type, fn){
		if (obj.addEventListener){
			obj.addEventListener(type, fn, false);
		}
		else if (obj.attachEvent){
			obj['e'+ type + fn] = fn;
			obj.detachEvent('on'+ type, obj['e'+ type + fn]);
			obj.attachEvent('on'+ type, obj['e'+ type + fn]);
		}
	}

	function removeEvent(obj, type, fn){
		if (obj.removeEventListener){
			obj.removeEventListener(type, fn, false);
		}
		else if (obj.detachEvent){
			obj.detachEvent('on'+ type, obj['e'+ type + fn]);
			obj['e'+ type + fn] = null;
		}
	}
	
	//自动绑定到所有form的onsubmit事件
	if(window.attachEvent){
		window.attachEvent("onload",function()
									{
										for(var i=0;i<document.forms.length;i++){
											var theFrom = document.forms[i]; 
												function mapping(f){
													var fn = function(){return me.checkForm(f);}
													addEvent(f,"submit",fn);
													addEvent(window,"unload",function(){removeEvent(f,"submit",fn);});
												} 

												if(theFrom){													
													mapping(theFrom); 
													var _ie_form_click =		function(){
																								var o = event.srcElement;
																								
																								//在按钮为提交时或
																								if(typeof(o.type)!="undefined"){
																									me.cancel = me.isCancel(o);																					
																								}																								
																							}

													addEvent(theFrom,"click",_ie_form_click); 													
													addEvent(window,"unload",function(){removeEvent(theFrom,"click",_ie_form_click);});
													
												}
										}
									}
							);

	}
	else
	{
	
		window.onsubmit = function(e){var theFrom = e.target;if(theFrom){return me.checkForm(theFrom);}}
		var _ff_form_click = function(e){	var o = e.target;
								if(typeof(o.type)!="undefined"){									
									me.cancel = me.isCancel(o);																									
									}
								}
		addEvent(window,"click",_ff_form_click);
		addEvent(window,"unload",function(){removeEvent(window,"click",_ff_form_click);});

		
	}
	
	this.isCancel = function(o){
	
		var check = o.getAttribute("check"); ;
		var whichFrom= o.getAttribute("MultiForms");     // 多表单
		
		if(typeof(whichFrom)!="undefined"&&whichFrom!=null)
		{
		    this.MultiForms	= whichFrom;
		}
		else
		{
		    this.MultiForms = "";
		}
		
		//兼容老格式,且优先考虑（主要是客户端控件）如果check=false则取消验证
		if(typeof(check)!="undefined"&&check!=null){
			if(check.toLowerCase()=="false")
				return true;
			else
				return false;
		}
		
		var s = "";	
		
		switch(o.tagName.toLowerCase()){
			case "input":
				if(o.onclick!=null&&typeof(o.onclick)!="undefined"){
					s = o.onclick.toString();
				}
				break;
			case "a":
				if(o.href!=null&&typeof(o.href)!="undefined"){
					s = o.href.toString();
				}			
				break;
		}
		
		//判断是否有此函数，有则不需要验证
		if(s.indexOf("Page_ClientValidate")>=0){
			return false;
		}
		
		return true;
		
	}
	
     //此keycheck函数暂时最多只支持3个表单，如需要，增加sUsage即可！
	this.keyCheck = function(){

		addEvent(window,"load",function(e){for(var i=0;i<document.forms.length;i++){var theFrom = document.forms[i]; if(theFrom){myKeyCheck(theFrom);}}});

		function myKeyCheck(oForm){

			var els = oForm.elements;
			//遍历所有表元素
			for(var i=0;i<els.length;i++){
				//取得格式
				var sUsage;
				var sUsage1= getAttribute(els[i],"usage");			//.getAttribute("Usage");
                var sUsage2= getAttribute(els[i],"usage2");			//.getAttribute("Usage2");	
                var sUsage3= getAttribute(els[i],"usage3");			//.getAttribute("Usage3");	
                
				if(typeof(sUsage1)=="undefined" || sUsage1==null)
				{
				         if(typeof(sUsage2)=="undefined" || sUsage2==null)
				         {
				                if(typeof(sUsage3)=="undefined" || sUsage3==null)
				                    var sUsage=null;
				                else
				                    sUsage=sUsage3;
				         }
				         else
				            sUsage=sUsage2;
				}
				else
				    sUsage=sUsage1;

				//如果设置Usage，则使用内置正则表达式，忽略Exp
				if(typeof(sUsage)!="undefined"&&sUsage!=null){	
				
					if(sUsage.substring(0,1)=="^"){
						if(aUsage[sUsage.substring(1,sUsage.length)]!=null){
							sUsage = sUsage.substring(1,sUsage.length);	}
					}		
					
					switch(sUsage.toLowerCase()){
						case "zipcode":
						case "int":
						case "int>0":
							els[i].onkeypress	= function(e){var chr;if(e)chr=e.charCode; else chr=window.event.keyCode;if(chr==0)return true; else return /\d/.test(String.fromCharCode(chr))||(this.value.indexOf('+')<0?String.fromCharCode(chr)=="+":false)||(this.value.indexOf('-')<0?String.fromCharCode(chr)=="-":false);}
							els[i].onpaste		= function(e){if(e==null)return !clipboardData.getData('text').match(/\D/); else return false;}
							els[i].ondragenter	= function(e){return false;}
							els[i].style.imeMode= "disabled";
							break;
						case "mobile":
						case "int+":
							els[i].onkeypress	= function(e){var chr;if(e)chr=e.charCode; else chr=window.event.keyCode;if(chr==0)return true; else return /\d/.test(String.fromCharCode(chr))||(this.value.indexOf('+')<0?String.fromCharCode(chr)=="+":false);}
							els[i].onpaste		= function(e){if(e==null)if(e==null)return !clipboardData.getData('text').match(/\D/); else return false; else return false;}
							els[i].ondragenter	= function(e){return false;}
							els[i].style.imeMode= "disabled";
							break;
						case "int-":
							els[i].onkeypress	= function(e){var chr;if(e)chr=e.charCode; else chr=window.event.keyCode;if(chr==0)return true; else return /\d/.test(String.fromCharCode(chr))||(this.value.indexOf('-')<0?String.fromCharCode(chr)=="-":false);}					
							els[i].onpaste		= function(e){if(e==null)return !clipboardData.getData('text').match(/\D/); else return false;}
							els[i].ondragenter	= function(e){return false;}
							els[i].style.imeMode= "disabled";
							break;
						case "float":
						case "num":
						case "intfloat+":
							els[i].onkeypress	= function(e){var chr;if(e)chr=e.charCode; else chr=window.event.keyCode;if(chr==0)return true; else return /[\+\-\.]|\d/.test(String.fromCharCode(chr));}
							els[i].onpaste = function (e) { if (e == null) return clipboardData.getData('text').match(/[\+\-\.]|\d/)!=null; else return false; }
							els[i].ondragenter	= function(e){return false;}
							els[i].style.imeMode= "disabled";
							break;
						case "float+":
						case "num+":
							els[i].onkeypress	= function(e){var chr;if(e)chr=e.charCode; else chr=window.event.keyCode;if(chr==0)return true; else return /[\+\.]|\d/.test(String.fromCharCode(chr));}
							els[i].onpaste = function (e) { if (e == null) return clipboardData.getData('text').match(/[\+\.]|\d/) != null; else return false; }
							els[i].ondragenter	= function(e){return false;}
							els[i].style.imeMode= "disabled";
							break;
						case "float-":
						case "num-":
							els[i].onkeypress	= function(e){var chr;if(e)chr=e.charCode; else chr=window.event.keyCode;if(chr==0)return true; else return /[\-\.]|\d/.test(String.fromCharCode(chr));}
							els[i].onpaste = function (e) { if (e == null) return clipboardData.getData('text').match(/[\-\.]|\d/) != null; else return false; }
							els[i].ondragenter	= function(e){return false;}
							els[i].style.imeMode= "disabled";
							break;
						case "ascii":
							els[i].style.imeMode= "disabled";
							break;
						case "ip4":
							els[i].onkeypress	= function(e){var chr;if(e)chr=e.charCode; else chr=window.event.keyCode;if(chr==0)return true; else return /[\.]|\d/.test(String.fromCharCode(chr));}
							els[i].onpaste = function (e) { if (e == null) return clipboardData.getData('text').match(/[\.]|\d/) != null; else return false; }
							els[i].ondragenter	= function(e){return false;}
							els[i].style.imeMode= "disabled";
							els[i].maxLength	= 15;
							break;
						case "color":
							els[i].onkeypress	= function(e){var chr;if(e)chr=e.charCode; else chr=window.event.keyCode;if(chr==0)return true; else return /[a-fA-Z]|\d/.test(String.fromCharCode(chr))||(this.value.indexOf('#')<0?String.fromCharCode(chr)=="#":false);}
							els[i].onpaste = function (e) { if (e == null) return clipboardData.getData('text').match(/[a-fA-Z]|\d/) != null; else return false; }
							els[i].ondragenter	= function(e){return false;}
							els[i].maxLength	= 7;
							els[i].style.imeMode= "disabled";
							break;
						case "date":
							els[i].onkeypress	= function(e){var chr;if(e)chr=e.charCode; else chr=window.event.keyCode;if(chr==0)return true; else return /[\/\-\.]|\d/.test(String.fromCharCode(chr));}
							els[i].onpaste = function (e) { if (e == null) return clipboardData.getData('text').match(/[\/\-\.]|\d/) != null; else return false; }
							els[i].ondragenter	= function(e){return false;}
							els[i].style.imeMode= "disabled";
							break;							
					}
				}
			}
		}
		
	}

	this.setup = function(o){

		addEvent(window,"load",function(e){me.display = document.getElementById(o);});

		/*
		if(window.attachEvent){
			window.attachEvent("onload",function(){me.display = document.getElementById(o);});
		} else {
			window.addEventListener("load",function(e){me.display = document.getElementById(o);},true);
		}*/
	}
}


// 初始化一个对象！！

if(typeof(CLASS_CHECK)=='function'){

	var	Checkform1 = new CLASS_CHECK();
		Checkform1.keyCheck();
		Checkform1.showAll=true;
		Checkform1.showNumPoint=0;
		Checkform1.alert = 1;
		Checkform1.MultiForms="";
		Checkform1.setup('aspnetForm');

}

