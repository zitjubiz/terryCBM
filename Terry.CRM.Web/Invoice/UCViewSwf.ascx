<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCViewSwf.ascx.cs" Inherits="Terry.CRM.Web.UCViewSwf" %>

<style type="text/css" media="screen"> 
			html, body	{ height:100%; }
			body { margin:0; padding:0; overflow:auto; }   
			#flashContent { display:none; }
        </style> 
		
		<script type="text/javascript" src="../js/swfobject/swfobject.js"></script>
		<script type="text/javascript" src="../js/flexpaper_flash.js"></script>
		<script type="text/javascript"> 
            <!-- For version detection, set to min. required Flash Player version, or 0 (or 0.0.0), for no version detection. --> 
            var swfVersionStr = "10.0.0";
            <!-- To use express install, set to playerProductInstall.swf, otherwise the empty string. -->
            var xiSwfUrlStr = "playerProductInstall.swf";
            
            var flashvars = { 
                  SwfFile : escape("<%=FilePath %>"),
				  Scale : 0.6, 
				  ZoomTransition : "easeOut",
				  ZoomTime : 0.5,
  				  ZoomInterval : 0.1,
  				  FitPageOnLoad : false,
  				  FitWidthOnLoad : true,
  				  PrintEnabled : true,
  				  FullScreenAsMaxWindow : false,
  				  ProgressiveLoading : true,
  				  
  				  PrintToolsVisible : true,
  				  ViewModeToolsVisible : true,
  				  ZoomToolsVisible : true,
  				  FullScreenVisible : true,
  				  NavToolsVisible : true,
  				  CursorToolsVisible : true,
  				  SearchToolsVisible : true,
  				  
  				  localeChain: "zh_CN"
				  };
				  
			 var params = {
				
			    }
            params.quality = "high";
            params.bgcolor = "#ffffff";
            params.allowscriptaccess = "sameDomain";
            params.allowfullscreen = "true";
            params.wmode="Opaque" ;
            var attributes = {};
            attributes.id = "FlexPaperViewer";
            attributes.name = "FlexPaperViewer";
            swfobject.embedSWF(
                "FlexPaperViewer.swf", "flashContent", 
                "900", "300",
                swfVersionStr, xiSwfUrlStr, 
                flashvars, params, attributes);
			swfobject.createCSS("#flashContent", "display:block;text-align:left;");
        </script> 

 <div style="position:absolute;left:10px;top:10px;">
	        <div id="flashContent"> 
	        	<p> 
		        	To view this page ensure that Adobe Flash Player version 
					10.0.0 or greater is installed. 
				</p> 
				<script type="text/javascript">
				    var pageHost = ((document.location.protocol == "https:") ? "https://" : "http://");
				    document.write("<a href='http://www.adobe.com/go/getflashplayer'><img src='"
									+ pageHost + "www.adobe.com/images/shared/download_buttons/get_flash_player.gif' alt='Get Adobe Flash player' /></a>"); 
				</script> 
	        </div>	        
        </div>