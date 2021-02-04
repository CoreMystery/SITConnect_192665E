<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SITConnect.Registration" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script src="https://www.google.com/recaptcha/api.js?render=<%= ConfigurationManager.AppSettings["RECAPTCHA_SITE_KEY"].ToString() %>"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.payment/1.0.3/jquery.payment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.inputmask/5.0.5/jquery.inputmask.min.js"></script>

    <style>
        label {
            margin-top: 10px;
        }

        input {
            margin-bottom: 10px;
            max-width: 1000px;
        }
        .auto-style1 {
            width: 545px;
        }
    </style>

    <div class="container" style="padding: 25px; width: 75%">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h2 class="panel-title" style="text-align: center; font-weight: bold"><%: Title %></h2>
            </div>

            <div class="panel-body" style="padding-left: 20%">
                <table style="width:545px">
                    <tr>
                        <td>
                            <label for="tbFName">First Name</label><br />
                            <asp:TextBox ID="tbFName" runat="server" placeholder="Enter First Name" Width="200px" pattern="[a-zA-Z]+" oninvalid="setCustomValidity('Please enter on alphabets only. ')"></asp:TextBox><br />
                            <asp:Label ID="lbFName" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                            <br />
                            <br />
                        </td>

                        <td style="padding-left: 135px">
                            <label for="tbLName">Last Name</label><br />
                            <asp:TextBox ID="tbLName" runat="server" placeholder="Enter Last Name" Width="200px"></asp:TextBox><br />
                            <asp:Label ID="lbLName" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>

                <label for="tbEmail">Email</label><br />
                <asp:TextBox ID="tbEmail" runat="server" Width="545px" placeholder="Enter Email" TextMode="Email"></asp:TextBox><br />
                <asp:Label ID="lbEmail" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                <br />
                <br />

                <label for="tbPassword">Password</label><br />
                <asp:TextBox ID="tbPassword" runat="server" Width="545px" placeholder="Enter Password" TextMode="Password"></asp:TextBox><br />
                <asp:Label ID="lbPassword" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label><br />
                <br />

                <label for="tbConfirmPassword">Confirm Password</label><br />
                <asp:TextBox ID="tbConfirmPassword" runat="server" Width="545px" placeholder="Enter Password Again" TextMode="Password"></asp:TextBox><br />
                <asp:Label ID="lbConfirmPassword" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                <br />
                <br />

                <label for="tbDOB">Date Of Birth</label><br />
                <asp:TextBox ID="tbDOB" runat="server" Width="545px" TextMode="Date"></asp:TextBox><br />
                <asp:Label ID="lbDOB" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                <br />
                <br />

                <table style="width:545px">
                    <tr>
                        <td>
                            <label for="tbCCNumber">Credit Card Number</label><br />
                            <asp:TextBox ID="tbCCNumber" runat="server" placeholder="xxxx xxxx xxxx xxxx" Width="178px"></asp:TextBox><br />
                            <asp:Label ID="lbCCNumber" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                            <br />
                        </td>

                        <td style="padding-left: 180px">
                            <label for="tbCCExpiryDate">Credit Card Expiry Date</label><br />
                            <asp:TextBox ID="tbCCExpiryDate" runat="server" placeholder="xx / xxxx" Width="178px"></asp:TextBox><br />
                            <asp:Label ID="lbCCExpiryDate" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                            <br />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="errorMsg" runat="server" ForeColor="Red"></asp:Label>
            </div>

            <div class="panel-footer" style="text-align: center; margin: auto">
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
                <button runat="server" onserverclick="btnRegister_Click" onclick="if (!validation()) return false;" class="btn btn-primary">Register</button>
            </div>
        </div>
    </div>
    
    <script>
        $('#<%=tbCCNumber.ClientID%>').inputmask('9999 9999 9999 9999');
        $('#<%=tbCCExpiryDate.ClientID%>').payment('formatCardExpiry');

        $(document).ready(function () {
            $("#<%=tbPassword.ClientID%>").keyup(function () {
                var password = document.getElementById('<%=tbPassword.ClientID%>').value;
                document.getElementById('<%=lbPassword.ClientID%>').innerHTML = "";
                if (password.length > 0) {
                    document.getElementById('<%=lbPassword.ClientID%>').innerHTML = "";
                }
                if (!(password.length >= 8)) {
                    document.getElementById('<%=lbPassword.ClientID%>').innerHTML += "Length should be at least 8 characters.<br>";
                }

                if (!password.match(/[a-z]+/)) {
                    document.getElementById('<%=lbPassword.ClientID%>').innerHTML += "Should contain at least one lowercase character.<br>";
                }

                if (!password.match(/[A-Z]+/)) {
                    document.getElementById('<%=lbPassword.ClientID%>').innerHTML += "Should contain at least one uppercase character.<br>";
                }

                if (!password.match(/\d+/)) {
                    document.getElementById('<%=lbPassword.ClientID%>').innerHTML += "Should contain at least numeric number.<br>";
                }

                if (!password.match(/[!@#$%^&*]+/)) {
                    document.getElementById('<%=lbPassword.ClientID%>').innerHTML += "Should contain at least one special character (!@#$%^&*).<br>";
                }
                if (document.getElementById('<%=lbPassword.ClientID%>').innerHTML != "") {
                    document.getElementById('<%=lbPassword.ClientID%>').innerHTML = "Complexity Requirements:<br>" + document.getElementById('<%=lbPassword.ClientID%>').innerHTML;
                }
            });

            $('#<%=tbPassword.ClientID%>, #<%=tbConfirmPassword.ClientID%>').keyup(function () {
                if ($('#<%=tbPassword.ClientID%>').val() == $('#<%=tbConfirmPassword.ClientID%>').val())
                    $('#<%=lbConfirmPassword.ClientID%>').html('Password match.').css('color', 'green');
                else
                    $('#<%=lbConfirmPassword.ClientID%>').html('Password does not match.').css('color', 'red');
            });
            $('#<%=tbPassword.ClientID%>, #<%=tbConfirmPassword.ClientID%>').click(function () {
                if ($('#<%=tbPassword.ClientID%>').val() == $('#<%=tbConfirmPassword.ClientID%>').val())
                    $('#<%=lbConfirmPassword.ClientID%>').html('Password match.').css('color', 'green');
                else
                    $('#<%=lbConfirmPassword.ClientID%>').html('Password does not match.').css('color', 'red');
            });
        });

        function validation() {
            var validation = true
            if (document.getElementById('<%=tbFName.ClientID%>').value == "") {
                document.getElementById('<%=lbFName.ClientID%>').innerHTML = "First Name cannot be empty.";
                validation = false;
            }
            else {
                if (!document.getElementById('<%=tbFName.ClientID%>').value.trim().match(/^[a-zA-Z ]+$/))
                {
                    document.getElementById('<%=lbFName.ClientID%>').innerHTML = "First Name must only contain alphabets.";
                    validation = false;
                }
                else
                {
                    document.getElementById('<%=lbFName.ClientID%>').innerHTML = "";
                }
            }
            if (document.getElementById('<%=tbLName.ClientID%>').value == "") {
                document.getElementById('<%=lbLName.ClientID%>').innerHTML = "Last Name cannot be empty.";
                validation = false;
            }
            else {
                if (!document.getElementById('<%=tbLName.ClientID%>').value.trim().match(/^[a-zA-Z ]+$/)) {
                    document.getElementById('<%=lbLName.ClientID%>').innerHTML = "Last Name must only contain alphabets.";
                    validation = false;
                }
                else
                {
                    document.getElementById('<%=lbLName.ClientID%>').innerHTML = "";
                }
            }
            if (document.getElementById('<%=tbEmail.ClientID%>').value == "") {
                document.getElementById('<%=lbEmail.ClientID%>').innerHTML = "Email cannot be empty.";
                validation = false;
            }
            else {
                if (!document.getElementById('<%=tbEmail.ClientID%>').value.match(/^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/))
                {
                    document.getElementById('<%=lbEmail.ClientID%>').innerHTML = "Email is in the wrong format. (E.g. SIT@mail.com)";
                    validation = false;
                }
                else
                {
                    document.getElementById('<%=lbEmail.ClientID%>').innerHTML = "";
                }
            }
            if (document.getElementById('<%=tbPassword.ClientID%>').value == "") {
                document.getElementById('<%=lbPassword.ClientID%>').innerHTML = "Password cannot be empty.";
                validation = false;
            }
            else {
                if (!passwordComplexity())
                    validation = false;
            }
            if (document.getElementById('<%=tbConfirmPassword.ClientID%>').value == "") {
                document.getElementById('<%=lbConfirmPassword.ClientID%>').style.color = "Red";
                document.getElementById('<%=lbConfirmPassword.ClientID%>').innerHTML = "Enter password again to confirm.";
                validation = false;
            }
            else {
                if (document.getElementById('<%=tbPassword.ClientID%>').value != document.getElementById('<%=tbConfirmPassword.ClientID%>').value) {
                    document.getElementById('<%=lbConfirmPassword.ClientID%>').innerHTML = "Password does not match.";
                    validation = false;
                }
                else {
                    document.getElementById('<%=lbConfirmPassword.ClientID%>').innerHTML = "";
                }
            }
            if (!document.getElementById('<%=tbDOB.ClientID%>').value.match(/^([0-9]{4})[-]([0]?[1-9]|[1][0-2])[-]([0][1-9]|[1|2][0-9]|[3][0|1])$/)) {
                document.getElementById('<%=lbDOB.ClientID%>').innerHTML = "Please select your date of birth.";
                validation = false;
            }
            else {
                document.getElementById('<%=lbDOB.ClientID%>').innerHTML = "";
            }
            if (!document.getElementById('<%=tbCCNumber.ClientID%>').value.match(/^\d{4} \d{4} \d{4} \d{4}$/)) {
                document.getElementById('<%=lbCCNumber.ClientID%>').innerHTML = "Please enter a valid card number.<br>(E.g. 1234 5678 1234 5678)";
                validation = false;
            }
            else {
                document.getElementById('<%=lbCCNumber.ClientID%>').innerHTML = "";
            }
            if (!document.getElementById('<%=tbCCExpiryDate.ClientID%>').value.match(/^([0][1-9]|[1][0-2]) [/] (\d{4})$/)) {
                document.getElementById('<%=lbCCExpiryDate.ClientID%>').innerHTML = "Please enter a valid expiry date.<br>(E.g. 01 / <%=currentYear%>)";
                validation = false;
            }
            else {
                document.getElementById('<%=lbCCExpiryDate.ClientID%>').innerHTML = "";
            }
            return validation;
        }

        function passwordComplexity() {
            var validation = true;
            var password = document.getElementById('<%=tbPassword.ClientID%>').value;
            document.getElementById('<%=lbPassword.ClientID%>').innerHTML = "";
            if (password.length > 0) {
                document.getElementById('<%=lbPassword.ClientID%>').innerHTML = "";
            }
            if (!(password.length >= 8)) {
                document.getElementById('<%=lbPassword.ClientID%>').innerHTML += "Length should be at least 8 characters.<br>";
                validation = false;
            }

            if (!password.match(/[a-z]+/)) {
                document.getElementById('<%=lbPassword.ClientID%>').innerHTML += "Should contain at least one lowercase character.<br>";
                validation = false;
            }

            if (!password.match(/[A-Z]+/)) {
                document.getElementById('<%=lbPassword.ClientID%>').innerHTML += "Should contain at least one uppercase character.<br>";
                validation = false;
            }

            if (!password.match(/\d+/)) {
                document.getElementById('<%=lbPassword.ClientID%>').innerHTML += "Should contain at least numeric number.<br>";
                validation = false;
            }

            if (!password.match(/[!@#$%^&*]+/)) {
                document.getElementById('<%=lbPassword.ClientID%>').innerHTML += "Should contain at least one special character (!@#$%^&*).<br>";
                validation = false;
            }
            if (document.getElementById('<%=lbPassword.ClientID%>').innerHTML != "") {
                document.getElementById('<%=lbPassword.ClientID%>').innerHTML = "Complexity Requirements:<br>" + document.getElementById('<%=lbPassword.ClientID%>').innerHTML;
            }
            return validation;
        }

        function reCaptcha() {
            grecaptcha.ready(function () {
                grecaptcha.execute('<%= ConfigurationManager.AppSettings["RECAPTCHA_SITE_KEY"].ToString() %>', { action: 'Register' }).then(function (token) {
                    document.getElementById("g-recaptcha-response").value = token;
                });
            });
        };
        reCaptcha();
        setInterval(function () { reCaptcha(); }, 90000);
    </script>

</asp:Content>
