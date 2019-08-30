import React, {Component} from 'react';
import {Link, NavLink} from "react-router-dom";

let azure = require('azure-storage/browser/azure-storage.blob.export.js');

let Config = require('./config.json');


class Breadcrumbs2 extends Component {
    private divider = " > ";

    constructor(props) {
        super(props);
        this.state = {
            breadCrumbs: []
        };
    }

    componentDidMount() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        fetch(Config.API_URL + "/BreadCrumbs/", {
            method: "GET",
            headers: myHeaders,
            body: this.props.location.pathname
        })
            .then(result => {
                if (!result.ok) {
                    throw result;
                }
                return result.json()
            })
            .then(breadCrumbs => {
                this.setState({breadCrumbs: breadCrumbs, loaded: true});
            })
            .catch(error => {
                console.error("Error:", error);
                if (error.status === 401) {
                    ////localStorage.clear();
                    ////window.location.replace("/");
                } else {
                    alert("Der skete en ukendt serverfejl ??");
                }
            });
    }

    render() {
        return (
            <div>
                {this.state.loaded
                    ? <span>
                        {this.state.breadCrumbs.map(breadCrumb =>
                            <span>
                                <NavLink to={breadCrumb.Url}>
                                    {breadCrumb.Title}
                                </NavLink>
                                {this.divider}
                            </span>
                        )
                        }
                    </span>
                    : <span>Loading...</span>}
            </div>
        );
    }

}

export default Breadcrumbs2;
