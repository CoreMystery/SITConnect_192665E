<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SITConnect.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="text-align: center;">

        <asp:Panel ID="ProfilePanel" Visible="false" runat="server" CssClass="alert alert-success">
            <asp:Label ID="lbFlashMessage" runat="server"></asp:Label>
        </asp:Panel>

        <div class="panel panel-default">
            <div class="panel-heading">
                <h2 class="panel-title" style="text-align: center; font-weight: bold">Account</h2>
            </div>

            <div class="panel-body">
                <div class="row">
                    <div class="col-md-6">
                        <h3>First Name</h3>
                        <asp:Label ID="lbFirstName" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-6">
                        <h3>Last Name</h3>
                        <asp:Label ID="lbLastName" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <h3>Email</h3>
                        <asp:Label ID="lbEmail" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-6">
                        <h3>Date Of Birth</h3>
                        <asp:Label ID="lbDateOfBirth" runat="server"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="panel-footer" style="text-align: center; margin: auto">
                <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn btn-primary" OnClick="btnChangePassword_Click" />
            </div>
        </div>
    </div>

</asp:Content>
