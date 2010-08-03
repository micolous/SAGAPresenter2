<%@ Page Language="C#" Inherits="SP2WebViewer.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
	<title>SAGAPresenter2 Web Viewer</title>
	<script type="text/javascript" src="media/script.js"></script>
	<link rel="stylesheet" href="media/style.css" type="text/css"/>
	<meta http-equiv="refresh" content="120" />
</head>
<body>
	<!-- 
		form is not required for this page because there are no postbacks.
		it just adds extra bloat.  so it is culled.
	-->
	<table class="h">
		<tr>
			<td><h1><asp:Label id="lblEventName" runat="server" OnLoad="lblEventName_Load" /></h1></td>
			<td id="clock"></td>
		</tr>
	</table>
	<asp:ObjectDataSource id="odsCompetitions" runat="server" TypeName="SAGAPresenter.libpresenterd.CompetitionsDataObject"
	 SelectMethod="SelectOnlyCurrent">
	 	<UpdateParameters>
	 	</UpdateParameters>
	</asp:ObjectDataSource>
	<asp:GridView id="gvCompetitions" runat="server" AutoGenerateColumns="false" DataSourceID="odsCompetitions"
		ShowFooter="false" EmptyDataText="No results!" Width="100%" ShowHeader="false" CssClass="ct">
		<Columns>
			<asp:BoundField DataField="Start" ItemStyle-CssClass="time" DataFormatString="{0:HH}:{0:mm}" />
			<asp:TemplateField>
				<ItemTemplate>
					<asp:Label id="lblName" CssClass="gamename" text='<%# Eval("Name") %>' runat="server" />
					<asp:Label id="lblPlatforms" CssClass="consolelogos" text='<%# Eval("Platforms") %>' runat="server"/>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField DataField="StartsFinishesInString" ItemStyle-CssClass="sfis"/>						
		</Columns>
	</asp:GridView>
	<marquee>
		<asp:Label id="lblMarquee" runat="server" />
	</marquee>
</body>
</html>