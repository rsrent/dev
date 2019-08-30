import React from 'react';
import PropTypes from 'prop-types';
import {matchPath, withRouter} from 'react-router';
import {Link, NavLink, Route} from 'react-router-dom';
import HelperMethods from "./HelperMethods";

let Config = require('./config.json');


class Breadcrumbs extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            breadCrumbs: [],
            loaded: false,
            location: null
        };
        window.onhashchange = ( location => {
            console.log(location);
            this.loadBreadcrumbs();
        });
        this.loadBreadcrumbs();
    }

    loadBreadcrumbs() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        myHeaders.append("Content-Type", "application/json");
        HelperMethods.postRequest("BreadCrumbs/", myHeaders,
            (breadCrumbs) => {
                this.setState({breadCrumbs: breadCrumbs, loaded: true});
            },
            () => {
                this.setState({breadCrumbs: [], loaded: true})
            }, '"' + window.location.hash.replace("#/", "") + '"');
        // fetch(Config.API_URL + "/BreadCrumbs/", {
        //     method: "POST",
        //     headers: myHeaders,
        //     body: '"' + window.location.hash.replace("#/", "") + '"'
        // })
        //     .then(result => {
        //         if (!result.ok) {
        //             throw result;
        //         }
        //         return result.json();
        //     })
        //     .then(breadCrumbs => {
        //         this.setState({breadCrumbs: breadCrumbs, loaded: true});
        //     })
        //     .catch(error => {
        //         this.setState({breadCrumbs: [], loaded: true})
        //     });
    }

    render() {
        return <div>{this.state.loaded
            ? this.state.breadCrumbs.map((crumb, i) =>
                <span><Link
                    to={crumb.url}>{crumb.title}</Link> {i !== this.state.breadCrumbs.length - 1 ? " > " : ""}</span>
            )
            : "Loading..."}</div>;
    }
}


export default withRouter(Breadcrumbs);