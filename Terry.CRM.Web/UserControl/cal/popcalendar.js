//	written	by Tan Ling	Wee	on 2 Dec 2001
//	last updated 10 Apr 2002
//	email :	fuushikaden@yahoo.com
//  Modified (very little) by Sai on 01/02/05
//  email :	sai_freelance@yahoo.com

	var	fixedX = -1 // x position (-1 if to appear below control)
	var	fixedY = -1 // y position (-1 if to appear below control)
	var startAt = 0 // 0 - sunday ; 1 - monday
	var showWeekNumber = 0	// 0 - don't show; 1 - show
	var showToday = 1		// 0 - don't show; 1 - show
	
	var width_with_WeekNumber = 250
	var width_non_WeekNumber = 170
	
	//var imgDir = getRootPath() + "UserControl/cal/"	// directory for images ... e.g. var imgDir="/img/"
    var imgDir ="../UserControl/cal/"
    
	var gotoString = "Go To Current Month"
	var todayString = "Today is"
	var weekString = "Wk"
	var scrollLeftMessage = "Click to scroll to previous month. Hold mouse button to scroll automatically."
	var scrollRightMessage = "Click to scroll to next month. Hold mouse button to scroll automatically."
	var selectMonthMessage = "Click to select a month."
	var selectYearMessage = "Click to select a year."
	var selectDateMessage = "Select [date] as date." // do not replace [date], it will be replaced by date.
    
	var	crossobj, crossMonthObj, crossYearObj, monthSelected, yearSelected, dateSelected, omonthSelected
	var oyearSelected, odateSelected, monthConstructed, yearConstructed, intervalID1, intervalID2
	var timeoutID1, timeoutID2, ctlToPlaceValue, ctlNow, dateFormat, nStartingYear

	var	bPageLoaded=false
	var	ie=document.all
	var	dom=document.getElementById

	var	ns4=document.layers
	var	today =	new	Date()
	var	dateNow	 = today.getDate()
	var	monthNow = today.getMonth()
	var	yearNow	 = today.getYear()
	var	imgsrc = new Array("drop1.gif","drop2.gif","left1.gif","left2.gif","right1.gif","right2.gif")
	var	img	= new Array()
	
	var oFunctions = ""

	var bShow = false;

    /* hides <select> and <applet> objects (for IE only) */
    function hideElement( elmID, overDiv )
    { 
      if( ie )
      {        
        for( i = 0; i < document.all.tags( elmID ).length; i++ )
        {        
          obj = document.all.tags( elmID )[i];
         
          if( !obj || !obj.offsetParent )
          {
            continue;
          }
      
          // Find the element's offsetTop and offsetLeft relative to the BODY tag.
          objLeft   = obj.offsetLeft;
          objTop    = obj.offsetTop;
          objParent = obj.offsetParent;
          
          while( objParent.tagName.toUpperCase() != "BODY" )
          {    
            objLeft  += objParent.offsetLeft;
            objTop   += objParent.offsetTop;
            objParent = objParent.offsetParent;
         
            if( objParent == null || objParent.tagName == null ) break;
          }

          objHeight = obj.offsetHeight;
          objWidth = obj.offsetWidth;

          if(( overDiv.offsetLeft + overDiv.offsetWidth ) <= objLeft );
          else if(( overDiv.offsetTop + overDiv.offsetHeight ) <= objTop );
          else if( overDiv.offsetTop >= ( objTop + objHeight ));
          else if( overDiv.offsetLeft >= ( objLeft + objWidth ));
          else
          {
            obj.style.visibility = "hidden";
          }
        }
      }
    }
     
    /*
    * unhides <select> and <applet> objects (for IE only)
    */
    function showElement( elmID )
    {
      if( ie )
      {
        for( i = 0; i < document.all.tags( elmID ).length; i++ )
        {
          obj = document.all.tags( elmID )[i];
          
          if( !obj || !obj.offsetParent )
          {
            continue;
          }
        
          obj.style.visibility = "";
        }
      }
    }

	function HolidayRec (d, m, y, desc)
	{
		this.d = d
		this.m = m
		this.y = y
		this.desc = desc
	}

	var HolidaysCounter = 0
	var Holidays = new Array()

	function addHoliday (d, m, y, desc)
	{
		Holidays[HolidaysCounter++] = new HolidayRec ( d, m, y, desc )
	}


	if (dom)
	{
		for	(i=0;i<imgsrc.length;i++)
		{
			img[i] = new Image
			img[i].src= img + imgsrc[i]
		}
		document.write ("<div onclick='bShow=true' id='calendar' class='div-style'><table cellpadding='0px' cellspacing='0px' width="+((showWeekNumber==1)?width_with_WeekNumber:width_non_WeekNumber)+" class='table-style'><tr class='title-background-style' ><td><table width='100%'><tr><td class='title-style'><B><span id='caption'></span></B></td><td align=right><a href='javascript:hideCalendar()'><IMG SRC='"+imgDir+"close.gif' WIDTH='15' HEIGHT='13' BORDER='0' ALT='Close the Calendar'></a></td></tr></table></td></tr><tr><td class='body-style'><span id='content'></span></td></tr>")
			
		if (showToday==1)
		{
			document.write ("<tr class='today-style'><td><span id='lblToday'></span></td></tr>")
		}
			
		document.write ("</table></div><div id='selectMonth' class='div-style'></div><div id='selectYear' class='div-style'></div>");
	}

	//var	monthName =	new	Array("January","February","March","April","May","June","July","August","September","October","November","December")
	var	monthName =	new	Array("Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec")
	if (startAt==0)
	{
		dayName = new Array	("Sun","Mon","Tue","Wed","Thu","Fri","Sat")
	}
	else
	{
		dayName = new Array	("Mon","Tue","Wed","Thu","Fri","Sat","Sun")
	}

	function swapImage(srcImg, destImg){
		if (ie)	{ document.getElementById(srcImg).setAttribute("src",imgDir + destImg) }
	}

	function init()	{

		if (!ns4)
		{
			if (!ie) { yearNow += 1900	}
			crossobj=(dom)?document.getElementById("calendar").style : ie? document.all.calendar : document.calendar
			hideCalendar()

			crossMonthObj=(dom)?document.getElementById("selectMonth").style : ie? document.all.selectMonth	: document.selectMonth

			crossYearObj=(dom)?document.getElementById("selectYear").style : ie? document.all.selectYear : document.selectYear

			monthConstructed=false;
			yearConstructed=false;

			if (showToday==1)
			{
				document.getElementById("lblToday").innerHTML =	todayString + " <a class='today-style' onmousemove='window.status=\""+gotoString+"\"' onmouseout='window.status=\"\"' title='"+gotoString+"' href='javascript:monthSelected=monthNow;yearSelected=yearNow;constructCalendar();'>"+dayName[(today.getDay()-startAt==-1)?6:(today.getDay()-startAt)]+", " + dateNow + " " + monthName[monthNow].substring(0,3)	+ "	" +	yearNow	+ "</a>"
			}

			sHTML1= "<span id='spanLeft'  class='title-control-normal-style' onmouseover='swapImage(\"changeLeft\",\"left2.gif\");this.className=\"title-control-select-style\";window.status=\""+scrollLeftMessage+"\"' onclick='javascript:decMonth()' onmouseout='clearInterval(intervalID1);swapImage(\"changeLeft\",\"left1.gif\");this.className=\"title-control-normal-style\";window.status=\"\"' onmousedown='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"StartDecMonth()\",500)'	onmouseup='clearTimeout(timeoutID1);clearInterval(intervalID1)'>&nbsp<IMG id='changeLeft' SRC='"+imgDir+"left1.gif' width=10 height=11 BORDER=0>&nbsp</span>&nbsp;"
			sHTML1+="<span id='spanRight' class='title-control-normal-style' onmouseover='swapImage(\"changeRight\",\"right2.gif\");this.className=\"title-control-select-style\";window.status=\""+scrollRightMessage+"\"' onmouseout='clearInterval(intervalID1);swapImage(\"changeRight\",\"right1.gif\");this.className=\"title-control-normal-style\";window.status=\"\"' onclick='incMonth()' onmousedown='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"StartIncMonth()\",500)'	onmouseup='clearTimeout(timeoutID1);clearInterval(intervalID1)'>&nbsp<IMG id='changeRight' SRC='"+imgDir+"right1.gif'	width=10 height=11 BORDER=0>&nbsp</span>&nbsp"
			sHTML1+="<span id='spanMonth' class='title-control-normal-style' onmouseover='swapImage(\"changeMonth\",\"drop2.gif\");this.className=\"title-control-select-style\";window.status=\""+selectMonthMessage+"\"' onmouseout='swapImage(\"changeMonth\",\"drop1.gif\");this.className=\"title-control-normal-style\";window.status=\"\"' onclick='popUpMonth()'></span>&nbsp;"
			sHTML1+="<span id='spanYear'  class='title-control-normal-style' onmouseover='swapImage(\"changeYear\",\"drop2.gif\");this.className=\"title-control-select-style\";window.status=\""+selectYearMessage+"\"'	onmouseout='swapImage(\"changeYear\",\"drop1.gif\");this.className=\"title-control-normal-style\";window.status=\"\"'	onclick='popUpYear()'></span>&nbsp;"
			
			document.getElementById("caption").innerHTML  =	sHTML1

			bPageLoaded=true
		}
	}

	function hideCalendar()	{
		if(crossobj){
			crossobj.visibility="hidden"
			if (crossMonthObj != null){crossMonthObj.visibility="hidden"}
			if (crossYearObj !=	null){crossYearObj.visibility="hidden"}

            // added by leander 
            if( ie ) 
               removeDivFrame( "my_Hide4Ie6Buga_XXDDDD" )            
            // end added by leander
			showElement( 'SELECT' );
			showElement( 'APPLET' );
		}
	}
	

	function padZero(num) {
		return (num	< 10)? '0' + num : num ;
	}

	function constructDate(d,m,y)
	{
		sTmp = dateFormat
		//date
		sTmp = sTmp.replace	("dd","<e>")
		sTmp = sTmp.replace	("d","<d>")
		sTmp = sTmp.replace	("<e>",padZero(d))
		sTmp = sTmp.replace	("<d>",d)
        //month
		sTmp = sTmp.replace	("MMM","<o>")
		sTmp = sTmp.replace	("MM","<n>")
		sTmp = sTmp.replace	("M","<m>")
		sTmp = sTmp.replace	("<m>",m+1)
		sTmp = sTmp.replace	("<n>",padZero(m+1))
		sTmp = sTmp.replace	("<o>",monthName[m])
		//year
		return sTmp.replace ("yyyy",y)
	}

	function closeCalendar() {
		var	sTmp

		hideCalendar();
		ctlToPlaceValue.value =	constructDate(dateSelected,monthSelected,yearSelected)
		
	}

	/*** Month Pulldown	***/

	function StartDecMonth()
	{
		intervalID1=setInterval("decMonth()",80)
	}

	function StartIncMonth()
	{
		intervalID1=setInterval("incMonth()",80)
	}

	function incMonth () {
		monthSelected++
		if (monthSelected>11) {
			monthSelected=0
			yearSelected++
		}
		constructCalendar()
	}

	function decMonth () {
		monthSelected--
		if (monthSelected<0) {
			monthSelected=11
			yearSelected--
		}
		constructCalendar()
	}

	function constructMonth() {
		popDownYear()
		if (!monthConstructed) {
			sHTML =	""
			for	(i=0; i<12;	i++) {
				sName =	monthName[i];
				if (i==monthSelected){
					sName =	"<B>" +	sName +	"</B>"
				}
				sHTML += "<tr><td id='m" + i + "' onmouseover='this.className=\"dropdown-select-style\"' onmouseout='this.className=\"dropdown-normal-style\"' onclick='monthConstructed=false;monthSelected=" + i + ";constructCalendar();popDownMonth();event.cancelBubble=true'>&nbsp;" + sName + "&nbsp;</td></tr>"
			}

			document.getElementById("selectMonth").innerHTML = "<table border='0px' width='50px' class='dropdown-style' cellpadding='0px' cellspacing='0px' onmouseover='clearTimeout(timeoutID1)'	onmouseout='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"popDownMonth()\",100);event.cancelBubble=true'>" +	sHTML +	"</table>"

			monthConstructed=true
		}
	}

	function popUpMonth() {
		constructMonth()
		crossMonthObj.visibility = (dom||ie)? "visible"	: "show"
		crossMonthObj.left = (parseInt(crossobj.left) + 50) + "px"
		crossMonthObj.top =	(parseInt(crossobj.top) + 26) + "px"
	}

	function popDownMonth()	{
		crossMonthObj.visibility= "hidden"
	}

	/*** Year Pulldown ***/

	function incYear() {
		for	(i=0; i<7; i++){
			newYear	= (i+nStartingYear)+1
			if (newYear==yearSelected)
			{ txtYear =	"&nbsp;<B>"	+ newYear +	"</B>&nbsp;" }
			else
			{ txtYear =	"&nbsp;" + newYear + "&nbsp;" }
			document.getElementById("y"+i).innerHTML = txtYear
		}
		nStartingYear ++;
		bShow=true
	}

	function decYear() {
		for	(i=0; i<7; i++){
			newYear	= (i+nStartingYear)-1
			if (newYear==yearSelected)
			{ txtYear =	"&nbsp;<B>"	+ newYear +	"</B>&nbsp;" }
			else
			{ txtYear =	"&nbsp;" + newYear + "&nbsp;" }
			document.getElementById("y"+i).innerHTML = txtYear
		}
		nStartingYear --;
		bShow=true
	}

	function selectYear(nYear) {
		yearSelected=parseInt(nYear+nStartingYear);
		yearConstructed=false;
		constructCalendar();
		popDownYear();
	}

	function constructYear() {
		popDownMonth()
		sHTML =	""
		if (!yearConstructed) {

			sHTML =	"<tr><td align='center'	onmouseover='this.className=\"dropdown-select-style\"' onmouseout='clearInterval(intervalID1);this.className=\"dropdown-normal-style\"' onmousedown='clearInterval(intervalID1);intervalID1=setInterval(\"decYear()\",30)' onmouseup='clearInterval(intervalID1)'>-</td></tr>"
			j =	0
			nStartingYear =	yearSelected-3
			for	(i=(yearSelected-3); i<=(yearSelected+3); i++) {
				sName =	i;
				if (i==yearSelected){
					sName =	"<B>" +	sName +	"</B>"
				}

				sHTML += "<tr><td id='y" + j + "' onmouseover='this.className=\"dropdown-select-style\"' onmouseout='this.className=\"dropdown-normal-style\"' onclick='selectYear("+j+");event.cancelBubble=true'>&nbsp;" + sName + "&nbsp;</td></tr>"
				j ++;
			}

			sHTML += "<tr><td align='center' onmouseover='this.className=\"dropdown-select-style\"' onmouseout='clearInterval(intervalID2);this.className=\"dropdown-normal-style\"' onmousedown='clearInterval(intervalID2);intervalID2=setInterval(\"incYear()\",30)'	onmouseup='clearInterval(intervalID2)'>+</td></tr>"

			document.getElementById("selectYear").innerHTML	= "<table width=44 class='dropdown-style' cellpadding='0px' cellspacing='0px' onmouseover='clearTimeout(timeoutID2)' onmouseout='clearTimeout(timeoutID2);timeoutID2=setTimeout(\"popDownYear()\",100)' cellspacing=0>"	+ sHTML	+ "</table>"

			yearConstructed	= true
		}
	}

	function popDownYear() {
		clearInterval(intervalID1)
		clearTimeout(timeoutID1)
		clearInterval(intervalID2)
		clearTimeout(timeoutID2)
		crossYearObj.visibility= "hidden"
	}

	function popUpYear() {
		var	leftOffset

		constructYear()
		crossYearObj.visibility	= (dom||ie)? "visible" : "show"
		leftOffset = parseInt(crossobj.left) + document.getElementById("spanYear").offsetLeft
		if (ie)
		{
			leftOffset += 6
		}
		crossYearObj.left =	leftOffset + "px"
		crossYearObj.top = (parseInt(crossobj.top) +	26 ) + "px"
	}

	/*** calendar ***/

	function WeekNbr(today)
    {
		Year = takeYear(today);
		Month = today.getMonth();
		Day = today.getDate();
		now = Date.UTC(Year,Month,Day+1,0,0,0);
		var Firstday = new Date();
		Firstday.setYear(Year);
		Firstday.setMonth(0);
		Firstday.setDate(1);
		then = Date.UTC(Year,0,1,0,0,0);
		var Compensation = Firstday.getDay();
		if (Compensation > 3) Compensation -= 4;
		else Compensation += 3;
		NumberOfWeek =  Math.round((((now-then)/86400000)+Compensation)/7);
		return NumberOfWeek;
	}

	function takeYear(theDate)
	{
		x = theDate.getYear();
		var y = x % 100;
		y += (y < 38) ? 2000 : 1900;
		return y;
	}

	function constructCalendar () {
		var dateMessage
		var	startDate =	new	Date (yearSelected,monthSelected,1)
		var	endDate	= new Date (yearSelected,monthSelected+1,1);
		endDate	= new Date (endDate	- (24*60*60*1000));
		numDaysInMonth = endDate.getDate()

		datePointer	= 0
		dayPointer = startDate.getDay() - startAt
		
		if (dayPointer<0)
		{
			dayPointer = 6
		}

		sHTML =	"<table	border='0px' cellpadding='1px' cellspacing='0px' class='body-style'><tr>"

		if (showWeekNumber==1)
		{
			sHTML += "<td width='25px' ><b>" + weekString + "</b></td><td width=1px rowspan=7 class='weeknumber-div-style'><img src='"+imgDir+"divider.gif' width=1px></td>"
		}

		for	(i=0; i<7; i++)	{
			sHTML += "<td width='25px'  heigh='15px' align='right'><B>"+ dayName[i]+"</B></td>"
		}
		sHTML +="</tr><tr>"
		
		if (showWeekNumber==1)
		{
			sHTML += "<td align=right>" + WeekNbr(startDate) + "&nbsp;</td>"
		}

		for	( var i=1; i<=dayPointer;i++ )
		{
			sHTML += "<td>&nbsp;</td>"
		}
	
		for	( datePointer=1; datePointer<=numDaysInMonth; datePointer++ )
		{
			dayPointer++;
			sHTML += "<td align='right' heigh='15px'>"

			var sStyle="normal-day-style"; //regular day

			if ((datePointer==dateNow)&&(monthSelected==monthNow)&&(yearSelected==yearNow)) //today
			{ sStyle = "current-day-style"; } 
			else if	(dayPointer % 7 == (startAt * -1) +1) //end-of-the-week day
			{ sStyle = "end-of-weekday-style"; }

			//selected day
			if ((datePointer==odateSelected) &&	(monthSelected==omonthSelected)	&& (yearSelected==oyearSelected))
			{ sStyle += " selected-day-style"; }

			sHint = ""
			for (k=0;k<HolidaysCounter;k++)
			{
				if ((parseInt(Holidays[k].d)==datePointer)&&(parseInt(Holidays[k].m)==(monthSelected+1)))
				{
					if ((parseInt(Holidays[k].y)==0)||((parseInt(Holidays[k].y)==yearSelected)&&(parseInt(Holidays[k].y)!=0)))
					{
						sStyle += " holiday-style";
						sHint+=sHint==""?Holidays[k].desc:"\n"+Holidays[k].desc
					}
				}
			}

			var regexp= /\"/g
			sHint=sHint.replace(regexp,"&quot;")

			dateMessage = "onmousemove='window.status=\""+selectDateMessage.replace("[date]",constructDate(datePointer,monthSelected,yearSelected))+"\"' onmouseout='window.status=\"\"' "

			sHTML += "<a class='"+sStyle+"' "+dateMessage+" title=\"" + sHint + "\" href='javascript:dateSelected="+datePointer+";closeCalendar();doOtherFunctions();'>&nbsp;" + datePointer + "&nbsp;</a>"

			sHTML += ""
			if ((dayPointer+startAt) % 7 == startAt) { 
				sHTML += "</tr><tr>" 
				if ((showWeekNumber==1)&&(datePointer<numDaysInMonth))
				{
					sHTML += "<td align=right>" + (WeekNbr(new Date(yearSelected,monthSelected,datePointer+1))) + "&nbsp;</td>"
				}
			}
		}

		document.getElementById("content").innerHTML   = sHTML
		document.getElementById("spanMonth").innerHTML = "&nbsp;" +	monthName[monthSelected] + "&nbsp;<IMG id='changeMonth' SRC='"+imgDir+"drop1.gif' WIDTH='12px' HEIGHT='10' BORDER=0>"
		document.getElementById("spanYear").innerHTML =	"&nbsp;" + yearSelected	+ "&nbsp;<IMG id='changeYear' SRC='"+imgDir+"drop1.gif' WIDTH='12' HEIGHT='10' BORDER=0>"
	}
	
	function doOtherFunctions(){
		//Comments By Sai
		//if(oFunctions != ""){
		//	eval(oFunctions)
		//}
		
		
	}

	function popUpCalendar(ctl,	ctl2, format, OtherFunctions) {
		var	leftpos=0
		var	toppos=0
		oFunctions = OtherFunctions
		if (bPageLoaded)
		{		
			if ( crossobj.visibility ==	"hidden" ) {
				ctlToPlaceValue	= ctl2
				dateFormat=format;

				formatChar = " "
				aFormat	= dateFormat.split(formatChar)
				if (aFormat.length<3)
				{
					formatChar = "/"
					aFormat	= dateFormat.split(formatChar)
					if (aFormat.length<3)
					{
						formatChar = "."
						aFormat	= dateFormat.split(formatChar)
						if (aFormat.length<3)
						{
							formatChar = "-"
							aFormat	= dateFormat.split(formatChar)
							if (aFormat.length<3)
							{
								// invalid date	format
								formatChar=""
							}
						}
					}
				}

				tokensChanged =	0
				if ( formatChar	!= "" )
				{
					// use user's date
					aData =	ctl2.value.split(formatChar)

					for	(i=0;i<3;i++)
					{
						if ((aFormat[i]=="d") || (aFormat[i]=="dd"))
						{
							dateSelected = parseInt(aData[i], 10)
							tokensChanged ++
						}
						else if	((aFormat[i]=="m") || (aFormat[i]=="mm"))
						{
							monthSelected =	parseInt(aData[i], 10) - 1
							tokensChanged ++
						}
						else if	(aFormat[i]=="yyyy")
						{
							yearSelected = parseInt(aData[i], 10)
							tokensChanged ++
						}
						else if	(aFormat[i]=="mmm")
						{
							for	(j=0; j<12;	j++)
							{
								if (aData[i]==monthName[j])
								{
									monthSelected=j
									tokensChanged ++
								}
							}
						}
					}
				}

				if ((tokensChanged!=3)||isNaN(dateSelected)||isNaN(monthSelected)||isNaN(yearSelected))
				{
					dateSelected = dateNow
					monthSelected =	monthNow
					yearSelected = yearNow
				}

				odateSelected=dateSelected
				omonthSelected=monthSelected
				oyearSelected=yearSelected

				aTag = ctl
				if( aTag != null ){
				    do {					
    					
					    aTag = aTag.offsetParent;
    					
					    if( aTag == null ) break;
    					
					    leftpos	+= aTag.offsetLeft;
					    toppos += aTag.offsetTop;
				    } while(aTag != null && aTag.tagName!="BODY");
                }
                
				// added by leander   
				var my_temp_width = (showWeekNumber==1)?width_with_WeekNumber:width_non_WeekNumber;
                var myFixedLeft = fixedX==-1 ? ctl.offsetLeft	+ leftpos :	fixedX;
                if( my_temp_width + myFixedLeft > document.body.clientWidth ){
                    myFixedLeft = myFixedLeft - my_temp_width;
                }
                
				//crossobj.left =	(fixedX==-1 ? ctl.offsetLeft	+ leftpos :	fixedX) + "px"
				crossobj.left = myFixedLeft + "px";
				crossobj.top = (fixedY==-1 ?	ctl.offsetTop +	toppos + ctl.offsetHeight +	2 :	fixedY)  + "px"
				
				
				constructCalendar (1, monthSelected, yearSelected);
				crossobj.visibility=(dom||ie)? "visible" : "show"
						  
				 // added by leander 
                 //if(navigator.appVersion.match(/7./i)=='7.')     
                if( ie && !(navigator.appVersion.match(/MSIE 7./i)=='MSIE 7.') )                       
				    hideElement( 'SELECT', document.getElementById("calendar") );
				//hideElement( 'APPLET', document.getElementById("calendar") );			

				bShow = true;
			}
		}
		else
		{
			init()
			popUpCalendar(ctl,	ctl2, format, OtherFunctions)
		}
	}
	
	document.onkeypress = function hidecal1 () { 
	    if(window.event)
	    {
		    if (event.keyCode==27 ) 
		    {
			    hideCalendar()
		    }	    
	    }

	}
	document.onclick = function hidecal2 () { 		
		if (!bShow)
		{
			hideCalendar()
		}
		bShow = false
	}

	//window.onload=init()
	
	// added by leander 	
	function isDateFormat(op, formatString){
		formatString = formatString || "ymd";
		var m, year, month, day;
		
		switch(formatString){
			case "ymd" :
				m = op.match(new RegExp("^((\\d{4})|(\\d{2}))([-./])(\\d{1,2})\\4(\\d{1,2})$"));
				if(m == null ) return false;
				day = m[6];
				month = m[5]--;
				year =  (m[2].length == 4) ? m[2] : GetFullYear(parseInt(m[3], 10));
				break;
			case "dmy" :
				m = op.match(new RegExp("^(\\d{1,2})([-./])(\\d{1,2})\\2((\\d{4})|(\\d{2}))$"));
				if(m == null ) return false;
				day = m[1];
				month = m[3]--;
				year = (m[5].length == 4) ? m[5] : GetFullYear(parseInt(m[6], 10));
				break;
			default :
				break;
		}
		if(!parseInt(month)) return false;
		month = month==12 ?0:month;
		var date = new Date(year, month, day);
//        if(parseInt(year,10)<1753&&year.length==4) return false;
        return (typeof(date) == "object" && year == date.getFullYear() && month == date.getMonth() && day == date.getDate());
		function GetFullYear(y){return ((y<30 ? "20" : "19") + y)|0;}
	}
	
	function checkedDateOnLeave( ctl ){
	    var input = ctl.value.replace(/(^\s*)/,"").replace(/(\s*$)/,"");
	    ctl.value = input;
	    if( input != "" ){
	        if ( isDateFormat(input,"ymd") == false ){	 
	            alert("Invalid date format!");           
	            ctl.focus();
	            ctl.select();	            
	        }
	    }
	}
	
	function getRootPath1(){
        var strFullPath=window.document.location.href;
        var strPath=window.document.location.pathname;
        var pos=strFullPath.indexOf(strPath);
        var prePath=strFullPath.substring(0,pos);
        var postPath=strPath.substring(0,strPath.substr(1).indexOf('/')+1);
        return(prePath+postPath);
    }
    function getRootPath(){
        var strFullPath=parent.location.href;
        var postPath=strFullPath.substring(0,strFullPath.lastIndexOf('/')+1);
        return(postPath);
    }
	
	function removeDivFrame( iframeId ){
		return;
	    !iframeId && (iframeId = 'Hide4Ie6Buga');    //默认 iframe 的 id
        var iframe_dom = document.getElementById(iframeId);
        if( iframe_dom == null ) return;
        
        document.body.removeChild( iframe_dom );
        
	}
	
	function topDiv( divId, iframeId ){	
	    return;
	    
        var div_dom = document.getElementById(divId);
        !iframeId && (iframeId = 'Hide4Ie6Buga');    //默认 iframe 的 id
        var iframe_dom = document.getElementById(iframeId);
        if(!iframe_dom)    //不存在 自动生成 iframe
        {
             var tmpIframeDom    = document.createElement("IFRAME");
            tmpIframeDom.id        = iframeId;
            document.body.appendChild(tmpIframeDom);
            iframe_dom = document.getElementById(iframeId);
            iframe_dom.src    = "javascript:void(0)";    //javascript:void(0);  about:blank
            iframe_dom.style.display = "block";//none
            iframe_dom.style.position = "absolute";
            iframe_dom.style.scrolling = "no";
            iframe_dom.style.frameBorder = 1;
            
    //        iframe_dom.style.backgroundColor = "#ff0000";
           // iframe_dom.setAttribute("style","position:absolute; top:0px; left:0px; "); //display:none;
        }
        //使iframe 处于 指定的 div 下面
        iframe_dom.style.width = div_dom.offsetWidth;
        iframe_dom.style.height = div_dom.offsetHeight;
        iframe_dom.style.top = div_dom.style.top;
        iframe_dom.style.left = div_dom.style.left;
        iframe_dom.style.zIndex = div_dom.style.zIndex - 1;        
        //iframe_dom.style.display = "block";
    }
    
    function processTextBoxKeyDown( inputId,imageId ){
        try{           
            if( window.event.keyCode == 40 ){
                popUpCalendar(document.getElementById(imageId), document.getElementById(inputId), "m/dd/yyyy", "__doPostBack(\'" & inputId & "\')");
            }else if( window.event.keyCode == 38 ){ 
                hideCalendar()
            }
        } 
        catch(e) {  }        
    }
   // change format  
   Date.prototype.toString= function()
   {  
        var varmonth=  this.getMonth()+1;
        if (parseInt(varmonth)<10)
        {
            varmonth="0"+varmonth;
        }
        var varday=this.getDate();
        if(parseInt(varday,10)<10)
        {
            varday="0"+varday;
        }
       return   varday+"/" +varmonth+"/"+this.getFullYear();   
   }   

//日期格式MM/dd/yyyy  
function validDateMMDDYYYY(which)
{
   var reg = /^(\d{2})(\/)(\d{2})\2(\d{4})$/;
   var matchArray = which.match(reg);
   if (matchArray == null)
   {
   
        return false;
       
   }
   else
   {
       var day = matchArray[1];
       var month = matchArray[3];
       var  year= matchArray[4];
       
       month--;
       var dt=new Date(year,month,day);    
       if(dt!=undefined&&year==dt.getFullYear()&&month==dt.getMonth()&&day==dt.getDate())
       return true;
       else
       return false;     
   } 
}

function validateDate(which) 
{

    var dds=document.getElementById(which).value;
   
    if(dds.length!=0)
    {
        if(!validDateMMDDYYYY(dds))
        {
            alert("Date Format is dd/MM/yyyy");
            document.getElementById(which).value="";
            document.getElementById(which).focus();
            document.getElementById(which).select();
            return false;
        }
    }
    return true;
}
