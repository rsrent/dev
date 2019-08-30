import React, {Component} from 'react';
import {Link, Redirect} from "react-router-dom";
import Breadcrumbs from "./Breadcrumbs";
import HelperMethods from "./HelperMethods";
// let HelperMethods = require("./HelperMethods.js");
let Config = require('./config.json');

class Navbar extends Component {

    constructor(props) {
        super(props);
        this.state = {loggedIn: false};
        this.logout = this.logout.bind(this);
    }

    logout() {
          let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.genericRequestWithoutBody("Logins/Logout", myHeaders, (success) =>
        {
            localStorage.clear();
            this.setState({loggedIn: false})
        }, undefined, "POST");

    }

    render() {
        if (localStorage.getItem("token") !== null && !this.state.loggedIn) {
            this.setState({loggedIn: true})
        }
        if ((!this.state.loggedIn && localStorage.getItem("token") === null)) {

            return (

                <div>
                    <nav/>
                    <Redirect to={"/"}/>
                </div>
            )
            // return (<div></div>)
        } else {
            return (
                <nav className="navbar navbar-expand-md navbar-simp bg-simp fixed-top">
                    <a className="navbar-brand no-underline pointer">
                        <div className="navbar-logo-back-button"/>
                    </a>

                    <Breadcrumbs />

                    <button className="navbar-toggler" type="button" data-toggle="collapse"
                            data-target="#navbarBurgerDropDown" aria-controls="navbarBurgerDropDown"
                            aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"/>
                    </button>

                    {/* BURGER ON SMALLER SCREENS */}
                    <div className="collapse navbar-collapse" id="navbarBurgerDropDown">
                        {/* NAVIGATION LEFT EMPTY */}
                        <ul className="navbar-nav mr-auto">
                            <nav aria-label="empty-nav">
                                <ol className="empty-nav">
                                </ol>
                            </nav>
                        </ul>
                        {/* /NAVIGATION LEFT EMPTY */}
                        {/* ACCOUNT DROPDOWN PULLS RIGHT */}
                        <li className="nav-item dropdown pointer">
                            <a className="nav-link dropdown-toggle no-underline" id="accountdropdown"
                               data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <div className="account-icon">
                                    <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink"
                                         version="1.1" width={30} height={30} viewBox="0 0 24 24">
                                        <path
                                            d="M12,19.2C9.5,19.2 7.29,17.92 6,16C6.03,14 10,12.9 12,12.9C14,12.9 17.97,14 18,16C16.71,17.92 14.5,19.2 12,19.2M12,5A3,3 0 0,1 15,8A3,3 0 0,1 12,11A3,3 0 0,1 9,8A3,3 0 0,1 12,5M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12C22,6.47 17.5,2 12,2Z"/>
                                    </svg>
                                </div>
                                <div className="account-name">{localStorage.getItem("fullname")}</div>
                            </a>
                            <div className="dropdown-menu" aria-labelledby="accountdropdown">
                                <a className="dropdown-item" onClick={this.logout}>LOGOUT</a>
                                <Link className="dropdown-item" to="/ChangePassword/">Skift kodeord</Link>
                            </div>
                        </li>
                        {/* /ACCOUNT DROPDOWN PULLS RIGHT */}
                    </div>
                    {/* /BURGER ON SMALLER SCREENS */}
                </nav>
            );
        }
    }
}

// class BreadcrumbCustom extends React.Component {
//
//     constructor(props) {
//         super(props);
//         this.state = {
//             breadCrumbs: [],
//             loaded: false,
//             location: null
//         };
//         window.onhashchange = ( location =>  {
//             console.log(location);
//             this.setState(this.state);
//         });
//     }
//
//     componentDidMount() {
//         let myHeaders = new Headers();
//         myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
//         myHeaders.append("Content-Type", "application/json");
//         fetch(Config.API_URL + "/BreadCrumbs/", {
//             method: "POST",
//             headers: myHeaders,
//             body: '"' + window.location.hash.replace("#/", "") + '"'
//         })
//             .then(result => {
//                 if (!result.ok) {
//                     throw result;
//                 }
//                 return result.json();
//             })
//             .then(breadCrumbs => {
//                 this.setState({breadCrumbs: breadCrumbs, loaded: true});
//             })
//             .catch(error => {
//                 this.setState({breadCrumbs: [], loaded: true})
//             });
//
//     }
//
//     render() {
//         return <span>{this.state.loaded
//             ? this.state.breadCrumbs.map(crumb =>
//                 crumb.title + " > "
//             )
//             : "Loading..."}</span>;
//     }
// }

export default Navbar;
