<%@ Page Language="C#" Inherits="SP2WebAdmin.Randomiser.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
	<title>The Randomiser</title>
</head>
<body>
	<form id="form1" runat="server">
		<p>Winner: <asp:Label id="lblWinner" runat="server" /></p>
		<ul>
			<li><asp:Button id="btnMinorPrize" runat="server" Text="Minor Prize" OnClick="btnMinorPrize_Click" /></li>
			<li><asp:Button id="btnMajorPrize" runat="server" Text="Major Prize" OnClick="btnMajorPrize_Click" /></li>
			<li><asp:Button id="btnSpecialPrize" runat="server" Text="Special Prize" OnClick="btnSpecialPrize_Click" /></li>
			<li><asp:Button id="btnEndRandomiser" runat="server" Text="Close Randomiser" OnClick="btnEndRandomiser_Click" /></li>
		</ul>
		<p><a href="Setup.aspx">Setup Participants</a> | <a href="/">Return to main</a></p>
	</form>
</body>
</html>