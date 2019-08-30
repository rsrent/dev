import React, {Component} from 'react';
import {Link} from "react-router-dom";
import HelperMethods from "./HelperMethods";
import MySearchBar from "./MySearchBar";
import LoadingIndicator from "./LoadingIndicator";

let Config = require('./config.json');

class ShowCustomers extends Component {
    constructor(props) {
        super(props);
        this.state = {
            customers: [],
            loaded: false,
            searchText: "",
        };
    }

    getFiltered() {
        return this.state.customers.filter(c => c.name.toLowerCase().includes(this.state.searchText.toLowerCase()));
    }


    componentDidMount() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("Customers", myHeaders,
            (customers) => {
                this.setState({customers: customers});
            }, undefined,
            () => {
                this.setState({loaded: true});
            }
        );
    }

    render() {
        return (
            <div>
                <div className="buttons-group">
                    <MySearchBar handler={(s) => this.setState(s)}/>
                    {
                        this.state.customers.length && this.state.loaded ?
                            this.getFiltered().map(customer =>
                                <div className="button-content" key={customer.id}>
                                    <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                                          to={{
                                              pathname: `${this.props.location.pathname}${customer.id}/`
                                          }}>{customer.name}</Link>
                                </div>)
                            :
                            this.state.loaded
                                ? <p>"Der er ikke nogle kunder i systemet"</p>
                                : <LoadingIndicator/>
                    }
                </div>
            </div>
        );
    }

}

export default ShowCustomers;