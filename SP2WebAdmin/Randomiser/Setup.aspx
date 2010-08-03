<%@ Page Language="C#" Inherits="SP2WebAdmin.Randomiser.Setup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
	<title>The Randomiser : Setup</title>
	<style type="text/css">
		table { width: 100%; }
		textarea { width: 100%; height: 400px; }
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<h1>The Randomiser - Setup</h1>
		<p>
			<a href="/">Return to main</a> | <a href="/Randomiser/">Start the Randomiser</a> | <asp:Button id="btnCommit" Text="Commit Changes" OnClick="btnCommit_Click" runat="server"/>
		</p>
		<table>
			<tr>
				<th>Participants</th>
				<th>Absentees</th>
				<th>Specials</th>
			</tr>
			
			<tr>
				<td>
					<asp:TextBox id="tbParticipants" runat="server" TextMode="MultiLine"/>
				</td>
				<td>
					<asp:TextBox id="tbAbsentees" runat="server" TextMode="MultiLine"/>
				</td>
				<td>
					<asp:TextBox id="tbSpecials" runat="server" TextMode="MultiLine"/>
				</td>
			
			</tr>
		
		</table>
	</form>
</body>
</html>