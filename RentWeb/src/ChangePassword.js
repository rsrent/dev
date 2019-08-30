import React, {Component} from 'react';
import {Link} from "react-router-dom";
import HelperMethods from "./HelperMethods";

let Config = require('./config.json');

class ChangePassword extends Component {
    constructor(props) {
        super(props);
        this.state = {
            oldPassword: "",
            newPassword: "",
            newPasswordAgain: "",
            success: false
        };
        this.changePassword = this.changePassword.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);

    }

    changePassword(event) {
        event.preventDefault();
        if (this.state.newPassword !== this.state.newPasswordAgain) {
            alert("De nye kodeord er ikke ens!");
            return;
        }
        if (this.state.newPassword.length < 6) {
            alert("Kodeordet kan ikke være mindre end 6 tegn");
            return;
        }
        let updateLogin = {
            Login: {
                ID: localStorage.getItem('userID'),
                UserName: localStorage.getItem('userName'),
                Password: this.state.oldPassword
            },
            newPassword: this.state.newPassword
        };
        updateLogin = JSON.stringify(updateLogin);
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        myHeaders.append("Content-Type", "application/json");
        HelperMethods.putRequest("Logins/UpdatePassword", myHeaders,
            (response) => {
                alert("Kodeord opdateret.");
                localStorage.setItem("token", response);
                this.setState({success: true});
            },
            (error) => {
                console.log(error);
                alert("Det gamle kodeord er forkert.");
                this.setState({success: false});
            }, updateLogin);
        // fetch(Config.API_URL + "/Logins/UpdatePassword", {
        //     method: "PUT",
        //     body: updateLogin,
        //     headers: myHeaders
        // })
        //     .then(result => {
        //         if (!result.ok) {
        //             alert("Det gamle kodeord er forkert.");
        //             this.setState({success: false});
        //             return;
        //         }
        //         alert("Kodeord opdateret.");
        //         this.setState({success: true});
        //     });
    }

    handleInputChange(event) {
        const name = event.target.name;
        console.log(name);
        this.setState({[name]: event.target.value});
    }

    render() {
        return (
            <div className="col-lg-6 col-md-8 col-sm-12" style={{margin: 'auto'}}>
                <form onSubmit={this.changePassword} style={{marginTop: '20px'}}>
                    <div className="form-group text-left">
                        <label className="sr-only" htmlFor="inputName">Gammelt kodeord</label>
                        <div className="input-group mb-2 mr-sm-2 mb-sm-0">
                            <div className="input-group-addon">
                                <svg xmlns="http://www.w3.org/2000/svg"
                                     xmlnsXlink="http://www.w3.org/1999/xlink" version="1.1" width={19}
                                     height={19} viewBox="0 0 24 24">
                                    <path
                                        d="M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z"/>
                                </svg>
                            </div>
                            <input type="password" id="inputName" className="form-control"
                                   placeholder="Gammelt kodeord"
                                   aria-describedby="nameHelp" tabIndex={1} required autoFocus
                                   name="oldPassword"
                                   value={this.state.oldPassword} onChange={this.handleInputChange}
                            />
                        </div>
                    </div>
                    <div className="form-group text-left">
                        <label className="sr-only" htmlFor="inputName">Nyt kodeord</label>
                        <div className="input-group mb-2 mr-sm-2 mb-sm-0">
                            <div className="input-group-addon">
                                <svg xmlns="http://www.w3.org/2000/svg"
                                     xmlnsXlink="http://www.w3.org/1999/xlink" version="1.1" width={19}
                                     height={19} viewBox="0 0 24 24">
                                    <path
                                        d="M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z"/>
                                </svg>
                            </div>
                            <input type="password" id="inputPassword" className="form-control"
                                   placeholder="Nyt kodeord"
                                   aria-describedby="nameHelp" tabIndex={2} required
                                   name="newPassword"
                                   value={this.state.newPassword} onChange={this.handleInputChange}
                            />
                        </div>
                    </div>
                    <div className="form-group text-left">
                        <label className="sr-only" htmlFor="inputName">Nyt kodeord igen</label>
                        <div className="input-group mb-2 mr-sm-2 mb-sm-0">
                            <div className="input-group-addon">
                                <svg xmlns="http://www.w3.org/2000/svg"
                                     xmlnsXlink="http://www.w3.org/1999/xlink" version="1.1" width={19}
                                     height={19} viewBox="0 0 24 24">
                                    <path
                                        d="M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z"/>
                                </svg>
                            </div>
                            <input type="password" id="inputPassword" className="form-control"
                                   placeholder="Nyt kodeord igen"
                                   aria-describedby="nameHelp" tabIndex={3} required
                                   name="newPasswordAgain"
                                   value={this.state.newPasswordAgain} onChange={this.handleInputChange}
                            />
                        </div>
                    </div>
                    <button type="submit" className="btn btn-simp select-button btn-primary"
                            style={{width: '100%', marginBottom: '20px', clear: 'both', display: 'block'}}>Skift kodeord
                    </button>
                </form>
            </div>
        );
    }

}

export default ChangePassword;