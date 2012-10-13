<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextArea.ascx.cs" Inherits="Terry.CRM.Web.UserControl.TextArea" %>

<textarea id="mytext" style="width:750px; height:80px;"  runat="server"
onkeydown="limitChars(this)" 
onchange="limitChars(this)" 
onpropertychange="limitChars(this)">
</textarea>
<script type="text/javascript">
/**
* 限制textarea文本域输入的字符个数
*/
function limitChars(obj){
        var count =<%= MaxLength %>;
	    if (obj.value.length > count){
	        obj.value = obj.value.substr(0, count);
	    }
	}
</script>