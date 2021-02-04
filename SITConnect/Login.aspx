<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script src="https://www.google.com/recaptcha/api.js?render=<%= ConfigurationManager.AppSettings["RECAPTCHA_SITE_KEY"].ToString() %>"></script>

    <style>
        label {
            margin-top: 10px;
        }

        input {
            margin-bottom: 10px;
            max-width: 1000px;
        }
    </style>

    <div class="container" style="padding: 25px; width: 75%">

        <asp:Panel ID="LoginPanel" Visible="false" runat="server" CssClass="alert alert-success">
            <asp:Label ID="lbFlashMessage" runat="server"></asp:Label>
        </asp:Panel>

        <div class="panel panel-default">
            <div class="panel-heading">
                <h2 class="panel-title" style="text-align: center; font-weight: bold"><%: Title %></h2>
            </div>

            <div class="panel-body" style="padding-left: 26%">
                <label for="tbEmail">Email</label><br />
                <asp:TextBox ID="tbEmail" runat="server" Width="435px" placeholder="Enter Email" TextMode="Email"></asp:TextBox><br />
                <asp:Label ID="lbEmail" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                <br />
                <label for="tbPassword">Password</label><br />
                <asp:TextBox ID="tbPassword" runat="server" Width="435px" placeholder="Enter Password" TextMode="Password"></asp:TextBox><br />
                <asp:Label ID="lbPassword" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                <br />
                <br />
                <asp:Label ID="errorMsg" runat="server" ForeColor="Red"></asp:Label>
            </div>

            <div class="panel-footer" style="text-align: center; margin: auto">
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
                <button runat="server" onserverclick="btnLogin_Click" onclick="if (!validation()) return false;" class="btn btn-primary">Login</button>
            </div>
        </div>
    </div>

    <script>
        function validation() {
            var validation = true
            if (document.getElementById('<%=tbEmail.ClientID%>').value == "") {
                document.getElementById('<%=lbEmail.ClientID%>').innerHTML = "Email cannot be empty.";
                validation = false;
            }
            else {
                if (!document.getElementById('<%=tbEmail.ClientID%>').value.match(/^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/)) {
                    document.getElementById('<%=lbEmail.ClientID%>').innerHTML = "Email is in the wrong format. (E.g. SIT@mail.com)";
                    validation = false;
                }
                else {
                    document.getElementById('<%=lbEmail.ClientID%>').innerHTML = "";
                }
            }
            if (document.getElementById('<%=tbPassword.ClientID%>').value == "") {
                document.getElementById('<%=lbPassword.ClientID%>').innerHTML = "Password cannot be empty.";
                validation = false;
            }
            else {
                document.getElementById('<%=lbPassword.ClientID%>').innerHTML = "";
            }
            return validation;
        }


        var duration = "<%=countdown%>";
        if (duration != "")
            startTimer(parseInt(duration) -1);
        function startTimer(duration) {
            var timer = duration, minutes, seconds;
            var countdown = setInterval(function () {
                minutes = parseInt(timer / 60, 10);
                seconds = parseInt(timer % 60, 10);

                minutes = minutes < 10 ? "0" + minutes : minutes;
                seconds = seconds < 10 ? "0" + seconds : seconds;

                document.getElementById("<%=errorMsg.ClientID%>").innerHTML = `Too many failed login attempts. Try again in ${minutes}:${seconds}.`;

                if (--timer < 0) {
                    timer = duration;
                    clearInterval(countdown)
                    document.getElementById("<%=errorMsg.ClientID%>").style.color = "green";
                    document.getElementById("<%=errorMsg.ClientID%>").innerHTML = "You may now login.";
                }
            }, 1000);
        }

        function reCaptcha() {
            grecaptcha.ready(function () {
                grecaptcha.execute('<%= ConfigurationManager.AppSettings["RECAPTCHA_SITE_KEY"].ToString() %>', { action: 'Login' }).then(function (token) {
                    document.getElementById("g-recaptcha-response").value = token;
                });
            });
        };
        reCaptcha();
        setInterval(function () { reCaptcha(); }, 90000);
    </script>

</asp:Content>
