<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Decrypt.aspx.cs" Inherits="WordSoapWF01.Decrypt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<br />

<div align="center">

<img alt="Unscramble" src="Graphics/Unscramble.gif" width="198px" height="35px" /><br />
            <span class="topicHeaderRed">
                <asp:Label ID="lblInfo" runat="server" />
            </span>
    <br />

<table width="90%">
    <tr>
        <td align="left">
            
            <span class="topicHeader">
                Step 1)&nbsp;-&nbsp;Paste in the Scrambled message text you received in your Email (or Instant Message)</span>&nbsp;
            <span class="topicHeaderRed">
                <asp:Label ID="lblTextEntry" runat="server" />
            </span>

            <br />
            <br />

            <asp:TextBox ID="tbxTextEntry" runat="server" Height="50px" 
                TextMode="MultiLine" Width="100%" CssClass="textbox" />
                
            <br />
            <br />

            <span class="topicHeader">
                Step 2)&nbsp;-&nbsp;Enter the Keyword that was used to Scramble the message<br />
            </span>

            <br />

            <div class="smallItalic">
                <span class="Red">*</span> This is the secret Keyword that was used to Scramble 
                the message and was given to you by the person who created this message.
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
            <asp:Button ID="btnScramble" runat="server" Text="Unscramble" onclick="btnUnscramble_Click" />
            </span>
            &nbsp;
            <span class="topicHeaderRed">
                <asp:Label ID="lblScramble" runat="server" />
            </span> 
             
            <br />
            <br />

            <asp:TextBox ID="tbxResult" runat="server" Width="100%" Height="900px" 
                TextMode="MultiLine" CssClass="textbox" />
            
            <br />
            <br />

        </td>
    </tr>
</table>

</div>

</asp:Content>
