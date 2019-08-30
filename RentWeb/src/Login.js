import React, {Component} from 'react';
import {Redirect} from "react-router-dom";
import HelperMethods from "./HelperMethods";

let Config = require('./config.json');

class Login extends Component {
    constructor(props) {
        super(props);
        this.state = {userName: '', password: '', userID: 0, redirect: false, roleID: -1, customerID:-1};
        this.login = this.login.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.forgotPassword = this.forgotPassword.bind(this);
        this.loginCheck();
    }

    loginCheck() {
        if (localStorage.getItem('token') !== null) {
            let myHeaders = new Headers();
            myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
            myHeaders.append("Content-Type", "application/json");
            fetch(Config.API_URL + "/Logins/HasValidToken",
                {
                    method: "GET",
                    headers: myHeaders
                })
                .then(res => {
                    if (!res.ok) {
                        throw res;
                    }
                    return res.json();
                })
                .then(res => {
                    console.log(res);
                    this.setState({"redirect": true, "userID": res})
                }).catch(() => {
                    ////localStorage.clear();
            });
        }
    }

    login(event) {
        event.preventDefault();

        let user = {
            UserName: this.state.userName,
            Password: this.state.password
        };
        user = JSON.stringify(user);
        let myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");
        HelperMethods.postRequest("Logins/Login", myHeaders,
            (data) => {
                localStorage.setItem('token', data.token);
                localStorage.setItem('fullname', data.user.firstName + " " + data.user.lastName);
                localStorage.setItem('title', data.user.title);
                localStorage.setItem('permissions', JSON.stringify(data.user.permissions));
                localStorage.setItem('userID', data.user.id);
                localStorage.setItem('roleID', data.user.roleID);
                localStorage.setItem('userName', this.state.userName);
                localStorage.setItem('customerID', data.user.customerID);
                this.setState({"redirect": true, "userID": data.user.id, roleID: data.user.roleID, customerID:data.user.customerID})
            },
            (error) => {

              console.log(error);

                if (error.status === 401) {
                    alert("Forkert brugernavn eller kodeord");
                } else {
                    alert("Der skete en ukendt serverfejl 1");
                }
            }, user);
        // fetch(Config.API_URL + "/Logins/Login",
        //     {
        //         method: "POST",
        //         body: user,
        //         headers: myHeaders
        //     })
        //     .then(function (res) {
        //         if (!res.ok) {
        //             throw res;
        //         }
        //         return res.json();
        //     })
        //     .then(data => {
        //         localStorage.setItem('token', data.token);
        //         localStorage.setItem('fullname', data.user.firstName + " " + data.user.lastName);
        //         localStorage.setItem('title', data.user.title);
        //         localStorage.setItem('permissions', JSON.stringify(data.user.permissions));
        //         localStorage.setItem('userID', data.user.id);
        //         localStorage.setItem('roleID', data.user.role.id);
        //         localStorage.setItem('userName', this.state.userName);
        //         console.log(data);
        //         this.setState({"redirect": true, "userID": data.user.id, roleID: data.user.role.id})
        //     })
        //     .catch(error => {
        //         if (error.status === 401) {
        //             alert("Forkert brugernavn eller kodeord");
        //         } else {
        //             alert("Der skete en ukendt serverfejl");
        //         }
        //     });
    }

    handleInputChange(event) {
        const name = event.target.name;
        this.setState({[name]: event.target.value});
    }

    forgotPassword(){
        alert("Send en email til info@rs-rent.dk for at få et nyt password");
    }

    render() {
        if(this.state.redirect && (localStorage.getItem("roleID") === "1" || localStorage.getItem("roleID") === "2")){
            return (
                <Redirect to={"Customers/"}/>
            );
        }else if (this.state.redirect) {
            return (
                <Redirect to={"Customers/" + localStorage.getItem("customerID") + "/"}/>
            );
        }
        return (
            <div className="login-wrapper">
                <div className="login-wrapper-inner">
                    <div className="login-container">
                        {/* MAIN LOGIN */}
                        <main role="main" className="inner">
                            <div className="logo"><img src="img/simp-logo.svg"/></div>
                            <h1 className="display-3 simp-logo-text">SIMP</h1>
                            {/* novalidate disables the browser default feedback tooltips (which can be annoying) but still provides access to the form validation APIs in JavaScript. When attempting to submit, you’ll see the :invalid and :valid styles applied to your form controls */}
                            <form className="container" id="needs-validation" noValidate onSubmit={this.login}>
                                {/* Do we need to include validation here? I have included pattern="[a-zA-Z]{4,30}" */}
                                {/* sr-only hides lable name apart from for screen readers */}
                                {/* autofocus sets user focus automatically to this input */}
                                <div className="form-group text-left">
                                    <label className="sr-only" htmlFor="inputName">Username</label>
                                    <div className="input-group mb-2 mr-sm-2 mb-sm-0">
                                        <div className="input-group-addon">
                                            <svg xmlns="http://www.w3.org/2000/svg"
                                                 xmlnsXlink="http://www.w3.org/1999/xlink" version="1.1" width={19}
                                                 height={19} viewBox="0 0 24 24">
                                                <path
                                                    d="M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z"/>
                                            </svg>
                                        </div>
                                        <input type="text" id="inputName" className="form-control"
                                               placeholder="User Name" pattern="[a-zA-Z]{4,30}"
                                               aria-describedby="nameHelp" tabIndex={1} required autoFocus
                                               name="userName"
                                               value={this.state.userName} onChange={this.handleInputChange}
                                        />
                                    </div>
                                    <small id="nameHelp" className="form-text text-muted">Please enter your username.
                                    </small>
                                </div>
                                {/* Do we need to include validation here? I have included pattern="[a-zA-Z]{8,60}" */}
                                {/* sr-only hides lable name apart from for screen readers */}
                                <div className="form-group text-left">
                                    <label className="sr-only" htmlFor="inputPassword">Password</label>
                                    <div className="input-group mb-2 mr-sm-2 mb-sm-0">
                                        <div className="input-group-addon">
                                            <svg xmlns="http://www.w3.org/2000/svg"
                                                 xmlnsXlink="http://www.w3.org/1999/xlink" version="1.1" width={20}
                                                 height={20} viewBox="0 0 24 24">
                                                <path
                                                    d="M7,14A2,2 0 0,1 5,12A2,2 0 0,1 7,10A2,2 0 0,1 9,12A2,2 0 0,1 7,14M12.65,10C11.83,7.67 9.61,6 7,6A6,6 0 0,0 1,12A6,6 0 0,0 7,18C9.61,18 11.83,16.33 12.65,14H17V18H21V14H23V10H12.65Z"/>
                                            </svg>
                                        </div>
                                        <input type="password" id="inputPassword" className="form-control"
                                               placeholder="Password" pattern="[a-zA-Z]{8,20}"
                                               aria-describedby="passwordHelp" tabIndex={2} required
                                               name="password"
                                               value={this.state.password} onChange={this.handleInputChange}
                                        />
                                    </div>
                                    <small id="passwordHelp" className="form-text text-muted">Must be 8-20 characters
                                        long.
                                    </small>
                                </div>
                                <button type="submit" className="btn btn-simp btn-primary login-button pointer"
                                        aria-describedby="loginHelp">LOGIN
                                </button>
                                <a href="#" onClick={this.forgotPassword}>
                                    <small id="loginHelp" className="form-text text-muted">Forgot password?</small>
                                </a>
                            </form>
                        </main>
                        {/* /MAIN LOGIN */}
                    </div>
                </div>
            </div>

        );
    }
}

export default Login;
