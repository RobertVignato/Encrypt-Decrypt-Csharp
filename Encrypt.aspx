<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Encrypt.aspx.cs" Inherits="WordSoapWF01.Encrypt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
function CopyToClipboard()   
{
    var textboxVal = document.getElementById("ContentPlaceHolder1_tbxResult").value; //txtText id of a textbox

    if (window.clipboardData && clipboardData.setData) {
        clipboardData.setData("Text", textboxVal);
        document.getElementById('ContentPlaceHolder1_tbxResult').className = "pasteGreenBkg";
    }
    else {
        alert("works only in IE4+");
    }   
}
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<br />

<div align="center">

<img alt="" src="Graphics/Scramble.gif" width="142px" height="35px" /><br />
            <span class="topicHeaderRed">
                <asp:Label ID="lblInfo" runat="server" />
            </span>

            <br />

<table width="90%">
    <tr>
        <td align="left">
            
            <span class="topicHeader">
                Step 1)&nbsp;-&nbsp;Type in (or paste) your message</span>&nbsp;
            <span class="topicHeaderRed">
                <asp:Label ID="lblTextEntry" runat="server" />
            </span>

            <br />
            <br />

            <asp:TextBox ID="tbxTextEntry" runat="server" Height="200px" 
                TextMode="MultiLine" Width="100%" CssClass="textbox" />
                
            <br />
            <br />

            <span class="topicHeader">
                Step 2)&nbsp;-&nbsp;Enter a Keyword<br />
            </span>

            <br />

            <div class="smallItalic">
                <span class="Red">*</span> This is a secret Keyword you create and will share
                <span class="style2">verbally</span> with the person who will receive and 
                un-scramble this message.<br />
                <span class="Red">*</span> Never Email or Text your Keyword!
            </div>
            
            <br />
            <asp:TextBox ID="tbxKeyword" runat="server" Width="20%" CssClass="textbox" />
            &nbsp;
            <span class="topicHeaderRed">
                <asp:Label ID="lblKeyword" runat="server" />
            </span>

            <br />
            <br />

            <span class="topicHeader">
            Step 3)&nbsp;-&nbsp;Click&nbsp;
            <asp:Button ID="btnScramble" runat="server" Text="Scramble" onclick="btnScramble_Click" />
            </span>
            &nbsp;
            <span class="topicHeaderRed">
                <asp:Label ID="lblScramble" runat="server" />
            </span> 
             
            <br />
            <br />

            <asp:TextBox ID="tbxResult" runat="server" Width="100%" Height="50px" 
                TextMode="MultiLine" CssClass="textbox" />
            
            <br />
            <br />
            
            <span class="topicHeader">
            Step 4)&nbsp;-&nbsp;Click&nbsp;&nbsp;<input type="button" value="Copy" onclick="CopyToClipboard();">
            to copy all of the Scrambled text on to your Clipboard</span>
            
                   
            <span class="topicHeaderRed">
                <asp:Label ID="lblCopy" runat="server" />
            </span>

            
            
            <br />
            <br />

            <div class="topicHeader">
            Step 5)&nbsp;-&nbsp;Go to your Email (or Instant Messaging program), paste in the Scrambled text, then send your message.
            </div>

            <br />
            <br />


        </td>
    </tr>
</table>

</div>

</asp:Content>
