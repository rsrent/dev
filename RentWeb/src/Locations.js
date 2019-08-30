import React, {Component} from 'react';
import {Link} from "react-router-dom";
import HelperMethods from "./HelperMethods";
import MySearchBar from "./MySearchBar";
import LoadingIndicator from "./LoadingIndicator";

let Config = require('./config.json');

class Locations extends Component {
    constructor(props) {
        super(props);
        this.state = {
            userID: typeof props.match.params.customerID === 'undefined' ? localStorage.getItem("userID") : props.match.params.customerID,
            locations: [],
            loaded: false,
            customerID: 0,
            customerName: "",
            searchText: "",
        };
    }

    getFiltered() {
        return this.state.locations.filter(l => l.name.toLowerCase().includes(this.state.searchText.toLowerCase()));
    }

    componentDidMount() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        let fetchURL = typeof this.props.match.params.customerID !== 'undefined' ? "Location/GetForCustomer/" + this.props.match.params.customerID : "Location/GetForUser/" + localStorage.getItem("userID");
        HelperMethods.getRequest(fetchURL, myHeaders, (locations) => {
            this.setState({locations: locations, customerID: locations[0].customerID});
        }, undefined, () => {
            HelperMethods.getRequest("Customers/GetCustomerName/" + this.state.customerID, myHeaders,
                (customerName) => {
                    this.setState({customerName: customerName});
                }, undefined, () => {this.setState({loaded: true});}
            );
        });

    }

    render() {
        const locations = this.state.locations;
        return (
            <div>
                <div className="buttons-group top-margin">
                <h2>{this.state.loaded ? this.state.customerName : "Loading..."}</h2>
                    {(localStorage.getItem("roleID") === "1" || localStorage.getItem("roleID") === "2" || localStorage.getItem("roleID") === "9") &&
                    <div className="button-content">
                        <Link className="btn btn-simp btn-secondary select-button" href="#" role="button"
                              to={{
                                  pathname: `/Customers/${this.state.customerID}/Documents/`
                              }}>Dokumenter</Link>
                    </div>
                    }
                    <MySearchBar handler={(s) => this.setState(s)}/>
                    {
                        locations.length && this.state.loaded ?
                            this.getFiltered().map(location =>
                                <div className="button-content" key={location.id}>
                                    <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                                          to={{
                                              pathname: `${this.props.location.pathname}Locations/${location.id}/`,
                                              state: {locationInformation: location}
                                          }}>{(location.customerName != null ? location.customerName + " - " : "") + location.name}</Link>
                                </div>)
                            :
                            this.state.loaded
                                ? <p>Der er ikke nogle lokationer til denne bruger</p>
                                : <LoadingIndicator/>
                    }
                </div>
            </div>
        );
    }

}

export default Locations;
