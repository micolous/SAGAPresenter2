<%@ Page Language="C#" Inherits="SP2WebAdmin.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
	<title>SAGAPresenter2 Web Administration</title>
</head>
<body>
	<form id="form1" runat="server" >
		<h1>SAGAPresenter2 Web Administration</h1>
		<ul>
			<li><a href="Variables.aspx">Setup Variables</a></li>
			<li><a href="Competitions.aspx">Setup Competitions</a></li>
			<li><a href="/Randomiser/Setup.aspx">Setup Participants</a></li>
			<li><a href="/Randomiser/Default.aspx">Run The Randomiser</a></li>
		</ul>
		
		
		<h2>Marquee</h2>
		<p><asp:TextBox id="txtMarquee" runat="server" Style="width:100%"/></p>
		<p><asp:Button id="btnMarqueeCommit" runat="server" Text="Commit" OnClick="btnMarqueeCommit_Click" /></p>
		

		
		<h2>Notepad</h2>
		<div>
		<asp:TextBox id="txtNotepad" runat="server" TextMode="Multiline" Style="width:100%;height:20em"/>
		</div>
		
		<p>
			<asp:DropDownList id="ddNotepadDuration" runat="server">
				<asp:ListItem value="60">1 minute</asp:ListItem>
				<asp:ListItem value="120">2 minutes</asp:ListItem>
				<asp:ListItem value="180">3 minutes</asp:ListItem>
				<asp:ListItem value="240">4 minutes</asp:ListItem>
				<asp:ListItem value="300" Selected="true">5 minutes</asp:ListItem>
				<asp:ListItem value="360">6 minutes</asp:ListItem>
				<asp:ListItem value="420">7 minutes</asp:ListItem>
				<asp:ListItem value="480">8 minutes</asp:ListItem>
				<asp:ListItem value="540">9 minutes</asp:ListItem>
				<asp:ListItem value="600">10 minutes</asp:ListItem>
			</asp:DropDownList>
			<asp:Button id="btnNotepadCommit" runat="server" Text="Show Notepad" OnClick="btnNotepadCommit_Click"/>
			<asp:Button id="btnNotepadHide" runat="server" Text="Hide Notepad" OnClick="btnNotepadHide_Click"/>
		</p>
			
			
	</form>
</body>
</html>