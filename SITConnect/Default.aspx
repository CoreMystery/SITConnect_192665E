<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SITConnect._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="text-align: center;">
        <h1>SITCONNECT</h1>
        <% if(Session["UserID"] != null) {%>
        <% } else { %>
        <p><a href="/Login" class="btn btn-primary btn-lg">Login</a> <a href="/Register" class="btn btn-primary btn-lg">Register</a></p>
        <%}%>
    </div>

</asp:Content>
