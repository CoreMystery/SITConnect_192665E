<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SITConnect.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container" style="padding: 25px; width: 75%">

        <asp:Panel ID="PanelErrorResult" Visible="false" runat="server" CssClass="alert alert-danger">
            <asp:Label ID="lbError" runat="server"></asp:Label>
        </asp:Panel>

        <div class="panel panel-default">
            <div class="panel-heading">
                <h2 class="panel-title" style="text-align: center; font-weight: bold"><%: Title %></h2>
            </div>

            <div class="panel-body" style="padding-left: 30%">
                <label for="tbCurrentPassword">Current Password</label><br />
                <asp:TextBox ID="tbCurrentPassword" runat="server" Width="435px" placeholder="Enter Current Password" TextMode="Password"></asp:TextBox><br />
                <asp:Label ID="lbCurrentPassword" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                <br />
                <label for="tbPassword">New Password</label><br />
                <asp:TextBox ID="tbNewPassword" runat="server" Width="435px" placeholder="Enter New Password" TextMode="Password"></asp:TextBox><br />
                <asp:Label ID="lbNewPassword" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                <br />
                <label for="tbConfirmPassword">New Password Again</label><br />
                <asp:TextBox ID="tbConfirmPassword" runat="server" Width="435px" placeholder="Enter New Password Again" TextMode="Password"></asp:TextBox><br />
                <asp:Label ID="lbConfirmPassword" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                <br />
                <br />
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
                <asp:Label ID="errorMsg" runat="server" ForeColor="Red"></asp:Label>
            </div>

            <div class="panel-footer" style="text-align: center; margin: auto">
                <button runat="server" onserverclick="btnChangePassword_Click" onclick="if (!validation()) return false;" class="btn btn-primary">Change Password</button>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            $("#<%=tbNewPassword.ClientID%>").keyup(function () {
                var password = document.getElementById('<%=tbNewPassword.ClientID%>').value;
                document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML = "";
                if (password.length > 0) {
                    document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML = "";
                }
                if (!(password.length >= 8)) {
                    document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML += "Length should be at least 8 characters.<br>";
                }

                if (!password.match(/[a-z]+/)) {
                    document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML += "Should contain at least one lowercase character.<br>";
                }

                if (!password.match(/[A-Z]+/)) {
                    document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML += "Should contain at least one uppercase character.<br>";
                }

                if (!password.match(/\d+/)) {
                    document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML += "Should contain at least numeric number.<br>";
                }

                if (!password.match(/[!@#$%^&*]+/)) {
                    document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML += "Should contain at least one special character (!@#$%^&*).<br>";
                }
                if (document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML != "") {
                    document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML = "Complexity Requirements:<br>" + document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML;
                }
            });

            $('#<%=tbNewPassword.ClientID%>, #<%=tbConfirmPassword.ClientID%>').on('keyup', function () {
                if ($('#<%=tbNewPassword.ClientID%>').val() == $('#<%=tbConfirmPassword.ClientID%>').val())
                    $('#<%=lbConfirmPassword.ClientID%>').html('Password match.').css('color', 'green');
                else
                    $('#<%=lbConfirmPassword.ClientID%>').html('Password does not match.').css('color', 'red');
            });
            $('#<%=tbNewPassword.ClientID%>, #<%=tbConfirmPassword.ClientID%>').on('click', function () {
                if ($('#<%=tbNewPassword.ClientID%>').val() == $('#<%=tbConfirmPassword.ClientID%>').val())
                    $('#<%=lbConfirmPassword.ClientID%>').html('Password match.').css('color', 'green');
                else
                    $('#<%=lbConfirmPassword.ClientID%>').html('Password does not match.').css('color', 'red');
            });
        });

        function validation() {
            var validation = true;
            if (document.getElementById('<%=tbCurrentPassword.ClientID%>').value == "") {
                document.getElementById('<%=lbCurrentPassword.ClientID%>').innerHTML = "Current password cannot be empty.";
                validation = false;
            }
            else {
                document.getElementById('<%=lbCurrentPassword.ClientID%>').innerHTML = "";
            }
            if (document.getElementById('<%=tbNewPassword.ClientID%>').value == "") {
                document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML = "New password cannot be empty.";
                validation = false;
            }
            else {
                if (!passwordComplexity())
                    validation = false;

            }
            if (document.getElementById('<%=tbConfirmPassword.ClientID%>').value == "") {
                document.getElementById('<%=lbConfirmPassword.ClientID%>').style.color = "Red";
                document.getElementById('<%=lbConfirmPassword.ClientID%>').innerHTML = "Enter new password again to confirm.";
                validation = false;
            }
            else {
                if (document.getElementById('<%=tbNewPassword.ClientID%>').value != document.getElementById('<%=tbConfirmPassword.ClientID%>').value) {
                    document.getElementById('<%=lbConfirmPassword.ClientID%>').innerHTML = "Password does not match.";
                    validation = false;
                }
                else {
                    document.getElementById('<%=lbConfirmPassword.ClientID%>').innerHTML = "";
                }
            }
            return validation;
        }

        function passwordComplexity() {
            var validation = true;
            var password = document.getElementById('<%=tbNewPassword.ClientID%>').value;
            document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML = "";
            if (password.length > 0) {
                document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML = "";
            }
            if (!(password.length >= 8)) {
                document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML += "Length should be at least 8 characters.<br>";
                validation = false;
            }

            if (!password.match(/[a-z]+/)) {
                document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML += "Should contain at least one lowercase character.<br>";
                validation = false;
            }

            if (!password.match(/[A-Z]+/)) {
                document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML += "Should contain at least one uppercase character.<br>";
                validation = false;
            }

            if (!password.match(/\d+/)) {
                document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML += "Should contain at least numeric number.<br>";
                validation = false;
            }

            if (!password.match(/[!@#$%^&*]+/)) {
                document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML += "Should contain at least one special character (!@#$%^&*).<br>";
                validation = false;
            }
            if (document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML != "") {
                document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML = "Complexity Requirements:<br>" + document.getElementById('<%=lbNewPassword.ClientID%>').innerHTML;
            }
            return validation;
        }

        var duration = "<%=countdown%>";
        console.log(duration)
        if (duration != "")
            startTimer(parseInt(duration) - 1);
        function startTimer(duration) {
            var timer = duration, minutes, seconds;
            var countdown = setInterval(function () {
                minutes = parseInt(timer / 60, 10);
                seconds = parseInt(timer % 60, 10);

                minutes = minutes < 10 ? "0" + minutes : minutes;
                seconds = seconds < 10 ? "0" + seconds : seconds;

                document.getElementById("<%=lbError.ClientID%>").innerHTML = `You can't change your password within 5 minutes from the last change of password. Time Remaining: ${minutes}:${seconds}`;

                if (--timer < 0) {
                    timer = duration;
                    clearInterval(countdown)
                    document.getElementById("<%=lbError.ClientID%>").innerHTML = "You may now change your password.";
                    document.getElementById("<%=PanelErrorResult.ClientID%>").className = "alert alert-dismissable alert-success";
                }
            }, 1000);
        }
    </script>

</asp:Content>
